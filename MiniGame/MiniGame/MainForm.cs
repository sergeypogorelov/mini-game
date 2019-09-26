using MiniGame.Logic;
using MiniGame.Logic.Entities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniGame
{
    public partial class MainForm : Form
    {
        public const int GAME_MAP_SIZE = 5;

        public const string WINNER_MESSAGE = "Congratulations! You are a winner!";

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
            GameWrapper.Game.Victory += Game_Victory;
        }

        private void MainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            GameWrapper.RenderGame(e.Graphics);
        }

        private void MainPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            var point = new Point { X = e.X, Y = e.Y };

            if (GameWrapper.SelectedCell.HasValue)
            {
                GameWrapper.SwapSelectedCellOnMap(point);
                GameWrapper.RemoveCellMarkFromMap();
            }
            else
            {
                var cell = GameWrapper.GetCellFromMap(point);
                if (cell.Type == CellTypes.Card)
                {
                    GameWrapper.MarkCellOnMap(point);
                }
            }
            
            MainPictureBox.Refresh();
        }

        private void LabelPictureBox_Paint(object sender, PaintEventArgs e)
        {
            GameWrapper.RenderLabel(e.Graphics);
        }

        private void Game_Victory()
        {
            GameWrapper.RemoveCellMarkFromMap();
            MainPictureBox.Refresh();

            MessageBox.Show(WINNER_MESSAGE);

            Close();
        }

    }
}
