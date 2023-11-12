using Common.Utils;
using UnityEngine;

namespace Game.Tiles
{
    public class SwitchPlacesOfPickedUp : MonoBehaviour
    {
        private ILogger<SwitchPlacesOfPickedUp> _logger;
        
        private bool _dragging = false;
        private bool _hasBeenPickedUp = false;
    
        public Vector3 initialPosition;
        [SerializeField] private string targetObjectTag = "Tile";
        
        public delegate void SwitchPlacesAction();
        public static event SwitchPlacesAction OnSwitch;
        
        private void Awake()
        {
            _logger = new Logger<SwitchPlacesOfPickedUp>(gameObject);
            initialPosition = transform.position; // todo: use local position instead
        }

        private void Update()
        {
            if (!_dragging)
            {
                return;
            }
        
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
            // z value returned by the Input.mouse position will be that of a camera (-10),
            // need to overwrite to prevent the sprite from disappearing
            mousePosition.z = 0;
        
            transform.position = mousePosition;
        }

        private void OnMouseDown()
        {
            _hasBeenPickedUp = true;
            _dragging = true;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void OnMouseUp()
        {
            if (!_hasBeenPickedUp)
            {
                return;
            }
        
            _dragging = false;
        
            var allHits = Physics2D.RaycastAll(transform.position, -Vector2.up);

            // Small trick: we need this to make sure that the tile doesn't change position twice during one swipe
            var swapped = false;
            
            foreach (var raycastHit2D in allHits)
            {
                if (swapped)
                {
                    break;
                }
                var isItself = raycastHit2D.collider.gameObject == gameObject;
                var isHitAJewel = raycastHit2D.collider.gameObject.CompareTag(targetObjectTag);
                
                // todo: from colour matching this should compare sprites
                var colourOfPickedUp = GetComponent<SpriteRenderer>().color;
                var colourOfDroppedOn = raycastHit2D.collider.gameObject.GetComponent<SpriteRenderer>().color;
                var areColoursMatching = colourOfPickedUp.Equals(colourOfDroppedOn);
            
                if (raycastHit2D.collider is null || isItself || !isHitAJewel || areColoursMatching)
                {
                    transform.position = initialPosition;
                    continue;
                }

                var hitGameObject = raycastHit2D.collider.gameObject;
                
                // Temporary variable that stores the initial position of the target object
                var hitGameObjectPosition = hitGameObject.transform.position;
            
                // The other object's position becomes the position that we picked our initial object from
                hitGameObject.transform.position = initialPosition;
            
                // need to get to and update initialPosition of the other as well
                var detectComponent = hitGameObject.GetComponent<SwitchPlacesOfPickedUp>();
            
                // Update the initial position so that when we pick up the other object it has the correct initial position,
                // not the position it was at during the initialization of the level 
                detectComponent.initialPosition = initialPosition;
            
                // This is the position of the object we picked up
                transform.position = hitGameObjectPosition;
                initialPosition = transform.position;

                swapped = true;
                // todo: send an event that some objects have switched places (e.g. re-evaluate rows)
                
                // send the event here
                OnSwitch?.Invoke();
                
                // todo: cast a ray between the two tiles a player tries to swap, if there is a tile between, break.
                _logger.Log("New position of picked up: " + hitGameObjectPosition);
                _logger.Log("New position of dropped " + detectComponent.gameObject.name + " on: "  + detectComponent.initialPosition);
            }
        
            // During dragging the object should create collisions with other jewels
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            _hasBeenPickedUp = false;
        }
    }
}
