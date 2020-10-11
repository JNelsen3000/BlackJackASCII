using System;

namespace BlackJack
{
    public class Game
    {
        /// <summary>
        /// Represents the deck of cards used in the game.
        /// </summary>
        private Deck mainDeck = new Deck();
        
        /// <summary>
        /// Represents all players and house
        /// </summary>
        public Player[] table { get; }
        
        /// <summary>
        /// Tracks the active player
        /// </summary>
        public Player CurrentPlayer { get; private set; }
        
        /// <summary>
        /// Tracks the end of game
        /// </summary>
        private bool GameOver { get; set; }
        
        /// <summary>
        /// Creates deck, populates table
        /// </summary>
        /// <param name="players">The number of players</param>
        public Game(int players)
        {
            if (players < 1) throw new ArgumentOutOfRangeException(nameof(players), "There must be at least 1 player");
            
            this.table = new Player[players + 1];
            this.table[0] = new House();
            for (int i = 1; i <= players; i++)
            {
                Console.WriteLine($"Player {i}, enter your name:");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name)) { name = "Player " + i; }
                this.table[i] = new Player(i, name);
                Console.WriteLine();
            }
            this.CurrentPlayer = table[1];
        }
        
        /// <summary>
        /// Makes players discard hands and shuffles deck
        /// </summary>
        private void CollectCards()
        {
            foreach (Player player in table)
            {
                player.DiscardHand();
                mainDeck.Shuffle();
            }
        }
        /// <summary>
        /// Keeps game running until GameOver == true.
        /// </summary>
        public void RunGame()
        {
            while (!this.GameOver)
            {
                PlayRound();
            }
        }
        
        /// <summary>
        /// Runs a full round of the game.
        /// </summary>
        private void PlayRound()
        {
            this.CollectCards();
            this.PlaceBets();
            mainDeck.Deal(table);
            this.RefreshDisplay(false);

            if (table[0].HasBlackJackOnDeal)
            {
                DealerWasDealtBlackJack();
                RefreshDisplay(true);
                Console.WriteLine("Dealer was dealt a natural blackjack!");
            }
            else
            {
                AskPlayersHitOrStand();
                AskDealerHitOrStand();
                ResolveBets();
            }

            if (EndGameLogic.PlayerAtTableWonOrLost(table))
            {
                EndGameLogic.DisplayWinnerAndScores(table);
                this.GameOver = true;
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("\nPress enter to start next round.");
            Console.ReadLine();
            this.AdvanceCurrentPlayer();
        }
        
        /// <summary>
        /// Takes appropriate action when house was dealt a natural blackjack
        /// </summary>
        private void DealerWasDealtBlackJack()
        {
            for (int i = 1; i < table.Length; i ++)
            {
                if (table[i].HasBlackJackOnDeal)
                {
                    table[i].TotalMoney += table[i].PlayerBet.BetAmount;
                    Console.WriteLine($"{table[i].PlayerName} also has a natural blackjack and gets their bet returned.");
                }
            }
        }
        
        /// <summary>
        /// Asks each player if they want to hit or stand.
        /// </summary>
        private void AskPlayersHitOrStand()
        {
            for (int i = 1; i < table.Length; i++)
            {
                Player player = table[i];
                if (player.HasBlackJackOnDeal)
                {
                    player.PlayerBet.BlackJackPayout();
                    Console.WriteLine($"{player.PlayerName} has a natural blackjack!  Payout = {(player.PlayerBet.BetAmount / 2) * 3}");
                    Console.WriteLine("Press enter to continue.");
                    Console.ReadLine();
                }
                else
                {
                    while (player.PlayerWantsHit())
                    {
                        player.Hit(mainDeck.GetFreshCard());
                        this.RefreshDisplay(false);
                    }
                }
                AdvanceCurrentPlayer();
                this.RefreshDisplay(false);
            }
        }
        /// <summary>
        /// Asks house hit or stand.
        /// </summary>
        private void AskDealerHitOrStand(){
            while(table[0].PlayerWantsHit()) 
            {
                table[0].Hit(mainDeck.GetFreshCard());
                this.RefreshDisplay(true);
            }
            this.RefreshDisplay(true);
        }
        /// <summary>
        /// Allows each player to input their bet.
        /// </summary>
        private void PlaceBets()
        {
            foreach (Player player in this.table)
            {
                player.PlaceBet();
                Console.Clear();
            }
        }
        /// <summary>
        /// Refreshes screen to show updated hands, scores, and bets.
        /// </summary>
        /// <param name="showHouse">Whether the house should be shown</param>
        private void RefreshDisplay(bool showHouse)
        {
            Console.Clear();
            DisplayBetsAndScores();
            Console.WriteLine();
            DisplayHands(showHouse);
            Console.WriteLine();
        }
        
        /// <summary>
        /// Displays scores and current bets
        /// </summary>
        private void DisplayBetsAndScores()
        {
            for (int i = 1; i < table.Length; i++)
            {
                Console.WriteLine($"{table[i].PlayerName} bet {table[i].PlayerBet.BetAmount} and has {table[i].TotalMoney} remaining.");
            }
            Console.WriteLine();
        }
        
        /// <summary>
        /// Displays hands, allowing program to show house's hand or not
        /// </summary>
        /// <param name="showHouse">Whether or not the house should be shown</param>
        private void DisplayHands(bool showHouse)
        {
            foreach (Player player in table)
            {
                if (player == CurrentPlayer) { Console.ForegroundColor = ConsoleColor.Red; }
                Console.Write($"{player.PlayerName}: ");
                Console.ResetColor();
                Console.WriteLine(player.DisplayHand(showHouse));
            }
        }
        
        /// <summary>
        /// Rotates current player through entire table
        /// </summary>
        private void AdvanceCurrentPlayer()
        {
            if (CurrentPlayer.PlayerNum == table.Length - 1)
            {
                CurrentPlayer = table[0];
            } 
            else
            {
                CurrentPlayer = table[CurrentPlayer.PlayerNum + 1];
            }
        }
        
        /// <summary>
        /// Makes appropriate payments for each bet.
        /// </summary>
        private void ResolveBets()
        {
            House house = (House)table[0];

            for (int i = 1; i < table.Length; i++)
            {
                Player player = table[i];
                if (player.HasBlackJackOnDeal)
                {
                    continue;
                }

                if (player.Busted || (player.TotalInHand < house.TotalInHand && house.Busted == false))
                {
                    Console.WriteLine($"{player.PlayerName} lost their bet of {player.PlayerBet.BetAmount}.");
                }
                else if (player.TotalInHand == house.TotalInHand)
                {
                    Console.WriteLine($"{player.PlayerName} tied with the House.  The bet of {player.PlayerBet.BetAmount} is returned.");
                    player.PlayerBet.ReturnBet();
                } 
                else
                {
                    Console.WriteLine($"{player.PlayerName} wins {player.PlayerBet.BetAmount * 2}!");
                    player.PlayerBet.NormalPayout();
                }
            }
        }
    }
}
