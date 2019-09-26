using MiniGame.Logic.Entities;
using MiniGame.Logic.Entities.Cells;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MiniGame
{
    /// <summary>
    /// This class is responsible for the map cells appearance
    /// </summary>
    public static class CellAppearance
    {
        /// <summary>
        /// Path to the image for a block cell
        /// </summary>
        public const string CELL_BLOCK_IMG_PATH = "Images/cell-block.png";

        /// <summary>
        /// Path to the image for an empty cell
        /// </summary>
        public const string CELL_EMPTY_IMG_PATH = "Images/cell-empty.png";

        /// <summary>
        /// Inits the class
        /// </summary>
        static CellAppearance()
        {
            _images = new Dictionary<string, Image>();

            _cardImagesPaths = new Dictionary<CardColors, string>
            {
                { CardColors.Orange, "Images/card-orange.png" },
                { CardColors.Red, "Images/card-red.png" },
                { CardColors.Yellow, "Images/card-yellow.png" }
            };
        }

        /// <summary>
        /// Returns the respective image for the specified cell
        /// </summary>
        /// <param name="cell">The cell to get the image for</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the respective image path for the specified cell
        /// </summary>
        /// <param name="cell">The cell to get the path for</param>
        /// <returns></returns>
        public static string GetCellImagePath(Cell cell)
        {
            if (cell == null)
                throw new ArgumentNullException();

            string path;

            switch (cell.Type)
            {
                case CellTypes.Empty:
                    path = CELL_EMPTY_IMG_PATH; break;
                case CellTypes.Block:
                    path = CELL_BLOCK_IMG_PATH; break;
                case CellTypes.Card:
                    var card = cell as Card;
                    path = GetCardImagePath(card.Color);
                    break;
                default:
                    throw new Exception("Cannot find the image path.");
            }

            return path;
        }

        /// <summary>
        /// Returns the respective image path for the specified card color
        /// </summary>
        /// <param name="color">The card color</param>
        /// <returns></returns>
        public static string GetCardImagePath(CardColors color)
        {
            if (!_cardImagesPaths.ContainsKey(color))
                throw new KeyNotFoundException();

            return _cardImagesPaths[color];
        }

        /// <summary>
        /// Stores the images by paths
        /// </summary>
        private static readonly Dictionary<string, Image> _images;

        /// <summary>
        /// Stores image paths by card color
        /// </summary>
        private static readonly Dictionary<CardColors, string> _cardImagesPaths;
    }
}
