using UnityEngine;

namespace _Game.Scripts
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Levels data")]
        public LevelDataScriptableObject levelDataScriptableObject;
        
        private void Awake()
        {
            var levelToPlay = levelDataScriptableObject.GetCurrentLevel();
            
            var gridSystem = transform.Find("GridSystem").GetComponent<GridSystem>();
            gridSystem.SetPositionOfGridBasedOnAmountOfColsAndRows(levelToPlay.rowsAmount, levelToPlay.colsAmount);
            gridSystem.InitializeGrid(levelToPlay.rows);
        }
    }
}
