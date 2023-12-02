using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace _Game.Scripts.UI.Buttons
{
    public class RestartButton: MonoBehaviour 
    {
        private ILogger<RestartButton> _logger;
        
        private void OnEnable()
        {
            _logger = new Logger<RestartButton>(gameObject);
            
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