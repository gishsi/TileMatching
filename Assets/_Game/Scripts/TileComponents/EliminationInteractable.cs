using _Game.Scripts.Events;
using UnityEngine;

namespace _Game.Scripts.TileComponents
{
    /// <summary>
    ///     Adds the ability to be eliminated from the grid
    /// </summary>
    public class EliminationInteractable : MonoBehaviour
    {
        [SerializeField]
        private EvaluateTriggerScriptableObject evaluateTrigger;
        
        private void OnMouseDown()
        {
            evaluateTrigger.RaiseEvent(new Evaluate(gameObject));
        }
    }
}