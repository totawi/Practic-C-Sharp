using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EventApp.Models;

namespace EventApp.Services
{
    /// <summary>
    /// Локальная авторизация через users.json (SHA-256 + salt)
    /// </summary>
    public class AuthService
    {
        private readonly JsonDataService _dataService;
        private List<User> _users = new();

        public User CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;
        public bool IsOrganizer => CurrentUser?.Role == UserRole.Organizer;

        public AuthService(JsonDataService dataService = null)
        {
            _dataService = dataService;
        }

        public async Task InitializeAsync()
        {
            _users = await _dataService.LoadUsersAsync();
        }

        /// <summary>Попытка входа. Возвращает null при ошибке.</summary>
        public User Login(string username, string password)
        {
            var user = _users.FirstOrDefault(u =>
                string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

            if (user == null) return null;

            var hash = HashPassword(password, user.Salt);
            if (hash != user.PasswordHash) return null;

            CurrentUser = user;
            return user;
        }

        public void Logout() => CurrentUser = null;

        /// <summary>Регистрация нового пользователя</summary>
        public async Task<User> RegisterAsync(
            string username, string password, string displayName,
            string email, UserRole role)
        {
            if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return null; // уже существует

            var salt = GenerateSalt();
            var user = new User
            {
                Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1,
                Username = username,
                Salt = salt,
                PasswordHash = HashPassword(password, salt),
                DisplayName = displayName,
                Email = email,
                Role = role
            };
            _users.Add(user);
            await _dataService.SaveUsersAsync(_users);
            return user;
        }

        public string GenerateSalt()
        {
            var bytes = RandomNumberGenerator.GetBytes(16);
            return Convert.ToBase64String(bytes);
        }

        public string HashPassword(string password, string salt)
        {
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = SHA256.HashData(combined);
            return Convert.ToBase64String(hash);
        }
    }
}
