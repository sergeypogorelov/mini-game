using System;
using System.Collections.Generic;
using System.Text;

namespace MiniGame.Logic.Entities.Cells
{
    public class Block : Cell
    {
        public Block()
            :base(CellTypes.Block)
        {

        }

        public override bool CanSwap(Cell cell)
        {
            if (cell == null)
                throw new ArgumentNullException();

            /// blocks cannot be moved
            return false;
        }
    }
}
