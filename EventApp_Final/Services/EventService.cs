using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EventApp.Data;
using EventApp.Models;
using EventApp.Repositories;

namespace EventApp.Services
{
    public class EventService
    {
        private readonly AppDbContext          _context;
        private readonly EventRepository       _eventRepo;
        private readonly ParticipantRepository _participantRepo;

        // JSON-fallback (если нет EF)
        private readonly JsonDataService _jsonData;

        public ObservableCollection<Event> Events { get; } = new();

        public EventService(AppDbContext context, JsonDataService jsonData)
        {
            _context         = context;
            _jsonData        = jsonData;
            _eventRepo       = new EventRepository(context);
            _participantRepo = new ParticipantRepository(context);
        }

        public async Task LoadAsync()
        {
            // Создать / мигрировать БД при первом запуске
            await _context.Database.EnsureCreatedAsync();

            var list = await _eventRepo.GetAllAsync();
            Events.Clear();
            foreach (var ev in list)
                Events.Add(ev);
        }

        public async Task SaveAsync()
        {
            // SaveChangesAsync вызывается в репозиториях,
            // здесь оставляем для совместимости с window closing
            await _context.SaveChangesAsync();
        }

        // ── Events ──────────────────────────────────────────────────────────

        /// <summary>Создать мероприятие → await SaveChangesAsync()</summary>
        public async Task<Event> CreateEventAsync(string name, DateTime date,
            string location, string section, string description)
        {
            var ev = new Event
            {
                Name = name, DateUtc = date.ToUniversalTime(),
                Location = location, Section = section, Description = description
            };
            await _eventRepo.AddAsync(ev);   // внутри — SaveChangesAsync
            ev.SyncParticipants();
            Events.Add(ev);
            return ev;
        }

        public async Task UpdateEventAsync(Event ev)
        {
            // Синхронизируем ObservableCollection → List перед сохранением
            ev.ParticipantsList.Clear();
            foreach (var p in ev.Participants) ev.ParticipantsList.Add(p);
            await _eventRepo.UpdateAsync(ev);
        }

        public async Task DeleteEventAsync(Event ev)
        {
            await _eventRepo.DeleteAsync(ev);
            Events.Remove(ev);
        }

        // ── Participants ──────────────────────────────────────────────────────

        /// <summary>Регистрация участника с имитацией отправки email (Task.Delay 3s)</summary>
        public async Task<Participant> RegisterParticipantAsync(
            Event ev,
            string firstName, string lastName,
            string email, string phone,
            string section, string organization,
            IProgress<string> progress = null)
        {
            var p = new Participant
            {
                EventId = ev.Id,
                FirstName = firstName, LastName = lastName,
                Email = email, Phone = phone,
                Section = section, Organization = organization
            };

            // Сначала добавить в БД через SaveChangesAsync
            await _participantRepo.AddAsync(p);  // ← SaveChangesAsync внутри

            // Добавить в ObservableCollection (UI обновится автоматически)
            ev.Participants.Add(p);

            // Имитация отправки приглашения
            progress?.Report($"📧 Отправка приглашения на {email}…");
            await Task.Delay(3000);
            progress?.Report($"✅ Приглашение отправлено на {email}");

            return p;
        }

        public async Task RemoveParticipantAsync(Event ev, Participant p)
        {
            await _participantRepo.DeleteAsync(p);
            ev.Participants.Remove(p);
        }

        public async Task UpdateParticipantAsync(Participant p)
            => await _participantRepo.UpdateAsync(p);

        // ── Calendar ─────────────────────────────────────────────────────────

        public IEnumerable<Event> GetEventsForDate(DateTime date)
            => Events.Where(e => e.Date.Date == date.Date);

        public IEnumerable<DateTime> GetEventDates()
            => Events.Select(e => e.Date.Date).Distinct();
    }
}
