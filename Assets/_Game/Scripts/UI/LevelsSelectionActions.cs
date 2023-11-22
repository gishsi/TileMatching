using _Game.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelsSelectionActions : MonoBehaviour
{
    [SerializeField] 
    private VisualTreeAsset LevelButtonDocument;
    
    [SerializeField] 
    private LevelDataScriptableObject _levelDataScriptableObject; 
    
    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        var container = root.Q<ScrollView>("LevelsContainer");

        PopulateContainerWithLevelButtons(container);
    }

    /// <summary>
    ///     Gets the levels and generates a button that will navigate to that level automatically. Makes adding a new level extremely easy.
    /// </summary>
    /// <param name="container">Container for the buttons.</param>
    private void PopulateContainerWithLevelButtons(VisualElement container)
    {
        for (var i = 0; i < _levelDataScriptableObject.levels.Length; i++)
        {
            var levelName = _levelDataScriptableObject.levels[i].levelName;
            var levelIndex = i + 1;
            
            var buttonInstance = LevelButtonDocument.Instantiate();

            var button = buttonInstance.Q<Button>("LevelButton");
            
            button.text = levelName;
            button.clicked += () => StartGameAtLevel(levelIndex);
            button.name = levelName;
            container.Add(buttonInstance);
        }
    }
    
    private void StartGameAtLevel(int levelToPlayIndex)
    {
        _levelDataScriptableObject.MoveCurrentLevelIndexToNewIndex(levelToPlayIndex);
        
        SceneManager.LoadScene("_Game/Scenes/Level");
    }
}
