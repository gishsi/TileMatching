using System.Collections.Generic;
using System.Linq;
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
        
        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            var mainMenuButton = root.Q<Button>("MainMenuButton");
            var nextButton = root.Q<Button>("NextButton");
            var previousButton = root.Q<Button>("PreviousButton");
            
            mainMenuButton.clicked += GoToMenu;
            nextButton.clicked += () => Debug.Log("Next"); // todo: implement
            previousButton.clicked += () => Debug.Log("Previous"); // todo: implement
            
            // Update the cards
            cards = CardList.CreateFromJson(howToPlayJson.text);

            SetupInitialCard(root);
        }

        private void SetupInitialCard(VisualElement root)
        {
            var cardTitleLabel = root.Q<Label>("CardTitle");
            cardTitleLabel.text = cards.data.First().title;
            
            var cardTextLabel  = root.Q<Label>("CardText");
            cardTextLabel.text = cards.data.First().text;

            _currentCard = cards.data.First();
        }

        private static void GoToMenu()
        {
            SceneManager.LoadScene("Start/Scene");
        }
    }
}
