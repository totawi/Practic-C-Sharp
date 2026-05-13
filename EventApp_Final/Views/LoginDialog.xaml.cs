using System;
using System.Windows;
using EventApp.Services;

namespace EventApp.Views
{
    public partial class LoginDialog : Window
    {
        private readonly AuthService     _auth;
        private readonly JsonDataService _data;

        public LoginDialog(AuthService auth, JsonDataService data)
        {
            InitializeComponent();
            _auth = auth;
            _data = data;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";

            var username = TxtUsername.Text.Trim();
            var password = TxtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TxtError.Text = "Введите логин и пароль.";
                return;
            }

            try
            {
                var user = _auth.Login(username, password);
                if (user == null)
                {
                    TxtError.Text = "Неверный логин или пароль.\n" +
                                    "Демо: admin / admin123  или  user1 / user123";
                    return;
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                TxtError.Text = $"Ошибка входа: {ex.Message}";
            }
        }

        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var regDlg = new RegisterDialog(_auth);
                regDlg.Owner = this;
                if (regDlg.ShowDialog() == true)
                    DialogResult = true;   // регистрация прошла — сразу закрываем LoginDialog
            }
            catch (Exception ex)
            {
                TxtError.Text = $"Ошибка открытия регистрации: {ex.Message}";
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
