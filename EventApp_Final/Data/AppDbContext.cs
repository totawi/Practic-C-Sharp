using Microsoft.EntityFrameworkCore;
using EventApp.Models;

namespace EventApp.Data
{
    /// <summary>
    /// EF Core DbContext — SQLite база данных (eventapp.db рядом с exe).
    /// Используется репозиториями. SaveChangesAsync() — единственный способ записи.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Event>       Events       { get; set; }
        public DbSet<Participant> Participants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var dbPath = System.IO.Path.Combine(
                System.AppDomain.CurrentDomain.BaseDirectory, "eventapp.db");
            options.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            // Event → Participants  (one-to-many)
            model.Entity<Event>()
                 .HasMany(e => e.ParticipantsList)
                 .WithOne()
                 .HasForeignKey("EventId")
                 .OnDelete(DeleteBehavior.Cascade);

            // Seed data
            model.Entity<Event>().HasData(
                new Event { Id = 1, Name = "IT-конференция 2025",
                    DateUtc = new System.DateTime(2025, 6, 15, 0, 0, 0, System.DateTimeKind.Utc),
                    Location = "Конференц-зал А", Section = "Технологии",
                    Description = "Ежегодная IT-конференция" },
                new Event { Id = 2, Name = "Бизнес-форум",
                    DateUtc = new System.DateTime(2025, 7, 20, 0, 0, 0, System.DateTimeKind.Utc),
                    Location = "Бизнес-центр", Section = "Бизнес",
                    Description = "Форум для предпринимателей" }
            );
        }
    }
}
