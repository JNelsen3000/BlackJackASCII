using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public static class EndGameLogic
    {
        // Determines if an end game condition was met: player loses or earns 300.
        public static bool PlayerAtTableWonOrLost(Player[] table)
        {
            for (int i = 1; i < table.Length; i++)
            {
                Player player = table[i];
                if (player.TotalMoney == 0 || player.TotalMoney > 299)
                {
                    return true;
                }
            }
            return false;
        }
        // Displays final scores and invokes DisplayWinners
        public static void DisplayWinnerAndScores(Player[] table)
        {
            List<string> winnerList = new List<string>();
            int currentHighest = 0;
            foreach (Player player in table)
            {
                winnerList.Add(player.PlayerName);
                if (player.PlayerName != "House")
                {
                    if (player.TotalMoney > currentHighest) { currentHighest = player.TotalMoney; }
                    Console.Write($"{player.PlayerName} has {player.TotalMoney}.  ");
                }
            }
            Console.WriteLine("\n");
            if (currentHighest == 0) { Console.WriteLine("The house wins!"); }
            else
            {
                foreach (Player player in table)
                {
                    if (player.TotalMoney != currentHighest)
                    {
                        winnerList.Remove(player.PlayerName);
                    }
                }
                DisplayWinners(winnerList);
            }
        }
        // Displays winner if singular and all winners if plural.
        public static void DisplayWinners(List<string> winnerList)
        {
            if (winnerList.Count > 1)
            {
                winnerList.Insert(winnerList.Count - 1, "and");
                string listOfWinners = "";
                foreach (string name in winnerList)
                {
                    listOfWinners += name + " ";
                }
                Console.WriteLine(listOfWinners + "win!");
            }
            else
            {
                Console.WriteLine("\n" + winnerList[0].ToString() + " wins!");
            }
            Console.WriteLine();
        }
    }
}
