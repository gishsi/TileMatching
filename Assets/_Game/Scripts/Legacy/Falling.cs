using System;
using _Game.Scripts.Utils;
using UnityEngine;

namespace _Game.Scripts.Legacy
{
    /// <summary>
    ///     This component enables a tile to fall in a grid in response to an elimination event.
    ///     During the falling phase the tile casts a ray of length one below itself to determine when to stops falling.
    ///     The player is not allowed to pick up a tile during that time.
    ///     Falling is enabled/disabled using the "Gravity scale" property on the Rigidbody2D.
    /// </summary>
    /// <remarks>
    ///     This approach resulted in a lot of edge cases that were growing as I was adding features.
    ///     The final solution I created for my grid is a lot simpler and easier to work with because
    ///     of the mathematical constraints put upon it.
    ///     However, it is worth mentioning that I think it was worth looking at this method as well.
    /// </remarks>
    [Obsolete]
    public class Falling : MonoBehaviour
    {
        private ILogger<Falling> _logger;

        private Rigidbody2D _rigidbody2D;

        public bool canFall = false;
        
        private void Awake()
        {
            _logger = new Logger<Falling>(gameObject);
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // TODO: ONLY TEMPORARY, HAVE THIS SUBSCRIBE TO THE MATCHING EVENT (GRID EVALUATION, WHATEVER)
        private void OnEnable()
        {
            SwitchPlacesOfPickedUp.OnSwitch += Fall;
            SwitchPlacesOfPickedUp.OnPickedUp += Fall;
        }

        private void OnDisable()
        {
            SwitchPlacesOfPickedUp.OnSwitch -= Fall;
            // To disable falling again
            SwitchPlacesOfPickedUp.OnPickedUp -= Fall;
        }

        private void Fall()
        {
            var placedOfPickedUpComponent = GetComponent<SwitchPlacesOfPickedUp>();
            // The first hit is the collider of the tile that has the Falling component attached, the second one
            // is the tile below or the end of the grid. 
            var results = new RaycastHit2D[2];
            var size = Physics2D.RaycastNonAlloc(transform.position, Vector2.down, results, 1);
            
            for (var i = 0; i < size; i++)
            {
                // todo: consider putting this code somewhere top-level (it's only a couple of lines, but might be worth)
                var isItself = results[i].collider.gameObject == gameObject;
            
                // This means there are no tiles (blockers, other tiles) so we can disable the freeze position constraint
                if (results[i].collider is null || isItself)
                {
                    canFall = true;
                    continue;
                }

                // There is a block below the tile, disable falling
                canFall = false;
                _rigidbody2D.gravityScale = 0f;
            }
            
            
            if (!canFall)
            {
                placedOfPickedUpComponent.initialPosition = transform.position;
                return;
            }
            
            _logger.Log("Falling down");
            _rigidbody2D.gravityScale = 1f;
        }

        private void Update()
        {
            Debug.DrawRay(transform.position,Vector2.down, Color.red);
        }
    }
}
