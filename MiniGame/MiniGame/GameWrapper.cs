﻿using MiniGame.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    public class GameWrapper
    {
        public Game Game { get; private set; }

        public GameCanvas GameCanvas { get; private set; }

        public GameWrapper(Game game, int renderWidth, int renderHeight)
        {
            Game = game ?? throw new ArgumentNullException();
            GameCanvas = new GameCanvas(game.Map.Size, game.Map.Size, renderWidth, renderHeight);
        }

        public void Render(Graphics graphicsInstance)
        {
            if (graphicsInstance == null)
                throw new ArgumentNullException();

            GameCanvas.Clear();

            for (var row = 0; row < Game.Map.Cells.GetLength(0); row++)
            {
                for (var column = 0; column < Game.Map.Cells.GetLength(1); column++)
                {
                    var image = CellAppearance.GetCellImage(Game.Map.Cells[row, column]);
                    GameCanvas.Draw(image, row, column);
                }
            }

            GameCanvas.Render(graphicsInstance);
        }
    }
}