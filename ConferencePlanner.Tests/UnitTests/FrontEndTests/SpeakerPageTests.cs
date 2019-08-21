using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Pages;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;

namespace ConferencePlanner.Tests.UnitTests.FrontEndTests
{
    public class SpeakerPageTests
    {
        [Fact]
        public async Task GetSpeakerAsyncFail_SpeakerNotFound()
        {
            var testSpeaker = GetTestSpeakerResponse();

            var apiClientStub = new Mock<IApiClient>();
            apiClientStub.Setup(a => a.GetSpeakerAsync(2))
                .ReturnsAsync(testSpeaker);

            var model = new SpeakerModel(apiClientStub.Object);

            var result = await model.OnGet(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetSpeakerAsyncSuccess()
        {
            var testSpeaker = GetTestSpeakerResponse();

            var apiClientStub = new Mock<IApiClient>();
            apiClientStub.Setup(a => a.GetSpeakerAsync(1))
                .ReturnsAsync(testSpeaker);

            var model = new SpeakerModel(apiClientStub.Object);

            var result = await model.OnGet(1);

            Assert.IsType<PageResult>(result);
        }

        private static SpeakerResponse GetTestSpeakerResponse()
        {
            return new SpeakerResponse
            {
                ID = 1,
                Bio = "bio",
                Name = "John",
                WebSite = "web.com"
            };
        }
    }
}
