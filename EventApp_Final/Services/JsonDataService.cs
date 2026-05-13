using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EventApp.Models;

namespace EventApp.Services
{
    public class JsonDataService
    {
        private static readonly string DataDir =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string EventsFile = Path.Combine(DataDir, "events.json");
        private static readonly string UsersFile  = Path.Combine(DataDir, "users.json");

        private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public JsonDataService() => Directory.CreateDirectory(DataDir);

        public async Task<List<Event>> LoadEventsAsync()
        {
            if (!File.Exists(EventsFile)) return GetSeedEvents();
            var json = await File.ReadAllTextAsync(EventsFile);
            return JsonSerializer.Deserialize<List<Event>>(json, JsonOpts) ?? new List<Event>();
        }

        public async Task SaveEventsAsync(IEnumerable<Event> events)
        {
            var json = JsonSerializer.Serialize(events, JsonOpts);
            await File.WriteAllTextAsync(EventsFile, json);
        }

        public async Task<List<User>> LoadUsersAsync()
        {
            if (!File.Exists(UsersFile))
            {
                var seed = GetSeedUsers();
                await SaveUsersAsync(seed);
                return seed;
            }
            var json = await File.ReadAllTextAsync(UsersFile);
            return JsonSerializer.Deserialize<List<User>>(json, JsonOpts) ?? new List<User>();
        }

        public async Task SaveUsersAsync(IEnumerable<User> users)
        {
            var json = JsonSerializer.Serialize(users, JsonOpts);
            await File.WriteAllTextAsync(UsersFile, json);
        }

        private List<Event> GetSeedEvents()
        {
            var e1 = new Event
            {
                Id = 1, Name = "IT-конференция 2025",
                Date = new DateTime(2025, 6, 15),
                Location = "Конференц-зал А", Section = "Технологии",
                Description = "Ежегодная конференция по информационным технологиям"
            };
            e1.Participants.Add(new Participant
                { Id=1, FirstName="Иван", LastName="Петров", Email="petrov@mail.ru",
                  Section="Разработка", Organization="ООО Техно" });
            e1.Participants.Add(new Participant
                { Id=2, FirstName="Мария", LastName="Иванова", Email="ivanova@mail.ru",
                  Section="Дизайн", Organization="АО Медиа" });

            var e2 = new Event
            {
                Id = 2, Name = "Бизнес-форум",
                Date = new DateTime(2025, 7, 20),
                Location = "Бизнес-центр", Section = "Бизнес",
                Description = "Форум для предпринимателей и инвесторов"
            };
            e2.Participants.Add(new Participant
                { Id=3, FirstName="Алексей", LastName="Смирнов", Email="smirnov@corp.ru",
                  Section="Инвестиции", Organization="Банк+" });

            return new List<Event> { e1, e2 };
        }

        private List<User> GetSeedUsers()
        {
            var auth = new AuthService();
            var salt1 = auth.GenerateSalt();
            var salt2 = auth.GenerateSalt();
            return new List<User>
            {
                new User { Id=1, Username="admin", DisplayName="Администратор",
                    Role=UserRole.Organizer, Email="admin@events.ru",
                    Salt=salt1, PasswordHash=auth.HashPassword("admin123", salt1) },
                new User { Id=2, Username="user1", DisplayName="Иван Петров",
                    Role=UserRole.Participant, Email="petrov@mail.ru",
                    Salt=salt2, PasswordHash=auth.HashPassword("user123", salt2) }
            };
        }
    }
}
