using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack
{
    public class Player
    {
        public Player(int playerNum, string playerName)
        {
            this.PlayerNum = playerNum;
            this.PlayerName = playerName;
        }

        public int PlayerNum { get; }
        public string PlayerName { get; }
        public Bet PlayerBet { get; private set; }
        public int TotalMoney { get; set; } = 100;
        public List<Card> Hand { get; } = new List<Card>();

        protected int AcesInHand { get; private set; }

        /// <summary>
        /// Returns string of cards in hand.
        /// </summary>
        /// <param name="showHouse">Whether or not the card's house should be shown</param>
        /// <returns>A user-readable string representing the cards in hand.</returns>
        public virtual string DisplayHand(bool showHouse)
        {
            // StringBuilder is a more efficient way of doing many string append operations in a row.
            StringBuilder result = new StringBuilder();
            foreach (var t in Hand)
            {
                result.Append($"{t.ViewCard()}, ");
            }

            result.Append($" TOTAL: {TotalInHand}");

            if (Busted)
            {
                result.Append(" BUST");
            }

            result.AppendLine();

            return result.ToString(); // Actually builds the final string
        }

        /// <summary>
        /// Adds card to hand and keeps track of any Aces
        /// </summary>
        /// <param name="card">The card to add</param>
        public void Hit(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));

            Hand.Add(card);

            if (card.Name == "Ace")
            {
                this.AcesInHand++;
            }
        }

        /// <summary>
        /// Empties hand and resets AcesInHand
        /// </summary>
        public void DiscardHand()
        {
            this.AcesInHand = 0;
            Hand.Clear();
        }

        /// <summary>
        /// Returns the total in hand, accounting for aces
        /// </summary>
        public int TotalInHand
        {
            get
            {
                int sum = Hand.Sum(card => card.Value);

                // If the player is over 21, we'll want to count the aces as low
                int tempAces = this.AcesInHand;
                while (sum > 21 && tempAces > 0)
                {
                    sum -= 10;
                    tempAces--;
                }

                return sum;
            }
        }

        /// <summary>
        /// Determines if hand has busted
        /// </summary>
        public bool Busted => this.TotalInHand > 21;

        /// <summary>
        /// Determines if player was dealt a natural blackjack
        /// </summary>
        public bool HasBlackJackOnDeal => TotalInHand == 21 && Hand.Count == 2;

        /// <summary>
        /// Returns whether or not this player is "the house"
        /// </summary>
        public virtual bool IsHouse => false;

        /// <summary>
        /// Asks if player wants hit or to stand, returns true if they want hit
        /// </summary>
        /// <returns>Whether or not the player wants to hit</returns>
        public virtual bool PlayerWantsHit()
        {
            if (this.Busted || HasBlackJackOnDeal || this.TotalInHand == 21)
            {
                return false;
            }

            Console.WriteLine($"{this.PlayerName}, (h)it or (s)tand?");
            while (true)
            {
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    input = "";
                }

                switch (input.ToLower())
                {
                    case "h":
                        return true;
                    case "s":
                        return false;
                    default:
                        Console.WriteLine("Not a valid selection.  Enter \"h\" or \"s\":");
                        break;
                }
            }
        }

        /// <summary>
        /// Records the bet amount and deducts from TotalMoney
        /// </summary>
        public virtual void PlaceBet()
        {
            bool isValid = false;
            int result = -1;
            Console.WriteLine($"{this.PlayerName}, you have {this.TotalMoney}.  Select amount to bet (1-50):");
            
            while (!isValid)
            {
                string input = Console.ReadLine();
                try
                {
                    result = int.Parse(input);
                    
                    if (result < 1 || result > 50)
                    {
                        Console.WriteLine("Not a valid amount.  Enter a positive, whole number (1-50):");
                    }
                    else if (result > this.TotalMoney)
                    {
                        Console.WriteLine("You don't have enough money for that amount.  Try again:");
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Not a valid amount.  Enter a positive, whole number (1-50):");
                }

            }

            this.TotalMoney -= result;
            this.PlayerBet = new Bet(result, this);
        }
    }
}