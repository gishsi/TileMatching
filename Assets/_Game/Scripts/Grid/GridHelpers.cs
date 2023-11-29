
using System;
using UnityEngine;

namespace _Game.Scripts.Grid
{
    public static class GridHelpers
    {
        public const float TileSpawnDisplacement = 1.4f;
        public const float SpacingBetweenTiles = 0.4f;
        // This is so that the origin of the new sprite is aligned to the origin of parent
        public const float MarginTopLeftOfTilesInGrid = 0.5f;
        
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
        public static Vector2 GetLocalPositionForGridCoordinate(Vector2 gridCoordinate)
        {
            return new Vector2((TileSpawnDisplacement * gridCoordinate.x) + MarginTopLeftOfTilesInGrid, (TileSpawnDisplacement * gridCoordinate.y) + MarginTopLeftOfTilesInGrid);
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
        public static Vector2Int ParseNameIntoVector2Int(string name)
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

    }
}