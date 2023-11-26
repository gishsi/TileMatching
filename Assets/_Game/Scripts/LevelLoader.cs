using System;
using TMPro;
using UnityEngine;

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

                    var pickFromInventory = item.GetComponent<PickFromInventory>();
                    pickFromInventory.PowerUp = powerUp;
                
                
                    var textComponent = item.GetComponentInChildren<TextMeshProUGUI>();
                    textComponent.text = powerUp.ToString();
                
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
