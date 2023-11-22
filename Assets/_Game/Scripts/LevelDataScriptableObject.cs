using System;
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
        public LevelScriptableObject[] levels;

        public int currentLevelIndex { get; private set; } = 0;
        
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
        ///     Move to the level specified, e.g. First level = 1, Second level = 2
        /// </summary>
        /// <param name="level">Level you want to navigate to</param>
        /// <exception cref="ArgumentOutOfRangeException">If the level does not exist in the levels array</exception>
        /// <remarks>
        ///     It is more intuitive for a game designer to count levels from 1, That is the assumption made by this method.
        /// </remarks>.
        public void MoveCurrentLevelIndexToNewIndex(int level)
        {
            var index = level - 1;
            
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index), 
                    $"This level index does not exist. The level counts begins from 1.");
            }
            
            if (index >= levels.Length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index), 
                    $"This level index does not exist. Ensure the level you are trying to access has been added to the {nameof(levels)} list.");
            }

            currentLevelIndex = index;
        }

        public string GetNameOfCurrentLevel()
        {
            return levels[currentLevelIndex].levelName;
        }
            
        public int GetAmountOfSwipesOfCurrentLevel()
        {
            return levels[currentLevelIndex].amountOfSwipes;
        }
        
        /// <summary>
        ///     Get next level index
        /// </summary>
        /// <returns>Index of the next level if it exists, current level otherwise (final)</returns>
        private int GetNextLevelIndex()
        {
            var i = currentLevelIndex + 1;

            return i < levels.Length ? i : currentLevelIndex;
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