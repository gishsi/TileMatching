﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace _Game.UI.Components.Buttons.LevelSelection
{
    public class LevelSelectionComponent : MonoBehaviour
    {
        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            var levelSelectionButton = root.Q<Button>("LevelSelectionButton");
            
            levelSelectionButton.clicked += GoToLevelSelection;
        }
        
        private static void GoToLevelSelection()
        {
            SceneManager.LoadScene("_Game/Scenes/LevelSelection");
        }
    }
}