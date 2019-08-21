using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BackEnd;
using BackEnd.Data;
using BackEnd.Repositories;
using ConferenceDTO;
using Moq;
using Xunit;

namespace ConferencePlanner.Tests.UnitTests.BackEndTests
{
    public class AttendeesControllerTests
    {
        [Fact]
        public async Task GetAttendeeSuccessful()
        {
            var username = "test";
            var testAttendee = GetTestAttendee(username);
            var attendeesRepositoryStub = new Mock<IAttendeesRepository>();
            attendeesRepositoryStub.Setup(ar => ar.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testAttendee);

            var sessionsRepositoryStub = new Mock<ISessionsRepository>();

            var controller = new AttendeesController(attendeesRepositoryStub.Object, sessionsRepositoryStub.Object);

            var result = (await controller.Get(username)).Value;
            var expectedValue = GetTestAttendeeResponse(username);

            Assert.Equal(expectedValue.ID, result.ID);
            Assert.Equal(expectedValue.FirstName, result.FirstName);
            Assert.Equal(expectedValue.LastName, result.LastName);
            Assert.Equal(expectedValue.Sessions.FirstOrDefault()?.ID, result.Sessions.FirstOrDefault()?.ID);
            Assert.Equal(expectedValue.Sessions.FirstOrDefault()?.Title, result.Sessions.FirstOrDefault()?.Title);
            Assert.Equal(expectedValue.Conferences.FirstOrDefault()?.ID, result.Conferences.FirstOrDefault()?.ID);
            Assert.Equal(expectedValue.Conferences.FirstOrDefault()?.Name, result.Conferences.FirstOrDefault()?.Name);
        }

        private static BackEnd.Data.Attendee GetTestAttendee(string username)
        {
            return new BackEnd.Data.Attendee
            {
                ID = 1,
                UserName = username,
                FirstName = "john",
                LastName = "doe",
                EmailAddress = "email@e.a",
                ConferenceAttendees = new List<ConferenceAttendee>
                {
                    new ConferenceAttendee
                    {
                        AttendeeId = 1,
                        Conference = new BackEnd.Data.Conference
                        {
                            ID = 1,
                            Name = "conf"
                        }
                    }
                },
                SessionAttendees = new List<SessionAttendee>
                {
                    new SessionAttendee
                    {
                        Session = new BackEnd.Data.Session
                        {
                            ID = 1,
                            Title = "session"
                        },
                        AttendeeId = 1
                    }
                }
            };
        }

        private static AttendeeResponse GetTestAttendeeResponse(string username)
        {
            return new AttendeeResponse
            {
                ID = 1,
                UserName = username,
                FirstName = "john",
                LastName = "doe",
                Conferences = new List<ConferenceDTO.Conference>
                {
                    new ConferenceDTO.Conference
                    {
                        ID = 1,
                        Name = "conf",
                    }
                },
                Sessions = new List<ConferenceDTO.Session>
                {
                    new ConferenceDTO.Session
                    {
                        ID = 1,
                        Title = "session",
                    }
                }
            };
        }
    }
}
