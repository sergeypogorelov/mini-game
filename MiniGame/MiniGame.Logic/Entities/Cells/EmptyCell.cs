using System;
using System.Collections.Generic;
using System.Text;

namespace MiniGame.Logic.Entities.Cells
{
    public class EmptyCell : Cell
    {
        public EmptyCell()
            :base(CellTypes.Empty)
        {

        }
    }
}
