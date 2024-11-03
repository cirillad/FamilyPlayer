using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        private string favoriteFolder = @"C:\Users\Roman\Desktop\Favorite Music";
        private DispatcherTimer timer; 

        public MusicPlayerWindow()
        {
            InitializeComponent();
            mediaPlayer = new MediaPlayer();
            songFiles = Array.Empty<string>();
            VolumeSlider.Value = mediaPlayer.Volume * 100; 

            if (!Directory.Exists(favoriteFolder))
            {
                Directory.CreateDirectory(favoriteFolder);
            }

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

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            if (songSelectionWindow == null || !songSelectionWindow.IsVisible)
            {
            string musicDirectory = @"C:\Users\Roman\Desktop\Music";
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
            }
        }

        private void PlaySelectedSong(string songPath)
        {
            StopCurrentSong();
            mediaPlayer.Open(new Uri(songPath));
            mediaPlayer.Play();

            mediaPlayer.MediaOpened += (sender, e) =>
            {
                TimeSpan duration = mediaPlayer.NaturalDuration.HasTimeSpan ? mediaPlayer.NaturalDuration.TimeSpan : TimeSpan.Zero;
                lblMusicLength.Text = $"{duration.Minutes}:{duration.Seconds:D2}";
                TimerSlider.Maximum = duration.TotalSeconds; 
            };

            isPlaying = true;
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
            }
            else
            {
                mediaPlayer.Play();
                isPlaying = true;
                timer.Start(); 
            }
        }

        private void btnLike_Click(object sender, RoutedEventArgs e)
        {
            if (songFiles != null && currentSongIndex >= 0)
            {
                string songPath = songFiles[currentSongIndex];
                string fileName = Path.GetFileName(songPath);
                string favoritePath = Path.Combine(favoriteFolder, fileName);

                if (File.Exists(favoritePath))
                {
                    MessageBox.Show("The song is already in the favorites.");
                }
                else
                {
                    File.Copy(songPath, favoritePath);
                    MessageBox.Show("The song has been added to favorites.");
                }
            }
            else
            {
                MessageBox.Show("No song is currently playing.");
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Volume = VolumeSlider.Value / 100; 
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

        private void TimerSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }


    }
}
