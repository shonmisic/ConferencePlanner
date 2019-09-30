using BackEnd.Data;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public class ConferencesRepository : IConferencesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ConferencesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Conference> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Conferences.FindAsync(id);
        }
    }
}
