using MiniGame.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniGame
{
    public partial class MainForm : Form
    {
        public const int GAME_MAP_SIZE = 5;

        public GameWrapper GameWrapper { get; private set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var gameWrapperParams = new GameWrapperParams
            {
                GameCanvasWidth = MainPictureBox.Width,
                GameCanvasHeight = MainPictureBox.Height,
                LabelCanvasWidth = LabelPictureBox.Width,
                LabelCanvasHeight = LabelPictureBox.Height
            };

            GameWrapper = new GameWrapper(new Game(GAME_MAP_SIZE), gameWrapperParams);
        }

        private void MainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            GameWrapper.RenderGame(e.Graphics);
        }

        private void MainPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            GameWrapper.MarkCellOnMap(new Point { X = e.X, Y = e.Y });
            MainPictureBox.Refresh();
        }

        private void LabelPictureBox_Paint(object sender, PaintEventArgs e)
        {
            GameWrapper.RenderLabel(e.Graphics);
        }
        
    }
}
