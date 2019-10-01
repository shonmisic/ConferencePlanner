using BackEnd.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Conference> AddAsync(Conference conference, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newConference = await _dbContext.Conferences.AddAsync(conference);

            await _dbContext.SaveChangesAsync();

            return newConference.Entity;
        }

        public async Task<Conference> DeleteByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var conference = await _dbContext.FindAsync<Conference>(id);

            if (conference != null)
            {
                _dbContext.Remove(conference);

                await _dbContext.SaveChangesAsync();
            }

            return conference;
        }

        public IQueryable<Conference> GetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _dbContext.Conferences.AsNoTracking()
                                        .Include(c => c.ConferenceAttendees)
                                            .ThenInclude(ca => ca.Attendee)
                                        .Include(c => c.Sessions)
                                        .Include(c => c.Speakers)
                                        .Include(c => c.Tracks);
        }

        public async Task<Conference> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Conferences.FindAsync(id);
        }

        public async Task<Conference> UpdateAsync(Conference conference, CancellationToken cancellationToken = default(CancellationToken))
        {
            var doesConferenceExist = await _dbContext.Conferences.AnyAsync(c => c.ID == conference.ID);

            if (!doesConferenceExist)
            {
                return null;
            }

            _dbContext.Entry(conference).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return conference;
        }
    }
}
