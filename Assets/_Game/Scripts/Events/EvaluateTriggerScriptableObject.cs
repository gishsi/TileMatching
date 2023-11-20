using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Events
{
    public record Evaluate(GameObject Tile);   
    
    [CreateAssetMenu(fileName = "EvaluateTriggerScriptableObject", menuName = "Events/Trigger evaluate")]
    public class EvaluateTriggerScriptableObject : ScriptableObject, IRaiseEvent<Evaluate>
    {
        [NonSerialized] 
        public UnityEvent<Evaluate> EvaluateTriggerEvent;
        
        public void OnEnable()
        {
            EvaluateTriggerEvent = new UnityEvent<Evaluate>();
        }

        public void RaiseEvent(Evaluate data)
        {
            // Debug.Log("[" + nameof(EvaluateTriggerScriptableObject) + "] Evaluate");
            EvaluateTriggerEvent.Invoke(data);
        }
    }
}