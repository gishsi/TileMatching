using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Events
{
    [CreateAssetMenu(fileName = "EvaluateTriggerScriptableObject", menuName = "Events/Trigger evaluate")]
    public class EvaluateTriggerScriptableObject : ScriptableObject, IRaiseEvent
    {
        [NonSerialized] 
        public UnityEvent EvaluateTriggerEvent;
        
        public void OnEnable()
        {
            EvaluateTriggerEvent = new UnityEvent();
        }

        public void RaiseEvent()
        {
            Debug.Log("[" + nameof(EvaluateTriggerScriptableObject) + "] Evaluate");
            EvaluateTriggerEvent.Invoke();
        }
    }
}