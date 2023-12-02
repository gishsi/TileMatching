using System.Runtime.InteropServices;
using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Game.Scripts.UI.Buttons
{
    public class QuitButton : MonoBehaviour
    {
        private ILogger<QuitButton> _logger;
        
        [DllImport("__Internal")]
        private static extern void CloseGame();
        
        private void OnEnable()
        {
            _logger = new Logger<QuitButton>(gameObject);
            
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            var quitButton = root.Q<Button>("QuitButton");
            
            quitButton.clicked += QuitGame;
        }
        
        private void QuitGame()
        {
            _logger.Log("Quiting the game.");
            
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        CloseGame();
#else
        Application.Quit();
#endif
        }   
    }
}