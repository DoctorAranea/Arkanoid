using System.Windows.Forms;

namespace Arkanoid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            ArkanoidGame.GameObject.Direction direction;
            switch (e.KeyCode)
            {
                case Keys.W:
                    direction = ArkanoidGame.GameObject.Direction.Up;
                    break;
                case Keys.S:
                    direction = ArkanoidGame.GameObject.Direction.Down;
                    break;
                case Keys.A:
                    direction = ArkanoidGame.GameObject.Direction.Left;
                    break;
                case Keys.D:
                    direction = ArkanoidGame.GameObject.Direction.Right;
                    break;
                case Keys.R:
                    panel1.Controls.Clear();
                    game1 = new ArkanoidGame.Game();
                    panel1.Controls.Add(game1);
                    return;
                default:
                    return;
            }
            game1.OnKeyDown(direction);
        }
    }
}
