using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Game
    {
        //Represents the deck of cards used in the game.
        public Deck MainDeck = new Deck();
        //Represents all players and house
        public Player[] Table { get; }
        //Tracks the active player
        public Player CurrentPlayer { get; private set; }
        //Tracks the end of game
        public bool GameOver { get; private set; } = false;
        //Creates deck, populates table
        public Game(int players)
        {
            this.Table = new Player[players + 1];
            this.Table[0] = new House();
            for (int i = 1; i <= players; i++)
            {
                Console.WriteLine($"Player {i}, enter your name:");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name)) { name = "Player " + i; }
                this.Table[i] = new Player(i, name);
                Console.WriteLine();
            }
            this.CurrentPlayer = Table[1];
        }
        //Makes players discard hands and shuffles deck
        public void CollectCards()
        {
            foreach (Player player in Table)
            {
                player.DiscardHand();
                MainDeck.Shuffle();
            }
        }
        //Keeps game running until GameOver == true.
        public void RunGame()
        {
            while (this.GameOver == false)
            {
                PlayRound();
            }
        }
        //Runs a full round of the game.
        public void PlayRound()
        {
            this.CollectCards();
            this.PlaceBets(Table);
            MainDeck.Deal(Table);
            this.RefreshDisplay(false);

            if (Table[0].HasBlackJackOnDeal())
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

            if (EndGameLogic.PlayerAtTableWonOrLost(Table))
            {
                EndGameLogic.DisplayWinnerAndScores(Table);
                this.GameOver = true;
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("\nPress enter to start next round.");
            Console.ReadLine();
            this.AdvanceCurrentPlayer();
        }
        //Takes appropriate action when house was dealt a natural blackjack
        private void DealerWasDealtBlackJack()
        {
            for (int i = 1; i < Table.Length; i ++)
            {
                if (Table[i].HasBlackJackOnDeal())
                {
                    Table[i].TotalMoney += Table[i].PlayerBet.BetAmount;
                    Console.WriteLine($"{Table[i].PlayerName} also has a natural blackjack and gets their bet returned.");
                }
            }
        }
        //Asks each player if they want to hit or stand.
        private void AskPlayersHitOrStand()
        {
            for (int i = 1; i < Table.Length; i++)
            {
                Player player = Table[i];
                if (player.HasBlackJackOnDeal())
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
                        player.Hit(MainDeck.GetFreshCard());
                        this.RefreshDisplay(false);
                    }
                }
                AdvanceCurrentPlayer();
                this.RefreshDisplay(false);
            }
        }
        //Asks house hit or stand.
        private void AskDealerHitOrStand(){
            while(Table[0].PlayerWantsHit()) 
            {
                Table[0].Hit(MainDeck.GetFreshCard());
                this.RefreshDisplay(true);
            }
            this.RefreshDisplay(true);
        }
        //Allows each player to input their bet.
        private void PlaceBets(Player[] table)
        {
            foreach (Player player in table)
            {
                player.PlaceBet();
                Console.Clear();
            }
        }
        //Refreshes screen to show updated hands, scores, and bets.
        private void RefreshDisplay(bool showHouse)
        {
            Console.Clear();
            DisplayBetsAndScores();
            Console.WriteLine();
            DisplayHands(showHouse);
            Console.WriteLine();
        }
        //Displays scores and current bets
        private void DisplayBetsAndScores()
        {
            for (int i = 1; i < Table.Length; i++)
            {
                Console.WriteLine(Table[i].PlayerName + " bet " + Table[i].PlayerBet.BetAmount + " and has " + Table[i].TotalMoney + " remaining.");
            }
            Console.WriteLine();
        }
        //Displays hands, allowing program to show house's hand or not
        private void DisplayHands(bool showHouse)
        {
            foreach (Player player in Table)
            {
                if (player == CurrentPlayer) { Console.ForegroundColor = ConsoleColor.Red; }
                Console.Write(player.PlayerName + ": ");
                Console.ResetColor();
                string hand;
                hand = player.DisplayHand(showHouse);
                Console.WriteLine(hand);
            }
        }
        //Rotates current player through entire table
        private void AdvanceCurrentPlayer()
        {
            if (CurrentPlayer.PlayerNum == Table.Length - 1)
            {
                CurrentPlayer = Table[0];
            } else
            {
                CurrentPlayer = Table[CurrentPlayer.PlayerNum + 1];
            }
        }
        //Makes appropriate payments for each bet.
        private void ResolveBets()
        {
            House house = (House)Table[0];

            for (int i = 1; i < Table.Length; i++)
            {
                Player player = Table[i];
                if (player.HasBlackJackOnDeal())
                {
                    continue;
                }
                else if (player.Busted || (player.TotalInHand < house.TotalInHand && house.Busted == false))
                {
                    Console.WriteLine($"{player.PlayerName} lost their bet of {player.PlayerBet.BetAmount}.");
                }
                else if (player.TotalInHand == house.TotalInHand)
                {
                    Console.WriteLine($"{player.PlayerName} tied with the House.  The bet of {player.PlayerBet.BetAmount} is returned.");
                    player.PlayerBet.ReturnBet();
                } else
                {
                    Console.WriteLine($"{player.PlayerName} wins {player.PlayerBet.BetAmount * 2}!");
                    player.PlayerBet.NormalPayout();
                }
            }
        }
    }
}
