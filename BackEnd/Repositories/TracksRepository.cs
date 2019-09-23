using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class TracksRepository : ITracksRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TracksRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<Track>> GetByConferenceIdAsync(int conferenceId)
        {
            return await _dbContext.Tracks.Where(t => t.ConferenceId == conferenceId).ToListAsync();
        }
    }
}
