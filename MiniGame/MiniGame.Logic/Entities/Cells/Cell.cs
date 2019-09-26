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

        public abstract bool CanSwap(Cell cell);
    }
}
