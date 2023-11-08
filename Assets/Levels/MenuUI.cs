using Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Levels
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private CurrentLevelScriptableObject currentLevel;

        
        private ILogger<MenuUI> _logger;
        
        private void OnEnable()
        {
            _logger = new Logger<MenuUI>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;

            var mainMenuButton = root.Q<Button>("MainMenuButton");
            var quitButton = root.Q<Button>("QuitButton");
            var levelSelectionButton = root.Q<Button>("LevelSelectionButton");
            var restartButton = root.Q<Button>("RestartButton");

            if (mainMenuButton != null)
            {
                mainMenuButton.clicked += GoToMenu;
            }

            if (quitButton != null)
            {
                quitButton.clicked += QuitGame;
            }

            if (levelSelectionButton != null)
            {
                levelSelectionButton.clicked += () => _logger.Log("Level selection");
            }

            if (restartButton != null)
            {
                // todo: scriptable object to remember which level we last played
                restartButton.clicked += Restart;
            }
        }

        private void QuitGame()
        {
            _logger.Log("Quiting the game.");
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
        private void GoToMenu()
        {
            _logger.Log("Going to start scene.");
            
            SceneManager.LoadScene("Start/Scene");
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
