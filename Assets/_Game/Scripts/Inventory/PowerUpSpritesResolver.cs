using System;
using UnityEngine;

namespace _Game.Scripts.Inventory
{
    /// <summary>
    ///     Stores information about sprites for power ups and resolves a power up type into a sprite for the UI to render.
    /// </summary>
    [Serializable]
    class PowerUpSpritesResolver
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
}