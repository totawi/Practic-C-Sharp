using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EventApp.Commands;
using EventApp.Models;
using EventApp.Views;

namespace EventApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Event _selectedEvent;
        private Participant _selectedParticipant;
        private int _nextEventId = 1;
        private int _nextParticipantId = 1;

        public ObservableCollection<Event> Events { get; } = new ObservableCollection<Event>();

        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentParticipants));
                ((RelayCommand)EditEventCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteEventCommand).RaiseCanExecuteChanged();
                ((RelayCommand)AddParticipantCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteParticipantCommand).RaiseCanExecuteChanged();
            }
        }

        public Participant SelectedParticipant
        {
            get => _selectedParticipant;
            set
            {
                _selectedParticipant = value;
                OnPropertyChanged();
                ((RelayCommand)DeleteParticipantCommand).RaiseCanExecuteChanged();
                ((RelayCommand)EditParticipantCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Participant> CurrentParticipants
            => SelectedEvent?.Participants ?? new ObservableCollection<Participant>();

        // Commands
        public ICommand CreateEventCommand { get; }
        public ICommand EditEventCommand { get; }
        public ICommand DeleteEventCommand { get; }
        public ICommand AddParticipantCommand { get; }
        public ICommand EditParticipantCommand { get; }
        public ICommand DeleteParticipantCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand ExitCommand { get; }

        public MainViewModel()
        {
            CreateEventCommand = new RelayCommand(CreateEvent);
            EditEventCommand = new RelayCommand(EditEvent, () => SelectedEvent != null);
            DeleteEventCommand = new RelayCommand(DeleteEvent, () => SelectedEvent != null);
            AddParticipantCommand = new RelayCommand(AddParticipant, () => SelectedEvent != null);
            EditParticipantCommand = new RelayCommand(EditParticipant, () => SelectedParticipant != null);
            DeleteParticipantCommand = new RelayCommand(DeleteParticipant,
                () => SelectedEvent != null && SelectedParticipant != null);
            AboutCommand = new RelayCommand(ShowAbout);
            ExitCommand = new RelayCommand(() => Application.Current.Shutdown());

            LoadSampleData();
        }

        private void LoadSampleData()
        {
            var e1 = new Event
            {
                Id = _nextEventId++,
                Name = "IT-конференция 2025",
                Date = new DateTime(2025, 6, 15),
                Location = "Конференц-зал А",
                Section = "Технологии",
                Description = "Ежегодная конференция по информационным технологиям"
            };
            e1.Participants.Add(new Participant { Id = _nextParticipantId++, FirstName = "Иван", LastName = "Петров", Email = "petrov@mail.ru", Section = "Разработка", Organization = "ООО Техно" });
            e1.Participants.Add(new Participant { Id = _nextParticipantId++, FirstName = "Мария", LastName = "Иванова", Email = "ivanova@mail.ru", Section = "Дизайн", Organization = "АО Медиа" });

            var e2 = new Event
            {
                Id = _nextEventId++,
                Name = "Бизнес-форум",
                Date = new DateTime(2025, 7, 20),
                Location = "Бизнес-центр",
                Section = "Бизнес",
                Description = "Форум для предпринимателей и инвесторов"
            };
            e2.Participants.Add(new Participant { Id = _nextParticipantId++, FirstName = "Алексей", LastName = "Смирнов", Email = "smirnov@corp.ru", Section = "Инвестиции", Organization = "Банк+" });

            Events.Add(e1);
            Events.Add(e2);
        }

        private void CreateEvent()
        {
            var newEvent = new Event
            {
                Id = _nextEventId++,
                Date = DateTime.Today,
                Name = "Новое мероприятие"
            };
            var dialog = new EventEditDialog(newEvent, isNew: true);
            if (dialog.ShowDialog() == true)
            {
                Events.Add(newEvent);
                SelectedEvent = newEvent;
            }
            else
            {
                _nextEventId--;
            }
        }

        private void EditEvent()
        {
            if (SelectedEvent == null) return;
            var dialog = new EventEditDialog(SelectedEvent, isNew: false);
            dialog.ShowDialog();
        }

        private void DeleteEvent()
        {
            if (SelectedEvent == null) return;
            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить мероприятие «{SelectedEvent.Name}»?\nВсе участники будут удалены.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Events.Remove(SelectedEvent);
                SelectedEvent = Events.FirstOrDefault();
            }
        }

        private void AddParticipant()
        {
            if (SelectedEvent == null) return;
            var participant = new Participant { Id = _nextParticipantId++ };
            var dialog = new ParticipantEditDialog(participant, isNew: true);
            if (dialog.ShowDialog() == true)
            {
                SelectedEvent.Participants.Add(participant);
                SelectedParticipant = participant;
                OnPropertyChanged(nameof(CurrentParticipants));
            }
            else
            {
                _nextParticipantId--;
            }
        }

        private void EditParticipant()
        {
            if (SelectedParticipant == null) return;
            var dialog = new ParticipantEditDialog(SelectedParticipant, isNew: false);
            dialog.ShowDialog();
        }

        private void DeleteParticipant()
        {
            if (SelectedEvent == null || SelectedParticipant == null) return;
            var result = MessageBox.Show(
                $"Удалить участника «{SelectedParticipant.FullName}»?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SelectedEvent.Participants.Remove(SelectedParticipant);
                SelectedParticipant = null;
                OnPropertyChanged(nameof(CurrentParticipants));
            }
        }

        private void ShowAbout()
        {
            MessageBox.Show(
                "Приложение «Организация мероприятий»\nВерсия 1.0\n\nРазработано на WPF + MVVM",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
