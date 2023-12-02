using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace _Game.Scripts.UI.Buttons
{
    public class NavigateToHowToPlay : MonoBehaviour
    {
        private ILogger<NavigateToHowToPlay> _logger;
        private void OnEnable()
        {
            _logger = new Logger<NavigateToHowToPlay>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            var howToPlayButton = root.Q<Button>("HowToPlay");
            howToPlayButton.clicked += () => SceneManager.LoadScene("_Game/Scenes/HowToPlay");
        }
    }
}
