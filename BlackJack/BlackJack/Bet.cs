using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Bet
    {
        public Bet(int betAmount, Player playerPlacingBet)
        {
            this.BetAmount = betAmount;
            this.PlayerThatPlacedBet = playerPlacingBet;
        }

        public int BetAmount { get; }
        public Player PlayerThatPlacedBet { get; }
        // If player was dealt a natural blackjack and dealer was not,
        // player wins 1.5 * bet.
        public void BlackJackPayout()
        {
            this.PlayerThatPlacedBet.TotalMoney += (this.BetAmount / 2) * 3;
        }
        // Pays player bet * 2.
        public void NormalPayout()
        {
            this.PlayerThatPlacedBet.TotalMoney += this.BetAmount * 2;
        }
        // Pays player back their bet.
        public void ReturnBet()
        {
            this.PlayerThatPlacedBet.TotalMoney += this.BetAmount;
        }
    }
}
