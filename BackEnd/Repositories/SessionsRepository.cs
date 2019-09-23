using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class SessionsRepository : ISessionsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SessionsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Session> GetAll()
        {
            return _dbContext.Sessions.Include(s => s.Conference)
                                        .Include(s => s.SessionSpeakers)
                                            .ThenInclude(ss => ss.Speaker)
                                        .Include(s => s.Track)
                                        .Include(s => s.SessionTags)
                                            .ThenInclude(ss => ss.Tag);
        }

        public async Task<Session> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Sessions.AsNoTracking()
                                            .Include(s => s.Track)
                                            .Include(s => s.SessionSpeakers)
                                               .ThenInclude(ss => ss.Speaker)
                                            .Include(s => s.SessionTags)
                                               .ThenInclude(st => st.Tag)
                                            .SingleOrDefaultAsync(s => s.ID == id);
        }

        public async Task<ICollection<Session>> GetByConferenceIdAsync(int conferenceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Sessions.AsNoTracking()
                                            .Include(s => s.Track)
                                            .Include(s => s.SessionSpeakers)
                                               .ThenInclude(ss => ss.Speaker)
                                            .Include(s => s.SessionTags)
                                               .ThenInclude(st => st.Tag)
                                            .Where(s => s.ConferenceId == conferenceId)
                                            .ToListAsync();
        }

        public async Task<Session> AddAsync(Session session, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newSession = await _dbContext.Sessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();

            return newSession.Entity;
        }

        public async Task<Session> UpdateAsync(ConferenceDTO.Session session, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newSession = await _dbContext.Sessions.FindAsync(session.ID);

            newSession.ID = session.ID;
            newSession.Title = session.Title;
            newSession.Abstract = session.Abstract;
            newSession.StartTime = session.StartTime;
            newSession.EndTime = session.EndTime;
            newSession.TrackId = session.TrackId;
            newSession.ConferenceId = session.ConferenceId;

            await _dbContext.SaveChangesAsync();

            return newSession;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var session = await _dbContext.Sessions.FindAsync(id);

            _dbContext.Sessions.Remove(session);

            await _dbContext.SaveChangesAsync();
        }
    }
}
