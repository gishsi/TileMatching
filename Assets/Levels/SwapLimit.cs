using Tiles;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Levels
{
    /// <summary>
    ///     Script attached to the UI document that represents the moves left.
    ///     The responsibility of this class is to keep track of moves left and pass that
    ///     information on to a higher-level game manager.
    /// </summary>
    public class SwapLimit : MonoBehaviour
    {
        [SerializeField] private CurrentLevelScriptableObject currentLevel; 
        
        public int swaps = 5;

        private VisualElement _root;
    
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _root.Q<Label>("SwapsLeft").text = swaps.ToString();
        }

        private void OnEnable()
        {
            SwitchPlacesOfPickedUp.OnSwitch += UpdateSwaps;
        }

        private void OnDisable()
        {
            SwitchPlacesOfPickedUp.OnSwitch -= UpdateSwaps;
        }

        private void UpdateSwaps()
        {
            var swapsLabel = _root.Q<Label>("SwapsLeft");
        
            swaps--;

            if (swaps <= 0) // && tiles still in the grid
            {
                // todo: send an event to the game that the user lost
                // todo: temp., remove later
                Debug.Log("Player lost!");
                
                currentLevel.nameOfLastLevelPlayed = "Levels/Level";

                SceneManager.LoadScene("Levels/LevelFinished");
            }
        
            swapsLabel.text = swaps.ToString();
        }
    }
}
