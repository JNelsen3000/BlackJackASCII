using System;
using System.Collections.Generic;

namespace BlackJack
{
    public class Deck
    {
        private readonly Card[] mainDeck = new Card[52];
        private readonly List<int> usedCards = new List<int>();
        private readonly Random random = new Random();

        public Deck()
        {
            int currentCard = 0;
            string[] suits = { "hearts", "clubs", "diamonds", "spades" };
            foreach (string suit in suits)
            {
                mainDeck[currentCard] = new Card("Ace", suit, 11);
                
                currentCard++;
                for (int i = 2; i < 11; i++)
                {
                    mainDeck[currentCard] = new Card(i.ToString(), suit, i);
                    currentCard++;
                }
                
                mainDeck[currentCard] = new Card("Jack", suit, 10);
                mainDeck[currentCard + 1] = new Card("Queen", suit, 10);
                mainDeck[currentCard + 2] = new Card("King", suit, 10);
                
                currentCard += 3;
            }
        }

        // Returns a card not currently in use by any player
        // and adds it to UsedCards
        public Card GetFreshCard()
        {
            if (usedCards.Count == mainDeck.Length) throw new InvalidOperationException("You've drawn all cards in the deck");
            
            while (true)
            {
                int nextCard = random.Next(52);
                if (!usedCards.Contains(nextCard))
                {
                    Card currentCard = mainDeck[nextCard];
                    usedCards.Add(nextCard);
                    
                    return currentCard;
                }
            }
        }

        /// <summary>
        /// Resets UsedCards
        /// </summary>
        public void Shuffle()
        {
            usedCards.Clear();
        }

        /// <summary>
        /// Add cards to each player's hand and to house's hand
        /// </summary>
        /// <param name="table">The players at the table</param>
        public void Deal(IEnumerable<Player> table)
        {
            foreach (Player player in table)
            {
                player.Hit(GetFreshCard());
                player.Hit(GetFreshCard());
            }
        }


    }
}
