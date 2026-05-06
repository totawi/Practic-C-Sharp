using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EventApp.Models
{
    public class Event : INotifyPropertyChanged
    {
        private string _name;
        private DateTime _date;
        private string _location;
        private string _section;
        private string _description;

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); OnPropertyChanged(nameof(DateDisplay)); }
        }

        public string DateDisplay => Date.ToString("dd.MM.yyyy");

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }

        public string Section
        {
            get => _section;
            set { _section = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Participant> Participants { get; set; } = new ObservableCollection<Participant>();

        // OneWay binding — количество участников
        public int ParticipantCount => Participants.Count;

        public Event()
        {
            Participants.CollectionChanged += (s, e) => OnPropertyChanged(nameof(ParticipantCount));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
