using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EventOrganizer
{
    public partial class MainWindow : Window
    {
        // Коллекции данных
        private ObservableCollection<Event> _events = new ObservableCollection<Event>();
        private ObservableCollection<Participant> _participants = new ObservableCollection<Participant>();

        private int _eventIdCounter = 1;
        private int _participantIdCounter = 1;

        public MainWindow()
        {
            InitializeComponent();
            LoadSampleData();
            BindData();
        }

        // ─── Placeholder-эмуляция для WPF TextBox ────────────────────────────

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == tb.Tag?.ToString())
            {
                tb.Text = "";
                tb.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = tb.Tag?.ToString() ?? "";
                tb.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private string GetFieldValue(TextBox tb)
        {
            string val = tb.Text.Trim();
            return val == tb.Tag?.ToString() ? "" : val;
        }

        private void ResetField(TextBox tb)
        {
            tb.Text = tb.Tag?.ToString() ?? "";
            tb.Foreground = System.Windows.Media.Brushes.Gray;
        }

        // ─── Инициализация ───────────────────────────────────────────────────

        private void LoadSampleData()
        {
            // Тестовые мероприятия
            _events.Add(new Event
            {
                Id = _eventIdCounter++,
                Name = "Конференция по информационным технологиям",
                Date = new DateTime(2025, 6, 15),
                Section = "Пленарная"
            });
            _events.Add(new Event
            {
                Id = _eventIdCounter++,
                Name = "Семинар по разработке ПО",
                Date = new DateTime(2025, 6, 20),
                Section = "Техническая"
            });
            _events.Add(new Event
            {
                Id = _eventIdCounter++,
                Name = "Воркшоп: WPF и XAML",
                Date = new DateTime(2025, 7, 5),
                Section = "Мастер-класс"
            });

            // Тестовые участники
            AddParticipantToData("Иван", "Петров", "i.petrov@mail.ru", "Докладчик", 1);
            AddParticipantToData("Мария", "Сидорова", "m.sidorova@mail.ru", "Слушатель", 1);
            AddParticipantToData("Алексей", "Козлов", "a.kozlov@mail.ru", "Модератор", 2);
            AddParticipantToData("Елена", "Новикова", "e.novikova@mail.ru", "Докладчик", 2);
            AddParticipantToData("Дмитрий", "Волков", "d.volkov@mail.ru", "Организатор", 3);
        }

        private void BindData()
        {
            EventsDataGrid.ItemsSource = _events;
            AllParticipantsListBox.ItemsSource = _participants;
            ParticipantEventBox.ItemsSource = _events;
            UpdateStats();
        }

        // ─── Мероприятия ─────────────────────────────────────────────────────

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            string name = GetFieldValue(EventNameBox);

            if (string.IsNullOrEmpty(name))
            {
                ShowStatus("⚠ Введите название мероприятия!", isError: true);
                EventNameBox.Focus();
                return;
            }

            if (EventDatePicker.SelectedDate == null)
            {
                ShowStatus("⚠ Выберите дату мероприятия!", isError: true);
                return;
            }

            var section = (EventSectionBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Другое";

            var newEvent = new Event
            {
                Id = _eventIdCounter++,
                Name = name,
                Date = EventDatePicker.SelectedDate.Value,
                Section = section
            };

            _events.Add(newEvent);

            // Обновляем список мероприятий в форме регистрации
            ParticipantEventBox.Items.Refresh();

            // Очистка полей
            ResetField(EventNameBox);
            EventDatePicker.SelectedDate = DateTime.Today;

            UpdateStats();
            ShowStatus($"✓ Мероприятие «{newEvent.Name}» успешно добавлено.");
        }

        private void DeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            if (EventsDataGrid.SelectedItem is Event selected)
            {
                // Удаляем участников этого мероприятия
                var toRemove = _participants.Where(p => p.EventId == selected.Id).ToList();
                foreach (var p in toRemove)
                    _participants.Remove(p);

                _events.Remove(selected);
                ParticipantEventBox.Items.Refresh();
                EventParticipantsListBox.ItemsSource = null;

                UpdateStats();
                ShowStatus($"✓ Мероприятие «{selected.Name}» и его участники удалены.");
            }
            else
            {
                ShowStatus("⚠ Выберите мероприятие для удаления.", isError: true);
            }
        }

        private void EventsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EventsDataGrid.SelectedItem is Event selected)
            {
                var eventParticipants = _participants.Where(p => p.EventId == selected.Id).ToList();
                EventParticipantsListBox.ItemsSource = eventParticipants;
                ParticipantsOfEventLabel.Text = $"Участники: {selected.Name}";
                ShowStatus($"Выбрано мероприятие «{selected.Name}» — {eventParticipants.Count} участников.");

                // Предвыбираем мероприятие в форме регистрации
                ParticipantEventBox.SelectedValue = selected.Id;
            }
        }

        // ─── Участники ───────────────────────────────────────────────────────

        private void RegisterParticipant_Click(object sender, RoutedEventArgs e)
        {
            string firstName = GetFieldValue(ParticipantFirstNameBox);
            string lastName = GetFieldValue(ParticipantLastNameBox);
            string email = GetFieldValue(ParticipantEmailBox);
            string role = (ParticipantRoleBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Слушатель";

            // Валидация
            if (string.IsNullOrEmpty(firstName))
            {
                ShowStatus("⚠ Введите имя участника!", isError: true);
                ParticipantFirstNameBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(lastName))
            {
                ShowStatus("⚠ Введите фамилию участника!", isError: true);
                ParticipantLastNameBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                ShowStatus("⚠ Введите корректный e-mail!", isError: true);
                ParticipantEmailBox.Focus();
                return;
            }
            if (ParticipantEventBox.SelectedItem == null)
            {
                ShowStatus("⚠ Выберите мероприятие для регистрации!", isError: true);
                ParticipantEventBox.Focus();
                return;
            }

            var selectedEvent = ParticipantEventBox.SelectedItem as Event;

            // Проверяем дублирование по e-mail в рамках одного мероприятия
            bool duplicate = _participants.Any(p =>
                p.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                p.EventId == selectedEvent.Id);

            if (duplicate)
            {
                ShowStatus($"⚠ Участник с e-mail «{email}» уже зарегистрирован на это мероприятие!", isError: true);
                return;
            }

            AddParticipantToData(firstName, lastName, email, role, selectedEvent.Id);

            // Обновляем список участников выбранного мероприятия
            if (EventsDataGrid.SelectedItem is Event currentEvent && currentEvent.Id == selectedEvent.Id)
            {
                var eventParticipants = _participants.Where(p => p.EventId == selectedEvent.Id).ToList();
                EventParticipantsListBox.ItemsSource = eventParticipants;
            }

            // Обновляем счётчик
            selectedEvent.ParticipantCount = _participants.Count(p => p.EventId == selectedEvent.Id);
            EventsDataGrid.Items.Refresh();

            // Очистка полей
            ResetField(ParticipantFirstNameBox);
            ResetField(ParticipantLastNameBox);
            ResetField(ParticipantEmailBox);

            UpdateStats();
            ShowStatus($"✓ Участник {firstName} {lastName} успешно зарегистрирован на «{selectedEvent.Name}».");
        }

        private void AddParticipantToData(string first, string last, string email, string role, int eventId)
        {
            var ev = _events.FirstOrDefault(e => e.Id == eventId);
            var participant = new Participant
            {
                Id = _participantIdCounter++,
                FirstName = first,
                LastName = last,
                Email = email,
                Role = role,
                EventId = eventId,
                EventName = ev?.Name ?? "—"
            };
            _participants.Add(participant);

            if (ev != null)
            {
                ev.ParticipantCount = _participants.Count(p => p.EventId == eventId);
            }
        }

        private void DeleteParticipant_Click(object sender, RoutedEventArgs e)
        {
            if (AllParticipantsListBox.SelectedItem is Participant selected)
            {
                int eventId = selected.EventId;
                _participants.Remove(selected);

                // Обновляем счётчик мероприятия
                var ev = _events.FirstOrDefault(e2 => e2.Id == eventId);
                if (ev != null)
                    ev.ParticipantCount = _participants.Count(p => p.EventId == eventId);

                EventsDataGrid.Items.Refresh();

                // Обновляем нижний список
                if (EventsDataGrid.SelectedItem is Event currentEvent && currentEvent.Id == eventId)
                {
                    EventParticipantsListBox.ItemsSource = _participants
                        .Where(p => p.EventId == eventId).ToList();
                }

                UpdateStats();
                ShowStatus($"✓ Участник {selected.FullName} удалён.");
            }
            else
            {
                ShowStatus("⚠ Выберите участника для удаления.", isError: true);
            }
        }

        // ─── Вспомогательные методы ──────────────────────────────────────────

        private void UpdateStats()
        {
            TotalEventsLabel.Text = $"Мероприятий: {_events.Count}";
            TotalParticipantsLabel.Text = $"Участников: {_participants.Count}";
            AllParticipantsCountLabel.Text = $"{_participants.Count} зарегистрировано";
        }

        private void ShowStatus(string message, bool isError = false)
        {
            StatusLabel.Text = message;
            StatusLabel.Foreground = isError
                ? new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(255, 138, 101))
                : new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(159, 168, 218));
        }
    }
}
