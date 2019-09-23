using BackEnd.Data;
using System;
using System.Collections.Generic;

namespace ConferencePlanner.Tests
{
    public class TestDataSeeder
    {
        public static readonly Session Session1 = new Session
        {
            ID = 1,
            Abstract = "abstract",
            Title = "title",
            StartTime = new DateTimeOffset(new DateTime(2019, 9, 20)),
            EndTime = new DateTimeOffset(new DateTime(2019, 9, 21)),
            Conference = conference1,
            ConferenceId = conference1.ID,
            SessionSpeakers = new List<SessionSpeaker>
            {
                new SessionSpeaker
                {
                    SessionId = Session1.ID,
                    SpeakerId = 1,
                    Speaker = new Speaker
                    {
                        ID = 1,
                        Name = "speaker1"
                    }
                }
            },
            SessionTags = new List<SessionTag>
            {
                new SessionTag
                {
                    SessionId = Session1.ID,
                    Tag = new Tag
                    {
                        ID = 1,
                        Name = "tag1"
                    },
                    TagId = 1
                }
            },
            TrackId = track1.ID,
            Track = track1
        };

        public static readonly Conference conference1 = new Conference
        {
            ID = 1
        };

        public static readonly Track track1 = new Track
        {
            ID = 1
        };

        private readonly ApplicationDbContext _context;

        public TestDataSeeder(ApplicationDbContext context)
        {
            _context = context;

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public void SeedToDoItems()
        {
            _context.Sessions.Add(Session1);
            _context.SaveChanges();
        }
    }
}
