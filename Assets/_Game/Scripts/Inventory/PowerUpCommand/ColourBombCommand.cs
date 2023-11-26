using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Inventory.PowerUpCommand
{
    /// <summary>
    ///      A jewel with the colour bomb power up attached will cause all the jewels of the same type to be destroyed.
    /// </summary>
    public class ColourBombCommand : IPowerUpCommand<List<GameObject>>
    {
        private readonly List<GameObject> _allTiles;
        private readonly GameObject _matchingJewel;
        
        public ColourBombCommand(GameObject matchingJewel, List<GameObject> allTiles)
        {
            _matchingJewel = matchingJewel;
            _allTiles = allTiles;
        }

        public List<GameObject> Execute()
        {
            var jewelsRemovedFromPowerUp = new List<GameObject>();
            
            foreach (var tile in _allTiles)
            {
                if (tile.GetComponent<SpriteRenderer>().color ==
                    _matchingJewel.GetComponent<SpriteRenderer>().color)
                {
                    jewelsRemovedFromPowerUp.Add(tile);
                }
            }

            return jewelsRemovedFromPowerUp;
        }
    }
}