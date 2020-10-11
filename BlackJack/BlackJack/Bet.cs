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
        
        /// <summary>
        /// If player was dealt a natural blackjack and dealer was not, player wins 1.5 * bet.
        /// </summary>
        public void BlackJackPayout()
        {
            this.PlayerThatPlacedBet.TotalMoney += (this.BetAmount / 2) * 3;
        }
        
        /// <summary>
        /// Pays player bet * 2.
        /// </summary>
        public void NormalPayout()
        {
            this.PlayerThatPlacedBet.TotalMoney += this.BetAmount * 2;
        }
        
        /// <summary>
        /// Pays player back their bet.
        /// </summary>
        public void ReturnBet()
        {
            this.PlayerThatPlacedBet.TotalMoney += this.BetAmount;
        }
    }
}
