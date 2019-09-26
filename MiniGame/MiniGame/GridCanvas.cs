using MiniGame.Logic;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MiniGame
{
    /// <summary>
    /// This canvas allows to draw using grid coordinate system
    /// </summary>
    public class GridCanvas
    {
        /// <summary>
        /// Width of the border for marking a cell
        /// </summary>
        public const int CELL_BORDER_WIDTH = 3;

        /// <summary>
        /// Checks if count of rows/columns is compatible with height/width in pixels
        /// </summary>
        /// <param name="renderSize">height or width</param>
        /// <param name="size">row or width</param>
        /// <returns></returns>
        public static bool CheckRenderSize(int renderSize, int size)
        {
            if (renderSize <= 0)
                throw new ArgumentOutOfRangeException();

            if (size <= 0)
                throw new ArgumentOutOfRangeException();

            return renderSize % size == 0;
        }

        /// <summary>
        /// Count of columns in grid
        /// </summary>
        public int Width
        {
            get { return _width; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();

                _width = value;
            }
        }

        /// <summary>
        /// Count of rows in grid
        /// </summary>
        public int Height
        {
            get { return _height; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();

                _height = value;
            }
        }

        /// <summary>
        /// Canvas width in pixels
        /// </summary>
        public int RenderWidth
        {
            get { return _renderWidth; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();

                _renderWidth = value;
            }
        }

        /// <summary>
        /// Canvas height in pixels
        /// </summary>
        public int RenderHeight
        {
            get { return _renderHeight; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();

                _renderHeight = value;
            }
        }

        /// <summary>
        /// Width in pixels of one cell
        /// </summary>
        public int CellWidth { get { return RenderWidth / Width; } }

        /// <summary>
        /// Height in pixels of one cell
        /// </summary>
        public int CellHeight { get { return RenderHeight / Height; } }

        /// <summary>
        /// Creates an instance of grid canvas
        /// </summary>
        /// <param name="width">Count of columns in grid</param>
        /// <param name="height">Count of rows in grid</param>
        /// <param name="renderWidth">Canvas width in pixels</param>
        /// <param name="renderHeight">Canvas height in pixels</param>
        public GridCanvas(int width, int height, int renderWidth, int renderHeight)
        {
            Width = width;
            Height = height;

            RenderWidth = renderWidth;
            RenderHeight = renderHeight;

            if (!CheckRenderSize(RenderWidth, Width))
                throw new ArgumentException("Render width is incompatible.");

            if (!CheckRenderSize(RenderHeight, Height))
                throw new ArgumentException("Render height is incompatible.");

            _bitmapInstance = new Bitmap(RenderWidth, RenderHeight);
            _graphicsInstane = Graphics.FromImage(_bitmapInstance);
        }

        /// <summary>
        /// Checks coordinate if it's within canvas area
        /// </summary>
        /// <param name="coordinate">Grid coordinate</param>
        /// <returns></returns>
        public bool CheckCoordinate(Coordinate coordinate)
        {
            return  coordinate.Column >= 0 && coordinate.Column < Width &&
                    coordinate.Row >= 0 && coordinate.Row < Height;
        }

        /// <summary>
        /// Checks coordinate if it's within canvas area 
        /// </summary>
        /// <param name="point">2D coordinate</param>
        /// <returns></returns>
        public bool CheckPoint(Point point)
        {
            return  point.X >= 0 && point.X < RenderWidth &&
                    point.Y >= 0 && point.Y < RenderHeight;
        }

        /// <summary>
        /// Parses 2D coordinate into grid coordinate
        /// </summary>
        /// <param name="point">2D coordinate to parse</param>
        /// <returns></returns>
        public Coordinate ParsePointToCoordinate(Point point)
        {
            if (!CheckPoint(point))
                throw new ArgumentOutOfRangeException();

            var row = point.Y / CellHeight;
            var column = point.X / CellWidth;

            return new Coordinate { Row = row, Column = column };
        }

        /// <summary>
        /// Clears out the canvas
        /// </summary>
        public void Clear()
        {
            _graphicsInstane.Clear(Color.Transparent);
        }

        /// <summary>
        /// Marks a cell specified by the coordinate
        /// </summary>
        /// <param name="coordinate">Coordinate of the cell to mark</param>
        public void MarkCell(Coordinate coordinate)
        {
            if (!CheckCoordinate(coordinate))
                throw new ArgumentOutOfRangeException();

            using (var pen = new Pen(Color.White, CELL_BORDER_WIDTH))
            {
                pen.DashStyle = DashStyle.Dot;

                var rectangle = new Rectangle(coordinate.Column * CellWidth, coordinate.Row * CellHeight, CellWidth, CellHeight);
                _graphicsInstane.DrawRectangle(pen, rectangle);
            }
        }

        /// <summary>
        /// Draws an image specified by the coordinate
        /// </summary>
        /// <param name="image">Image to draw</param>
        /// <param name="coordinate">Coordinate of the cell to draw the image on</param>
        public void Draw(Image image, Coordinate coordinate)
        {
            if (image == null)
                throw new ArgumentNullException();

            if (!CheckCoordinate(coordinate))
                throw new ArgumentOutOfRangeException();

            var destRect = new Rectangle(coordinate.Column * CellWidth, coordinate.Row * CellHeight, CellWidth, CellHeight);
            var srcRect = new Rectangle(0, 0, image.Width, image.Height);

            _graphicsInstane.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Renders the canvas on the specified graphics instance
        /// </summary>
        /// <param name="graphicsInstance">Graphics instance to draw on</param>
        public void Render(Graphics graphicsInstance)
        {
            if (graphicsInstance == null)
                throw new ArgumentNullException();

            graphicsInstance.DrawImage(_bitmapInstance, 0, 0);
        }

        /// <summary>
        /// Count of columns in grid
        /// </summary>
        private int _width;

        /// <summary>
        /// Count of rows in grid
        /// </summary>
        private int _height;

        /// <summary>
        /// Canvas width in pixels
        /// </summary>
        private int _renderWidth;

        /// <summary>
        /// Canvas height in pixels
        /// </summary>
        private int _renderHeight;

        /// <summary>
        /// Bitmap instance used for rendering
        /// </summary>
        private Bitmap _bitmapInstance;

        /// <summary>
        /// Graphics instance used for rendering
        /// </summary>
        private Graphics _graphicsInstane;
    }
}
