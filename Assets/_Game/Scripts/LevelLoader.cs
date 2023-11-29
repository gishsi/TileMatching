using System;
using _Game.Scripts.Inventory;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Levels data")]
        public LevelDataScriptableObject levelDataScriptableObject;

        [Header("Inventory")]
        [Tooltip("Inventory item prefab")]
        [SerializeField]
        private GameObject inventoryItem;
        
        [Header("Sprites for power ups")] 
        [SerializeField]
        private PowerUpSpritesResolverScriptableObject powerUpSpritesResolverScriptableObject;
        
        [Header("Tile types data")]
        [SerializeField]
        private TileTypesConfigurationScriptableObject tileTypes;
        
        private void Awake()
        {
            var levelToPlay = levelDataScriptableObject.GetCurrentLevel();

            InitializeInventory(levelToPlay);
            
            var gridSystem = transform.Find("GridSystem").GetComponent<GridSystem>();

            var gridConfiguration = new GridConfiguration(levelToPlay, gridSystem.transform, tileTypes);
            gridSystem.GridConfiguration = gridConfiguration;
        }

        
        /// <summary>
        ///     Initialize the inventory with the appropriate items. 
        /// </summary>
        /// <param name="levelToPlay">Level to play</param>
        /// <exception cref="Exception">Thrown if there was a critical failure and the inventory item could not be instantiated.</exception>
        private void InitializeInventory(LevelScriptableObject levelToPlay)
        {
            var inventoryContainer = GameObject.Find("InventoryContainer");

            if (levelToPlay.powerUps == null)
            {
                return;
            }
            
            foreach (var powerUp in levelToPlay.powerUps)
            {
                if (powerUp == PowerUps.None)
                {
                    continue;
                }

                try
                {
                    var item = Instantiate(inventoryItem, inventoryContainer.transform.position, Quaternion.identity);

                    var pickFromInventory = item.GetComponent<InventoryPickable>();
                    pickFromInventory.PowerUp = powerUp;

                    item.GetComponent<Image>().sprite = powerUpSpritesResolverScriptableObject.GetSpriteForPowerUpType(powerUp);
            
                    item.transform.parent = inventoryContainer.transform;
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    throw new Exception("Critical failure during inventory initialization.");
                }
            }
        }
    }
}
