using UnityEngine;
using UnityEngine.UIElements;

namespace _Game.Scripts.UI
{
    public class Header : MonoBehaviour
    {
        public string headerText = "Header";
        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            var headerTitle = root.Q<Label>("Header");

            headerTitle.text = headerText;
        }
    }
}
