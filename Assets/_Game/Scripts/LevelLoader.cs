using System;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    [Serializable]
    class PowerUpSprites
    {
        [Tooltip("None power up sprite")]
        [SerializeField]    
        private Sprite noneSpriteAsset;
        
        [Tooltip("Bomb power up sprite")]
        [SerializeField]    
        private Sprite bombSpriteAsset;
        
        [Tooltip("Concretion power up sprite")]
        [SerializeField]    
        private Sprite concretionSpriteAsset;
        
        [Tooltip("Fragile power up sprite")]
        [SerializeField]    
        private Sprite fragileSpriteAsset;
        
        [Tooltip("Colour bomb power up sprite")]
        [SerializeField]    
        private Sprite colourBombSpriteAsset;
        
        public Sprite GetSpriteForPowerUpType(PowerUps powerUp)
        {
            return powerUp switch
            {
                PowerUps.None => noneSpriteAsset,
                PowerUps.Bomb => bombSpriteAsset,
                PowerUps.ColourBomb => colourBombSpriteAsset,
                PowerUps.Concretion => concretionSpriteAsset,
                PowerUps.Fragile => fragileSpriteAsset,
                _ => noneSpriteAsset
            };
        }
    }
    
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
        private PowerUpSprites _powerUpSprites;
       
        
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

                   item.GetComponent<Image>().sprite = _powerUpSprites.GetSpriteForPowerUpType(powerUp);
                
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
