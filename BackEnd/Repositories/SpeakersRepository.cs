using BackEnd.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public class SpeakersRepository : ISpeakersRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SpeakersRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Speaker> AddAsync(Speaker speaker, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entityEntry = await _dbContext.Speakers.AddAsync(speaker);

            await _dbContext.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public IQueryable<Speaker> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _dbContext.Speakers.AsNoTracking()
                                     .Include(s => s.SessionSpeakers)
                                        .ThenInclude(ss => ss.Session);
        }

        public async Task<Speaker> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Speakers.AsNoTracking()
                                            .Include(s => s.SessionSpeakers)
                                                .ThenInclude(ss => ss.Session)
                                            .SingleOrDefaultAsync(s => s.ID == id);
        }

        public async Task<Speaker> RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var speaker = await _dbContext.FindAsync<Speaker>(id);

            if (speaker != null)
            {
                _dbContext.Remove(speaker);
                await _dbContext.SaveChangesAsync();
            }

            return speaker;
        }

        public async Task<Speaker> UpdateAsync(Speaker updatedSpeaker, CancellationToken cancellationToken = default(CancellationToken))
        {
            var speaker = await _dbContext.FindAsync<Speaker>(updatedSpeaker.ID);

            if (speaker != null)
            {
                speaker.Name = updatedSpeaker.Name;
                speaker.Bio = updatedSpeaker.Bio;
                speaker.WebSite = updatedSpeaker.WebSite;
            }

            await _dbContext.SaveChangesAsync();

            return speaker;
        }
    }
}
