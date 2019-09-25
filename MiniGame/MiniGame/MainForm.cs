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

        private void MainPictureBox_Click(object sender, EventArgs e)
        {
            GameWrapper.Game.CheckIfRiddleSolved();
            MainPictureBox.Refresh();
        }

        private void LabelPictureBox_Paint(object sender, PaintEventArgs e)
        {
            GameWrapper.RenderLabel(e.Graphics);
        }
    }
}
