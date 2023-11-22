using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace _Game.Scripts.UI
{
    public class LevelFinished : MonoBehaviour
    {
        [SerializeField] private LevelDataScriptableObject levelDataScriptableObject;

        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            var mainMenuButton = root.Q<Button>("MainMenuButton");
            var levelFinishedLabel = root.Q<Label>("LevelFinishedLabel");
            
            mainMenuButton.clicked += GoToMenu;
            levelFinishedLabel.text = $"{levelDataScriptableObject.GetCurrentLevel().name} finished!";
        }
        
        private void GoToMenu()
        {
            levelDataScriptableObject.MoveUpALevel();
            
            SceneManager.LoadScene("_Game/Scenes/StartScene");
        }
    }
}
