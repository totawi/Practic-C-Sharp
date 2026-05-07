using System.Windows;
using EventApp.Models;
using EventApp.Services;

namespace EventApp.Views
{
    public partial class LoginDialog : Window
    {
        private readonly AuthService    _auth;
        private readonly JsonDataService _data;

        public LoginDialog(AuthService auth, JsonDataService data)
        {
            InitializeComponent();
            _auth = auth;
            _data = data;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = _auth.Login(TxtUsername.Text.Trim(), TxtPassword.Password);
            if (user == null)
            {
                TxtError.Text = "Неверный логин или пароль.";
                return;
            }
            DialogResult = true;
        }

        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            var regDlg = new RegisterDialog(_auth);
            if (regDlg.ShowDialog() == true)
                DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
