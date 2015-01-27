using Windows.UI.Xaml.Controls;

namespace CodeFestApp
{
    public sealed partial class MainWindow : Page
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AppBootstrapper();
        }
    }
}
