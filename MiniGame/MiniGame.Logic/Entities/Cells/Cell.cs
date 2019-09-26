namespace MiniGame.Logic.Entities
{
    /// <summary>
    /// Types of map cells
    /// </summary>
    public enum CellTypes
    {
        Empty,
        Block,
        Card
    }

    /// <summary>
    /// Represents a cell on the game map
    /// </summary>
    public abstract class Cell
    {
        /// <summary>
        /// The cell type
        /// </summary>
        public CellTypes Type { get; private set; }

        /// <summary>
        /// Creates a map cell
        /// </summary>
        /// <param name="type">The cell type</param>
        public Cell(CellTypes type)
        {
            Type = type;
        }

        /// <summary>
        /// Checks if the current cell can swap with the specified one
        /// </summary>
        /// <param name="cell">The cell to check the current one</param>
        /// <returns></returns>
        public abstract bool CanSwap(Cell cell);
    }
}
