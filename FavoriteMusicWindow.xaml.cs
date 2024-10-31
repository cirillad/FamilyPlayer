using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MusicPlayer
{
    public partial class FavoriteMusicWindow : Window
    {
        private string favoritesFilePath = @"C:\Users\Roman\Desktop\Favorite Music\Favorites.txt";
        private MediaPlayer mediaPlayer;
        private bool isRepeatMode = false; 

        public FavoriteMusicWindow()
        {
            InitializeComponent();
            mediaPlayer = new MediaPlayer();
            LoadFavoriteSongs();
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded; 
        }

        private void LoadFavoriteSongs()
        {
            if (File.Exists(favoritesFilePath))
            {
                var favoriteSongs = File.ReadAllLines(favoritesFilePath);
                FavoriteSongsComboBox.ItemsSource = favoriteSongs.Select(Path.GetFileName).ToList();
            }
            else
            {
                MessageBox.Show("No favorite songs found.");
            }
        }

        private void FavoriteSongsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FavoriteSongsComboBox.SelectedItem != null)
            {
                string selectedSong = (string)FavoriteSongsComboBox.SelectedItem;
                string songPath = Path.Combine(@"C:\Users\Roman\Desktop\Music", selectedSong);

                try
                {
                    mediaPlayer.Stop(); 
                    mediaPlayer.Open(new Uri(songPath)); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading song: {ex.Message}");
                }
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (FavoriteSongsComboBox.SelectedItem != null)
            {
                mediaPlayer.Play();
            }
            else
            {
                MessageBox.Show("Please select a song to play.");
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void RemoveFromFavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedSong = (string)FavoriteSongsComboBox.SelectedItem;

            if (selectedSong != null)
            {
                string songFileName = selectedSong;

                var lines = File.ReadAllLines(favoritesFilePath).Where(line => Path.GetFileName(line) != songFileName).ToList();

                File.WriteAllLines(favoritesFilePath, lines);

                MessageBox.Show("The song has been removed from favorites.");
                LoadFavoriteSongs();
            }
            else
            {
                MessageBox.Show("Please select a song to remove from your favorites.");
            }
        }

        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            this.Close();
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            mediaPlayer.Stop(); 
            base.OnClosing(e);
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (RepeatModeRadioButton.IsChecked == true)
            {
                mediaPlayer.Position = TimeSpan.Zero;
                mediaPlayer.Play();
            }
            else if (SequentialModeRadioButton.IsChecked == true)
            {
                int currentIndex = FavoriteSongsComboBox.SelectedIndex;

                if (currentIndex < FavoriteSongsComboBox.Items.Count - 1)
                {
                    FavoriteSongsComboBox.SelectedIndex = currentIndex + 1;
                }
                else
                {
                    FavoriteSongsComboBox.SelectedIndex = 0;
                }

                PlayButton_Click(FavoriteSongsComboBox, new RoutedEventArgs());
            }
        }

    }
}
