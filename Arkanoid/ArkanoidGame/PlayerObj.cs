using System;
using System.Drawing;

namespace Arkanoid.ArkanoidGame
{
    public class PlayerObj : GameObject
    {
        public bool LevitationDirection { get; set; } = true;
        public int Levitation { get; set; } = 0;

        public PlayerObj(Size size, Point location) : base(size, location)
        {

        }

        public override bool[] Move(Direction direction, int step)
        {
            switch (direction)
            {
                case Direction.Left:
                    location.X -= step;
                    break;
                case Direction.Right:
                    location.X += step;
                    break;
            }
            var filterResult = Game.Filter(size, location);
            location = filterResult.Item1;
            return filterResult.Item2;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.White),
                    Location.X,
                    Location.Y + Levitation / 20,
                    Size.Width,
                    Size.Height
                );
        }

        public void ChangeLevitation()
        {
            if (LevitationDirection)
                Levitation++;
            else
                Levitation--;

            if (Levitation >= 100)
            {
                LevitationDirection = false;
            }
            else if (Levitation < -100)
            {
                LevitationDirection = true;
            }

        }
    }
}
