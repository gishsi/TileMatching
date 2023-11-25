using System;
using _Game.Scripts.Events;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts
{
    [Serializable]
    public enum PowerUps
    {
        None,
        Bomb,
        ColouredBomb,
        Concretion,
        Fragile
    }

    public class ItemDropped
    {
        public string NameOfTile;
        public PowerUps PowerUp;
        
        public ItemDropped(string nameOfTile, PowerUps powerUp)
        {
            NameOfTile = nameOfTile;
            PowerUp = powerUp;
        }
    }

    /// <summary>
    ///     Used to store data about which item has been picked up.
    ///     If there were items other than power ups we could make the field "PowerUps" generic.
    /// </summary>
    [CreateAssetMenu(fileName = "PickedUpItem", menuName = "Inventory/PickedUpItem")]
    public class PickedUpItemScriptableObject : ScriptableObject, IRaiseEvent<ItemDropped>
    {
        [NonSerialized] 
        public UnityEvent<ItemDropped> ItemDroppedEvent;

        public void OnEnable()
        {
            ItemDroppedEvent = new UnityEvent<ItemDropped>();
        }

        public void RaiseEvent(ItemDropped data)
        {
            ItemDroppedEvent.Invoke(data);
        }
    }
}
