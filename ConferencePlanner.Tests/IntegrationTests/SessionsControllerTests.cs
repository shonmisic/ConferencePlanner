using ConferenceDTO;
using ConferencePlanner.Tests.WebApplicationFactories;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ConferencePlanner.Tests.IntegrationTests
{
    public class SessionsControllerTests : IClassFixture<WebApplicationFactoryWithInMemory>
    {
        protected BaseWebApplicationFactory<TestStartup> Factory { get; }

        public SessionsControllerTests(WebApplicationFactoryWithInMemory factory)
        {
            Factory = factory;
        }

        [Fact]
        public async Task GetSessionsAsyncSuccess()
        {
            var _httpClient = Factory.CreateClient();

            var response = await _httpClient.GetAsync("/api/sessions");

            response.EnsureSuccessStatusCode();

            var sessions = await response.Content.ReadAsAsync<ICollection<SessionResponse>>();

            Assert.True(sessions.Count > 0);
        }
    }
}
