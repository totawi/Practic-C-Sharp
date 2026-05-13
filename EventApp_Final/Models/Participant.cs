using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace EventApp.Models
{
    public class Participant : INotifyPropertyChanged
    {
        private string _firstName    = string.Empty;
        private string _lastName     = string.Empty;
        private string _email        = string.Empty;
        private string _phone        = string.Empty;
        private string _section      = string.Empty;
        private string _organization = string.Empty;
        private string _displayName  = string.Empty;

        [Key]
        public int Id { get; set; }
        public int EventId { get; set; }

        public string DisplayName
        {
            get => _displayName;
            set { _displayName = value; OnPropertyChanged(); }
        }

        [MaxLength(100)]
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); }
        }

        [MaxLength(100)]
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); }
        }

        [NotMapped, JsonIgnore]
        public string FullName => $"{LastName} {FirstName}".Trim();

        [MaxLength(200)]
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        [MaxLength(30)]
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        [MaxLength(100)]
        public string Section
        {
            get => _section;
            set { _section = value; OnPropertyChanged(); }
        }

        [MaxLength(200)]
        public string Organization
        {
            get => _organization;
            set { _organization = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
