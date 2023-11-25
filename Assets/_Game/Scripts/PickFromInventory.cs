using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace _Game.Scripts
{
    public class PickFromInventory : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        private PowerUps PowerUp = PowerUps.None;
        
        [SerializeField]
        private PickedUpItemScriptableObject _pickedUpItemScriptableObject;
        
        // private void Start()
        // {
        //     try
        //     {
        //         var root = GetComponent<UIDocument>().rootVisualElement;
        //         var itemButton = root.Q<Button>("ItemButton");
        //     
        //
        //         // itemButton.clicked += () => Debug.Log("Clicked on item");
        //         
        //         itemButton.RegisterCallback<PointerDownEvent>(
        //             e => {
        //                 Debug.Log("Button pointer down!");
        //             },
        //             TrickleDown.TrickleDown);
        //     }
        //     catch (Exception e)
        //     {
        //         return;
        //     }
        //    
        // }
        
        // /// <summary>
        // ///     Pick an item from the inventory
        // /// </summary>
        // public void On(PointerEventData eventData)
        // {
        //     Debug.Log($"======{nameof(PickFromInventory)}=======");
        //     Debug.Log("Picking up bomb");
        //     _pickedUpItemScriptableObject.PowerUp = PowerUps.Bomb; // todo: dummy data
        //     Debug.Log($"=================================");
        // }

        // private void Update()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         Debug.Log($"======{nameof(PickFromInventory)}=======");
        //         Debug.Log("Picking up bomb");
        //         _pickedUpItemScriptableObject.PowerUp = PowerUps.Bomb; // todo: dummy data
        //         Debug.Log($"=================================");
        //     }
        // }
        
        
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
            PowerUp = PowerUps.Concretion; // todo: dummy data
        }
    }
}
