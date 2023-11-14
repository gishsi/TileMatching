using System;
using Common.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Game.Grid
{
    /// <summary>
    ///     This class represents the system that manages all tiles.
    /// </summary>
    public class GridSystem : MonoBehaviour
    {
        private ILogger<GridSystem> _logger;
        
        [SerializeField] private GameObject jewel;
        [SerializeField] private GameObject blocker;


        private const int MaxRows = 6;
        private const int MaxColumns = 4;
        
        public int rows = MaxRows;
        public int cols = MaxColumns;

        
        private readonly Color _greenTileColor = new (0, 0.4235294f, 0.3098039f);
        private readonly Color _blueTileColor = new (0, 0.5381241f, 1);
        
        
        private void Awake()
        {
            _logger = new Logger<GridSystem>(gameObject);
            
            if (rows > MaxRows || cols > MaxColumns)
            {
                throw new Exception($"Rows cannot exceed {MaxRows}, columns cannot exceed {MaxColumns}");
            }
        }

        private void Start()
        {
            SetupGridBottom();
            SpawnTiles();
        }

        private void SetupGridBottom()
        {
            var gridBottom = GameObject.Find("GridBottom");

            if (gridBottom == null)
            {
                throw new Exception("A grid bottom must be present as a child to the GridSystem.");
            }
            
            // Every tile has a 0.4f margin to the right. Three tiles will add 1.2f margin, which is pretty close to an additional tile.
            // Therefore for every triple of tiles we add an additional tile width to the scale of the grid bottom.
            var offset = rows / 3;
            
            // The anchor of the grid bottom is the middle, that's why we only shift the position by half
            gridBottom.transform.position = transform.position + new Vector3((rows + offset) / 2f, -0.5f, 0);
            gridBottom.transform.localScale = new Vector3(rows + offset, 1, 1);
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

                    const float spacing = 1.4f;
                    // This is so that the origin of the new sprite is aligned to the origin of parent
                    const float marginTopLeft = 0.5f;

                    var xDisplacement = x * spacing + marginTopLeft;
                    var yDisplacement = y * spacing + marginTopLeft;
                    var objectToSpawn = rand > .2f ? jewel : blocker;
                    
                    var newJewel = Instantiate(objectToSpawn, transform.position + new Vector3(xDisplacement, yDisplacement, 0), Quaternion.identity);
                    // So that Jewels are spawned under the GridSystem gameObject in the hierarchy
                    newJewel.transform.parent = transform;
                }
            }
        }

        /// <summary>
        ///     Listens for an event broadcast produced by a clicked tile and evaluates the matching rules for it.
        /// </summary>
        private void EvaluateGrid(Tile tile)
        {
            
        }
    }
}
