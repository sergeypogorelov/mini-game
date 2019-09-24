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

        public Game Game { get; private set; }

        public RenderHost RenderHost { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            Game = new Game(GAME_MAP_SIZE);
            RenderHost = new RenderHost(Game.Map.Size, Game.Map.Size, MainPictureBox.Width, MainPictureBox.Height);
        }

        private void MainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            RenderHost.Clear();
            RenderHost.Render(e.Graphics);
        }

        private void MainPictureBox_Click(object sender, EventArgs e)
        {
            Game.CheckIfRiddleSolved();
            MainPictureBox.Refresh();
        }
    }
}
