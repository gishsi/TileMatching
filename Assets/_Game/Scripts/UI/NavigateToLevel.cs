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

            var level1Button = root.Q<Button>("Level_1");

            level1Button.clicked += GoToLevel1;
        }
        
        private void GoToLevel1()
        {
            _logger.Log("Going to level 1.");
            
            SceneManager.LoadScene("_Game/Scenes/Level");
        }
    }
}
