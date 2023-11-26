using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Inventory.PowerUpCommand
{
    /// <summary>
    ///     A bomb power-up extends the area that marks jewels to be deleted. The bomb power up ignores the matching criteria.
    /// </summary>
    public class BombCommand : IPowerUpCommand<List<GameObject>>
    {
        private static readonly List<Vector2Int> BombKernel = new List<Vector2Int>()
        {
            new(-1, 1), new(0, 1), new(1, 1),
            new(-1, 0), new(0, 0), new(1, 0),
            new(-1, -1), new(0, -1), new(1, -1),
        };

        private readonly GameObject _matchingJewel;
        
        public BombCommand(GameObject matchingJewel)
        {
            _matchingJewel = matchingJewel;
        }
        
        public List<GameObject> Execute()
        {
            var jewelsRemovedFromPowerUp = new List<GameObject>();
            
            foreach (var offset in BombKernel)
            {
                var gridCoordinateOfTileInMatchZone = GridSystem.ParseNameIntoVector2Int(_matchingJewel.name) + offset;
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

            return jewelsRemovedFromPowerUp;
        }
    }
}