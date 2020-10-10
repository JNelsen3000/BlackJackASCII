using System;

namespace BlackJack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BlackJack, ASCII Edition!\n\n" +
                "In this version, you'll play until someone has\n" +
                "lost all their points, or has reached 300.\n" +
                "\n" +
                "Enter the number of players (1-4):");
            int playerCount = 0;
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out playerCount))
                {
                    if (playerCount > 0 && playerCount < 5)
                    { 
                    break;
                    }
                }
                Console.WriteLine("Not valid input.  Enter 1, 2, 3, or 4.");
            }
            Console.Clear();
            Game game = new Game(playerCount);
            game.RunGame();
        }
    }
}
