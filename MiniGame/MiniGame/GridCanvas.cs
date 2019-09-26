using MiniGame.Logic;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MiniGame
{
    public class GridCanvas
    {
        public const int CELL_BORDER_WIDTH = 3;

        public static bool CheckRenderSize(int renderSize, int size)
        {
            if (renderSize <= 0)
                throw new ArgumentOutOfRangeException();

            if (size <= 0)
                throw new ArgumentOutOfRangeException();

            return renderSize % size == 0;
        }

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

        public int CellWidth { get { return RenderWidth / Width; } }

        public int CellHeight { get { return RenderHeight / Height; } }

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

        public bool CheckCoordinate(Coordinate coordinate)
        {
            return  coordinate.Column >= 0 && coordinate.Column < Width &&
                    coordinate.Row >= 0 && coordinate.Row < Height;
        }

        public bool CheckPoint(Point point)
        {
            return  point.X >= 0 && point.X < RenderWidth &&
                    point.Y >= 0 && point.Y < RenderHeight;
        }

        public Coordinate ParsePointToCoordinate(Point point)
        {
            if (!CheckPoint(point))
                throw new ArgumentOutOfRangeException();

            var row = point.Y / CellHeight;
            var column = point.X / CellWidth;

            return new Coordinate { Row = row, Column = column };
        }

        public void Clear()
        {
            _graphicsInstane.Clear(Color.Transparent);
        }

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

        public void Render(Graphics graphicsHost)
        {
            if (graphicsHost == null)
                throw new ArgumentNullException();

            graphicsHost.DrawImage(_bitmapInstance, 0, 0);
        }

        private int _width;

        private int _height;

        private int _renderWidth;

        private int _renderHeight;

        private Bitmap _bitmapInstance;

        private Graphics _graphicsInstane;
    }
}
