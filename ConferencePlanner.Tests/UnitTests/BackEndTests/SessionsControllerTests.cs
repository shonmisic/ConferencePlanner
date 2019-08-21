﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackEnd.Controllers;
using BackEnd.Data;
using BackEnd.Repositories;
using ConferenceDTO;
using Moq;
using Xunit;

namespace ConferencePlanner.Tests.UnitTests.BackEndTests
{
    public class SessionsControllerTests
    {

        [Fact]
        public async Task GetSessionsSuccessful()
        {
            var sessionsRepositoryStub = new Mock<ISessionsRepository>();
            sessionsRepositoryStub.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(GetAllSessions());

            var controller = new SessionsController(sessionsRepositoryStub.Object);

            var result = (await controller.Get()).Value;
            var expectedResult = GetSessionResponses();

            Assert.Equal(expectedResult.Count, result.Count);
            Assert.Equal(expectedResult.ElementAt(0).ID, result.ElementAt(0).ID);
            Assert.Equal(expectedResult.ElementAt(0).Title, result.ElementAt(0).Title);
            Assert.Equal(expectedResult.ElementAt(0).Abstract, result.ElementAt(0).Abstract);
            Assert.Equal(expectedResult.ElementAt(1).ID, result.ElementAt(1).ID);
            Assert.Equal(expectedResult.ElementAt(1).Title, result.ElementAt(1).Title);
            Assert.Equal(expectedResult.ElementAt(1).Abstract, result.ElementAt(1).Abstract);
        }

        [Fact]
        public async Task GetSessionByIdSuccessful()
        {
            var id = 1;
            var session = new BackEnd.Data.Session
            {
                ID = id,
                Abstract = "abstract",
                Title = "title"
            };

            var sessionsRepositoryStub = new Mock<ISessionsRepository>();
            sessionsRepositoryStub.Setup(s => s.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(session);

            var controller = new SessionsController(sessionsRepositoryStub.Object);

            var result = (await controller.Get(id)).Value;
            var expectedResult = GetSingleSessionResponse();

            Assert.Equal(expectedResult.ID, result.ID);
            Assert.Equal(expectedResult.Title, result.Title);
            Assert.Equal(expectedResult.Abstract, result.Abstract);
        }

        private SessionResponse GetSingleSessionResponse()
        {
            return new SessionResponse
            {
                ID = 1,
                Abstract = "abstract",
                Title = "title"
            };
        }

        private ICollection<SessionResponse> GetSessionResponses()
        {
            return new List<SessionResponse>
            {
                new SessionResponse
                {
                    ID = 1,
                    Abstract = "abstract1",
                    Title = "title1"
                },
                new SessionResponse
                {
                    ID = 2,
                    Abstract = "abstract2",
                    Title = "title2"
                }
            };
        }

        private ICollection<BackEnd.Data.Session> GetAllSessions()
        {
            return new List<BackEnd.Data.Session>
            {
                new BackEnd.Data.Session
                {
                    ID = 1,
                    Abstract = "abstract1",
                    Title = "title1"
                },
                new BackEnd.Data.Session
                {
                    ID = 2,
                    Abstract = "abstract2",
                    Title = "title2"
                }
            };
        }
    }
}
