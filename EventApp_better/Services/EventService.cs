using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EventApp.Models;

namespace EventApp.Services
{
    /// <summary>
    /// Сервис управления мероприятиями — вся бизнес-логика здесь, ViewModel только делегирует.
    /// </summary>
    public class EventService
    {
        private readonly JsonDataService _data;
        private int _nextEventId = 1;
        private int _nextParticipantId = 1;

        public ObservableCollection<Event> Events { get; } = new();

        public EventService(JsonDataService data) => _data = data;

        public async Task LoadAsync()
        {
            var list = await _data.LoadEventsAsync();
            Events.Clear();
            foreach (var ev in list)
                Events.Add(ev);

            _nextEventId   = Events.Count > 0 ? Events.Max(e => e.Id) + 1 : 1;
            _nextParticipantId = Events
                .SelectMany(e => e.Participants)
                .Select(p => p.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;
        }

        public async Task SaveAsync() => await _data.SaveEventsAsync(Events);

        // ── EVENTS ──────────────────────────────────────────────────────────

        public Event CreateEvent(string name, DateTime date,
            string location, string section, string description)
        {
            var ev = new Event
            {
                Id = _nextEventId++, Name = name, Date = date,
                Location = location, Section = section, Description = description
            };
            Events.Add(ev);
            _ = SaveAsync();
            return ev;
        }

        public void UpdateEvent(Event ev) => _ = SaveAsync();

        public void DeleteEvent(Event ev)
        {
            Events.Remove(ev);
            _ = SaveAsync();
        }

        // ── PARTICIPANTS ─────────────────────────────────────────────────────

        /// <summary>
        /// Асинхронная регистрация участника с имитацией отправки приглашения.
        /// </summary>
        public async Task<Participant> RegisterParticipantAsync(
            Event ev,
            string firstName, string lastName,
            string email, string phone,
            string section, string organization,
            IProgress<string> progress = null)
        {
            var participant = new Participant
            {
                Id = _nextParticipantId++,
                FirstName = firstName, LastName = lastName,
                Email = email, Phone = phone,
                Section = section, Organization = organization
            };

            ev.Participants.Add(participant);

            // Имитация отправки приглашения
            progress?.Report("Отправка приглашения…");
            await Task.Delay(3000);
            progress?.Report($"✅ Приглашение отправлено на {email}");

            await SaveAsync();
            return participant;
        }

        public void RemoveParticipant(Event ev, Participant participant)
        {
            ev.Participants.Remove(participant);
            _ = SaveAsync();
        }

        public void UpdateParticipant(Participant p) => _ = SaveAsync();

        // ── CALENDAR ────────────────────────────────────────────────────────

        /// <summary>Возвращает события на конкретную дату.</summary>
        public IEnumerable<Event> GetEventsForDate(DateTime date)
            => Events.Where(e => e.Date.Date == date.Date);

        /// <summary>Возвращает все даты у которых есть мероприятия.</summary>
        public IEnumerable<DateTime> GetEventDates()
            => Events.Select(e => e.Date.Date).Distinct();
    }
}
