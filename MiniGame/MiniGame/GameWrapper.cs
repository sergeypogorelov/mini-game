using MiniGame.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    public struct GameWrapperParams
    {
        public int GameCanvasWidth { get; set; }

        public int GameCanvasHeight { get; set; }

        public int LabelCanvasWidth { get; set; }

        public int LabelCanvasHeight { get; set; }
    }

    public class GameWrapper
    {
        public const int LABEL_HEIGHT = 1;

        public Game Game { get; private set; }

        public GridCanvas GameCanvas { get; private set; }

        public GridCanvas LabelCanvas { get; private set; }

        public Coordinate? SelectedCell { get; set; }

        public GameWrapper(Game game, GameWrapperParams parameters)
        {
            Game = game ?? throw new ArgumentNullException();

            GameCanvas = new GridCanvas(game.Map.Size, game.Map.Size, parameters.GameCanvasWidth, parameters.GameCanvasHeight);
            LabelCanvas = new GridCanvas(game.Map.Size, LABEL_HEIGHT, parameters.LabelCanvasWidth, parameters.LabelCanvasHeight);
        }

        public void MarkCellOnMap(Point point)
        {
            SelectedCell = GameCanvas.ParsePointToCoordinate(point);
        }

        public void RemoveMarkOnMap()
        {
            SelectedCell = null;
        }

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
    }
}
