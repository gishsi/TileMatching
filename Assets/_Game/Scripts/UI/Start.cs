using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace _Game.Scripts.UI
{
    public class Start : MonoBehaviour
    {
        private ILogger<Start> _logger;
        private void OnEnable()
        {
            _logger = new Logger<Start>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;

            var startButton = root.Q<Button>("StartButton");

            startButton.clicked += StartGame;
        }
        
        private void StartGame()
        {
            SceneManager.LoadScene("_Game/Scenes/Level");
        }
    }
}
