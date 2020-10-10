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
        //Returns true if house is at 17 or higher.
        public override bool PlayerWantsHit()
        {
            if (this.TotalInHand < 17)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Does nothing, allows ease of manipulation in Game.
        public override void PlaceBet()
        {
            return;
        }
        //Displays full hand if showHouse == true, otherwise
        //hides first card.
        public override string DisplayHand(bool showHouse)
        {
            if (showHouse)
            {
                return base.DisplayHand(showHouse);
            }
            else
            {
                return "Facedown card, " + Hand[1].ViewCard() + "\n";
            }
        }

    }
}
