using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace _Game.Scripts.UI
{
    public class NavigateToLevel : MonoBehaviour
    {
        private ILogger<NavigateToLevel> _logger;
        private void OnEnable()
        {
            _logger = new Logger<NavigateToLevel>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;

            //var startButton = root.Q<Button>("Start");

            //startButton.clicked += StartGame;
        }
        
        private void StartGame()
        {
            SceneManager.LoadScene("_Game/Scenes/Level");
        }
    }
}
