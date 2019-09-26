using System;

namespace MiniGame.Logic.Entities.Cells
{
    /// <summary>
    /// Represents a block on the game map
    /// </summary>
    public class Block : Cell
    {
        /// <summary>
        /// Creates an instance of a block
        /// </summary>
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
