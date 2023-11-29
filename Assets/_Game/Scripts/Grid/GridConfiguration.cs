using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Game.Scripts.Grid
{
    public class GridConfiguration
    {
        public readonly TileTypesConfigurationScriptableObject TileTypes;
        public readonly LevelScriptableObject LevelData;
        public readonly Transform ParentTransform;

        public GridConfiguration(LevelScriptableObject levelData, Transform transform, TileTypesConfigurationScriptableObject tileTypes)
        {
            LevelData = levelData;
            ParentTransform = transform;
            TileTypes = tileTypes;
        }
        
        
         /// <summary>
        ///     Initialize the grid with the levels data 
        /// </summary>
        public void InitializeGrid()
        {
            // Amount of rows is columns
            var columnsCount = LevelData.rows.Count();
            // We have a number of rows, the length of the array of the rows represents how many columns we have
            for (var y = 0; y < columnsCount; y++)
            {
                var numberOfTilesInRow = LevelData.rows[y].tilesInRow.Count();
                
                for (var x = 0; x < numberOfTilesInRow; x++)
                {
                    var tileType = LevelData.rows[y].tilesInRow[x];

                    var objectToSpawn = TileTypes.ResolveTileTypeIntoGameObject(tileType);
                    
                    var xDisplacement = x * GridHelpers.TileSpawnDisplacement + GridHelpers.MarginTopLeftOfTilesInGrid;
                    var yDisplacement = y * GridHelpers.TileSpawnDisplacement + GridHelpers.MarginTopLeftOfTilesInGrid;
                    
                    var newJewel = Object.Instantiate(objectToSpawn, ParentTransform.position + new Vector3(xDisplacement, yDisplacement, 0), Quaternion.identity);
                    newJewel.name = GridHelpers.ParseVector2IntIntoNameString(new Vector2Int(x, y));
                    
                    // So that Jewels are spawned under the GridSystem gameObject in the hierarchy
                    newJewel.transform.parent = ParentTransform;
                }
            }
        }
        
        public void SetPositionOfGridBasedOnAmountOfColsAndRows()
        {
            // Needs spacing. For every tile, except the last one add .4f margin to right, if last add .2f,
            //      so that the grid is exactly in the middle, no matter how many tiles it has.
            var halfOfRows = LevelData.rowsAmount / 2f;
            var fullMarginRows = (halfOfRows - 1) * GridHelpers.SpacingBetweenTiles;
            var halfOfCols = LevelData.colsAmount / 2f;
            var fullMarginCols = (halfOfCols - 1) * GridHelpers.SpacingBetweenTiles;

            var verticalOffset = (LevelData.rowsAmount / 2f + (fullMarginRows + (GridHelpers.SpacingBetweenTiles / 2f))) * -1;
            var horizontalOffset = (LevelData.colsAmount  / 2f + (fullMarginCols + (GridHelpers.SpacingBetweenTiles / 2f))) * -1;

            ParentTransform.position = new Vector3(horizontalOffset, verticalOffset, 0);
            
            var background = GameObject.Find("GridBackground");
            background.transform.localScale = new Vector3(
                LevelData.colsAmount  + (float)Math.Floor(halfOfCols) + GridHelpers.SpacingBetweenTiles, 
                LevelData.rowsAmount + (float)Math.Floor(halfOfRows)+ GridHelpers.SpacingBetweenTiles, 
                1);
        }
    }
}