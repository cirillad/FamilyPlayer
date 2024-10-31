using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Net;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace MusicPlayer
{
    public partial class MusicPlayerWindow : Window
    {
        private MediaPlayer mediaPlayer;
        private bool isPaused;
        private List<string> favoriteSongs;
        private string favoritesFilePath = @"C:\Users\Roman\Desktop\Favorite Music\Favorites.txt";
        private string favoritesFolderPath = @"C:\Users\Roman\Desktop\Favorite Music";

        public MusicPlayerWindow()
        {
            InitializeComponent();

            string ImgPath = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString()}\\Images\\MusicIcon.png";
            Musicimg.Source = new BitmapImage(new Uri(ImgPath));

            mediaPlayer = new MediaPlayer();
            //mediaPlayer.MediaEnded += MediaPlayer_MediaEnded; 
            isPaused = false;
            //favoriteSongs = LoadFavoriteSongs();

            //LoadSongs();
        }

        //private void LoadSongs()
        //{
        //    string musicFolder = @"C:\Users\Note\Desktop\Music";
        //    var files = Directory.GetFiles(musicFolder, "*.mp3");
        //    foreach (var file in files)
        //    {
        //        SongsComboBox.Items.Add(Path.GetFileName(file));
        //    }
        //}

        //private void PlayButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (isPaused)
        //    {
        //        mediaPlayer.Play();
        //        isPaused = false;
        //    }
        //    else
        //    {
        //        if (SongsComboBox.SelectedItem != null)
        //        {
        //            string selectedSong = (string)SongsComboBox.SelectedItem;
        //            string fullPath = Path.Combine(@"C:\Users\Note\Desktop\Music", selectedSong);
        //            mediaPlayer.Open(new Uri(fullPath));
        //            mediaPlayer.Play();
        //        }
        //    }
        //}

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
            isPaused = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            isPaused = false;
        }

        //private void AddToFavoritesButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SongsComboBox.SelectedItem != null)
        //    {
        //        string selectedSong = (string)SongsComboBox.SelectedItem;
        //        string fullPath = Path.Combine(@"C:\Users\Note\Desktop\Music", selectedSong);

        //        if (!favoriteSongs.Contains(fullPath))
        //        {
        //            if (!Directory.Exists(favoritesFolderPath))
        //            {
        //                try
        //                {
        //                    Directory.CreateDirectory(favoritesFolderPath);
        //                    MessageBox.Show("Favorite Music folder created.");
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show($"Error creating folder: {ex.Message}");
        //                    return;
        //                }
        //            }

        //            string destinationPath = Path.Combine(favoritesFolderPath, selectedSong);
        //            try
        //            {
        //                File.Copy(fullPath, destinationPath, true);
        //                favoriteSongs.Add(destinationPath);
        //                SaveFavoriteSongs();
        //                MessageBox.Show($"{selectedSong} has been added to favorites!");
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"Error adding to favorites: {ex.Message}");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show($"{selectedSong} is already in favorites.");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select a song to add to favorites.");
        //    }
        //}

        private List<string> LoadFavoriteSongs()
        {
            string directoryPath = Path.GetDirectoryName(favoritesFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(favoritesFilePath))
            {
                return File.ReadAllLines(favoritesFilePath).ToList();
            }
            return new List<string>();
        }

        private void SaveFavoriteSongs()
        {
            File.WriteAllLines(favoritesFilePath, favoriteSongs);
        }

        private void SongsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
        }

        //private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        //{
        //    if (RepeatModeRadioButton.IsChecked == true)
        //    {
        //        mediaPlayer.Position = TimeSpan.Zero;
        //        mediaPlayer.Play(); 
        //    }
        //    else if (SequentialModeRadioButton.IsChecked == true)
        //    {
        //        int currentIndex = SongsComboBox.SelectedIndex;

        //        if (currentIndex < SongsComboBox.Items.Count - 1)
        //        {
        //            SongsComboBox.SelectedIndex = currentIndex + 1;
        //        }
        //        else
        //        {
        //            SongsComboBox.SelectedIndex = 0;
        //        }

        //        PlayButton_Click(SongsComboBox, new RoutedEventArgs());
        //    }
        //}



        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            mediaPlayer.Stop();
            base.OnClosing(e);
        }

        //private void DownloadButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var downloadWindow = new DownloadSongWindow();
        //    if (downloadWindow.ShowDialog() == true)
        //    {
        //        string songUrl = downloadWindow.SongUrl;
        //        DownloadSong(songUrl);
        //    }
        //}

        //private void DownloadSong(string url)
        //{
        //    try
        //    {
        //        using (WebClient client = new WebClient())
        //        {
        //            string musicFolder = @"C:\Users\Note\Desktop\Music";
        //            string fileName = Path.GetFileName(url);  
        //            string destinationPath = Path.Combine(musicFolder, fileName);

        //            client.DownloadFile(url, destinationPath);

        //            SongsComboBox.Items.Add(fileName);
        //            MessageBox.Show("Song downloaded successfully!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error downloading song: {ex.Message}");
        //    }
        //}

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
