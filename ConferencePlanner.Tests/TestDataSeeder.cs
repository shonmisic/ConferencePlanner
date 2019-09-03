using BackEnd.Data;

namespace ConferencePlanner.Tests
{
    public class TestDataSeeder
    {
        public static readonly Session FirstItem = new Session
        {
            ID = 1,
            Abstract = "abstract",
            Title = "title"
        };

        private readonly ApplicationDbContext _context;

        public TestDataSeeder(ApplicationDbContext context)
        {
            _context = context;

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public void SeedToDoItems()
        {
            _context.Sessions.Add(FirstItem);
            _context.SaveChanges();
        }
    }
}
