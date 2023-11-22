using _Game.Scripts;
using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace _Game.UI.Components.Buttons.Restart
{
    public class RestartComponent: MonoBehaviour 
    {
        private ILogger<RestartComponent> _logger;
        
        private void OnEnable()
        {
            _logger = new Logger<RestartComponent>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            var restartButton = root.Q<Button>("RestartButton");
            
            restartButton.clicked += Restart;
        }

        private static void Restart()
        {
            SceneManager.LoadScene("_Game/Scenes/Level");
        }
    }
}