using _Game.Scripts;
using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace _Game.UI.Components.Buttons.Restart
{
    public class RestartComponent: MonoBehaviour 
    {
        [SerializeField] private CurrentLevelScriptableObject currentLevel;
        
        private ILogger<RestartComponent> _logger;
        
        private void OnEnable()
        {
            _logger = new Logger<RestartComponent>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            var restartButton = root.Q<Button>("RestartButton");
            
            restartButton.clicked += Restart;
        }

        private void Restart()
        {
            if (currentLevel.nameOfLastLevelPlayed is null)
            {
                _logger.Log("Could not restart the level.");
                return;
            }
            
            SceneManager.LoadScene(currentLevel.nameOfLastLevelPlayed);
        }
    }
}