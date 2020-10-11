using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class House : Player
    {
        public House() : base(0, "House")
        {
            this.TotalMoney = 0;
        }

        public override bool IsHouse => true;

        /// <summary>
        /// Returns true if house is at 17 or higher.
        /// </summary>
        /// <returns>true if house is at 17 or higher, otherwise false</returns>
        public override bool PlayerWantsHit()
        {
            return this.TotalInHand < 17;
        }
        
        public override void PlaceBet()
        {
        }
        
        public override string DisplayHand(bool showHouse)
        {
            if (showHouse)
            {
                return base.DisplayHand(showHouse);
            }

            return $"Facedown card, {Hand[1].ViewCard()}\n";
        }

    }
}
