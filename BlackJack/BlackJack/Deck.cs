using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Deck
    {
        private Card[] MainDeck = new Card[52];
        private List<int> UsedCards = new List<int>();

        public Deck()
        {
            int currentCard = 0;
            string[] suits = { "hearts", "clubs", "diamonds", "spades" };
            foreach (string suit in suits)
            {
                MainDeck[currentCard] = new Card("Ace", suit, 11);
                currentCard++;
                for (int i = 2; i < 11; i++)
                {
                    MainDeck[currentCard] = new Card(i.ToString(), suit, i);
                    currentCard++;
                }
                MainDeck[currentCard] = new Card("Jack", suit, 10);
                MainDeck[currentCard + 1] = new Card("Queen", suit, 10);
                MainDeck[currentCard + 2] = new Card("King", suit, 10);
                currentCard += 3;
            }
        }

        // Returns a card not currently in use by any player
        // and adds it to UsedCards
        public Card GetFreshCard()
        {
            Random random = new Random();
            Card currentCard;
            while (true)
            {
                int nextCard = random.Next(52);
                if (!UsedCards.Contains(nextCard))
                {
                    currentCard = MainDeck[nextCard];
                    UsedCards.Add(nextCard);
                    return currentCard;
                }
            }
        }

        // Resets UsedCards
        public void Shuffle()
        {
            UsedCards.Clear();
        }

        // Add cards to each player's hand and to house's hand
        public void Deal(Player[] table)
        {
            foreach (Player player in table)
            {
                player.Hit(GetFreshCard());
                player.Hit(GetFreshCard());
            }
        }


    }
}
