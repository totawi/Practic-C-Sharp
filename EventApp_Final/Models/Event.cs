using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace EventApp.Models
{
    public class Event : INotifyPropertyChanged
    {
        private string _name        = string.Empty;
        private DateTime _dateUtc   = DateTime.UtcNow;
        private string _location    = string.Empty;
        private string _section     = string.Empty;
        private string _description = string.Empty;

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public DateTime DateUtc
        {
            get => _dateUtc;
            set { _dateUtc = value; OnPropertyChanged(); OnPropertyChanged(nameof(Date)); OnPropertyChanged(nameof(DateDisplay)); }
        }

        [NotMapped]
        public DateTime Date
        {
            get => DateUtc.ToLocalTime();
            set => DateUtc = value.ToUniversalTime();
        }

        [NotMapped, JsonIgnore]
        public string DateDisplay => Date.ToString("dd.MM.yyyy");

        [MaxLength(300)]
        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }

        [MaxLength(100)]
        public string Section
        {
            get => _section;
            set { _section = value; OnPropertyChanged(); }
        }

        [MaxLength(2000)]
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public List<Participant> ParticipantsList { get; set; } = new List<Participant>();

        [NotMapped, JsonIgnore]
        public ObservableCollection<Participant> Participants { get; private set; } = new ObservableCollection<Participant>();

        [NotMapped, JsonIgnore]
        public int ParticipantCount => Participants.Count;

        public void SyncParticipants()
        {
            Participants = new ObservableCollection<Participant>(ParticipantsList);
            Participants.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(ParticipantCount));
                ParticipantsList.Clear();
                foreach (var p in Participants) ParticipantsList.Add(p);
            };
            OnPropertyChanged(nameof(ParticipantCount));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
