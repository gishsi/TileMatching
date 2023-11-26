using UnityEngine;

namespace _Game.Scripts.Inventory
{
    /// <summary>
    ///  This probably should no be attached from the beginning,
    ///     but rather after a jewel has had a power up dropped on it
    /// </summary>
    public class PowerUpSlot : MonoBehaviour
    {
        [SerializeField] 
        private PowerUpSpritesResolverScriptableObject _powerUpSpritesResolverScriptableObject;
        
        public PowerUps PowerUp { get; private set; }= PowerUps.None;

        [SerializeField]    
        private GameObject powerUpAttachedToJewelSprite;

        private void Awake()
        {
            powerUpAttachedToJewelSprite.SetActive(false);
        }


        public void SetPowerUp(PowerUps powerUp)
        {
            if (powerUp == PowerUps.None)
            {
                return;
            }

            powerUpAttachedToJewelSprite.SetActive(true);
            
            var sprite = _powerUpSpritesResolverScriptableObject.GetSpriteForPowerUpType(powerUp);

            powerUpAttachedToJewelSprite.GetComponent<SpriteRenderer>().sprite = sprite;

            PowerUp = powerUp;
        }
    }
}