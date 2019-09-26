using MiniGame.Logic.Entities;
using MiniGame.Logic.Entities.Cells;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MiniGame
{
    public static class CellAppearance
    {
        public const string CELL_BLOCK_IMG_PATH = "images/cell-block.png";

        public const string CELL_EMPTY_IMG_PATH = "images/cell-empty.png";

        static CellAppearance()
        {
            _images = new Dictionary<string, Image>();

            _cardImagesPaths = new Dictionary<CardColors, string>
            {
                { CardColors.Orange, "images/card-orange.png" },
                { CardColors.Red, "images/card-red.png" },
                { CardColors.Yellow, "images/card-yellow.png" }
            };
        }

        public static Image GetCellImage(Cell cell)
        {
            if (cell == null)
                throw new ArgumentNullException();

            var path = GetCellImagePath(cell);

            if (_images.ContainsKey(path))
                return _images[path];

            var image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + path);
            _images.Add(path, image);

            return image;
        }

        public static string GetCellImagePath(Cell cell)
        {
            if (cell == null)
                throw new ArgumentNullException();

            var path = string.Empty;

            if (cell.Type == CellTypes.Empty)
            {
                path = CELL_EMPTY_IMG_PATH;
            }
            else if (cell.Type == CellTypes.Block)
            {
                path = CELL_BLOCK_IMG_PATH;
            }
            else if (cell.Type == CellTypes.Card)
            {
                var card = cell as Card;
                path = GetCardImagePath(card.Color);
            }

            if (string.IsNullOrWhiteSpace(path))
                throw new Exception("Cannot find the image path.");

            return path;
        }

        public static string GetCardImagePath(CardColors color)
        {
            if (!_cardImagesPaths.ContainsKey(color))
                throw new KeyNotFoundException();

            return _cardImagesPaths[color];
        }

        private static readonly Dictionary<string, Image> _images;

        private static readonly Dictionary<CardColors, string> _cardImagesPaths;
    }
}
