using System;
using UnityEngine;
using UnityEngine.Events;

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit
    {

    }
}

namespace _Game.Scripts.Events
{
    public record Swipe(string TileName, Vector2 DirectionOfTheSwipe);
    
    /// <summary>
    ///     
    /// </summary>
    [CreateAssetMenu(fileName = "SwipeTriggerScriptableObject", menuName = "Events/Trigger swipe")]
    public class SwipeTriggerScriptableObject : ScriptableObject, IRaiseEvent<Swipe>
    {
        [NonSerialized] 
        public UnityEvent<Swipe> SwipeTriggerEvent;
        
        public void OnEnable()
        {
            SwipeTriggerEvent = new UnityEvent<Swipe>();
        }

        public void RaiseEvent(Swipe data)
        {
            Debug.Log("[" + nameof(SwipeTriggerScriptableObject) + "] Swipe");
            Debug.Log("[" + nameof(data) + "]" + data.TileName + ", " + data.DirectionOfTheSwipe);
            
            SwipeTriggerEvent.Invoke(data);
        }
    }
}