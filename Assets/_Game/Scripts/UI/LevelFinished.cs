using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace _Game.Scripts.UI
{
    public class LevelFinished : MonoBehaviour
    {
        [SerializeField] private CurrentLevelScriptableObject currentLevel;

        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            var mainMenuButton = root.Q<Button>("MainMenuButton");
            var levelFinishedLabel = root.Q<Label>("LevelFinishedLabel");
            
            mainMenuButton.clicked += GoToMenu;
            levelFinishedLabel.text = $"{currentLevel.nameOfLastLevelPlayed.ToString()} finished!";
        }
        
        private void GoToMenu()
        {
            currentLevel.nameOfLastLevelPlayed = Levels.Level2;
            
            SceneManager.LoadScene("_Game/Scenes/StartScene");
        }
    }
}
