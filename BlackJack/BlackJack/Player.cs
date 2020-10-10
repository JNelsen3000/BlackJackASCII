using System;
using System.Collections.Generic;
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
        public List<Card> Hand { get; private set; } = new List<Card>();
        protected int AcesInHand { get; private set; }
        //Returns string of cards in hand.
        public virtual string DisplayHand(bool showHouse)
        {
            string result = "";
            for (int i = 0; i < Hand.Count; i ++)
            {
                result += Hand[i].ViewCard() + ", ";
            }
            result += " TOTAL: " + TotalInHand;
            if (Busted) { result += " BUST"; }
            return result + "\n";
        }
        //Adds card to hand and keeps track of any Aces
        public void Hit(Card card)
        {
            Hand.Add(card);
            if (card.Name == "Ace") { this.AcesInHand++; }
        }
        //Empties hand and resets AcesInHand
        public void DiscardHand()
        {
            this.AcesInHand = 0;
            Hand.Clear();
        }
        //Returns the total in hand, accounting for aces
        public int TotalInHand
        {
            get
            {
                int sum = 0;
                foreach (Card card in Hand)
                {
                    sum += card.Value;
                }
                int tempAces = this.AcesInHand;
                while (sum > 21 && tempAces > 0)
                {
                    sum -= 10;
                    tempAces--;
                }
                return sum;
            }
        }
        //Determines if hand has busted
        public bool Busted
        {
            get
            {
                return this.TotalInHand > 21;
            }
        }
        //Determines if player was dealt a natural blackjack
        public bool HasBlackJackOnDeal()
        {
            if (TotalInHand == 21 && Hand.Count == 2)
            {
                return true;
            }
            return false;
        }
        //Asks if player wants hit or to stand, returns true if they want hit
        public virtual bool PlayerWantsHit()
        {
            if (this.Busted || HasBlackJackOnDeal() || this.TotalInHand == 21) { return false; }
            Console.WriteLine($"{this.PlayerName}, (h)it or (s)tand?");
            while (true)
            {
            string input = Console.ReadLine().ToLower();
            if (string.IsNullOrEmpty(input)) { input = ""; }
                switch (input)
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
        //Records bet amount and deducts from TotalMoney
        public virtual void PlaceBet()
        {
            int result = 0;
            Console.WriteLine($"{this.PlayerName}, you have {this.TotalMoney}.  Select amount to bet (1-50):");
            while (true)
            {
            string input = Console.ReadLine();
                try { result = int.Parse(input); }
                catch (FormatException)
                { 
                    Console.WriteLine("Not a valid amount.  Enter a positive, whole number (1-50):");
                    continue;
                }
                if (result < 1 || result > 50 )
                {
                    Console.WriteLine("Not a valid amount.  Enter a positive, whole number (1-50):");
                    continue;
                }
                if (result > this.TotalMoney)
                {
                    Console.WriteLine("You don't have enough money for that amount.  Try again:");
                    continue;
                }
                break;
            }
            this.TotalMoney -= result;
            this.PlayerBet = new Bet(result, this);
        }
    }
}
