using System.IO;
using System.Windows;

namespace MusicPlayer
{
    public partial class RegisterWindow : Window
    {
        private string userFilePath = "users.txt"; 

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            if (RegisterUser(username, password))
            {
                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                BackToMainMenu();
            }
            else
            {
                MessageBox.Show("Username already exists. Please choose another one.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool RegisterUser(string username, string password)
        {
            if (!File.Exists(userFilePath))
            {
                File.Create(userFilePath).Close();
            }

            var users = File.ReadAllLines(userFilePath);
            foreach (var user in users)
            {
                var parts = user.Split(':');
                if (parts.Length == 2 && parts[0] == username)
                {
                    return false; 
                }
            }

            using (StreamWriter sw = File.AppendText(userFilePath))
            {
                sw.WriteLine($"{username}:{password}");
            }
            return true; 
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); 
        }

        private void BackToMainMenu()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); 
        }
    }
}
