using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using _Game.Scripts.Events;
using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

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
        
        private const int MaxRows = 6;
        private const int MaxColumns = 4;

        private readonly Color _blueTileColor = new (0, 0.5381241f, 1);
        private readonly Color _greenTileColor = new (0, 0.4235294f, 0.3098039f);

        // *********** Serializable *********** //
        [Header("Tiles")]
        [SerializeField] private GameObject jewel;
        [SerializeField] private GameObject blocker;

        [Header("Grid")]
        [Range(0, MaxRows)]
        public int rows = MaxRows;

        [Range(0, MaxColumns)]
        public int cols = MaxColumns;
        
        // **************** MonoBehaviour **************
        private void Awake()
        {
            _logger = new Logger<GridSystem>(gameObject);
        }

        private void Start()
        {
            updateSwap.Reset();
            SetPositionOfGridBasedOnAmountOfColsAndRows();
            SpawnTiles();
            
            EvaluateGrid(new Evaluate(new GameObject("0;2")));
        }
        
        private void OnEnable()
        {
            // Subscribing to OnEvaluate first will trigger this method first if both get triggered
            evaluateTrigger.EvaluateTriggerEvent.AddListener(EvaluateGrid);
            swipeTrigger.SwipeTriggerEvent.AddListener(SwitchPlaces);
            
        }
        
        private void OnDisable()
        {
            evaluateTrigger.EvaluateTriggerEvent.RemoveListener(EvaluateGrid);
            swipeTrigger.SwipeTriggerEvent.RemoveListener(SwitchPlaces);
        }

        // **************** Private **************

        /// <summary>
        ///     Listens for an event broadcast produced by a clicked tile. If there was an event it evaluates all matching zones, removes matched jewels, and restructures the grid.
        /// </summary>
        private static void EvaluateGrid(Evaluate data)
        {
            var matchingJewels = MatchingEvaluationHelper.GetMatchingJewels(ParseNameIntoVector2Int(data.Tile.name));

            if (matchingJewels.Count == 0)
            {
                Debug.Log("Tile does not meet matching criteria.");
            }
            
            matchingJewels.ForEach(Destroy);
            
            // todo: Restructure the grid
        }
        
        /// <summary>
        ///     After the player performs a Swipe action the grid system will evaluate whether that swipe will result in tiles being swiped by comparing the next tile in the direction of the swipe.
        /// </summary>
        /// <param name="swipe">The name of the tile corresponds to its position in the grid, e.g. [1, 1] and the direction of the swipe</param>
        private void SwitchPlaces(Swipe swipe)
        {
            Debug.Log("Swipe starting at [" + swipe.TileName + "]. In the direction of [" + swipe.DirectionOfTheSwipe + "]");
            
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
                
                // Debug.Log("Comparing origin's colour [" + originSpriteColour + "] with target's colour [" + targetSpriteColour + "]");

                // todo: Is this really a good idea? What if we add more rules, e.g. blockers?
                if (originSpriteColour == targetSpriteColour || targetTile.GetComponent<Actionable>() == null)
                {
                    Debug.Log("Swap not valid. Colours are different or the tile is missing the actionable component.");
                    
                    return;
                }
                
                // Position of the first one is (0.5f, 0.5f)
                // if we swapped a tile at 0, 0 with a tile at 1, 0 the position would be 0.5f, 0.5f, and (1 * 1.4f) + 0.5f = 1.9f, 0.5f
                
                // Get new positions
                var newPositionForTarget = GetLocalPositionForGridCoordinate(origin);
                // Debug.Log("New position for target" + newPositionForTarget);

                var newPositionForOrigin = GetLocalPositionForGridCoordinate(target);
                // Debug.Log("New position for origin" + newPositionForOrigin);
                
                // Debug.Log("Old position of target: " + targetTile.transform.localPosition);
                // Debug.Log("Old position of origin: " + originTile.transform.localPosition);
                
                targetTile.transform.localPosition = newPositionForTarget;
                targetTile.name = swipe.TileName;
                
                originTile.transform.localPosition = newPositionForOrigin;
                originTile.name = ParseVector2IntIntoNameString(new Vector2Int((int)target.x, (int)target.y));
                
                Debug.Log("Successful swap between [" + targetTile.name + "] and [" + originTile.name + "]");
                
                updateSwap.RaiseEvent(1);
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not swap the tiles:\n{e}");
            }
        }
        
        private void SetPositionOfGridBasedOnAmountOfColsAndRows()
        {
            // needs some spacing: for every tile that is not last add .4f, if last add .2f
            var halfOfRows = rows / 2f;
            var fullMarginRows = (halfOfRows - 1) * SpacingBetweenTiles;
            var halfOfCols = cols / 2f;
            var fullMarginCols = (halfOfCols - 1) * SpacingBetweenTiles;
            
            transform.position = new Vector3((rows / 2f + fullMarginRows + (SpacingBetweenTiles / 2f)) * -1, (cols / 2f + fullMarginCols + (SpacingBetweenTiles / 2f)) * -1, 0);
        }
        
        /// <summary>
        ///     Spawns the tiles in the grid.
        /// </summary>
        /// <remarks>
        ///     Grid goes from the top-bottom corner - all items will be aligned with the grid bottom.
        /// </remarks>
        private void SpawnTiles()
        {
            for(var x = 0; x < rows; x++)
            {
                for (var y = 0; y < cols; y++)
                {
                    var rand = Random.value;
                    var color = rand > .5f ? _greenTileColor : _blueTileColor;
                
                    jewel.GetComponent<SpriteRenderer>().color = color;
                    
                    var xDisplacement = x * TileSpawnDisplacement + MarginTopLeftOfTilesInGrid;
                    var yDisplacement = y * TileSpawnDisplacement + MarginTopLeftOfTilesInGrid;
                    var objectToSpawn = rand > .0f ? jewel : blocker;
                    
                    var newJewel = Instantiate(objectToSpawn, transform.position + new Vector3(xDisplacement, yDisplacement, 0), Quaternion.identity);
                    newJewel.name = ParseVector2IntIntoNameString(new Vector2Int(x, y));
                    
                    // So that Jewels are spawned under the GridSystem gameObject in the hierarchy
                    newJewel.transform.parent = transform;
                }
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
    }
}
