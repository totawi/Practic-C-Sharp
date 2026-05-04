using System;
using System.ComponentModel;

namespace EventOrganizer
{
    // ─── Модель: Мероприятие ─────────────────────────────────────────────────
    public class Event : INotifyPropertyChanged
    {
        private int _participantCount;

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Section { get; set; }

        public int ParticipantCount
        {
            get => _participantCount;
            set
            {
                _participantCount = value;
                OnPropertyChanged(nameof(ParticipantCount));
            }
        }

        // Форматированная дата для отображения
        public string DateFormatted => Date.ToString("dd.MM.yyyy");

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    // ─── Модель: Участник ────────────────────────────────────────────────────
    public class Participant : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public int EventId { get; set; }
        public string EventName { get; set; } = "";

        // Вычисляемые свойства
        public string FullName => $"{LastName} {FirstName}";

        public string Initials
        {
            get
            {
                string f = FirstName?.Length > 0 ? FirstName[0].ToString() : "";
                string l = LastName?.Length > 0 ? LastName[0].ToString() : "";
                return (f + l).ToUpper();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
