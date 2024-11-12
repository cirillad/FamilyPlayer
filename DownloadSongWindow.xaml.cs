using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer
{
    public partial class DownloadSongWindow : Window
    {
        public event EventHandler<(string songTitle, string artist, string songPath)> SongDownloaded;

        public DownloadSongWindow()
        {
            InitializeComponent();
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string songUrl = SongUrlTextBox.Text;
            string songTitle = SongTitleTextBox.Text;
            string artist = ArtistTextBox.Text;

            if (string.IsNullOrWhiteSpace(songUrl) || string.IsNullOrWhiteSpace(songTitle) || string.IsNullOrWhiteSpace(artist))
            {
                MessageBox.Show("Please enter all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string musicFolderPath = @"C:\Users\Roman\Desktop\Music";
            string fileName = $"{songTitle} - {artist}.mp3";
            string filePath = Path.Combine(musicFolderPath, fileName);

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    // Завантаження файлу за тимчасовим шляхом
                    string tempFilePath = Path.Combine(musicFolderPath, $"{Guid.NewGuid()}.tmp");
                    webClient.DownloadFile(new Uri(songUrl), tempFilePath);

                    // Перейменування завантаженого файлу в формат .mp3
                    File.Move(tempFilePath, filePath);
                }

                SaveSongMetadata(songTitle, artist, fileName);

                SongDownloaded?.Invoke(this, (songTitle, artist, filePath));

                MessageBox.Show("Song downloaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading song: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveSongMetadata(string title, string artist, string fileName)
        {
            string metadataFilePath = @"C:\Users\Roman\Desktop\Music\metadata.txt";
            using (StreamWriter sw = new StreamWriter(metadataFilePath, true))
            {
                sw.WriteLine($"{fileName}:{title}:{artist}");
            }
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
        }
    }
}
