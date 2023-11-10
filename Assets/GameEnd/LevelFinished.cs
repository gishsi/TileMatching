using Game.Levels;
using UnityEngine;

namespace GameEnd
{
    public class LevelFinished : MonoBehaviour
    {
        [SerializeField] private CurrentLevelScriptableObject currentLevel;

        private void Start()
        {
            // todo: remove
            Debug.Log("Current level: " + currentLevel.nameOfLastLevelPlayed);
        }
    }
}
