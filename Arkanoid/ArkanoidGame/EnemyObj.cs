using System;
using System.Drawing;

namespace Arkanoid.ArkanoidGame
{
    public class EnemyObj : GameObject
    {
        public int Healthpoints { get; set; } = 1;
        public Color color;

        public EnemyObj(Size size, Point location) : base(size, location)
        {
            color = Color.FromArgb(Game.Rand.Next(255), Game.Rand.Next(255), 0);
        }

        public void TakeDamage(int dmg)
        {
            Healthpoints -= dmg;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(color),
                    Location.X,
                    Location.Y,
                    Size.Width,
                    Size.Height
                );
        }
    }
}
