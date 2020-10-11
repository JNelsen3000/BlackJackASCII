using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack
{
    public static class EndGameLogic
    {
        /// <summary>
        /// Determines if an end game condition was met: player loses or earns 300.
        /// </summary>
        /// <param name="table">The players at the table</param>
        /// <returns>True if a player has won or lost, otherwise false</returns>
        public static bool PlayerAtTableWonOrLost(IEnumerable<Player> table)
        {
            return table.Where(p => !p.IsHouse)
                        .Any(player => player.TotalMoney == 0 || player.TotalMoney > 299);
        }
        
        /// <summary>
        /// Displays final scores and invokes DisplayWinners
        /// </summary>
        /// <param name="table">The players at the table</param>
        public static void DisplayWinnerAndScores(Player[] table)
        {
            DisplayPlayerMoney(table);

            int highest = table.Where(p => !p.IsHouse)
                               .Max(p => p.TotalMoney);

            Console.WriteLine("\n");
            if (highest == 0)
            {
                Console.WriteLine("The house wins!");
            }
            else
            {
                List<string> winners = 
                    table.Where(p => p.TotalMoney == highest)
                         .Select(p => p.PlayerName)
                         .ToList();
                
                DisplayWinners(winners);
            }
        }

        private static void DisplayPlayerMoney(IEnumerable<Player> table)
        {
            foreach (Player player in table.Where(p => !p.IsHouse))
            {
                Console.Write($"{player.PlayerName} has {player.TotalMoney:C}.  ");
            }
        }

        /// <summary>
        /// Displays winner if singular and all winners if plural.
        /// </summary>
        /// <param name="winnerList">The names of the players who won</param>
        private static void DisplayWinners(IList<string> winnerList)
        {
            if (winnerList.Count > 1)
            {
                winnerList.Insert(winnerList.Count - 1, "and");
                StringBuilder listOfWinners = new StringBuilder();
                foreach (string name in winnerList)
                {
                    listOfWinners.Append($"{name} ");
                }

                listOfWinners.Append("win!");
                Console.WriteLine(listOfWinners.ToString());
            }
            else
            {
                Console.WriteLine($"\n{winnerList.First()} wins!");
            }
            Console.WriteLine();
        }
    }
}
