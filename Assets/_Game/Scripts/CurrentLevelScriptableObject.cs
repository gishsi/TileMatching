using UnityEngine;

namespace _Game.Scripts
{
    [CreateAssetMenu(fileName = "CurrentLevel", menuName = "Levels/CurrentLevelScriptableObject")]
    public class CurrentLevelScriptableObject : ScriptableObject
    {
        public Levels nameOfLastLevelPlayed = Levels.Level1;
    }
}