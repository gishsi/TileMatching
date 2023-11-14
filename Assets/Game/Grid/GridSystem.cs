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


        private const int MaxRows = 7;
        private const int MaxColumns = 4;
        
        public int rows = MaxRows;
        public int cols = MaxColumns;

        
        private readonly Color _greenTileColor = new (0, 0.4235294f, 0.3098039f);
        private readonly Color _blueTileColor = new (0, 0.5381241f, 1);

        private void Awake()
        {
            if (rows > MaxRows || cols > MaxColumns)
            {
                throw new Exception($"Rows cannot exceed {MaxRows}, columns cannot exceed {MaxColumns}");
            }
            
            _logger = new Logger<GridSystem>(gameObject);
            SpawnTiles();
        }

        /// <summary>
        ///     Spawns the tiles in the grid.
        /// </summary>
        /// <remarks>
        ///     Grid goes from the top-bottom corner - all items will be aligned with the grid bottom.
        /// </remarks>
        private void SpawnTiles()
        {
            for(var x = 0; x < 7; x++)
            {
                for (var y = 0; y < 2; y++)
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
