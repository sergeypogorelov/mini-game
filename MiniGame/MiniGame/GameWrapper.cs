using MiniGame.Logic;
using MiniGame.Logic.Entities;
using System;
using System.Drawing;
using System.Linq;

namespace MiniGame
{
    /// <summary>
    /// Parameters used to initialize the game wrapper
    /// </summary>
    public struct GameWrapperParams
    {
        public int GameCanvasWidth { get; set; }

        public int GameCanvasHeight { get; set; }

        public int LabelCanvasWidth { get; set; }

        public int LabelCanvasHeight { get; set; }
    }

    /// <summary>
    /// Wrapper of the game instance
    /// </summary>
    public class GameWrapper
    {
        /// <summary>
        /// Height of the label showing the goal cards
        /// </summary>
        public const int LABEL_HEIGHT = 1;

        /// <summary>
        /// Game instance
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// Canvas to render the game world
        /// </summary>
        public GridCanvas GameCanvas { get; private set; }

        /// <summary>
        /// Canvas to render the label showing the goal cards
        /// </summary>
        public GridCanvas LabelCanvas { get; private set; }

        /// <summary>
        /// Currently selected cell
        /// </summary>
        public Coordinate? SelectedCell { get { return _selectedCell; } }

        /// <summary>
        /// Creates an instance of the game wrapper
        /// </summary>
        /// <param name="game">Game instance to wrap</param>
        /// <param name="parameters">Parameters to initialize the game wrapper</param>
        public GameWrapper(Game game, GameWrapperParams parameters)
        {
            Game = game ?? throw new ArgumentNullException();

            GameCanvas = new GridCanvas(game.Map.Size, game.Map.Size, parameters.GameCanvasWidth, parameters.GameCanvasHeight);
            LabelCanvas = new GridCanvas(game.Map.Size, LABEL_HEIGHT, parameters.LabelCanvasWidth, parameters.LabelCanvasHeight);
        }

        /// <summary>
        /// Searches for the cell on the map and returns it
        /// </summary>
        /// <param name="point">2D coordinate of the cell to search for</param>
        /// <returns></returns>
        public Cell GetCellFromMap(Point point)
        {
            var coordinate = GameCanvas.ParsePointToCoordinate(point);

            if (!Game.Map.CheckIfCoordinateIsValid(coordinate))
                throw new ArgumentOutOfRangeException();

            return Game.Map[coordinate];
        }

        /// <summary>
        /// Marks the cell on the map
        /// </summary>
        /// <param name="point">2D coordinate of the cell to mark</param>
        public void MarkCellOnMap(Point point)
        {
            var coordinate = GameCanvas.ParsePointToCoordinate(point);

            if (!Game.Map.CheckIfCoordinateIsValid(coordinate))
                throw new ArgumentOutOfRangeException();

            _selectedCell = coordinate;
        }

        /// <summary>
        /// Removes the mark of the currently selected cell
        /// </summary>
        public void RemoveCellMarkFromMap()
        {
            _selectedCell = null;
        }

        /// <summary>
        /// Swaps the currently selected cell with the specified cell by 2D coordinate
        /// </summary>
        /// <param name="point">2D coordinate of the cell which is going to be swapped</param>
        /// <returns></returns>
        public bool SwapSelectedCellOnMap(Point point)
        {
            if (!SelectedCell.HasValue)
                return false;

            var coordinate = GameCanvas.ParsePointToCoordinate(point);
            return Game.Map.SwapCells(SelectedCell.Value, coordinate);
        }

        /// <summary>
        /// Renders the game on the specified graphics instance
        /// </summary>
        /// <param name="graphicsInstance">Graphics instance to draw on</param>
        public void RenderGame(Graphics graphicsInstance)
        {
            if (graphicsInstance == null)
                throw new ArgumentNullException();

            GameCanvas.Clear();

            for (var row = 0; row < Game.Map.Cells.GetLength(0); row++)
            {
                for (var column = 0; column < Game.Map.Cells.GetLength(1); column++)
                {
                    var image = CellAppearance.GetCellImage(Game.Map.Cells[row, column]);
                    GameCanvas.Draw(image, new Coordinate { Row = row, Column = column });
                }
            }

            if (SelectedCell.HasValue)
            {
                GameCanvas.MarkCell(SelectedCell.Value);
            }

            GameCanvas.Render(graphicsInstance);
        }

        /// <summary>
        /// Renders the label on the specified graphics instance
        /// </summary>
        /// <param name="graphicsInstance">Graphics instance to draw on</param>
        public void RenderLabel(Graphics graphicsInstance)
        {
            if (graphicsInstance == null)
                throw new ArgumentNullException();

            LabelCanvas.Clear();

            var indexesOfColumnsWithCards = Enumerable.Range(0, Game.Map.Size).Where(i => i % 2 == 0).ToArray();
            foreach (var index in indexesOfColumnsWithCards)
            {
                var image = CellAppearance.GetCellImage(Game.GoalCards[index / 2]);
                LabelCanvas.Draw(image, new Coordinate { Row = 0, Column = index });
            }

            LabelCanvas.Render(graphicsInstance);
        }

        /// <summary>
        /// Currently selected cell
        /// </summary>
        private Coordinate? _selectedCell;
    }
}
