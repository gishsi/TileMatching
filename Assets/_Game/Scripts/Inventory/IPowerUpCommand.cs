using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Inventory
{
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

    public class FragileCommand : IPowerUpCommand<List<GameObject>>
    {
        private readonly GameObject _fragileJewel;

        public FragileCommand(GameObject fragileJewel)
        {
            _fragileJewel = fragileJewel;
        }

        public List<GameObject> Execute()
        {
            try
            {
                var powerUp = _fragileJewel.GetComponent<PowerUpSlot>().PowerUp;
                if (powerUp == PowerUps.Fragile)
                {
                    _fragileJewel.name = "fragile";
                    var fragileContainer = GameObject.Find(("Fragile"));
                    _fragileJewel.transform.parent = fragileContainer.transform;
                    
                    _fragileJewel.GetComponent<BoxCollider2D>().enabled = false;
                    _fragileJewel.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    _fragileJewel.GetComponent<SpriteRenderer>().enabled = false;
                    _fragileJewel.tag = "Untagged";
                    
                    var fragileJewels = new List<GameObject>();
            
                    foreach (Transform child in fragileContainer.transform)
                    {
                        fragileJewels.Add(child.gameObject);
                    }

                    return fragileJewels;
                }
            }
            catch (Exception e)
            {
                Debug.Log("No power up on that fellow.");
            }

            return new List<GameObject>();
        }
    }

    public class ConcretionCommand : IPowerUpCommand<object>
    {
        private readonly GameObject _blocker;
        private readonly GameObject _jewel;
        private readonly Transform _parentTransform;

        public ConcretionCommand(GameObject blocker, GameObject jewel, Transform parentTransform)
        {
            _blocker = blocker;
            _jewel = jewel;
            _parentTransform = parentTransform;
        }
        
        public object Execute()
        {
            var blockerToSpawn = GameObject.Instantiate(_blocker, _jewel.transform.position, Quaternion.identity) as GameObject;
            blockerToSpawn.gameObject.name = _jewel.gameObject.name;
            blockerToSpawn.transform.parent = _parentTransform;

            return null;
        }
    }
    
    public interface IPowerUpCommand<out T>
    {
        T Execute();
    }
}