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
using EventApp.Data;
using EventApp.Models;
using EventApp.Services;
using EventApp.Views;

namespace EventApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // ── Services & Context ───────────────────────────────────────────────
        private readonly AppDbContext           _context;
        private readonly JsonDataService        _dataService;
        private readonly AuthService            _authService;
        public  readonly EventService           _eventService;   // public — для Window_Closing
        private readonly PipesChatService       _pipeService;
        private readonly MmfNotificationService _mmfService;

        // ── State ────────────────────────────────────────────────────────────
        private Event       _selectedEvent = null;
        private Participant _selectedParticipant = null;
        private DateTime    _selectedCalendarDate = DateTime.Today;
        private bool        _isLoading;
        private string      _statusMessage = string.Empty;
        private bool        _isRegistering;
        private string      _chatInput = string.Empty;

        // ── Collections ──────────────────────────────────────────────────────
        public ObservableCollection<Event>    Events            => _eventService.Events;
        public ObservableCollection<string>   ChatMessages      { get; } = new();
        public ObservableCollection<string>   Notifications     { get; } = new();
        public ObservableCollection<Event>    CalendarDayEvents { get; } = new();

        // ── Auth props ───────────────────────────────────────────────────────
        public User CurrentUser    => _authService.CurrentUser;
        public bool IsLoggedIn     => _authService.IsLoggedIn;
        public bool IsOrganizer    => _authService.IsOrganizer;
        public bool IsNotOrganizer => !IsOrganizer;

        // ── Bindable props ───────────────────────────────────────────────────
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
            set { _selectedCalendarDate = value; OnPropertyChanged(); RefreshCalendarDay(); }
        }

        public bool   IsLoading     { get => _isLoading;     set { _isLoading = value;     OnPropertyChanged(); } }
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }
        public bool   IsRegistering { get => _isRegistering; set { _isRegistering = value; OnPropertyChanged(); } }
        public string ChatInput     { get => _chatInput;     set { _chatInput = value;     OnPropertyChanged(); } }

        public ObservableCollection<Participant> CurrentParticipants
            => SelectedEvent?.Participants ?? new ObservableCollection<Participant>();

        // ── Commands ─────────────────────────────────────────────────────────
        public ICommand CreateEventCommand       { get; }
        public ICommand EditEventCommand         { get; }
        public ICommand DeleteEventCommand       { get; }
        public ICommand ShowEventDetailCommand   { get; }
        public ICommand RegisterParticipantCommand { get; }
        public ICommand AddParticipantCommand    { get; }
        public ICommand EditParticipantCommand   { get; }
        public ICommand DeleteParticipantCommand { get; }
        public ICommand LoginCommand             { get; }
        public ICommand LogoutCommand            { get; }
        public ICommand SendChatCommand          { get; }
        public ICommand AboutCommand             { get; }
        public ICommand ExitCommand              { get; }

        // ── Constructor ──────────────────────────────────────────────────────
        public MainViewModel()
        {
            _context     = new AppDbContext();
            _dataService = new JsonDataService();
            _authService = new AuthService(_dataService);
            _eventService = new EventService(_context, _dataService);
            _pipeService = new PipesChatService();
            _mmfService  = new MmfNotificationService();

            CreateEventCommand     = new RelayCommand(async () => await CreateEventAsync(),  () => IsOrganizer);
            EditEventCommand       = new RelayCommand(async () => await EditEventAsync(),    () => IsOrganizer && SelectedEvent != null);
            DeleteEventCommand     = new RelayCommand(async () => await DeleteEventAsync(),  () => IsOrganizer && SelectedEvent != null);
            ShowEventDetailCommand = new RelayCommand(ShowEventDetail, () => SelectedEvent != null);
            RegisterParticipantCommand = new RelayCommand(
                async () => await RegisterParticipantAsync(),
                () => IsLoggedIn && SelectedEvent != null && !IsRegistering);
            AddParticipantCommand    = new RelayCommand(async () => await AddParticipantAsync(),    () => IsOrganizer && SelectedEvent != null);
            EditParticipantCommand   = new RelayCommand(async () => await EditParticipantAsync(),   () => IsOrganizer && SelectedParticipant != null);
            DeleteParticipantCommand = new RelayCommand(async () => await DeleteParticipantAsync(), () => IsOrganizer && SelectedEvent != null && SelectedParticipant != null);
            LoginCommand    = new RelayCommand(ShowLogin,  () => !IsLoggedIn);
            LogoutCommand   = new RelayCommand(DoLogout,   () => IsLoggedIn);
            SendChatCommand = new RelayCommand(async () => await SendChatAsync(),
                              () => IsLoggedIn && !string.IsNullOrWhiteSpace(ChatInput));
            AboutCommand = new RelayCommand(ShowAbout);
            ExitCommand  = new RelayCommand(async () => { await SaveDataAsync(); Application.Current.Shutdown(); });

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
                await _eventService.LoadAsync();          // EnsureCreated + загрузка из SQLite

                _pipeService.MessageReceived += msg =>
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ChatMessages.Add(msg);
                        if (ChatMessages.Count > 200) ChatMessages.RemoveAt(0);
                    });
                _pipeService.StartServer();

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
            if (dlg.ShowDialog() != true) return;
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsOrganizer));
            OnPropertyChanged(nameof(IsNotOrganizer));
            StatusMessage = $"Добро пожаловать, {CurrentUser.DisplayName}!";
            RaiseAllCommands();
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

        // ── CRUD Events ───────────────────────────────────────────────────────

        /// <summary>CreateEventCommand → await _context.SaveChangesAsync() внутри репозитория</summary>
        private async Task CreateEventAsync()
        {
            var ev = new Event { DateUtc = DateTime.UtcNow, Name = "Новое мероприятие" };
            var dlg = new EventEditDialog(ev, isNew: true);
            if (dlg.ShowDialog() != true) return;

            await _eventService.CreateEventAsync(   // ← SaveChangesAsync внутри
                ev.Name, ev.Date, ev.Location, ev.Section, ev.Description);

            SelectedEvent = Events.Last();
            RefreshCalendarDay();
            PublishScheduleNotification(ev.Name, "создано");
            ShowToast($"✅ Мероприятие «{ev.Name}» создано");
        }

        private async Task EditEventAsync()
        {
            if (SelectedEvent == null) return;
            var dlg = new EventEditDialog(SelectedEvent, isNew: false);
            if (dlg.ShowDialog() != true) return;
            await _eventService.UpdateEventAsync(SelectedEvent);   // ← SaveChangesAsync
            RefreshCalendarDay();
            PublishScheduleNotification(SelectedEvent.Name, "изменено");
        }

        private async Task DeleteEventAsync()
        {
            if (SelectedEvent == null) return;
            var r = MessageBox.Show($"Удалить «{SelectedEvent.Name}»?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (r != MessageBoxResult.Yes) return;
            var name = SelectedEvent.Name;
            await _eventService.DeleteEventAsync(SelectedEvent);   // ← SaveChangesAsync
            SelectedEvent = Events.FirstOrDefault();
            RefreshCalendarDay();
            PublishScheduleNotification(name, "удалено");
        }

        // ── Detail popup (клик по мероприятию) ────────────────────────────────
        private void ShowEventDetail()
        {
            if (SelectedEvent == null) return;
            var popup = new EventDetailPopup(SelectedEvent)
            {
                Owner = Application.Current.MainWindow
            };
            popup.Show();    // не-модальное — можно открыть несколько
        }

        // ── RegisterParticipantCommand → await SaveChangesAsync() ─────────────
        private async Task RegisterParticipantAsync()
        {
            if (SelectedEvent == null || CurrentUser == null) return;

            var p = new Participant
            {
                DisplayName = CurrentUser.DisplayName,
                FirstName   = CurrentUser.DisplayName.Split(' ').FirstOrDefault() ?? "",
                LastName    = CurrentUser.DisplayName.Split(' ').Skip(1).FirstOrDefault() ?? "",
                Email       = CurrentUser.Email,
                Section     = SelectedEvent.Section
            };
            var dlg = new ParticipantEditDialog(p, isNew: true);
            if (dlg.ShowDialog() != true) return;

            IsRegistering = true;
            ((RelayCommand)RegisterParticipantCommand).RaiseCanExecuteChanged();

            var progress = new Progress<string>(msg => StatusMessage = msg);
            try
            {
                await _eventService.RegisterParticipantAsync(   // ← SaveChangesAsync + Task.Delay(3000)
                    SelectedEvent,
                    p.FirstName, p.LastName,
                    p.Email, p.Phone, p.Section, p.Organization,
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

        private async Task AddParticipantAsync()
        {
            if (SelectedEvent == null) return;
            var p = new Participant();
            var dlg = new ParticipantEditDialog(p, isNew: true);
            if (dlg.ShowDialog() != true) return;
            await _eventService.RegisterParticipantAsync(
                SelectedEvent, p.FirstName, p.LastName,
                p.Email, p.Phone, p.Section, p.Organization);
            OnPropertyChanged(nameof(CurrentParticipants));
        }

        private async Task EditParticipantAsync()
        {
            if (SelectedParticipant == null) return;
            var dlg = new ParticipantEditDialog(SelectedParticipant, isNew: false);
            if (dlg.ShowDialog() != true) return;
            await _eventService.UpdateParticipantAsync(SelectedParticipant);  // ← SaveChangesAsync
        }

        private async Task DeleteParticipantAsync()
        {
            if (SelectedEvent == null || SelectedParticipant == null) return;
            var r = MessageBox.Show($"Удалить «{SelectedParticipant.FullName}»?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r != MessageBoxResult.Yes) return;
            await _eventService.RemoveParticipantAsync(SelectedEvent, SelectedParticipant); // ← SaveChangesAsync
            SelectedParticipant = null;
            OnPropertyChanged(nameof(CurrentParticipants));
        }

        // ── Chat ─────────────────────────────────────────────────────────────
        private async Task SendChatAsync()
        {
            if (string.IsNullOrWhiteSpace(ChatInput)) return;
            var msg = $"[{CurrentUser.DisplayName}] {ChatInput.Trim()}";
            ChatInput = "";
            await _pipeService.SendMessageAsync(msg);
        }

        private void PublishScheduleNotification(string name, string change)
            => _mmfService.PublishScheduleChange(name, change);

        // ── Calendar ─────────────────────────────────────────────────────────
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
            var t = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            t.Tick += (s, e) => { StatusMessage = ""; t.Stop(); };
            t.Start();
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private void RaiseEventCommands()
        {
            ((RelayCommand)EditEventCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteEventCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ShowEventDetailCommand).RaiseCanExecuteChanged();
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

        public async Task SaveDataAsync() => await _context.SaveChangesAsync();

        private void ShowAbout() =>
            MessageBox.Show("EventApp v3.0\nWPF + MVVM + EF Core SQLite\nNamed Pipes + MMF + Animations",
                "О программе", MessageBoxButton.OK, MessageBoxImage.Information);

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
