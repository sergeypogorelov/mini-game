using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    public class RenderHost
    {
        public static bool CheckRenderSize(int renderSize, int cellSize)
        {
            if (renderSize <= 0)
                throw new ArgumentOutOfRangeException();

            if (cellSize <= 0)
                throw new ArgumentOutOfRangeException();

            return renderSize % cellSize == 0;
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

        public int CellWidth
        {
            get { return RenderWidth / Width; }
        }

        public int CellHeight
        {
            get { return RenderHeight / Height; }
        }

        public RenderHost(int width, int height, int renderWidth, int renderHeight)
        {
            Width = width;
            Height = height;

            RenderWidth = renderWidth;
            RenderHeight = renderHeight;

            if (!CheckRenderSize(RenderWidth, Width))
                throw new ArgumentException("Render width is incompatible.");

            if (!CheckRenderSize(RenderHeight, Height))
                throw new ArgumentException("Render height is incompatible.");

            _bitmapHost = new Bitmap(RenderWidth, RenderHeight);
            _graphicsHost = Graphics.FromImage(_bitmapHost);
        }

        public void Clear()
        {
            _bitmapHost.MakeTransparent();
        }

        public void Draw(Image image, int row, int column)
        {
            if (image == null)
                throw new ArgumentNullException();

            if (column < 0 || column >= Width)
                throw new ArgumentOutOfRangeException();

            if (row < 0 || row >= Height)
                throw new ArgumentOutOfRangeException();

            var destRect = new Rectangle(column * CellWidth, row * CellHeight, CellWidth, CellHeight);
            var srcRect = new Rectangle(0, 0, image.Width, image.Height);

            _graphicsHost.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
        }

        public void Render(Graphics graphicsHost)
        {
            if (graphicsHost == null)
                throw new ArgumentNullException();

            graphicsHost.DrawImage(_bitmapHost, new Point(0, 0));
        }

        private int _width;

        private int _height;

        private int _renderWidth;

        private int _renderHeight;

        private Bitmap _bitmapHost;

        private Graphics _graphicsHost;
    }
}
