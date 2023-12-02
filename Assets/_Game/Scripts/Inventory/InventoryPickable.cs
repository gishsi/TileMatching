using _Game.Scripts.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.Inventory
{
    public class InventoryPickable : MonoBehaviour, IPointerUpHandler
    {
        public PowerUps PowerUp = PowerUps.None;
        
        [SerializeField]
        private PickedUpPowerUpScriptableObject pickedUpPowerUpScriptableObject;
        
        public void OnPointerUp(PointerEventData eventData)
        {
            var ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            var hit = Physics2D.Raycast(ray, ray);
            
            if (hit.collider == null || hit.collider.gameObject.GetComponent<ReceiveItem>() == null)
            {
                return;
            }
            
            var data = new PowerUpDropped(hit.collider.gameObject.name, PowerUp);
            
            pickedUpPowerUpScriptableObject.RaiseEvent(data);
            
            Destroy(gameObject);
        }
    }
}
