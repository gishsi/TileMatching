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
            mainMenuButton.clicked += GoToMenu;
            
            var levelFinishedLabel = root.Q<Label>("LevelFinishedLabel");
            levelFinishedLabel.text = $"{levelDataScriptableObject.GetNameOfCurrentLevel()} finished!";

            var container = root.Q<VisualElement>("Container");
            
            var goToNextLevelButton = root.Q<Button>("GoToNextLevelButton");
            goToNextLevelButton.clicked += GoToNextLevel;
            
            var playFromBeginningButton = root.Q<Button>("PlayFromBeginningButton");
            playFromBeginningButton.clicked += PlayFromBeginning;

            if (WasFinalLevelCompleted())
            {
                levelFinishedLabel.style.fontSize = 48;
                levelFinishedLabel.text = $"Congratulations! All levels finished!";

                container.Remove(goToNextLevelButton);
            }
            else
            {
                container.Remove(playFromBeginningButton);
            }
        }

        private void GoToNextLevel()
        {
            levelDataScriptableObject.MoveUpALevel();

            Debug.Log("Got to next level " + levelDataScriptableObject.GetNameOfCurrentLevel());
            
            SceneManager.LoadScene("_Game/Scenes/Level");
        }
        
        private void PlayFromBeginning()
        {
            levelDataScriptableObject.MoveCurrentLevelIndexToNewIndex(1);
            
            Debug.Log("Play from beginning" + levelDataScriptableObject.GetNameOfCurrentLevel());
            
            SceneManager.LoadScene("_Game/Scenes/Level");
        }
        
        
        private void GoToMenu()
        {
            levelDataScriptableObject.MoveUpALevel();
            
            SceneManager.LoadScene("_Game/Scenes/StartScene");
        }

        private bool WasFinalLevelCompleted()
        {
            var currentLevel = levelDataScriptableObject.GetCurrentLevelIndex();
            var amountOfLevels = levelDataScriptableObject.levels;

            return currentLevel == (amountOfLevels.Length - 1);
        }
    }
}
