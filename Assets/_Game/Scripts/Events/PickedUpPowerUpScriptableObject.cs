using System;
using _Game.Scripts.Events;
using _Game.Scripts.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Events
{
    /// <summary>
    ///     Used to store data about which item has been picked up.
    ///     If there were items other than power ups we could make the field "PowerUps" generic.
    /// </summary>
    [CreateAssetMenu(fileName = "PickedUpItem", menuName = "Inventory/PickedUpItem")]
    public class PickedUpPowerUpScriptableObject : ScriptableObject, IRaiseEvent<PowerUpDropped>
    {
        [NonSerialized] 
        public UnityEvent<PowerUpDropped> ItemDroppedEvent;

        public void OnEnable()
        {
            ItemDroppedEvent = new UnityEvent<PowerUpDropped>();
        }

        public void RaiseEvent(PowerUpDropped data)
        {
            ItemDroppedEvent.Invoke(data);
        }
    }
}
