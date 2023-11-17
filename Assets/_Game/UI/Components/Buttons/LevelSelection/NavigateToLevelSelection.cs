using UnityEngine;
using UnityEngine.UIElements;

namespace _Game.UI.Components.Buttons.LevelSelection
{
    public class LevelSelectionComponent : MonoBehaviour
    {
        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            var levelSelectionButton = root.Q<Button>("LevelSelectionButton");
            
            levelSelectionButton.clicked += () => Debug.Log("Level selection");
        }
    }
}