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
                                            .SingleOrDefaultAsync(a => a.UserName == username);
        }

        public async Task<Attendee> AddAsync(Attendee attendee, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newAttendee = _dbContext.Attendees.Add(attendee);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return newAttendee.Entity;
        }

        public async Task<Attendee> AddSessionAsync(string username, int sessionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var attendee = await _dbContext.Attendees.Include(a => a.SessionAttendees)
                                                     .Include(a => a.ConferenceAttendees)
                                                     .SingleOrDefaultAsync(a => a.UserName == username);

            var session = await _dbContext.Sessions.FindAsync(sessionId);

            attendee.SessionAttendees.Add(new SessionAttendee
            {
                AttendeeId = attendee.ID,
                SessionId = sessionId,
            });

            if (!IsAttendingConference(attendee, session.ConferenceId))
            {
                attendee.ConferenceAttendees.Add(new ConferenceAttendee
                {
                    AttendeeId = attendee.ID,
                    ConferenceId = session.ConferenceId
                });
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return await GetByUsernameAsync(username);
        }

        public async Task<bool> RemoveSessionAsync(string username, int sessionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var attendee = await _dbContext.Attendees.Include(a => a.SessionAttendees)
                                                        .ThenInclude(sa => sa.Session)
                                                    .Include(a => a.ConferenceAttendees)
                                                    .SingleOrDefaultAsync(a => a.UserName == username);

            var sessionAttendee = attendee.SessionAttendees.SingleOrDefault(sa => sa.SessionId == sessionId);

            var success = attendee.SessionAttendees.Remove(sessionAttendee);

            var conferenceId = sessionAttendee.Session.ConferenceId;
            if (success && !DoesContainOtherSessionsFromTheSameConference(attendee, conferenceId))
            {
                var conferenceAttendee = attendee.ConferenceAttendees.SingleOrDefault(ca => ca.ConferenceId == conferenceId);

                attendee.ConferenceAttendees.Remove(conferenceAttendee);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return success;
        }

        public async Task UpdateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private static bool IsAttendingConference(Attendee attendee, int conferenceId)
        {
            return attendee.ConferenceAttendees.Any(ca => ca.ConferenceId == conferenceId);
        }

        private static bool DoesContainOtherSessionsFromTheSameConference(Attendee attendee, int conferenceId)
        {
            return attendee.SessionAttendees.Select(sa => sa.Session).Any(s => s.ConferenceId == conferenceId);
        }
    }
}
