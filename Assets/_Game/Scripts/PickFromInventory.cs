using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts
{
    public class PickFromInventory : MonoBehaviour, IPointerUpHandler
    {
        public PowerUps PowerUp = PowerUps.None;
        
        [SerializeField]
        private PickedUpItemScriptableObject _pickedUpItemScriptableObject;
        
        public void OnPointerUp(PointerEventData eventData)
        {
            var ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            var hit = Physics2D.Raycast(ray, ray);

            if (hit.collider == null)
            {
                return;
            }

            if (!hit.collider.gameObject.CompareTag("Tile"))
            {
                return;
            }
            
            var data = new ItemDropped(hit.collider.gameObject.name, PowerUp);
            
            _pickedUpItemScriptableObject.RaiseEvent(data);
            
            Destroy(gameObject);
        }
    }
}
