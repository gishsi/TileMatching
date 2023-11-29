using System;
using UnityEngine;

namespace _Game.Scripts
{
    [CreateAssetMenu(fileName = "Tile Types", menuName = "Tile types")]
    public class TileTypesConfigurationScriptableObject : ScriptableObject
    {
        [Header("Tiles")]
        public GameObject jewelPrefab;
        public GameObject blockerPrefab;
        public GameObject sandPrefab;
        
        [Header("Jewel alternatives")]
        public Color blueJewelColour = new (0, 0.6235294f, 0.7176471f);
        public Color greenJewelColour = new (0.6235294f, 0.827451f, 0.3372549f);
        public Color redJewelColour = new (0.6196079f, 0.1647059f, 0.1686275f);
        public Color pinkJewelColour = new (1f, 0.4f, 0.7019608f);
        
        /// <summary>
        ///     ScriptableObjects that hold data require this to not unload between the scenes.
        /// </summary>
        private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
        
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
                case Tiles.BlueJewel:
                    objectToSpawn = jewelPrefab;
                    objectToSpawn.GetComponent<SpriteRenderer>().color = blueJewelColour;
                    break;
                case Tiles.GreenJewel:
                    objectToSpawn = jewelPrefab;
                    objectToSpawn.GetComponent<SpriteRenderer>().color = greenJewelColour;
                    break;
                case Tiles.PinkJewel:
                    objectToSpawn = jewelPrefab;
                    objectToSpawn.GetComponent<SpriteRenderer>().color = pinkJewelColour;
                    break;
                case Tiles.RedJewel:
                    objectToSpawn = jewelPrefab;
                    objectToSpawn.GetComponent<SpriteRenderer>().color = redJewelColour;
                    break;
                case Tiles.Blocker:
                    objectToSpawn = blockerPrefab;
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