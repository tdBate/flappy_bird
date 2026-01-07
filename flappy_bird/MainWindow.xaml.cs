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

        public MainWindow()
        {
            InitializeComponent();
            bird = rectangleBird;
            Gravity();
        }

        public async void Gravity() 
        {
            while (true)
            { 
                velocity += gravity;

                double topPos = Canvas.GetTop(bird);

                Canvas.SetTop(bird, topPos+velocity);

                if (topPos >= 450-bird.Height)
                {
                    Application.Current.Shutdown();
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
    }
}