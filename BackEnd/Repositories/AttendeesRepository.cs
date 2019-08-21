﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repositories
{
    public class AttendeesRepository : IAttendeesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AttendeesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Attendee> GetByUsernameAsync(string username, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Attendees.Include(a => a.SessionAttendees)
                                                .ThenInclude(sa => sa.Session)
                                             .Include(a => a.ConferenceAttendees)
                                                .ThenInclude(ca => ca.Conference)
                                             .SingleOrDefaultAsync(a => a.UserName == username);
        }

        public async Task<Attendee> AddAsync(Attendee attendee, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newAttendee = _dbContext.Attendees.Add(attendee);

            await _dbContext.SaveChangesAsync();

            return newAttendee.Entity;
        }

        public async Task<Attendee> AddSessionAsync(string username, int sessionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var attendee = await _dbContext.Attendees.SingleOrDefaultAsync(a => a.UserName == username);
            attendee.SessionAttendees.Add(new SessionAttendee
            {
                AttendeeId = attendee.ID,
                SessionId = sessionId,
            });

            await _dbContext.SaveChangesAsync();

            return attendee;
        }

        public async Task<bool> RemoveSessionAsync(string username, int sessionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var attendee = await _dbContext.Attendees.SingleOrDefaultAsync(a => a.UserName == username);
            var sessionAttendee = attendee.SessionAttendees.SingleOrDefault(sa => sa.SessionId == sessionId);

            var success = attendee.SessionAttendees.Remove(sessionAttendee);

            await _dbContext.SaveChangesAsync();

            return success;
        }
    }
}