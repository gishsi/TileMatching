using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class PickFromInventory : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        private PowerUps PowerUp = PowerUps.None;
        
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
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PowerUp = PowerUps.ColourBomb; // todo: dummy data
        }
    }
}
