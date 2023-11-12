using Common.Utils;
using UnityEngine;

namespace Game.Tiles
{
    /// <summary>
    ///     This component enables a tile to fall in a grid in response to an elimination event.
    ///     During the falling phase the tile casts a ray of length one below itself to determine when to stops falling.
    ///     The player is not allowed to pick up a tile during that time.
    ///     Falling is enabled/disabled using the "Gravity scale" property on the Rigidbody2D.
    /// </summary>
    public class Falling : MonoBehaviour
    {
        private ILogger<Falling> _logger;

        private Rigidbody2D _rigidbody2D;

        public bool canFall = false;
        
        private void Awake()
        {
            _logger = new Logger<Falling>(gameObject);
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            var l = LayerMask.LayerToName(gameObject.layer);
            
            _logger.Log("Layer mask: " + l);
        }

        private void Update()
        {
            Debug.DrawRay(transform.position,Vector2.down, Color.red);
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
                
                
                placedOfPickedUpComponent.initialPosition = transform.position;
                canFall = false;
                _rigidbody2D.gravityScale = 0f;
            }
            
            
            if (!canFall)
            {
                return;
            }
            
            _logger.Log("Falling down");
            _rigidbody2D.gravityScale = 1f;
            
            placedOfPickedUpComponent.initialPosition = transform.position;
        }
    }
}
