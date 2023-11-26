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
        [FormerlySerializedAs("_pickedUpItemScriptableObject")] [SerializeField]
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

            // todo: Could attach the power up slot here and instantiate it with a type that the effect returns!
            GetComponent<PowerUpSlot>().PowerUp = data.PowerUp;
        }
    }
}