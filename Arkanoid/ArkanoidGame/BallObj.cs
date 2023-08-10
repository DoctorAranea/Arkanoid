using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid.ArkanoidGame
{
    public class BallObj : GameObject
    {
        public bool Down { get; set; } = false;
        public bool Right { get; set; } = true;
        public int Damage { get; set; } = 1;

        public BallObj(Size size, Point location) : base(size, location)
        {
            Right = Game.Rand.Next(2) == 0;
        }

        public override bool[] Move(Direction direction, int step)
        {
            if (Down) location.Y += step; else location.Y -= step;
            if (Right) location.X += step; else location.X -= step;
            var filterResult = Game.Filter(size, location);
            location = filterResult.Item1;
            return filterResult.Item2;
        }

        public override void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.White),
                    Location.X,
                    Location.Y,
                    Size.Width,
                    Size.Height
                );
        }
    }
}
