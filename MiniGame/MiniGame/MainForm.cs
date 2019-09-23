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
        public RenderHost RenderHost { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            RenderHost = new RenderHost(5, 5, MainPictureBox.Width, MainPictureBox.Height);
        }

        private void MainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            RenderHost.Clear();
            RenderHost.Render(e.Graphics);
        }

        private void MainPictureBox_Click(object sender, EventArgs e)
        {
            var image = Image.FromFile("./images/img.png");
            RenderHost.Draw(image, 0, 4);
            MainPictureBox.Refresh();
        }
    }
}
