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
                                            .ThenInclude(ss => ss.Tag)
                                        .Include(s => s.SessionAttendees)
                                            .ThenInclude(s => s.Attendee);
        }

        public async Task<Session> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Sessions.AsNoTracking()
                                            .Include(s => s.Track)
                                            .Include(s => s.SessionSpeakers)
                                               .ThenInclude(ss => ss.Speaker)
                                            .Include(s => s.SessionTags)
                                               .ThenInclude(st => st.Tag)
                                            .Include(s => s.SessionAttendees)
                                                .ThenInclude(s => s.Attendee)
                                            .SingleOrDefaultAsync(s => s.ID == id);
        }

        public Task<IQueryable<Session>> GetByConferenceIdAsync(int conferenceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(
                        _dbContext.Sessions.AsNoTracking()
                                        .Include(s => s.Track)
                                        .Include(s => s.SessionSpeakers)
                                            .ThenInclude(ss => ss.Speaker)
                                        .Include(s => s.SessionTags)
                                            .ThenInclude(st => st.Tag)
                                        .Include(s => s.SessionAttendees)
                                            .ThenInclude(s => s.Attendee)
                                        .Where(s => s.ConferenceId == conferenceId));
        }

        public Task<IQueryable<Session>> GetByTrackIdAsync(int trackId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(
                        _dbContext.Sessions.AsNoTracking()
                                        .Include(s => s.Track)
                                        .Include(s => s.SessionSpeakers)
                                            .ThenInclude(ss => ss.Speaker)
                                        .Include(s => s.SessionTags)
                                            .ThenInclude(st => st.Tag)
                                        .Include(s => s.SessionAttendees)
                                            .ThenInclude(s => s.Attendee)
                                        .Where(s => s.TrackId == trackId));
        }

        public async Task<Session> AddAsync(Session session, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newSession = await _dbContext.Sessions.AddAsync(session);
            await _dbContext.SaveChangesAsync(cancellationToken);

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

            await _dbContext.SaveChangesAsync(cancellationToken);

            return newSession;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var session = await _dbContext.Sessions.FindAsync(id);

            _dbContext.Sessions.Remove(session);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
