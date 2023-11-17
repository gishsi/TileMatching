using _Game.Scripts.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Game.Scripts.UI
{
    /// <summary>
    ///     Script attached to the UI document that represents the moves left.
    ///     The responsibility of this class is to update the label that represents
    ///     how many swipes the user has.
    /// </summary>
    public class SwapsLabelUpdater : MonoBehaviour
    {
        [SerializeField] private CurrentLevelScriptableObject currentLevel; 
        
        private VisualElement _root;

        [SerializeField]
        private UpdateSwapScriptableObject updateSwap;
        
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _root.Q<Label>("SwapsLeft").text = updateSwap.Swaps.ToString();
        }

        private void OnEnable()
        {
            updateSwap.SwapsChangeEvent.AddListener(UpdateSwapsInTheLabel);
        }

        private void OnDisable()
        {
            updateSwap.SwapsChangeEvent.RemoveListener(UpdateSwapsInTheLabel);
        }

        /// <summary>
        ///     Updates the UI with the amount of swaps left received from the event
        /// </summary>
        /// <param name="data">Amount of swaps left</param>
        private void UpdateSwapsInTheLabel(int data)
        {
            var swapsLabel = _root.Q<Label>("SwapsLeft");
            
            swapsLabel.text = data.ToString();
        }
    }
}
