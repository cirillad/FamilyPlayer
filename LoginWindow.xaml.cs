using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer
{
    public partial class LoginWindow : Window
    {
        private string userFilePath = "users.txt"; 

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text; 
            string password = PasswordTextBox.Password; 

            if (ValidateUser(username, password))
            {
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateUser(string username, string password)
        {
            if (File.Exists(userFilePath))
            {
                var users = File.ReadAllLines(userFilePath);
                foreach (var user in users)
                {
                    var parts = user.Split(':');
                    if (parts.Length == 2 && parts[0] == username && parts[1] == password)
                    {
                        return true; 
                    }
                }
            }
            return false; 
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
