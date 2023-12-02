using _Game.Scripts.Events;
using UnityEngine;

namespace _Game.Scripts.TileComponents
{
    /// <summary>
    ///     Adds the ability to be swiped to the tile
    /// </summary>
    public class SwipeInteractable : MonoBehaviour
    {
        private bool _hasBeenPickedUp;

        [SerializeField]
        private SwipeTriggerScriptableObject swipeTrigger;
        
        private void OnMouseDown()
        {
            _hasBeenPickedUp = true;
        }

        /// <summary>
        ///     Calculates the vector between the game object and the mouse. The normalized vector is the direction of the swipe.
        /// </summary>
        private void OnMouseUp()
        {
            // Tile must be picked up in order to be swiped
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
            var direction =Vector2Int.RoundToInt(difference.normalized);

            // Send an event to the Grid system that an evaluation should take place.
            // The name of the tile is the position in the grid.
            swipeTrigger.RaiseEvent(new Swipe(gameObject.name, direction));
            
            _hasBeenPickedUp = false;
        }
    }
}