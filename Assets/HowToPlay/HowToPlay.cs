using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace HowToPlay
{
    /// <summary>
    ///     This class is responsible for setting up the UI for the How To Play help screen.
    ///     It utilizes JSON to read how to play data stored in JSON.
    /// </summary>
    public class HowToPlay : MonoBehaviour
    {
        [System.Serializable]
        public class Card
        {
            public string title;
            public string text;
        }

        [System.Serializable]
        public class CardList
        {
            public List<Card> data;
            public static CardList CreateFromJson(string jsonString)
            {
                return JsonUtility.FromJson<CardList>(jsonString);
            }
        }
        
        [SerializeField] private TextAsset howToPlayJson;

        public CardList cards;
        private Card _currentCard;

        private VisualElement _root;
        
        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            var mainMenuButton = _root.Q<Button>("MainMenuButton");
            var nextButton = _root.Q<Button>("NextButton");
            var previousButton = _root.Q<Button>("PreviousButton");
            
            mainMenuButton.clicked += GoToMenu;
            nextButton.clicked += Next;
            previousButton.clicked += Previous;
            
            // Get the cards
            cards = CardList.CreateFromJson(howToPlayJson.text);

            // After loading the How To Play screen, load the first tutorial page
            SetupCardAtIndex(0);
        }

        private void SetupCardAtIndex(int index)
        {
            _currentCard = cards.data[index];
            
            var cardTitleLabel = _root.Q<Label>("CardTitle");
            cardTitleLabel.text = _currentCard.title;
            
            var cardTextLabel  = _root.Q<Label>("CardText");
            cardTextLabel.text = _currentCard.text;
        }
        
        /// <summary>
        ///     Loop over the card's list if it goes above its length: circular arrays CS21120 (<see cref="Previous"/> also utilizes circular arrays)
        /// </summary>
        private void Next()
        {
            var index = cards.data.FindIndex((c) => c.Equals(_currentCard));
            
            SetupCardAtIndex((index + 1) % cards.data.Count);
        }
        
        private void Previous()
        {
            var index = cards.data.FindIndex((c) => c.Equals(_currentCard));
            
            SetupCardAtIndex((index - 1 + cards.data.Count) % cards.data.Count);
        }
        
        private static void GoToMenu()
        {
            SceneManager.LoadScene("Start/Scene");
        }
    }
}
