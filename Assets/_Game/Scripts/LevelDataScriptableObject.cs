using UnityEngine;

namespace _Game.Scripts
{
    /// <summary>
    ///     Data container for all data related things in level setup, i.e. current level, and all the available levels.
    ///     Keeps track of the current levels.
    /// </summary>
    [CreateAssetMenu(fileName = "Level", menuName = "Levels/Level data")]
    public class LevelDataScriptableObject : ScriptableObject
    {
        [SerializeField]
        private LevelScriptableObject[] levels;
        
        private int currentLevelIndex = 0;
        
        public LevelScriptableObject GetCurrentLevel()
        {
            return levels[currentLevelIndex];
        }

        public void MoveDownALevel()
        {
            currentLevelIndex = GetPreviousLevelIndex();
        }

        public void MoveUpALevel()
        {
            currentLevelIndex = GetNextLevelIndex();
        }
        
        /// <summary>
        ///     Get next level index
        /// </summary>
        /// <returns>Index of the next level if it exists, current level otherwise (final)</returns>
        private int GetNextLevelIndex()
        {
            var i = currentLevelIndex + 1;

            if (i < levels.Length)
            {
                return i;
            }
            
            i = currentLevelIndex;
            Debug.Log("Reached the final level");

            return i;
        }

        /// <summary>
        ///     Get previous level index
        /// </summary>
        /// <returns>Index of the previous level if it exists, current level otherwise (first)</returns>
        private int GetPreviousLevelIndex()
        {
            var i = currentLevelIndex - 1;

            if (i < 0)
            {
                return i;
            }
            
            i = currentLevelIndex;
            Debug.Log("Reached the first level");

            return i;
        }
    }
}