using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventApp.Data;
using EventApp.Models;

namespace EventApp.Repositories
{
    /// <summary>
    /// CRUD-репозиторий участников поверх EF Core.
    /// </summary>
    public class ParticipantRepository
    {
        private readonly AppDbContext _context;

        public ParticipantRepository(AppDbContext context) => _context = context;

        public async Task<List<Participant>> GetByEventAsync(int eventId)
            => await _context.Participants
                .Where(p => p.EventId == eventId)
                .ToListAsync();

        public async Task AddAsync(Participant participant)
        {
            await _context.Participants.AddAsync(participant);
            await _context.SaveChangesAsync();   // ← SaveChangesAsync
        }

        public async Task UpdateAsync(Participant participant)
        {
            _context.Participants.Update(participant);
            await _context.SaveChangesAsync();   // ← SaveChangesAsync
        }

        public async Task DeleteAsync(Participant participant)
        {
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();   // ← SaveChangesAsync
        }
    }
}
