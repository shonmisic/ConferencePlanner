using BackEnd.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BackEnd.Repositories
{
    public class AttendeesRepository : IAttendeesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AttendeesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Attendee> GetAll()
        {
            return _dbContext.Attendees.AsNoTracking()
                                       .Include(a => a.SessionAttendees)
                                            .ThenInclude(sa => sa.Session)
                                       .Include(a => a.ConferenceAttendees)
                                            .ThenInclude(ca => ca.Conference)
                                       .Include(a => a.AttendeeImages)
                                            .ThenInclude(ai => ai.Image);
        }

        public async Task<Attendee> GetByUsernameAsync(string username, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Attendees.AsNoTracking()
                                            .Include(a => a.SessionAttendees)
                                                .ThenInclude(sa => sa.Session)
                                            .Include(a => a.ConferenceAttendees)
                                                .ThenInclude(ca => ca.Conference)
                                            .Include(a => a.AttendeeImages)
                                                .ThenInclude(ai => ai.Image)
                                            .SingleOrDefaultAsync(a => a.UserName == username, cancellationToken);
        }

        public async Task<Attendee> AddAsync(Attendee attendee, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newAttendee = await _dbContext.Attendees.AddAsync(attendee, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return newAttendee.Entity;
        }

        public async Task<Attendee> AddSessionAsync(string username, int sessionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var attendee = await GetByUsernameTrackingAsync(username, cancellationToken);

            var session = await _dbContext.Sessions.FindAsync(new object[] { sessionId }, cancellationToken);

            attendee.SessionAttendees.Add(new SessionAttendee
            {
                AttendeeId = attendee.ID,
                SessionId = sessionId,
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return attendee;
        }

        public async Task<Attendee> AddConferenceAsync(string username, int conferenceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var attendee = await GetByUsernameTrackingAsync(username, cancellationToken);

            var conference = await _dbContext.Conferences.FindAsync(new object[] { conferenceId }, cancellationToken);

            attendee.ConferenceAttendees.Add(new ConferenceAttendee
            {
                AttendeeId = attendee.ID,
                ConferenceId = conferenceId,
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return attendee;
        }

        public async Task<bool> RemoveSessionAsync(string username, int sessionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var attendee = await GetByUsernameTrackingAsync(username, cancellationToken);

            var sessionAttendee = attendee.SessionAttendees.SingleOrDefault(sa => sa.SessionId == sessionId);

            var isSuccess = attendee.SessionAttendees.Remove(sessionAttendee);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return isSuccess;
        }

        public async Task UpdateAsync(Attendee attendee, CancellationToken cancellationToken = default(CancellationToken))
        {
            _dbContext.Update(attendee);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private static bool IsAttendingConference(Attendee attendee, int conferenceId)
        {
            return attendee.ConferenceAttendees.Any(ca => ca.ConferenceId == conferenceId);
        }

        public async Task<bool> RemoveConferenceAsync(string username, int conferenceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var attendee = await GetByUsernameTrackingAsync(username, cancellationToken);

            var conferenceAttendee = attendee.ConferenceAttendees.SingleOrDefault(ca => ca.ConferenceId == conferenceId);

            var isSuccess = attendee.ConferenceAttendees.Remove(conferenceAttendee);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return isSuccess;
        }

        private async Task<Attendee> GetByUsernameTrackingAsync(string username, CancellationToken cancellationToken)
        {
            return await _dbContext.Attendees.Include(a => a.SessionAttendees)
                                                .ThenInclude(sa => sa.Session)
                                            .Include(a => a.ConferenceAttendees)
                                                .ThenInclude(ca => ca.Conference)
                                            .Include(a => a.AttendeeImages)
                                                .ThenInclude(ai => ai.Image)
                                            .SingleOrDefaultAsync(a => a.UserName == username, cancellationToken);
        }
    }
}
