using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Game.UI.Components.Buttons.Quit
{
    public class QuitComponent : MonoBehaviour
    {
        private ILogger<QuitComponent> _logger;
        
        private void OnEnable()
        {
            _logger = new Logger<QuitComponent>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            var quitButton = root.Q<Button>("QuitButton");
            
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
    }
}