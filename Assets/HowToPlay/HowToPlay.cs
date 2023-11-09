using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}

namespace HowToPlay
{
    public class HowToPlay : MonoBehaviour
    {
        //  https://stackoverflow.com/questions/64749385/predefined-type-system-runtime-compilerservices-isexternalinit-is-not-defined
        private record Card(string Title, string Text);

        private List<Card> _cards = new()
        {
            new Card("How to win", "This is how you win the game"),
            new Card("Swiping tiles", "This is how you swipe the tiles"),
        };
        
        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            var mainMenuButton = root.Q<Button>("MainMenuButton");
            var nextButton = root.Q<Button>("NextButton");
            var previousButton = root.Q<Button>("PreviousButton");

            if (mainMenuButton != null)
            {
                mainMenuButton.clicked += GoToMenu;
            }

            if (nextButton != null)
            {
                nextButton.clicked += () => Debug.Log("Next"); // todo: implement
            }

            if (previousButton != null)
            {
                previousButton.clicked += () => Debug.Log("Previous"); // todo: implement
            }
            
            var cardTitleLabel = root.Q<Label>("CardTitle");
            var cardTextLabel  = root.Q<Label>("CardText");

            cardTitleLabel.text = _cards.First().Title;
            cardTextLabel.text = _cards.First().Text;
        }
        
        private static void GoToMenu()
        {
            SceneManager.LoadScene("Start/Scene");
        }
    }
}
