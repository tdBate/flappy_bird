using System.Diagnostics;
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
        //fizikai változók
        double gravity = 0.05;
        double velocity = 0;
        double ugrasSpeed = 2.5;

        //objektumok
        Rectangle bird;
        List<Rectangle> oszlopokTop = new List<Rectangle>();
        List<Rectangle> oszlopokBot = new List<Rectangle>();
        Random rnd = new Random();
        Rectangle elozoOszlop;

        //logikai változók
        int pontSzam = 0;
        bool athaladas = true;

        public MainWindow()
        {
            InitializeComponent();
            bird = rectangleBird;
            SetUp();
            Animation();
        } 

        public async void SetUp()
        {
            for (int i = 0; i < 8; i++) 
            {
                UjOszlop();
                //await Task.Delay(3000);
            }
            
        }

        public void UjOszlop()
        {
            int oszlopHeight = 140;
            int oszlopWidht = 40;
            

            int gap = rnd.Next(50, 100);
            if (rnd.Next(2) == 0) gap *= -1;

            Rectangle oszlop = new Rectangle()
            {
                Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF6CFF00"),
                Height = oszlopHeight - gap,
                Width = oszlopWidht
            };

            Rectangle oszlopBottom = new Rectangle()
            {
                Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF6CFF00"),
                Height = oszlopHeight + gap,
                Width = oszlopWidht
            };

            oszlopokTop.Add(oszlop);
            oszlopokBot.Add(oszlopBottom);
            canvas.Children.Add(oszlop);
            canvas.Children.Add(oszlopBottom);

            double oszlopLeftPos =800;
            if (elozoOszlop != null) { oszlopLeftPos = Canvas.GetLeft(elozoOszlop) + rnd.Next(250, 300); }
            Canvas.SetLeft(oszlop, oszlopLeftPos);
            Canvas.SetLeft(oszlopBottom, oszlopLeftPos);
            Canvas.SetTop(oszlop, 0);
            Canvas.SetTop(oszlopBottom, -gap + 450 - oszlopHeight);

            elozoOszlop = oszlop;
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
                for (int i = 0; i < oszlopokBot.Count; i++)
                {
                    Rectangle oszlopTop = oszlopokTop[i];
                    Rectangle oszlopBot = oszlopokBot[i];
                    double oszlopPos = Canvas.GetLeft(oszlopTop);
                    Canvas.SetLeft(oszlopTop, oszlopPos - 1);
                    Canvas.SetLeft(oszlopBot, oszlopPos - 1);
                    if (oszlopPos < -40)
                    {
                        canvas.Children.Remove(oszlopTop);
                        oszlopokBot.RemoveAt(i);
                        oszlopokTop.RemoveAt(i);
                        UjOszlop();
                    }

                    
                    //label.Content = Canvas.GetTop(oszlopTop) + " - "+Canvas.GetTop(rectangleBird);
                }


                //collision check
                bool widhtCheckRight = Canvas.GetLeft(oszlopokTop[0]) + oszlopokTop[0].Width > Canvas.GetLeft(rectangleBird);
                bool widthCheckLeft = Canvas.GetLeft(rectangleBird) + rectangleBird.Width > Canvas.GetLeft(oszlopokTop[0]);

                //bool oszlopCollisionWidth = (Canvas.GetLeft(oszlopokTop[0]) + oszlopokTop[0].Width>Canvas.GetLeft(rectangleBird) && Canvas.GetLeft(rectangleBird) + rectangleBird.Width > Canvas.GetLeft(oszlopokTop[0]));
                bool oszlopCollisionWidth = widhtCheckRight && widthCheckLeft;
                bool oszlopColliisionHeight = (Canvas.GetTop(oszlopokBot[0]) < Canvas.GetTop(rectangleBird)+rectangleBird.Height || oszlopokTop[0].Height > (Canvas.GetTop(rectangleBird)));

                if (oszlopCollisionWidth && oszlopColliisionHeight) Halal();

                //pontszam
                if (oszlopCollisionWidth)
                {
                    athaladas = true;
                }

                if (widhtCheckRight == false&&athaladas)
                {
                    pontSzam++;
                    athaladas = false;
                }

                lbScore.Content = pontSzam;

                await Task.Delay(10);
            }
        }

        public void Ugras()
        {
            velocity = -ugrasSpeed;
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

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ugras();
        }
    }
}