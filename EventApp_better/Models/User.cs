using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EventApp.Models
{
    public enum UserRole { Participant, Organizer }

    public class User : INotifyPropertyChanged
    {
        private string _username;
        private string _displayName;
        private UserRole _role;

        public int Id { get; set; }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        // Хранится как bcrypt-like hash (для учебного проекта — SHA256+salt)
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public string DisplayName
        {
            get => _displayName;
            set { _displayName = value; OnPropertyChanged(); }
        }

        public UserRole Role
        {
            get => _role;
            set { _role = value; OnPropertyChanged(); OnPropertyChanged(nameof(RoleDisplay)); }
        }

        public string RoleDisplay => Role == UserRole.Organizer ? "Организатор" : "Участник";
        public string Email { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
