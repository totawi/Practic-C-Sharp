using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventApp.Data;
using EventApp.Models;

namespace EventApp.Repositories
{
    /// <summary>
    /// CRUD-репозиторий мероприятий поверх EF Core.
    /// Все записи — через await _context.SaveChangesAsync().
    /// </summary>
    public class EventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context) => _context = context;

        public async Task<List<Event>> GetAllAsync()
        {
            var events = await _context.Events
                .Include(e => e.ParticipantsList)
                .AsNoTracking()
                .ToListAsync();

            // Подключить ObservableCollection к каждому событию
            foreach (var ev in events)
                ev.SyncParticipants();

            return events;
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            var ev = await _context.Events
                .Include(e => e.ParticipantsList)
                .FirstOrDefaultAsync(e => e.Id == id);
            ev?.SyncParticipants();
            return ev;
        }

        public async Task AddAsync(Event ev)
        {
            await _context.Events.AddAsync(ev);
            await _context.SaveChangesAsync();   // ← SaveChangesAsync
        }

        public async Task UpdateAsync(Event ev)
        {
            _context.Events.Update(ev);
            await _context.SaveChangesAsync();   // ← SaveChangesAsync
        }

        public async Task DeleteAsync(Event ev)
        {
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();   // ← SaveChangesAsync
        }
    }
}
