using _Game.Scripts.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Inventory
{
    /// <summary>
    ///     Receives an item dropped on a tile.
    /// </summary>
    public class ReceiveItem : MonoBehaviour
    {
        [SerializeField]
        private PickedUpPowerUpScriptableObject pickedUpPowerUpScriptableObject;
        
        private void OnEnable()
        {
            pickedUpPowerUpScriptableObject.ItemDroppedEvent.AddListener(ReceivedItem);
        }
        
        private void OnDisable()
        {
            pickedUpPowerUpScriptableObject.ItemDroppedEvent.RemoveListener(ReceivedItem);
        }
        
        private void ReceivedItem(PowerUpDropped data)
        {
            if (data.NameOfTile != gameObject.name)
            {
                return;
            }
            
            Debug.Log($"Tile {gameObject.name} received {data.PowerUp} power up.");
            
            GetComponent<PowerUpSlot>().PowerUp = data.PowerUp;
        }
    }
}