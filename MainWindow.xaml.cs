using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CollisionDetection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Point playerPoint = new Point(400, 500);
        public Point targetPoint = new Point(350, 450);
        public Rectangle targetbox = new Rectangle();
        public Point enemyPoint = new Point(800, 1000);
        Player player = new Player();
        List<Projectile> projectiles = new List<Projectile>();
        DispatcherTimer GameTimer = new DispatcherTimer();
        DispatcherTimer projectileTimer = new DispatcherTimer();
        public Tuple<Rectangle, Point> target;
        

        public MainWindow()
        {
            InitializeComponent();

            bool isGenerated = false;
            if (isGenerated == false)
            {
                player.GeneratePlayer(Canvas, playerPoint);
                isGenerated = true;

                targetbox.Height = 50;
                targetbox.Width = 50;
                targetbox.Fill = Brushes.Blue;
                target = new Tuple<Rectangle, Point>(targetbox, targetPoint);
                Canvas.Children.Add(target.Item1);
                Canvas.SetLeft(target.Item1, target.Item2.X);
                Canvas.SetTop(target.Item1, target.Item2.Y);
            }

            GameTimer.Tick += GameTimer_Tick;
            GameTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);//fps
            GameTimer.Start();

            projectileTimer.Tick += MovementTimer_Tick;
            projectileTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            projectileTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            playerPoint = player.Move(player.tempPlayer.Item1, Canvas, playerPoint);

            if (checkCollision(target, player.tempPlayer))
            {
                player.hit();
            }
        }

        private void MovementTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < projectiles.Count(); i++)
            {
                projectiles[i].move();
                projectiles[i].checkCollision(target);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // MessageBox.Show(e.GetPosition(Canvas).ToString());
            projectiles.Add(new Projectile(Canvas, this, playerPoint));
        }

        public bool checkCollision(Tuple<Rectangle, Point> target, Tuple<Rectangle, Point> source)
        {
            bool temp = false;
            int counter = 0;
            if (playerPoint.X == targetPoint.X)
            {
                counter++;
            }
            if (playerPoint.Y == targetPoint.Y)
            {
                counter++;
            }
            if (playerPoint.X + source.Item1.Width == targetPoint.X + target.Item1.Width)
            {
                counter++;
            }
            if (playerPoint.Y + source.Item1.Height == targetPoint.Y + target.Item1.Height)
            {
                counter++;
            }
            if (counter <= 2)
            {
                temp = true;
            }
            return temp;
        }
    }
}
