using Common.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Common.UIComponents.Buttons.HowToPlay
{
    public class NavigateToHowToPlay : MonoBehaviour
    {
        private ILogger<NavigateToHowToPlay> _logger;
        private void OnEnable()
        {
            _logger = new Logger<NavigateToHowToPlay>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            var howToPlayButton = root.Q<Button>("HowToPlay");
            howToPlayButton.clicked += () => SceneManager.LoadScene("HowToPlay/HowToPlay");
        }
    }
}
