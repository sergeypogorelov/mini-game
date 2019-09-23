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
        public static CardColors[] GetAllAvailableColors()
        {
            return new CardColors[3] { CardColors.Orange, CardColors.Red, CardColors.Yellow };
        }

        public CardColors Color { get; private set; }

        public Card(CardColors color)
            :base(CellTypes.Card)
        {
            Color = color;
        }
    }
}
