using System;
using System.Collections.Generic;
using System.Text;

namespace MiniGame.Logic.Entities
{
    public enum CellTypes
    {
        Empty,
        Block,
        Card
    }

    public abstract class Cell
    {
        public CellTypes Type { get; private set; }

        public Cell(CellTypes type)
        {
            Type = type;
        }
    }
}
