using FamilyPlayer.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer
{
    public partial class LoginWindow : Window
    {
        private readonly MusicPlayerContext context = new MusicPlayerContext();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Password;

            if (AuthenticateUser(username, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            return context.Users.Any(u => u.Username == username && u.Password == password);
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
