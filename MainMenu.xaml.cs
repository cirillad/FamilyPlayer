using System.Windows;

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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); 
        }
    }
}
