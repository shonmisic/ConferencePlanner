using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using FrontEnd.Pages;
using FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace ConferencePlanner.Tests.UnitTests.FrontEndTests
{
    public class SessionPageTests
    {
        [Fact]
        public async Task OnGetAsyncSuccessfull()
        {
            var sessionId = 2;
            var testSession = GetTestSessionResponse();
            var apiClientStub = new Mock<IApiClient>();

            apiClientStub.Setup(a => a.GetSessionAsync(It.IsAny<int>()))
                .ReturnsAsync(testSession);
            apiClientStub.Setup(a => a.GetSessionsAsync())
                .ReturnsAsync(GetTestAllSessionsAsync());
            apiClientStub.Setup(a => a.GetSessionsByAttendeeAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestAllSessionsAsync());

            var displayName = "User name";
            var identity = new GenericIdentity(displayName);
            var principle = new ClaimsPrincipal(identity);
            // use default context with user
            var httpContext = new DefaultHttpContext()
            {
                User = principle
            };
            //need these as well for the page context
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            // need page context for the page model
            var pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };
            //create model with necessary dependencies
            var model = new SessionModel(apiClientStub.Object)
            {
                PageContext = pageContext
            };
            await model.OnGetAsync(sessionId);

            Assert.Equal(testSession.ID, model.Session.ID);
            Assert.Equal(testSession.Title, model.Session.Title);
            Assert.Equal(testSession.Abstract, model.Session.Abstract);

            Assert.True(model.IsInPersonalAgenda);

            Assert.Equal(1, model.DayOffset);
        }

        private static List<ConferenceDTO.SessionResponse> GetTestAllSessionsAsync()
        {
            return new List<ConferenceDTO.SessionResponse>
                {
                    new ConferenceDTO.SessionResponse
                    {
                        ID = 1,
                        Title = "title",
                        Abstract = "abstract",
                        ConferenceId = 1,
                        Track = new ConferenceDTO.Track
                        {
                            ConferenceId = 1,
                            ID = 1,
                            Name = "track1",
                        },
                        StartTime = new DateTimeOffset(2019, 8, 8, 18, 0, 0, new TimeSpan(0, 0, 0)),
                        EndTime = new DateTimeOffset(2019, 8, 8, 19, 0, 0, new TimeSpan(0, 0, 0)),
                    },
                    new ConferenceDTO.SessionResponse
                    {
                        ID = 2,
                        Title = "title2",
                        Abstract = "abstract2",
                        ConferenceId = 1,
                        Track = new ConferenceDTO.Track
                        {
                            ConferenceId = 1,
                            ID = 2,
                            Name = "track2",
                        },
                        StartTime = new DateTimeOffset(2019, 08, 09, 18, 0, 0, new TimeSpan(0, 0, 0)),
                        EndTime = new DateTimeOffset(2019, 08, 09, 19, 0, 0, new TimeSpan(0, 0, 0)),
                    },
                };
        }

        private static ConferenceDTO.SessionResponse GetTestSessionResponse()
        {
            return new ConferenceDTO.SessionResponse
            {
                ID = 2,
                Title = "title2",
                Abstract = "abstract2",
                ConferenceId = 1,
                Track = new ConferenceDTO.Track
                {
                    ConferenceId = 1,
                    ID = 2,
                    Name = "track2",
                },
                StartTime = new DateTimeOffset(2019, 08, 09, 18, 0, 0, new TimeSpan(0, 0, 0)),
                EndTime = new DateTimeOffset(2019, 08, 09, 19, 0, 0, new TimeSpan(0, 0, 0)),
            };
        }
    }
}
