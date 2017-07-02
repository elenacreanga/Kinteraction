using System.Windows;
using System.Windows.Controls;

namespace Kinteraction
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ShapeModelling_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ShapeModellingPage());
        }

        private void PostureAnalysis_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PostureAnalysisPage());
        }
    }
}
