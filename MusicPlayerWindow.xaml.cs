using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MusicPlayer
{
    public partial class MusicPlayerWindow : Window
    {
        private MediaPlayer mediaPlayer;
        private string[] songFiles;
        private int currentSongIndex;
        private bool isPlaying = false;
        private string favoriteFolder = @"C:\Users\Note\Desktop\Favorite Music";
        private DispatcherTimer timer;
        private bool isFavorite = false;
        private bool isRepeatMode = false;
        private bool isMuted = false;

        public MusicPlayerWindow()
        {
            InitializeComponent();
            mediaPlayer = new MediaPlayer();
            songFiles = Array.Empty<string>();
            VolumeSlider.Value = mediaPlayer.Volume * 100;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            string musicDirectory = @"C:\Users\Note\Desktop\Music";
            songFiles = Directory.GetFiles(musicDirectory, "*.mp3");

            var songSelectionWindow = new SongSelectionWindow(songFiles);
            songSelectionWindow.SongSelected += SongSelectionWindow_SongSelected;
            songSelectionWindow.ShowDialog();
        }

        private void SongSelectionWindow_SongSelected(object sender, string songPath)
        {
            currentSongIndex = Array.IndexOf(songFiles, songPath);
            if (currentSongIndex >= 0)
            {
                PlaySelectedSong(songPath);
                UpdateSongInfo(songPath);
                isPlaying = true;
                isFavorite = CheckIfFavorite(songPath);
                UpdateLikeButtonIcon(); 
            }
        }

        private void PlaySelectedSong(string songPath)
        {
            StopCurrentSong();
            mediaPlayer.Open(new Uri(songPath));
            mediaPlayer.Play();

            mediaPlayer.MediaOpened += (sender, e) =>
            {
                if (mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    TimeSpan duration = mediaPlayer.NaturalDuration.TimeSpan;
                    lblMusicLength.Text = $"{duration.Minutes}:{duration.Seconds:D2}";
                    TimerSlider.Maximum = duration.TotalSeconds;
                }
                timer.Start(); 
            };

            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

            isPlaying = true;
            (btnPlay.Content as StackPanel).Children.Clear();
            (btnPlay.Content as StackPanel).Children.Add(new PackIcon { Kind = PackIconKind.Pause, Width = 20, Height = 20 });
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            btnPNext_Click(null, null); 
        }

        private void StopCurrentSong()
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Stop();
                isPlaying = false;
                timer.Stop();
                TimerSlider.Value = 0;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DownloadSongWindow downloadWindow = new DownloadSongWindow();
            downloadWindow.SongDownloaded += DownloadWindow_SongDownloaded;
            downloadWindow.ShowDialog();
        }

        private void DownloadWindow_SongDownloaded(object sender, (string songTitle, string artist, string songPath) songInfo)
        {
            var updatedSongFiles = songFiles.ToList();
            updatedSongFiles.Add(songInfo.songPath);
            songFiles = updatedSongFiles.ToArray();

            lblSongName.Text = songInfo.songTitle;
            lblArtistName.Text = songInfo.artist;

            PlaySelectedSong(songInfo.songPath);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            StopCurrentSong();
            Application.Current.Shutdown();
        }

        private void UpdateSongInfo(string songPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(songPath);
            string[] parts = fileName.Split('-');

            if (parts.Length >= 2)
            {
                lblSongName.Text = parts[0].Trim();
                lblArtistName.Text = parts[1].Trim();
            }
        }

        private void btnPNext_Click(object sender, RoutedEventArgs e)
        {
            if (songFiles != null && songFiles.Length > 0)
            {
                currentSongIndex++;
                if (currentSongIndex >= songFiles.Length)
                {
                    currentSongIndex = 0;
                }

                PlaySelectedSong(songFiles[currentSongIndex]);
                UpdateSongInfo(songFiles[currentSongIndex]);
                isFavorite = CheckIfFavorite(songFiles[currentSongIndex]);
                UpdateLikeButtonIcon();
            }
            else
            {
                MessageBox.Show("Song list is empty.");
            }
        }

        private void btnPRewind_Click(object sender, RoutedEventArgs e)
        {
            if (songFiles != null && songFiles.Length > 0)
            {
                currentSongIndex--;
                if (currentSongIndex < 0)
                {
                    currentSongIndex = songFiles.Length - 1;
                }

                PlaySelectedSong(songFiles[currentSongIndex]);
                UpdateSongInfo(songFiles[currentSongIndex]);
                isFavorite = CheckIfFavorite(songFiles[currentSongIndex]);
                UpdateLikeButtonIcon();
            }
            else
            {
                MessageBox.Show("Song list is empty.");
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                mediaPlayer.Pause();
                isPlaying = false;
                timer.Stop();

                (btnPlay.Content as StackPanel).Children.Clear();
                (btnPlay.Content as StackPanel).Children.Add(new PackIcon { Kind = PackIconKind.Play, Width = 20, Height = 20 });
            }
            else
            {
                mediaPlayer.Play();
                isPlaying = true;
                timer.Start(); 

                (btnPlay.Content as StackPanel).Children.Clear();
                (btnPlay.Content as StackPanel).Children.Add(new PackIcon { Kind = PackIconKind.Pause, Width = 20, Height = 20 });
            }
        }

        private void btnMode_Click(object sender, RoutedEventArgs e)
        {
            isRepeatMode = !isRepeatMode;

            // Змінюємо іконку відповідно до режиму
            modeIcon.Kind = isRepeatMode ? PackIconKind.Repeat : PackIconKind.ShuffleVariant;

            // Змінюємо підказку для користувача
            btnMode.ToolTip = isRepeatMode ? "Repeat Mode" : "Sequential Mode";

            // Додайте логіку для керування режимами відтворення за потреби
            // Наприклад: player.SetPlayMode(isRepeatMode ? PlayMode.Repeat : PlayMode.Sequential);
        }

        private void btnLike_Click(object sender, RoutedEventArgs e)
        {
            if (songFiles != null && currentSongIndex >= 0)
            {
                string songPath = songFiles[currentSongIndex];
                string fileName = Path.GetFileName(songPath);
                string favoritePath = Path.Combine(favoriteFolder, fileName);

                try
                {
                    if (isFavorite)
                    {
                        if (File.Exists(favoritePath))
                        {
                            File.Delete(favoritePath);
                            MessageBox.Show("The song has been removed from favorites.");
                            isFavorite = false; 
                        }
                        else
                        {
                            MessageBox.Show("The song is not in favorites.");
                        }
                    }
                    else
                    {
                        if (!File.Exists(favoritePath))
                        {
                            File.Copy(songPath, favoritePath);
                            MessageBox.Show("The song has been added to favorites.");
                            isFavorite = true; 
                        }
                        else
                        {
                            MessageBox.Show("The song is already in favorites.");
                        }
                    }

                    UpdateLikeButtonIcon(); 

                    UpdateFavoriteStatus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No song is currently playing.");
            }
        }

        private void UpdateFavoriteStatus()
        {
            string songPath = songFiles[currentSongIndex];
            string fileName = Path.GetFileName(songPath);
            string favoritePath = Path.Combine(favoriteFolder, fileName);

            isFavorite = File.Exists(favoritePath);
        }

        private void UpdateLikeButtonIcon()
        {
            (btnLike.Content as StackPanel).Children.Clear();

            if (isFavorite)
            {
                (btnLike.Content as StackPanel).Children.Add(new PackIcon { Kind = PackIconKind.Heart, Width = 23, Height = 23 });
            }
            else
            {
                (btnLike.Content as StackPanel).Children.Add(new PackIcon { Kind = PackIconKind.HeartOutline, Width = 23, Height = 23 });
            }
        }

        private void btnMute_Click(object sender, RoutedEventArgs e)
        {
            if (isMuted)
            {
                // Якщо звук вимкнений, вмикаємо його
                mediaPlayer.Volume = 1.0; // Встановлюємо максимальний рівень гучності
                volumeIcon.Kind = PackIconKind.VolumeHigh; // Змінюємо іконку на VolumeHigh
            }
            else
            {
                // Якщо звук включений, вимикаємо його
                mediaPlayer.Volume = 0; // Встановлюємо звук на 0
                volumeIcon.Kind = PackIconKind.VolumeMute; // Змінюємо іконку на VolumeMute
            }

            // Перемикаємо стан
            isMuted = !isMuted;
        }

        private bool CheckIfFavorite(string songPath)
        {
            string fileName = Path.GetFileName(songPath);
            string favoritePath = Path.Combine(favoriteFolder, fileName);
            return File.Exists(favoritePath);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Volume = VolumeSlider.Value / 100;

                // Оновлюємо іконку залежно від рівня гучності
                if (VolumeSlider.Value == 0)
                {
                    // Якщо гучність на нулі, ставимо іконку без звуку
                    volumeIcon.Kind = PackIconKind.VolumeMute;
                }
                else
                {
                    // Якщо гучність більше нуля, ставимо іконку зі звуком
                    volumeIcon.Kind = PackIconKind.VolumeHigh;
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.NaturalDuration.HasTimeSpan && mediaPlayer.Position < mediaPlayer.NaturalDuration.TimeSpan)
            {
                lblCurrentTime.Text = $"{mediaPlayer.Position.Minutes}:{mediaPlayer.Position.Seconds:D2}";
                TimerSlider.Value = mediaPlayer.Position.TotalSeconds;
            }
        }

        private void TimerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaPlayer != null && TimerSlider.Value >= 0)
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(TimerSlider.Value);
            }
        }
    }
}
