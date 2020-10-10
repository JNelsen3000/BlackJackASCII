using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Card
    {
        private string name { get; }
        public string Name { get { return this.name; } }
        private string Suit { get; }
        private int value { get; }
        public int Value
        {
            get
            {
                return value;
            }
        }

        public Card(string name, string suit, int value)
        {
            this.name = name;
            this.Suit = suit;
            this.value = value;
        }

        public string ViewCard()
        {
            return $"{Name} of {Suit}";
        }
    }
}
