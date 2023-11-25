using UnityEngine;

namespace _Game.Scripts
{
    /// <summary>
    ///     Receives an item dropped on a tile.
    /// </summary>
    public class ReceiveItem : MonoBehaviour
    {
        [SerializeField]
        private PickedUpItemScriptableObject _pickedUpItemScriptableObject;
        
        private void OnEnable()
        {
            _pickedUpItemScriptableObject.ItemDroppedEvent.AddListener(ReceivedItem);
        }
        
        private void OnDisable()
        {
            _pickedUpItemScriptableObject.ItemDroppedEvent.RemoveListener(ReceivedItem);
        }
        
        private void ReceivedItem(ItemDropped data)
        {
            if (data.NameOfTile != gameObject.name)
            {
                return;
            }
            
            Debug.Log($"Tile {gameObject.name} received {data.PowerUp} power up.");
        }
    }
}