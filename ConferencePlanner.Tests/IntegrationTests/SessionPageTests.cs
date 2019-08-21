using System.Net.Http;
using System.Threading.Tasks;
using ConferencePlanner.Tests.Mocks;
using FrontEnd;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ConferencePlanner.Tests.IntegrationTests
{
    public class SessionPageTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _httpClient;

        public SessionPageTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _httpClient = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddTransient<IApiClient, MockApiClient>();
                        services.AddSingleton<IAdminService, MockAdminService>();
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }

        [Fact]
        public async Task OnGetAsyncSuccessful()
        {
            var defaultPage = await _httpClient.GetAsync("/Session/1");

            defaultPage.EnsureSuccessStatusCode();

            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
            var titleElement = content.QuerySelector("#title");

            Assert.Equal("title", titleElement.TextContent);
        }
    }
}
