using BackEnd.Data;
using System;
using System.Collections.Generic;

namespace ConferencePlanner.Tests
{
    public class TestDataSeeder
    {
        private readonly ApplicationDbContext _context;

        public TestDataSeeder(ApplicationDbContext context)
        {
            _context = context;

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public void SeedToDoItems()
        {
            _context.Attendees.Add(new Attendee
            {
                ID = 1
            });
            _context.Conferences.Add(new Conference
            {
                ID = 1
            });
            _context.Tags.Add(new Tag
            {
                ID = 1,
                Name = "tag1"
            });
            _context.Speakers.Add(new Speaker
            {
                ID = 1,
                Name = "speaker1"
            });
            _context.Tracks.Add(new Track
            {
                ID = 1,
                Name = "track1"
            });
            _context.Sessions.Add(new Session
            {
                ID = 1,
                Abstract = "abstract",
                Title = "title",
                StartTime = new DateTimeOffset(new DateTime(2019, 9, 20)),
                EndTime = new DateTimeOffset(new DateTime(2019, 9, 21)),
                ConferenceId = 1,
                SessionSpeakers = new List<SessionSpeaker>
                {
                    new SessionSpeaker
                    {
                        SessionId = 1,
                        SpeakerId = 1,
                    }
                },
                SessionTags = new List<SessionTag>
                {
                    new SessionTag
                    {
                        SessionId = 1,
                        TagId = 1
                    }
                },
                TrackId = 1,
                SessionAttendees = new List<SessionAttendee>
                {
                    new SessionAttendee
                    {
                        SessionId = 1,
                        AttendeeId = 1
                    }
                }
            });

            _context.SaveChanges();
        }
    }
}
