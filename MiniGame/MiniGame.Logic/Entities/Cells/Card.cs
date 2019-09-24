using System;
using System.Collections.Generic;
using System.Text;

namespace MiniGame.Logic.Entities.Cells
{
    public enum CardColors
    {
        Yellow,
        Orange,
        Red
    }

    public class Card : Cell
    {
        public const int MIN_COUNT_OF_COLORS = 2;

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

        public CardColors Color { get; private set; }

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
