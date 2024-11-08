using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer
{
    public partial class SongSelectionWindow : Window
    {
        public event EventHandler<string> SongSelected;

        private string[] _songPaths;
        private string[] _songNames;

        public SongSelectionWindow(string[] songPaths)
        {
            InitializeComponent();

            // Store song paths and names
            _songPaths = songPaths;

            // Get only the song names from the full paths
            _songNames = new string[songPaths.Length];
            for (int i = 0; i < songPaths.Length; i++)
            {
                _songNames[i] = Path.GetFileNameWithoutExtension(songPaths[i]);
            }

            // Set the items source to the song names
            SongListBox.ItemsSource = _songNames;
            SongListBox.Tag = songPaths; // Store the paths in the Tag property for future access
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Hide the watermark when the TextBox gets focus
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                WatermarkTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Show the watermark when the TextBox loses focus and is empty
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                WatermarkTextBlock.Visibility = Visibility.Visible;
            }
        }

        // Search logic to filter songs based on the search query
        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchQuery = SearchTextBox.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                // If the search query is empty, show all songs
                SongListBox.ItemsSource = _songNames;
            }
            else
            {
                // Filter songs based on the search query
                var filteredSongs = _songNames.Where(song => song.ToLower().Contains(searchQuery)).ToArray();
                SongListBox.ItemsSource = filteredSongs;
            }
        }

        private void SongListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SongListBox.SelectedIndex >= 0)
            {
                // Get the full path to the selected song through the Tag property
                var songPaths = (string[])SongListBox.Tag;
                string selectedSongPath = songPaths[SongListBox.SelectedIndex];

                SongSelected?.Invoke(this, selectedSongPath); // Trigger the event with the full path
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a song to play.", "No Song Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
