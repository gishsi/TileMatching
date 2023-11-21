using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Current level")]
        public CurrentLevelScriptableObject currentLevelScriptableObject;
        
        [Header("Levels")]
        public LevelScriptableObject level1;
        public LevelScriptableObject level2;

        private void Awake()
        {
            var levelToPlay = GetLevelToPlayFromName(currentLevelScriptableObject.nameOfLastLevelPlayed);
            
            var gridSystem = transform.Find("GridSystem").GetComponent<GridSystem>();
            gridSystem.SetPositionOfGridBasedOnAmountOfColsAndRows(levelToPlay.rowsAmount, levelToPlay.colsAmount);
            gridSystem.InitializeGrid(levelToPlay.rows);
        }
        
        /// <summary>
        ///     This breaks the assumption that all a designer needs to do when they create a level is to create a new scriptable object from the editor.
        ///     A possible extension would be to load all levels using the Resources.Load.
        /// </summary>
        private LevelScriptableObject GetLevelToPlayFromName(Levels levelName)
        {
            var levelToPlay = level1;
            switch (levelName)
            {
                case Levels.Level1:
                    break;
                case Levels.Level2:
                    levelToPlay = level2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(levelName), levelName, "Level specified in the enum does not have a corresponding level scriptable object.");
            }

            return levelToPlay;
        }
    }
}
