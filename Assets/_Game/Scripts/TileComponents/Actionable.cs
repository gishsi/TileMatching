﻿using _Game.Scripts.Events;
using UnityEngine;

namespace _Game.Scripts.TileComponents
{
    /// <summary>
    ///     This components makes it possible to pick up a tile, perform evaluation and switching actions.
    /// </summary>
    public class Actionable : MonoBehaviour
    {
        // *********** Private variables *********** //
        private bool _hasBeenPickedUp;
        
        // *********** Events *********** //
        [SerializeField]
        private EvaluateTriggerScriptableObject evaluateTrigger;
        
        [SerializeField]
        private SwipeTriggerScriptableObject swipeTrigger;
        
        // *********** Mouse events *********** //
        private void OnMouseDown()
        {
            _hasBeenPickedUp = true;
     
            evaluateTrigger.RaiseEvent(new Evaluate(gameObject));
        }

        private void OnMouseUp()
        {
            if (!_hasBeenPickedUp)
            {
                return;
            }
            
            var mousePosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var difference = (mousePosition - (Vector2)transform.position);
            var magnitude = difference.magnitude;
            
            // To prevent swiping with minimal movement, 0.9f feels appropriate during gameplay
            if (magnitude < 0.9f)
            {
                _hasBeenPickedUp = false;
                return;
            }
            
            // Vector2Int wil result in a vector whose values are only 1, 0, or -1
            var direction = Vector2Int.RoundToInt(difference.normalized);
            
            // Send an event to the Grid system that an evaluation should take place.
            // The name of the tile is the position in the grid.
            swipeTrigger.RaiseEvent(new Swipe(gameObject.name, direction));
            
            _hasBeenPickedUp = false;
        }
    }
}