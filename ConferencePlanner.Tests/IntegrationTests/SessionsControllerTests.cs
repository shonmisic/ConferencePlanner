using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BackEnd;
using ConferenceDTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ConferencePlanner.Tests.IntegrationTests
{
    public class SessionsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _httpClient;

        public SessionsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task GetSessionsAsyncSuccess()
        {
            var response = await _httpClient.GetAsync("/api/sessions");

            response.EnsureSuccessStatusCode();

            var sessions = await response.Content.ReadAsAsync<ICollection<SessionResponse>>();

            Assert.True(sessions.Count > 0);
        }
    }
}
