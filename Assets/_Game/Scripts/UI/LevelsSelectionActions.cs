using _Game.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelsSelectionActions : MonoBehaviour
{
    [SerializeField] 
    private LevelDataScriptableObject _levelDataScriptableObject; 
    
    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // todo: Could be automated?
        var level1 = root.Q<Button>("Level1");
        var level2 = root.Q<Button>("Level2");
        
        level1.clicked += () => StartGameAtLevel(1);
        level2.clicked += () => StartGameAtLevel(2);
    }
    
    private void StartGameAtLevel(int levelToPlayIndex)
    {
        _levelDataScriptableObject.MoveCurrentLevelIndexToNewIndex(levelToPlayIndex);
        
        SceneManager.LoadScene("_Game/Scenes/Level");
    }
}
