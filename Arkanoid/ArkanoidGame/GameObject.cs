using System.Drawing;

namespace Arkanoid.ArkanoidGame
{
    public abstract class GameObject
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public Point Location { get => location; }
        public Size Size { get => size; }

        protected Point location = new Point(0, 0);
        protected Size size = new Size(0, 0);

        public int UpWall { get => location.Y; }
        public int DownWall { get => location.Y + size.Height; }
        public int LeftWall { get => location.X; }
        public int RightWall { get => location.X + size.Width; }

        public GameObject(Size size, Point location)
        {
            this.size = new Size(size.Width * Game.CELLSIZE, size.Height * Game.CELLSIZE);
            this.location = new Point(location.X * Game.CELLSIZE, location.Y * Game.CELLSIZE);
            this.location = Game.Filter(this.size, this.location).Item1;
        }
        
        public virtual bool[] Move(Direction direction, int step)
        {
            switch (direction)
            {
                case Direction.Up:
                    location.Y -= step;
                    break;
                case Direction.Down:
                    location.Y += step;
                    break;
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

        public virtual void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.White),
                    Location.X,
                    Location.Y,
                    Size.Width,
                    Size.Height
                );
        }
    }
}
