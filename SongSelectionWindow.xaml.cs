﻿using System;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer
{
    public partial class SongSelectionWindow : Window
    {
        public event EventHandler<string> SongSelected;

        public SongSelectionWindow(string[] songs)
        {
            InitializeComponent();
            SongListBox.ItemsSource = songs;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongListBox.SelectedItem != null)
            {
                string selectedSong = SongListBox.SelectedItem.ToString();
                SongSelected?.Invoke(this, selectedSong);
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
            // Закриття вікна
            this.Close();
        }
    }
}
