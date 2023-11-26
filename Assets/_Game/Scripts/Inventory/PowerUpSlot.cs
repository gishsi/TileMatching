using UnityEngine;

namespace _Game.Scripts.Inventory
{
    /// <summary>
    ///  This probably should no be attached from the beginning,
    ///     but rather after a jewel has had a power up dropped on it
    /// </summary>
    public class PowerUpSlot : MonoBehaviour
    {
        public PowerUps PowerUp = PowerUps.None;
    }
}