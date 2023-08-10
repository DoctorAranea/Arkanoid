using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arkanoid.ArkanoidGame
{
    public class Game : Control
    {
        public const int CELLSIZE = 25;
        public static Size FieldSize { get; } = new Size(18, 24);
        public static (Point, bool[]) Filter(Size size, Point location)
        {
            Point endLocation = new Point();
            bool[] collisions = new bool[4] { false, false, false, false };

            if (location.X < 0)
            {
                endLocation.X = 0;
                collisions[0] = true;
            }
            else if (location.X + size.Width >= FieldSize.Width * CELLSIZE)
            {
                endLocation.X = FieldSize.Width * CELLSIZE - size.Width;
                collisions[1] = true;
            }
            else
                endLocation.X = location.X;

            if (location.Y < 0)
            {
                endLocation.Y = 0;
                collisions[2] = true;
            }
            else if (location.Y + size.Height >= FieldSize.Height * CELLSIZE)
            {
                endLocation.Y = FieldSize.Height * CELLSIZE - size.Height;
                collisions[3] = true;
            }
            else
                endLocation.Y = location.Y;
            return (endLocation, collisions);
        }
        public static Random Rand { get; } = new Random();
        public Status GameOver { get; set; } = Status.Game;

        public enum Status
        {
            Game,
            Victory,
            GameOver
        }

        PlayerObj player;
        BallObj ball;
        List<GameObject> gameObjects;
        PictureBox picture;

        List<System.Windows.Forms.Timer> fpsTimers;

        public Game() : base()
        {
            Dock = DockStyle.Fill;

            player = new PlayerObj(new Size(4, 1), new Point(FieldSize.Width / 2 - 2, FieldSize.Height - FieldSize.Height / 5));
            ball = new BallObj(new Size(1, 1), new Point(Rand.Next(3, FieldSize.Width - 4), FieldSize.Height / 2));

            gameObjects = new List<GameObject>();
            gameObjects.Add(player);
            gameObjects.Add(ball);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (i == 2 && j > 1 && j < 7) continue;
                    if (i == 5 && j > 0 && j < 8) continue;
                    if (i >= 8 && j > 0 && j < 8) continue;
                    var enemy = new EnemyObj(new Size(2, 1), new Point(j * 2, i));
                    gameObjects.Add(enemy);
                }
            }

            picture = new PictureBox();
            picture.Parent = this;
            picture.Dock = DockStyle.Fill;
            picture.Paint += new PaintEventHandler(Picture_Paint);
            picture.MouseClick += new MouseEventHandler(OnPictureClicked);

            fpsTimers = new List<System.Windows.Forms.Timer>();
            for (int i = 0; i < 2; i++)
            {
                System.Windows.Forms.Timer fpsTimer = new System.Windows.Forms.Timer();
                fpsTimer = new System.Windows.Forms.Timer();
                fpsTimer.Interval = 1;
                fpsTimer.Tick += FpsTimer_Tick;
                fpsTimer.Start();
                fpsTimers.Add(fpsTimer);
            }
        }

        public void OnKeyDown(GameObject.Direction direction)
        {
            player.Move(direction, CELLSIZE / 2);
        }

        private void FpsTimer_Tick(object sender, EventArgs e)
        {
            MoveBall();
            player.ChangeLevitation();
            picture.Invalidate();
        }

        private void MoveBall()
        {
            var collisions = ball.Move(GameObject.Direction.Down, 2);
            CheckWallCollisions(collisions);
            CheckBallCollisions(player);
            foreach (var enemy in gameObjects.Where(x => x is EnemyObj).Select(x => x as EnemyObj).ToList())
                CheckBallCollisions(enemy);
            gameObjects.RemoveAll(x => x is EnemyObj && (x as EnemyObj).Healthpoints <= 0);
        }

        private void CheckWallCollisions(bool[] collisions)
        {
            if (collisions[0]) ball.Right = true;
            if (collisions[1]) ball.Right = false;
            if (collisions[2]) ball.Down = true;
            if (collisions[3])
            {
                fpsTimers.ForEach(x => x.Stop());
                GameOver = Status.GameOver;
            }
        }

        private void CheckBallCollisions(GameObject gameObject)
        {
            bool containsX = false;
            bool containsY = false;
            if (ball.RightWall >= gameObject.LeftWall && ball.LeftWall < gameObject.RightWall)
                containsX = true;
            if (ball.DownWall >= gameObject.UpWall && ball.UpWall < gameObject.DownWall)
                containsY = true;
            if (containsX && containsY)
            {
                if (Enumerable.Range(-5, 10).Contains(gameObject.LeftWall - ball.RightWall))
                    ball.Right = false;
                if (Enumerable.Range(-5, 10).Contains(gameObject.RightWall - ball.LeftWall))
                    ball.Right = true;
                if (Enumerable.Range(-5, 10).Contains(gameObject.UpWall - ball.DownWall))
                {
                    ball.Down = false;
                    if (gameObject is PlayerObj)
                        player.Levitation += 200;
                }
                if (Enumerable.Range(-5, 10).Contains(gameObject.DownWall - ball.UpWall))
                    ball.Down = true;
                if (gameObject is EnemyObj)
                    (gameObject as EnemyObj).TakeDamage(ball.Damage);
                if (gameObjects.Where(x => x is EnemyObj).ToList().Count == 0)
                {
                    fpsTimers.ForEach(x => x.Stop());
                    GameOver = Status.Victory;
                }
            }
        }

        private void OnPictureClicked(object sender, MouseEventArgs e)
        {

        }

        private void Picture_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.DarkBlue), 0, 0, FieldSize.Width * CELLSIZE, FieldSize.Height * CELLSIZE);

            if (GameOver == Status.GameOver)
            {
                g.DrawString(
                    "GAME OVER",
                    new Font("Comic Sans MS", 24, FontStyle.Bold),
                    new SolidBrush(Color.Yellow),
                    new RectangleF(0, 0, FieldSize.Width * CELLSIZE, FieldSize.Height * CELLSIZE),
                    new StringFormat()
                    { 
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center 
                    }
                );
                return;
            }
            if (GameOver == Status.Victory)
            {
                g.DrawString(
                    "VICTORY",
                    new Font("Comic Sans MS", 48, FontStyle.Bold),
                    new SolidBrush(Color.Yellow),
                    new RectangleF(0, 0, FieldSize.Width * CELLSIZE, FieldSize.Height * CELLSIZE),
                    new StringFormat()
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    }
                );
                return;
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(g);
            }
        }
    }
}
