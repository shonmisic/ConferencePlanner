using ConferencePlanner.Tests.WebApplicationFactories;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ConferencePlanner.Tests.IntegrationTests
{
    public class SessionPageTests : IClassFixture<FrontEndWebApplicationFactory>
    {
        private readonly FrontEndWebApplicationFactory _factory;
        private readonly HttpClient _httpClient;

        public SessionPageTests(FrontEndWebApplicationFactory factory)
        {
            _factory = factory;
            _httpClient = _factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }

        [Fact]
        public async Task OnGetAsyncSuccessful()
        {
            var defaultPage = await _httpClient.GetAsync("/Session/1, 1");

            defaultPage.EnsureSuccessStatusCode();

            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
            var titleElement = content.QuerySelector("#title");

            Assert.Equal("title", titleElement.TextContent);
        }
    }
}
