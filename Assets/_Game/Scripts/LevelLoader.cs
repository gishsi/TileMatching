using System;
using _Game.Scripts.Inventory;
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
        private PowerUpSpritesResolver powerUpSpritesResolver;
       
        
        private void Awake()
        {
            var levelToPlay = levelDataScriptableObject.GetCurrentLevel();

            InitializeInventory(levelToPlay);
            
            var gridSystem = transform.Find("GridSystem").GetComponent<GridSystem>();
            gridSystem.SetPositionOfGridBasedOnAmountOfColsAndRows(levelToPlay.rowsAmount, levelToPlay.colsAmount);
            gridSystem.InitializeGrid(levelToPlay.rows);
        }

        
        /// <summary>
        ///     Initialize the inventory with the appropriate items. 
        /// </summary>
        /// <param name="levelToPlay">Level to play</param>
        /// <exception cref="Exception">Thrown if there was a critical failure and the inventory item could not be instantiated.</exception>
        private void InitializeInventory(LevelScriptableObject levelToPlay)
        {
            var inventoryContainer = GameObject.Find("InventoryContainer");
            
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

                   item.GetComponent<Image>().sprite = powerUpSpritesResolver.GetSpriteForPowerUpType(powerUp);
                
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
