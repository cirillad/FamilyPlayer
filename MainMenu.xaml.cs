using System.Windows;
using System.Windows.Input;

namespace MusicPlayer
{
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void AllSongsButton_Click(object sender, RoutedEventArgs e)
        {
            MusicPlayerWindow musicPlayerWindow = new MusicPlayerWindow();
            musicPlayerWindow.Show();
            this.Close(); 
        }

        private void FavoriteSongsButton_Click(object sender, RoutedEventArgs e)
        {
            FavoriteMusicWindow favoriteMusicWindow = new FavoriteMusicWindow();
            favoriteMusicWindow.Show();
            this.Close(); 
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Закриття поточного вікна
            this.Close();

            // Створення та відкриття MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to log out?", "Exit confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
