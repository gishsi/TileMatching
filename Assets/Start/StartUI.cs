using Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Start
{
    public class StartUI : MonoBehaviour
    {
        private ILogger<StartUI> _logger;
        private void OnEnable()
        {
            _logger = new Logger<StartUI>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;

            var level1Button = root.Q<Button>("Level_1");
            var quitButton = root.Q<Button>("Quit");

            level1Button.clicked += GoToLevel1;
            quitButton.clicked += QuitGame;
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
        
        private void GoToLevel1()
        {
            _logger.Log("Going to level 1.");
            
            SceneManager.LoadScene("Levels/Level");
        }
    }
}
