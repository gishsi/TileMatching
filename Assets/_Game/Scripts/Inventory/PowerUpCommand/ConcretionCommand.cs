using UnityEngine;

namespace _Game.Scripts.Inventory.PowerUpCommand
{
    /// <summary>
    ///     A jewel with the concretion power up will leave a blocker in their place after execution.
    /// </summary>
    public class ConcretionCommand : IPowerUpCommand<object>
    {
        private readonly GameObject _blocker;
        private readonly GameObject _jewel;
        private readonly Transform _parentTransform;

        public ConcretionCommand(GameObject blocker, GameObject jewel, Transform parentTransform)
        {
            _blocker = blocker;
            _jewel = jewel;
            _parentTransform = parentTransform;
        }
        
        public object Execute()
        {
            var blockerToSpawn = GameObject.Instantiate(_blocker, _jewel.transform.position, Quaternion.identity) as GameObject;
            blockerToSpawn.gameObject.name = _jewel.gameObject.name;
            blockerToSpawn.transform.parent = _parentTransform;

            return null;
        }
    }
}