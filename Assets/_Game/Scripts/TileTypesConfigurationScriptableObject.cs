using System;
using UnityEngine;

namespace _Game.Scripts
{
    [CreateAssetMenu(fileName = "Tile Types", menuName = "Tile types")]
    public class TileTypesConfigurationScriptableObject : ScriptableObject
    {
        [Header("Tiles")]
        public GameObject jewel;
        public GameObject blocker;
        public GameObject sandPrefab;
        
        [Header("Jewel alternatives")]
        public Color blueJewelColour = new (0, 0.5381241f, 1);
        public Color greenJewelColour = new (0, 0.4235294f, 0.3098039f);
        
        
        /// <summary>
        ///     Resolve the type of tile into a game object
        /// </summary>
        /// <param name="tileType">Tile type</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When a non existing tile type has been passed in.</exception>
        public GameObject ResolveTileTypeIntoGameObject(Tiles tileType)
        {
            GameObject objectToSpawn;
            
            switch (tileType)
            {
                case Tiles.BlueTile:
                    objectToSpawn = jewel;
                    objectToSpawn.GetComponent<SpriteRenderer>().color = blueJewelColour;
                    break;
                case Tiles.GreenTile:
                    objectToSpawn = jewel;
                    objectToSpawn.GetComponent<SpriteRenderer>().color = greenJewelColour;
                    break;
                case Tiles.Blocker:
                    objectToSpawn = blocker;
                    break;
                case Tiles.Sand:
                    objectToSpawn = sandPrefab;
                    break;
                default:
                    throw new ArgumentException($"There is no game object representation for type [{tileType}]", nameof(tileType));
            }

            return objectToSpawn;
        }
    }
}