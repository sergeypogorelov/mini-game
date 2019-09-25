using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGame
{
    public class GridCanvas
    {
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

        public void Clear()
        {
            _graphicsInstane.Clear(Color.Transparent);
        }

        public void Draw(Image image, int row, int column)
        {
            if (image == null)
                throw new ArgumentNullException();

            if (column < 0 || column >= Width)
                throw new ArgumentOutOfRangeException();

            if (row < 0 || row >= Height)
                throw new ArgumentOutOfRangeException();

            var cellWidth = RenderWidth / Width;
            var cellHeight = RenderHeight / Height;

            var destRect = new Rectangle(column * cellWidth, row * cellHeight, cellWidth, cellHeight);
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
