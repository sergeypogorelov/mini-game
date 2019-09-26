using System;

namespace MiniGame.Logic.Entities.Cells
{
    public class EmptyCell : Cell
    {
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
