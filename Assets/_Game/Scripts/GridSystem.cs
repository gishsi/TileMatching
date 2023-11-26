using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Events;
using _Game.Scripts.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    /// <summary>
    ///     This class represents the system that manages all tiles.
    /// </summary>
    public class GridSystem : MonoBehaviour
    {
        // *********** Constants *********** //
        private const float TileSpawnDisplacement = 1.4f;
        private const float SpacingBetweenTiles = 0.4f;
        // This is so that the origin of the new sprite is aligned to the origin of parent
        private const float MarginTopLeftOfTilesInGrid = 0.5f;
        
        // *********** Events *********** //
        [Header("Events")]
        [SerializeField]
        private UpdateSwapScriptableObject updateSwap;
        
        [SerializeField]
        private EvaluateTriggerScriptableObject evaluateTrigger;
        
        [SerializeField]
        private SwipeTriggerScriptableObject swipeTrigger;
        
        // *********** Private  *********** //
        private ILogger<GridSystem> _logger;
        
        private readonly Color _blueTileColor = new (0, 0.5381241f, 1);
        private readonly Color _greenTileColor = new (0, 0.4235294f, 0.3098039f);

        // *********** Serializable *********** //
        [Header("Tiles")]
        [SerializeField] private GameObject jewel;
        [SerializeField] private GameObject blocker;
        
        // **************** MonoBehaviour **************
        private void Awake()
        {
            _logger = new Logger<GridSystem>(gameObject);
        }

        private void Start()
        {
            updateSwap.Reset();
        }
        
        private void OnEnable()
        {
            // Subscribing to OnEvaluate first will trigger this method first if both get triggered
            evaluateTrigger.EvaluateTriggerEvent.AddListener(EvaluateGrid);
            swipeTrigger.SwipeTriggerEvent.AddListener(SwitchPlaces);
            updateSwap.SwapsBelowZeroEvent.AddListener(IsGameOver);
        }
        
        private void OnDisable()
        {
            evaluateTrigger.EvaluateTriggerEvent.RemoveListener(EvaluateGrid);
            swipeTrigger.SwipeTriggerEvent.RemoveListener(SwitchPlaces);
            updateSwap.SwapsBelowZeroEvent.RemoveListener(IsGameOver);
        }

        // **************** Init *******************

        /// <summary>
        ///     Initialize the grid with the levels data 
        /// </summary>
        /// <param name="rowsArray"></param>
        public void InitializeGrid(Rows[] rowsArray)
        {
            // Amount of rows is columns
            var columnsCount = rowsArray.Count();
            // We have a number of rows, the length of the array of the rows represents how many columns we have
            for (var y = 0; y < columnsCount; y++)
            {
                var numberOfTilesInRow = rowsArray[y].tilesInRow.Count();
                
                for (var x = 0; x < numberOfTilesInRow; x++)
                {
                    var tileType = rowsArray[y].tilesInRow[x];

                    var objectToSpawn = ResolveTileTypeIntoGameObject(tileType);
                    
                    var xDisplacement = x * TileSpawnDisplacement + MarginTopLeftOfTilesInGrid;
                    var yDisplacement = y * TileSpawnDisplacement + MarginTopLeftOfTilesInGrid;
                    
                    var newJewel = Instantiate(objectToSpawn, transform.position + new Vector3(xDisplacement, yDisplacement, 0), Quaternion.identity);
                    newJewel.name = ParseVector2IntIntoNameString(new Vector2Int(x, y));
                    
                    // So that Jewels are spawned under the GridSystem gameObject in the hierarchy
                    newJewel.transform.parent = transform;
                }
            }
        }
        
        public void SetPositionOfGridBasedOnAmountOfColsAndRows(int rows, int cols)
        {
            // Needs spacing. For every tile, except the last one add .4f margin to right, if last add .2f,
            //      so that the grid is exactly in the middle, no matter how many tiles it has.
            var halfOfRows = rows / 2f;
            var fullMarginRows = (halfOfRows - 1) * SpacingBetweenTiles;
            var halfOfCols = cols / 2f;
            var fullMarginCols = (halfOfCols - 1) * SpacingBetweenTiles;

            var verticalOffset = (rows / 2f + (fullMarginRows + (SpacingBetweenTiles / 2f))) * -1;
            var horizontalOffset = (cols / 2f + (fullMarginCols + (SpacingBetweenTiles / 2f))) * -1;

            transform.position = new Vector3(horizontalOffset, verticalOffset, 0);
        }
        
        // **************** Private **************

        /// <summary>
        ///     Triggered when swipe reach below or equal 0, and when the player performs an elimination. If conditions are met, navigates to the player lost screen.
        /// </summary>
        private void IsGameOver()
        {
            var CanPlayerEliminate = AreThereAnyMatchingSets();
            
            // Automatic game over if there are no swaps or eliminations left.
            // todo: also needs to check if all remaining tiles are the same
            var tilesLeftThatCannotBeEliminated = GetAllTilesInGrid().Count > 0 && !CanPlayerEliminate;
            
            var isGameOver = tilesLeftThatCannotBeEliminated && updateSwap.Swaps <= 0;

            if (!isGameOver)
            {
                return;
            }

            SceneManager.LoadScene("_Game/Scenes/PlayerLost");
        }
        
        /// <summary>
        ///     Resolve the type of tile into a game object
        /// </summary>
        /// <param name="tileType">Tile type</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When a non existing tile type has been passed in.</exception>
        private GameObject ResolveTileTypeIntoGameObject(Tiles tileType)
        {
            GameObject objectToSpawn;
            
            switch (tileType)
            {
                case Tiles.BlueTile:
                    objectToSpawn = jewel;
                    objectToSpawn.GetComponent<SpriteRenderer>().color = _blueTileColor;
                    break;
                case Tiles.GreenTile:
                    objectToSpawn = jewel;
                    objectToSpawn.GetComponent<SpriteRenderer>().color = _greenTileColor;
                    break;
                case Tiles.Blocker:
                    objectToSpawn = blocker;
                    break;
                default:
                    throw new ArgumentException($"There is no game object representation for type [{tileType}]", nameof(tileType));
            }

            return objectToSpawn;
        }
        
        /// <summary>
        ///     Listens for an event broadcast produced by a clicked tile. If there was an event it evaluates all matching zones, removes matched jewels, and restructures the grid.
        /// </summary>
        private void EvaluateGrid(Evaluate data)
        {
            var matchingJewels = MatchingEvaluationHelper.GetMatchingJewels(ParseNameIntoVector2Int(data.Tile.name));

            if (matchingJewels.Count == 0)
            {
                Debug.Log("Tile does not meet matching criteria.");
                return;
            }

            var jewelsRemovedFromPowerUp = new List<GameObject>();
            
            foreach (var matchingJewel in matchingJewels)
            {
                try
                {
                    // This is bad. Grid system is doing too much.
                    var powerUp = matchingJewel.GetComponent<PowerUpSlot>().PowerUp;
                    
                    if (powerUp == PowerUps.Bomb)
                    {
                        foreach (var offset in MatchingEvaluationHelper.PowerUpMatchZone)
                        {
                            var gridCoordinateOfTileInMatchZone = ParseNameIntoVector2Int(matchingJewel.name) + offset;
                            var nameOfTileInMatchZone =
                                GridSystem.ParseVector2IntIntoNameString(gridCoordinateOfTileInMatchZone);

                            GameObject tile;
                    
                            try
                            {
                                tile = GameObject.Find(nameOfTileInMatchZone);
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
         
                            if (tile == null)
                            {
                                continue;
                            }

                            if (!tile.CompareTag("Tile"))
                            {
                                continue;
                            }

                            jewelsRemovedFromPowerUp.Add(tile);
                        }
                       
                    }

                    if (powerUp == PowerUps.ColourBomb)
                    {
                        var allTiles = GetAllTilesInGrid();
 
                        foreach (var tile in allTiles)
                        {
                            if (tile.GetComponent<SpriteRenderer>().color ==
                                matchingJewel.GetComponent<SpriteRenderer>().color)
                            {
                                jewelsRemovedFromPowerUp.Add(tile);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("No power up on that fellow.");
                }
            }

            // Hash set so that we don't try to remove two of the same tiles twice
            var allMatched = matchingJewels.Concat(jewelsRemovedFromPowerUp).ToHashSet();
            
            // Needs to wait until the end of frame - this is when destroy will be finished.
            StartCoroutine(HandleDestroyJewels(allMatched.ToList()));
        }
        
        /// <summary>
        ///     After the player performs a Swipe action the grid system will evaluate whether that swipe will result in tiles being swiped by comparing the next tile in the direction of the swipe.
        /// </summary>
        /// <param name="swipe">The name of the tile corresponds to its position in the grid, e.g. [1, 1] and the direction of the swipe</param>
        private void SwitchPlaces(Swipe swipe)
        {
            if (updateSwap.Swaps <= 0)
            {
                Debug.Log("No swaps left.");
                IsGameOver();
                return;
            }
            
            // Find the first tile in the direction of the swipe
            var origin = ParseNameIntoVector2Int(swipe.TileName);
            var target = origin + swipe.DirectionOfTheSwipe;

            try
            {
                // Get the object that was picked up to be swiped (from tileName + direction)
                var targetTile = GameObject.Find(ParseVector2IntIntoNameString(new Vector2Int((int)target.x, (int)target.y)));
                var originTile = GameObject.Find(ParseVector2IntIntoNameString(new Vector2Int(origin.x, origin.y)));
                
                // Find out if its okay to swipe (same colour)
                var originSpriteColour = originTile.GetComponent<SpriteRenderer>().color;
                var targetSpriteColour = targetTile.GetComponent<SpriteRenderer>().color;

                // todo: Is this really a good idea? What if we add more rules, e.g. blockers?
                if (originSpriteColour == targetSpriteColour || targetTile.GetComponent<Actionable>() == null)
                {
                    Debug.Log("Swap not valid. Colours are different or the tile is missing the actionable component.");
                    
                    return;
                }
                
                updateSwap.RaiseEvent(1);
                
                // E.g.: Position of the first one is (0.5f, 0.5f),
                // if we swapped a tile at 0, 0 with a tile at 1, 0 the position would be 0.5f, 0.5f, and (1 * 1.4f) + 0.5f = 1.9f, 0.5f
                
                // Get new positions
                var newPositionForTarget = GetLocalPositionForGridCoordinate(origin);
                var newPositionForOrigin = GetLocalPositionForGridCoordinate(target);

                targetTile.transform.localPosition = newPositionForTarget;
                targetTile.name = swipe.TileName;
                
                originTile.transform.localPosition = newPositionForOrigin;
                originTile.name = ParseVector2IntIntoNameString(new Vector2Int((int)target.x, (int)target.y));
                
                Debug.Log("Successful swap between [" + targetTile.name + "] and [" + originTile.name + "]");
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not swap the tiles:\n{e}");
            }
        }
        
        /// <summary>
        ///     The name of the tile represents its position in the grid.
        /// </summary>
        /// <param name="coordinates">Coordinates of the tile in the grid.</param>
        public static string ParseVector2IntIntoNameString(Vector2Int coordinates)
        {
            return $"{coordinates.x};{coordinates.y}";
        }

        /// <summary>
        ///     Parses the name of the tile (its coordinates in the grid) into a vector
        /// </summary>
        /// <param name="name">Position of the tile</param>
        /// <returns>A vector representing the position in the grid</returns>
        private static Vector2Int ParseNameIntoVector2Int(string name)
        {
            try
            {
                var values = name.Split(';');
                var x = int.Parse(values[0]);
                var y = int.Parse(values[1]);

                return new Vector2Int(x, y);
            }
            catch
            {
                throw new ArgumentException("Could not parse the name of the tile into a position in a grid.",
                    nameof(name));
            }
        }
        
        /// <summary>
        ///     Translates a grid coordinate into a position in the world space.
        /// </summary>
        /// <param name="gridCoordinate">Grid coordinate, e.g. [0, 1]</param>
        /// <returns>
        ///     A Vector2 that represents the position in the world space.
        /// </returns>
        /// <remarks>
        ///     We need to change the local position of the tile for it to be positioned correctly in the grid.
        /// </remarks>
        private static Vector2 GetLocalPositionForGridCoordinate(Vector2 gridCoordinate)
        {
            return new Vector2((TileSpawnDisplacement * gridCoordinate.x) + MarginTopLeftOfTilesInGrid, (TileSpawnDisplacement * gridCoordinate.y) + MarginTopLeftOfTilesInGrid);
        }
        
        /// <summary>
        ///     Check if there are any tiles that the player can remove
        /// </summary>
        private bool DoesGridContainTilesToRemove()
        {
            var actionableTiles = new List<Transform>();
            foreach (Transform child in transform)
            {
                if(child.CompareTag("Tile"))
                {
                    actionableTiles.Add(child);
                }
            }
            
            return actionableTiles.Count > 0;
        }
        
        /// <summary>
        ///     Destroy jewels and perform all actions that happen afterwards: destroying jewels, moving on to the level finished scene, restructuring the grid
        /// </summary>
        /// <param name="jewels"></param>
        /// <remarks></remarks>
        private IEnumerator HandleDestroyJewels(List<GameObject> jewels)
        {
            foreach (var jewel in jewels)
            {
                try
                {
                    var powerUp = jewel.GetComponent<PowerUpSlot>().PowerUp;
                    if (powerUp == PowerUps.Concretion)
                    {
                        var blockerToSpawn = Instantiate(blocker, jewel.transform.position, Quaternion.identity);
                        blockerToSpawn.gameObject.name = jewel.gameObject.name;
                        blockerToSpawn.transform.parent = transform;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("No power up on that fellow.");
                }
            }
            
            jewels.ForEach(Destroy);

            yield return new WaitForEndOfFrame();
            
            var isLevelFinished = !DoesGridContainTilesToRemove();
            if (isLevelFinished)
            {
                // load the level finished screen
                SceneManager.LoadScene("_Game/Scenes/LevelFinished");
            }    
            
            RepositionGrid();
            IsGameOver();
        }

        /// <summary>
        ///     Repositions the grid after tiles have been removed. 
        /// </summary>
        private void RepositionGrid()
        {
            var tiles = GetAllTilesInGrid();

            // For all children, move them down
            foreach (var tile in tiles)
            {
                // unless they are already in the bottom row.
                if (tile.name.EndsWith('0'))
                {
                    Debug.Log("Don't need ot reposition the bottom row.");
                    continue;
                }

                var positionInGrid = ParseNameIntoVector2Int(tile.name);

                // If a tile is at (0;2), it can have two tiles below. If they disappear that's how far the tile needs to fall.
                var maximumThatTheTileCanMove = positionInGrid.y;
                
                // If we start from one we will not evaluate the tile that needs to move
                for (var i = 1; i <= maximumThatTheTileCanMove; i++)
                {
                    // E.g.: for a tile that sits at (0;2) this will be (0;1), and (0;0)
                    var positionOfTheTileBelow = new Vector2Int(positionInGrid.x, positionInGrid.y - i);
                    var nameOfTheTileBelow = ParseVector2IntIntoNameString(positionOfTheTileBelow);

                    var tileBelow = GameObject.Find(nameOfTheTileBelow);

                    if (tileBelow != null)
                    {
                        // Since we are going from top to bottom (on tiles below the tile being evaluated), this can break immediately
                        break;
                    }
                    
                    try
                    {
                        var powerUp = tile.GetComponent<PowerUpSlot>().PowerUp;
                        if (powerUp == PowerUps.Fragile)
                        {
                            MarkJewelAsFragile(tile);
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("No power up on that fellow.");
                    }
                    
                    Debug.Log($"If fragile, shouldn't get here: {tile.name}");

                    // Fall down
                    tile.name = nameOfTheTileBelow;
                    tile.transform.localPosition = GetLocalPositionForGridCoordinate(positionOfTheTileBelow);
                }
            }
            
        }

        private void MarkJewelAsFragile(GameObject fragileJewel)
        {
            try
            {
                var powerUp = fragileJewel.GetComponent<PowerUpSlot>().PowerUp;
                if (powerUp == PowerUps.Fragile)
                {
                    fragileJewel.name = "fragile";
                    var fragileContainer = GameObject.Find(("Fragile"));
                    fragileJewel.transform.parent = fragileContainer.transform;
                    
                    fragileJewel.GetComponent<BoxCollider2D>().enabled = false;
                    fragileJewel.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    fragileJewel.GetComponent<SpriteRenderer>().enabled = false;
                    fragileJewel.tag = "Untagged";
                    
                    
                    StartCoroutine(RemoveFragile(fragileJewel, fragileContainer));
                }
            }
            catch (Exception e)
            {
                Debug.Log("No power up on that fellow.");
            }
        }
        
        
        private IEnumerator RemoveFragile(GameObject fragileJewel, GameObject fragileContainer)
        {
            var fragileJewels = new List<GameObject>();
            
            foreach (Transform child in fragileContainer.transform)
            {
                fragileJewels.Add(child.gameObject);
            }
            
            fragileJewels.ForEach(Destroy);
            
            yield return new WaitForEndOfFrame();
        }
        
        private List<GameObject> GetAllTilesInGrid()
        {
            var tiles = new List<GameObject>();
            
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Tile"))
                {
                    tiles.Add(child.gameObject);
                }
            }

            return tiles.OrderBy(t => t.name).ToList();
        }
        
        private bool AreThereAnyMatchingSets()
        {
            var tiles = GetAllTilesInGrid();

            foreach (var tile in tiles)
            {
                var jewelToMatchPosition = ParseNameIntoVector2Int(tile.name);

                var result = MatchingEvaluationHelper.GetMatchingJewels(jewelToMatchPosition);

                if (result.Count != 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
