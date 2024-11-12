using FamilyPlayer.Data;
using FamilyPlayer.Models;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer
{
    public partial class RegisterWindow : Window
    {

        private readonly MusicPlayerContext context = new MusicPlayerContext();
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Password;
            string confirmPassword = ConfirmPasswordTextBox.Password;

            if (!IsPasswordValid(password))
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Invalid Password", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please make sure both passwords are the same.", "Password Mismatch", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (IsUsernameTaken(username))
            {
                MessageBox.Show("Username already exists. Please choose another one.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RegisterUser(username, password);
            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            BackToMainMenu();
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length >= 8;
        }

        private bool IsUsernameTaken(string username)
        {
            return context.Users.Any(u => u.Username == username);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RegisterUser(string username, string password)
        {
            var newUser = new User { Username = username, Password = password };
            context.Users.Add(newUser);
            context.SaveChanges();
        }

        private void BackToMainMenu()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}