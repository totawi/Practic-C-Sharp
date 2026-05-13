using System;
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
            // Сбрасываем ошибку
            TxtError.Text = "";
            BtnRegisterBtn.IsEnabled = false;

            try
            {
                var username = TxtUsername.Text.Trim();
                var password = TxtPassword.Password;
                var display  = TxtDisplay.Text.Trim();
                var email    = TxtEmail.Text.Trim();
                var role     = ChkOrganizer.IsChecked == true
                    ? UserRole.Organizer : UserRole.Participant;

                // Валидация
                if (string.IsNullOrEmpty(username))
                { TxtError.Text = "Введите логин."; return; }

                if (string.IsNullOrEmpty(password))
                { TxtError.Text = "Введите пароль."; return; }

                if (password.Length < 3)
                { TxtError.Text = "Пароль минимум 3 символа."; return; }

                if (string.IsNullOrEmpty(display))
                { TxtError.Text = "Введите отображаемое имя."; return; }

                // Регистрация
                var user = await _auth.RegisterAsync(username, password, display, email, role);

                if (user == null)
                {
                    TxtError.Text = $"Пользователь «{username}» уже существует.";
                    return;
                }

                // Сразу логиним зарегистрированного пользователя
                var logged = _auth.Login(username, password);
                if (logged == null)
                {
                    // Не должно случиться, но на всякий случай
                    TxtError.Text = "Регистрация прошла, но войти не удалось. Войдите вручную.";
                    return;
                }

                DialogResult = true;
            }
            catch (Exception ex)
            {
                // async void — исключения нужно ловить явно
                TxtError.Text = $"Ошибка: {ex.Message}";
            }
            finally
            {
                BtnRegisterBtn.IsEnabled = true;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
