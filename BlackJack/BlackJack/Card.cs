using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Card
    {
        private readonly string suit;
        
        public string Name { get; }
        
        public int Value { get; }

        public Card(string name, string suit, int value)
        {
            this.Name = name;
            this.suit = suit;
            this.Value = value;
        }

        public string ViewCard() => $"{Name} of {suit}";
    }
}
