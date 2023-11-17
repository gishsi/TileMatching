using JetBrains.Annotations;
using UnityEngine;

namespace _Game.Scripts.Utils
{
    public class Logger<T> : ILogger<T>
    {
        [CanBeNull] private readonly GameObject _gameObject;

        public Logger([CanBeNull] GameObject gameObject = null)
        {
            _gameObject = gameObject;
        }
    
        public void Log(string message)
        {
            if (_gameObject is not null)
            {
                Debug.Log("[" + typeof(T) + "/" + _gameObject.name + "]" + ": " + message);
                return;
            }
        
            Debug.Log("[" + typeof(T) + "]" + ": " + message);
        }
    }
}

