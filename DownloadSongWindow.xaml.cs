using System;
using System.Net;
using System.Windows;

namespace MusicPlayer
{
    public partial class DownloadSongWindow : Window
    {
        public string SongUrl { get; private set; }

        public DownloadSongWindow()
        {
            InitializeComponent();
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            SongUrl = UrlTextBox.Text;
            this.DialogResult = true; 
        }
    }
}
