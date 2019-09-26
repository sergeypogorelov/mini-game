using System;

namespace MiniGame.Logic.Entities.Cells
{
    /// <summary>
    /// Colors of cards on the game map
    /// </summary>
    public enum CardColors
    {
        Yellow,
        Orange,
        Red
    }

    /// <summary>
    /// Represent a card on the game map
    /// </summary>
    public class Card : Cell
    {
        /// <summary>
        /// Min count of colors for cards
        /// </summary>
        public const int MIN_COUNT_OF_COLORS = 2;

        /// <summary>
        /// Returns all available card colors
        /// </summary>
        /// <returns></returns>
        public static CardColors[] GetAllAvailableColors()
        {
            var result = new CardColors[]
            {
                CardColors.Orange,
                CardColors.Red,
                CardColors.Yellow
            };

            if (result.Length < MIN_COUNT_OF_COLORS)
                throw new Exception($"There should be at least {MIN_COUNT_OF_COLORS} colors.");

            return result;
        }

        /// <summary>
        /// Color of the card
        /// </summary>
        public CardColors Color { get; private set; }

        /// <summary>
        /// Creates an instance of a card
        /// </summary>
        /// <param name="color">The card color</param>
        public Card(CardColors color)
            :base(CellTypes.Card)
        {
            Color = color;
        }

        public override bool CanSwap(Cell cell)
        {
            if (cell == null)
                throw new ArgumentNullException();

            return cell.Type == CellTypes.Card || cell.Type == CellTypes.Empty;
        }
    }
}
