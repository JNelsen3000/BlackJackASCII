using BlackJack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackJackTests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void BustedReturnsFalseIfOver21()
        {
            // Arrange
            Deck testDeck = new Deck();
            Player testPlayer = new Player(1, "test");
            Player[] table = { testPlayer };

            // Act
            for (int i = 6; i > 0; i--)
            {
                testDeck.Deal(table);
            }
            // Assert
            Assert.IsTrue(testPlayer.Busted);

        }

        [TestMethod]
        [DataRow(1, 2, 3, false)]
        [DataRow(1, 10, 10, false)]
        [DataRow(1, 10, 11, true)]
        [DataRow(10, 1, 6, false)]
        [DataRow(10, 12, 1, true)]
        public void BustedIsAccurate(int a, int b, int c, bool expected)
        {
            // Arrange
            Player testPlayer = new Player(1, "test");
            Card card1 = new Card("sample", "soooooots", a);
            Card card2 = new Card("sample", "soooooots", b);
            Card card3 = new Card("sample", "soooooots", c);
            testPlayer.Hit(card1);
            testPlayer.Hit(card2);
            testPlayer.Hit(card3);

            // Act
            bool result = testPlayer.Busted;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void BustedIsAccurateWithAce()
        {
            // Arrange
            Player testPlayer = new Player(1, "test");
            Card card1 = new Card("Ace", "soooooots", 11);
            Card card2 = new Card("sample", "soooooots", 10);
            Card card3 = new Card("sample", "soooooots", 4);
            testPlayer.Hit(card1);
            testPlayer.Hit(card2);
            testPlayer.Hit(card3);

            // Act
            bool result = testPlayer.Busted;

            // Assert
            Assert.AreEqual(false, result);
        }

    }
}
