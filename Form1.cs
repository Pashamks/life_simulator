using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LifeSimulator
{
    public partial class Form1 : Form
    {
        
        private Graphics graphics;
        private int relosution;
        private GameEngine gameEngine;
        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            
            Text = $"Generation {0}";
            if (timer1.Enabled)
                return;

            numResolution.Enabled = false;
            numDensity.Enabled = false;

            relosution = (int)numResolution.Value;

            gameEngine = new GameEngine(
                rows: pictureBox1.Height / relosution,
                cols: pictureBox1.Width / relosution,
                density: (int)numDensity.Minimum + (int)numDensity.Maximum - (int)numDensity.Value);

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }
        private void DrawNextGeneration()
        {
            graphics.Clear(Color.Black);

            var field = gameEngine.GetCurrentGeneration();

            for (int i = 0; i < field.GetLength(0); ++i)
            {
                for (int j = 0; j < field.GetLength(1); ++j)
                {
                    if(field[i, j]){
                        graphics.FillRectangle(Brushes.Crimson, i * relosution, j * relosution, relosution - 1, relosution - 1);
                   }
                }
            }
            pictureBox1.Refresh();
            Text = $"Generation {gameEngine.counterGeneration}";
            gameEngine.NextGeneration();
        }

        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            numResolution.Enabled = true ;
            numDensity.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;

            int x = e.Location.X / relosution, y = e.Location.Y / relosution;
            if (e.Button == MouseButtons.Left)
            {
                gameEngine.AddCell(x, y);
            }
            if (e.Button == MouseButtons.Right)
            {
                gameEngine.RemoveCell(x, y);
            }
        }
        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void bContinue_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled||gameEngine == null)
                return;

            numResolution.Enabled = false;
            numDensity.Enabled = false;

            timer1.Start();
        }
    }
}
