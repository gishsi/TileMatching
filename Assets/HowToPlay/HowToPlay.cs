using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace HowToPlay
{
    public class HowToPlay : MonoBehaviour
    {
        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            var mainMenuButton = root.Q<Button>("MainMenuButton");
            var nextButton = root.Q<Button>("NextButton");
            var previousButton = root.Q<Button>("PreviousButton");

            if (mainMenuButton != null)
            {
                mainMenuButton.clicked += GoToMenu;
            }

            if (nextButton != null)
            {
                nextButton.clicked += () => Debug.Log("Next");
            }

            if (previousButton != null)
            {
                previousButton.clicked += () => Debug.Log("Previous");
            }
        }
        
        private void GoToMenu()
        {
            SceneManager.LoadScene("Start/Scene");
        }
    }
}
