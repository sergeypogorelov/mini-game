using MiniGame.Logic;
using MiniGame.Logic.Entities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniGame
{
    /// <summary>
    /// The main form
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Size of the game map
        /// </summary>
        public const int GAME_MAP_SIZE = 5;

        /// <summary>
        /// Message displayed in case the player wins the game
        /// </summary>
        public const string WINNER_MESSAGE = "Congratulations! You are a winner!";

        /// <summary>
        /// The game wrapper instance
        /// </summary>
        public GameWrapper GameWrapper { get; private set; }

        /// <summary>
        /// Creates the main form
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the main form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Called each time the main picture box needs to redraw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            GameWrapper.RenderGame(e.Graphics);
        }

        /// <summary>
        /// Called each time the player clicks on the main picture box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Called each time the label picture box needs to redraw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelPictureBox_Paint(object sender, PaintEventArgs e)
        {
            GameWrapper.RenderLabel(e.Graphics);
        }

        /// <summary>
        /// Called when the player wins the game
        /// </summary>
        private void Game_Victory()
        {
            GameWrapper.RemoveCellMarkFromMap();
            MainPictureBox.Refresh();

            MessageBox.Show(WINNER_MESSAGE);

            Close();
        }

    }
}
