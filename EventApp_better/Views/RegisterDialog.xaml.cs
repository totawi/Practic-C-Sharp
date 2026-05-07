using System.Windows;
using EventApp.Models;
using EventApp.Services;

namespace EventApp.Views
{
    public partial class RegisterDialog : Window
    {
        private readonly AuthService _auth;

        public RegisterDialog(AuthService auth)
        {
            InitializeComponent();
            _auth = auth;
        }

        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";
            var username = TxtUsername.Text.Trim();
            var password = TxtPassword.Password;
            var display  = TxtDisplay.Text.Trim();
            var email    = TxtEmail.Text.Trim();
            var role     = ChkOrganizer.IsChecked == true
                ? UserRole.Organizer : UserRole.Participant;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(display))
            {
                TxtError.Text = "Заполните обязательные поля.";
                return;
            }

            var user = await _auth.RegisterAsync(username, password, display, email, role);
            if (user == null)
            {
                TxtError.Text = "Пользователь с таким логином уже существует.";
                return;
            }

            _auth.Login(username, password);
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
