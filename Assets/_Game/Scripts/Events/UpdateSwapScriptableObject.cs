using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Events
{
    /// <summary>
    ///     This ScriptableObject is responsible for decreasing the swap amount and sending and event
    ///     out that swaps have changed. UI might listens for this event so that it can update
    ///     accordingly.
    /// </summary>
    /// <remarks>
    ///     Worth noting is that we cannot change this data at runtime.
    ///     This means that if we want to change the amount of swaps, we need to create a new ScriptableObject and assign that to the level's update swaps events (GridSystem, and SwapsUI scripts)
    ///     This provides a type of workflow that is easier for artists and game designers.
    /// </remarks>
    [CreateAssetMenu(fileName = "UpdateSwapScriptableObject", menuName = "Events/Update Swap")]
    public class UpdateSwapScriptableObject : ScriptableObject, IRaiseEvent<int>
    {
        [SerializeField] private LevelDataScriptableObject _levelDataScriptableObject;
        
        [NonSerialized] 
        public UnityEvent<int> SwapsChangeEvent;
        
        [NonSerialized] 
        public UnityEvent SwapsBelowZeroEvent;
        
        [NonSerialized]
        public int Swaps = 5;
        
        public void OnEnable()
        {
            Swaps = _levelDataScriptableObject.GetAmountOfSwipesOfCurrentLevel();
            SwapsChangeEvent ??= new UnityEvent<int>();
            SwapsBelowZeroEvent = new UnityEvent();
        }

        /// <summary>
        ///     Invokes events related to the swap amount.
        /// </summary>
        /// <param name="data">Amount to decrease the swaps by</param>
        public void RaiseEvent(int data)
        {
            Swaps -= data;

            SwapsChangeEvent.Invoke(Swaps);
        }
        
        /// <summary>
        ///     ScriptableObjects persists data even when we are switching scenes.
        ///     This method restarts the data as is called by the trigger.
        /// </summary>
        public void Reset()
        {
            Swaps = _levelDataScriptableObject.GetAmountOfSwipesOfCurrentLevel();
            SwapsChangeEvent.Invoke(Swaps);
        }
    }
}