using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "CurrentLevel", menuName = "Levels/CurrentLevelScriptableObject")]
    public class CurrentLevelScriptableObject : ScriptableObject
    {
        // Should default to the last level player. 
        // Persistent only in the session.
        // todo: If there are more levels, make sure this is updated accordingly
        public string nameOfLastLevelPlayed = "Levels/Level";
    }
}