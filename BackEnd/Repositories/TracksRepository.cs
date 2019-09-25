using BackEnd.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public class TracksRepository : ITracksRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TracksRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Track> AddAsync(Track track, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newTrack = await _dbContext.Tracks.AddAsync(track);

            await _dbContext.SaveChangesAsync();

            return newTrack.Entity;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var track = await _dbContext.Tracks.Include(t => t.Sessions).SingleOrDefaultAsync(t => t.ID == id);

            _dbContext.Tracks.Remove(track);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Track>> GetAllByConferenceIdAsync(int conferenceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Tracks.AsNoTracking()
                                        .Where(t => t.ConferenceId == conferenceId)
                                        .ToListAsync();
        }

        public async Task<Track> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Tracks.AsNoTracking()
                                        .Include(t => t.Conference)
                                        .Include(t => t.Sessions)
                                        .SingleOrDefaultAsync(t => t.ID == id);
        }
    }
}
