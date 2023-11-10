using Common.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Common.UIComponents.Buttons.MainMenu
{
    public class MainMenuComponent : MonoBehaviour
    {
                
        private ILogger<MainMenuComponent> _logger;
        
        private void OnEnable()
        {
            _logger = new Logger<MainMenuComponent>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;
            var mainMenuButton = root.Q<Button>("MainMenuButton");
            
            mainMenuButton.clicked += GoToMenu;
        }
        
        private void GoToMenu()
        {
            _logger.Log("Going to start scene.");
            
            SceneManager.LoadScene("Start/StartScene");
        }
    }
}