using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
 using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace flappy_bird
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Rectangle bird;
        double gravity = 0.05;
        double velocity = 0;
        double ugrasSpeed = -3;
        List<Rectangle> oszlopok = new List<Rectangle>();
        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            bird = rectangleBird;
            SetUp();
            Animation();
        }

        public void SetUp()
        {
            int oszlopHeight = 140;
            int oszlopWidht = 40;
            for (int i = 0; i < 1000; i++) 
            {
                int gap = rnd.Next(50, 100);
                if (rnd.Next(2) == 0) gap *= -1;
                 
                Rectangle oszlop = new Rectangle()
                {
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF6CFF00"),
                    Height = oszlopHeight- gap,
                    Width = oszlopWidht
                };

                Rectangle oszlopBottom = new Rectangle()
                {
                    Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF6CFF00"),
                    Height = oszlopHeight+gap,
                    Width = oszlopWidht
                };

                oszlopok.Add(oszlop);
                oszlopok.Add(oszlopBottom);
                canvas.Children.Add(oszlop);
                canvas.Children.Add(oszlopBottom);

                double oszlopLeftPos = 800 + i * 300 + rnd.Next(100, 200);
                Canvas.SetLeft(oszlop, oszlopLeftPos);
                Canvas.SetLeft(oszlopBottom, oszlopLeftPos);
                Canvas.SetTop(oszlop, 0);
                Canvas.SetTop(oszlopBottom, -gap+450-oszlopHeight);
            }
            
        }

        public async void Animation() 
        {
            while (true)
            { 
                //madar

                velocity += gravity;

                double topPos = Canvas.GetTop(bird);

                Canvas.SetTop(bird, topPos+velocity);

                if (topPos >= 450-bird.Height)
                {
                    Halal();
                }

                //--oszlop--
                for (int i = 0; i < oszlopok.Count; i++)
                {
                    double oszlopPos = Canvas.GetLeft(oszlopok[i]);
                    Canvas.SetLeft(oszlopok[i], oszlopPos - 1);
                    if (oszlopPos < -40)
                    {
                        canvas.Children.Remove(oszlopok[i]);
                        oszlopok.RemoveAt(i);
                    }
                }

                await Task.Delay(10);
            }
        }

        public void Ugras()
        {
            velocity = ugrasSpeed;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.Up || e.Key == Key.W)
            {
                Ugras();
            }
        }

        public void Halal() 
        {
            Application.Current.Shutdown();
        }
    }
}