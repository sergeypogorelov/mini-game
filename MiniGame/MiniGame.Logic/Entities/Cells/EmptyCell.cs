using System;

namespace MiniGame.Logic.Entities.Cells
{
    /// <summary>
    /// Represents an empty cell on the game map
    /// </summary>
    public class EmptyCell : Cell
    {
        /// <summary>
        /// Creates an empty cell
        /// </summary>
        public EmptyCell()
            :base(CellTypes.Empty)
        {

        }

        public override bool CanSwap(Cell cell)
        {
            if (cell == null)
                throw new ArgumentNullException();

            return cell.Type == CellTypes.Card;
        }
    }
}
