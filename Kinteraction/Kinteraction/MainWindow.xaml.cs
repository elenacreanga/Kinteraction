using System.Windows;

namespace Kinteraction
{
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new MainPage());
        }
    }
}