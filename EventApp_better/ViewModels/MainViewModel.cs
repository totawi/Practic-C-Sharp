using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EventApp.Commands;
using EventApp.Models;
using EventApp.Services;
using EventApp.Views;

namespace EventApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // ── Services ────────────────────────────────────────────────────────
        private readonly JsonDataService      _dataService;
        private readonly AuthService          _authService;
        private readonly EventService         _eventService;
        private readonly PipesChatService     _pipeService;
        private readonly MmfNotificationService _mmfService;

        // ── State ────────────────────────────────────────────────────────────
        private Event       _selectedEvent;
        private Participant _selectedParticipant;
        private DateTime    _selectedCalendarDate = DateTime.Today;
        private bool        _isLoading;
        private string      _statusMessage;
        private bool        _isRegistering;
        private string      _chatInput;

        // ── Collections ──────────────────────────────────────────────────────
        public ObservableCollection<Event>       Events         => _eventService.Events;
        public ObservableCollection<string>      ChatMessages   { get; } = new();
        public ObservableCollection<string>      Notifications  { get; } = new();
        public ObservableCollection<Event>       CalendarDayEvents { get; } = new();

        // ── Properties ───────────────────────────────────────────────────────
        public User CurrentUser => _authService.CurrentUser;
        public bool IsLoggedIn  => _authService.IsLoggedIn;
        public bool IsOrganizer => _authService.IsOrganizer;
        public bool IsNotOrganizer => !IsOrganizer;

        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentParticipants));
                RaiseEventCommands();
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

        public DateTime SelectedCalendarDate
        {
            get => _selectedCalendarDate;
            set
            {
                _selectedCalendarDate = value;
                OnPropertyChanged();
                RefreshCalendarDay();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public bool IsRegistering
        {
            get => _isRegistering;
            set { _isRegistering = value; OnPropertyChanged(); }
        }

        public string ChatInput
        {
            get => _chatInput;
            set { _chatInput = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Participant> CurrentParticipants
            => SelectedEvent?.Participants ?? new ObservableCollection<Participant>();

        // ── Commands ─────────────────────────────────────────────────────────
        public ICommand CreateEventCommand     { get; }
        public ICommand EditEventCommand       { get; }
        public ICommand DeleteEventCommand     { get; }
        public ICommand RegisterParticipantCommand { get; }
        public ICommand AddParticipantCommand  { get; }
        public ICommand EditParticipantCommand { get; }
        public ICommand DeleteParticipantCommand { get; }
        public ICommand LoginCommand           { get; }
        public ICommand LogoutCommand          { get; }
        public ICommand SendChatCommand        { get; }
        public ICommand AboutCommand           { get; }
        public ICommand ExitCommand            { get; }

        // ── Constructor ──────────────────────────────────────────────────────
        public MainViewModel()
        {
            _dataService  = new JsonDataService();
            _authService  = new AuthService(_dataService);
            _eventService = new EventService(_dataService);
            _pipeService  = new PipesChatService();
            _mmfService   = new MmfNotificationService();

            // Commands
            CreateEventCommand     = new RelayCommand(CreateEvent,  () => IsOrganizer);
            EditEventCommand       = new RelayCommand(EditEvent,    () => IsOrganizer && SelectedEvent != null);
            DeleteEventCommand     = new RelayCommand(DeleteEvent,  () => IsOrganizer && SelectedEvent != null);
            RegisterParticipantCommand = new RelayCommand(async () => await RegisterParticipantAsync(),
                                          () => IsLoggedIn && SelectedEvent != null && !IsRegistering);
            AddParticipantCommand  = new RelayCommand(AddParticipant,  () => IsOrganizer && SelectedEvent != null);
            EditParticipantCommand = new RelayCommand(EditParticipant, () => IsOrganizer && SelectedParticipant != null);
            DeleteParticipantCommand = new RelayCommand(DeleteParticipant,
                                        () => IsOrganizer && SelectedEvent != null && SelectedParticipant != null);
            LoginCommand   = new RelayCommand(ShowLogin,  () => !IsLoggedIn);
            LogoutCommand  = new RelayCommand(DoLogout,   () => IsLoggedIn);
            SendChatCommand = new RelayCommand(async () => await SendChatAsync(),
                              () => IsLoggedIn && !string.IsNullOrWhiteSpace(ChatInput));
            AboutCommand   = new RelayCommand(ShowAbout);
            ExitCommand    = new RelayCommand(async () => { await _eventService.SaveAsync(); Application.Current.Shutdown(); });

            InitializeAsync();
        }

        // ── Init ─────────────────────────────────────────────────────────────
        private async void InitializeAsync()
        {
            IsLoading = true;
            StatusMessage = "Загрузка данных…";
            try
            {
                await _authService.InitializeAsync();
                await _eventService.LoadAsync();

                // Named Pipes — чат
                _pipeService.MessageReceived += msg =>
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ChatMessages.Add(msg);
                        if (ChatMessages.Count > 200) ChatMessages.RemoveAt(0);
                    });
                _pipeService.StartServer();

                // MMF — уведомления
                _mmfService.NotificationReceived += msg =>
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Notifications.Add(msg);
                        StatusMessage = msg;
                    });
                _mmfService.Initialize();

                SelectedEvent = Events.FirstOrDefault();
                RefreshCalendarDay();
                StatusMessage = "Готово. Войдите для работы с мероприятиями.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки: {ex.Message}";
            }
            finally { IsLoading = false; }
        }

        // ── Auth ─────────────────────────────────────────────────────────────
        private void ShowLogin()
        {
            var dlg = new LoginDialog(_authService, _dataService);
            if (dlg.ShowDialog() == true)
            {
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(IsLoggedIn));
                OnPropertyChanged(nameof(IsOrganizer));
                OnPropertyChanged(nameof(IsNotOrganizer));
                StatusMessage = $"Добро пожаловать, {CurrentUser.DisplayName}!";
                RaiseAllCommands();
            }
        }

        private void DoLogout()
        {
            _authService.Logout();
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsOrganizer));
            OnPropertyChanged(nameof(IsNotOrganizer));
            StatusMessage = "Вы вышли из системы.";
            RaiseAllCommands();
        }

        // ── Events ───────────────────────────────────────────────────────────
        private void CreateEvent()
        {
            var ev = new Event { Date = DateTime.Today, Name = "Новое мероприятие" };
            var dlg = new EventEditDialog(ev, isNew: true);
            if (dlg.ShowDialog() != true) return;

            _eventService.CreateEvent(ev.Name, ev.Date, ev.Location, ev.Section, ev.Description);
            SelectedEvent = Events.Last();
            RefreshCalendarDay();
            PublishScheduleNotification(ev.Name, "создано");
        }

        private void EditEvent()
        {
            if (SelectedEvent == null) return;
            var dlg = new EventEditDialog(SelectedEvent, isNew: false);
            if (dlg.ShowDialog() != true) return;
            _eventService.UpdateEvent(SelectedEvent);
            RefreshCalendarDay();
            PublishScheduleNotification(SelectedEvent.Name, "расписание изменено");
        }

        private void DeleteEvent()
        {
            if (SelectedEvent == null) return;
            var r = MessageBox.Show(
                $"Удалить мероприятие «{SelectedEvent.Name}»?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (r != MessageBoxResult.Yes) return;
            var name = SelectedEvent.Name;
            _eventService.DeleteEvent(SelectedEvent);
            SelectedEvent = Events.FirstOrDefault();
            RefreshCalendarDay();
            PublishScheduleNotification(name, "удалено");
        }

        // ── Async registration ───────────────────────────────────────────────
        private async Task RegisterParticipantAsync()
        {
            if (SelectedEvent == null || CurrentUser == null) return;

            var participant = new Participant
            {
                DisplayName = CurrentUser.DisplayName,
                FirstName   = CurrentUser.DisplayName.Split(' ').FirstOrDefault() ?? "",
                LastName    = CurrentUser.DisplayName.Split(' ').Skip(1).FirstOrDefault() ?? "",
                Email       = CurrentUser.Email,
                Section     = SelectedEvent.Section
            };
            var dlg = new ParticipantEditDialog(participant, isNew: true);
            if (dlg.ShowDialog() != true) return;

            IsRegistering = true;
            ((RelayCommand)RegisterParticipantCommand).RaiseCanExecuteChanged();

            var progress = new Progress<string>(msg => StatusMessage = msg);
            try
            {
                await _eventService.RegisterParticipantAsync(
                    SelectedEvent,
                    participant.FirstName, participant.LastName,
                    participant.Email, participant.Phone,
                    participant.Section, participant.Organization,
                    progress);

                OnPropertyChanged(nameof(CurrentParticipants));
                ShowToast($"✅ Вы зарегистрированы на «{SelectedEvent.Name}»!");
            }
            finally
            {
                IsRegistering = false;
                ((RelayCommand)RegisterParticipantCommand).RaiseCanExecuteChanged();
            }
        }

        // ── Organizer participant management ─────────────────────────────────
        private void AddParticipant()
        {
            if (SelectedEvent == null) return;
            var p = new Participant();
            var dlg = new ParticipantEditDialog(p, isNew: true);
            if (dlg.ShowDialog() != true) return;
            _ = _eventService.RegisterParticipantAsync(
                SelectedEvent, p.FirstName, p.LastName,
                p.Email, p.Phone, p.Section, p.Organization);
            OnPropertyChanged(nameof(CurrentParticipants));
        }

        private void EditParticipant()
        {
            if (SelectedParticipant == null) return;
            var dlg = new ParticipantEditDialog(SelectedParticipant, isNew: false);
            if (dlg.ShowDialog() != true) return;
            _eventService.UpdateParticipant(SelectedParticipant);
        }

        private void DeleteParticipant()
        {
            if (SelectedEvent == null || SelectedParticipant == null) return;
            var r = MessageBox.Show($"Удалить участника «{SelectedParticipant.FullName}»?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r != MessageBoxResult.Yes) return;
            _eventService.RemoveParticipant(SelectedEvent, SelectedParticipant);
            SelectedParticipant = null;
            OnPropertyChanged(nameof(CurrentParticipants));
        }

        // ── Chat (Named Pipes) ───────────────────────────────────────────────
        private async Task SendChatAsync()
        {
            if (string.IsNullOrWhiteSpace(ChatInput)) return;
            var msg = $"[{CurrentUser.DisplayName}] {ChatInput.Trim()}";
            ChatInput = "";
            await _pipeService.SendMessageAsync(msg);
        }

        // ── MMF Notifications ─────────────────────────────────────────────────
        private void PublishScheduleNotification(string eventName, string change)
            => _mmfService.PublishScheduleChange(eventName, change);

        // ── Calendar ──────────────────────────────────────────────────────────
        private void RefreshCalendarDay()
        {
            CalendarDayEvents.Clear();
            foreach (var ev in _eventService.GetEventsForDate(SelectedCalendarDate))
                CalendarDayEvents.Add(ev);
        }

        // ── Toast ─────────────────────────────────────────────────────────────
        private void ShowToast(string message)
        {
            StatusMessage = message;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            timer.Tick += (s, e) => { StatusMessage = ""; timer.Stop(); };
            timer.Start();
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private void RaiseEventCommands()
        {
            ((RelayCommand)EditEventCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteEventCommand).RaiseCanExecuteChanged();
            ((RelayCommand)AddParticipantCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteParticipantCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RegisterParticipantCommand).RaiseCanExecuteChanged();
        }

        private void RaiseAllCommands()
        {
            RaiseEventCommands();
            ((RelayCommand)CreateEventCommand).RaiseCanExecuteChanged();
            ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            ((RelayCommand)LogoutCommand).RaiseCanExecuteChanged();
            ((RelayCommand)EditParticipantCommand).RaiseCanExecuteChanged();
            ((RelayCommand)SendChatCommand).RaiseCanExecuteChanged();
        }

        public async System.Threading.Tasks.Task SaveDataAsync()
            => await _eventService.SaveAsync();

        private void ShowAbout() =>
            MessageBox.Show("EventApp v2.0\nWPF + MVVM + Named Pipes + MMF + JSON",
                "О программе", MessageBoxButton.OK, MessageBoxImage.Information);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
