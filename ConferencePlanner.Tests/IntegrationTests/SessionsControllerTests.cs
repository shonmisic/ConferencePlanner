using ConferenceDTO;
using ConferencePlanner.Tests.WebApplicationFactories;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ConferencePlanner.Tests.IntegrationTests
{
    public class SessionsControllerTests : IClassFixture<BackEndWebApplicationFactoryWithInMemory>
    {
        protected BackEndWebApplicationFactoryWithInMemory Factory { get; }

        public SessionsControllerTests(BackEndWebApplicationFactoryWithInMemory factory)
        {
            Factory = factory;
        }

        [Fact]
        public async Task GetSessionsAsyncSuccess()
        {
            var _httpClient = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var response = await _httpClient.GetAsync("/api/sessions");

            response.EnsureSuccessStatusCode();

            var sessions = await response.Content.ReadAsAsync<ICollection<SessionResponse>>();

            Assert.True(sessions.Count > 0);
        }
    }
}
