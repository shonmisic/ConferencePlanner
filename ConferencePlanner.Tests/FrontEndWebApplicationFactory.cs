using ConferencePlanner.Tests.Mocks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ConferencePlanner.Tests
{
    public class FrontEndWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IApiClient, MockApiClient>();
                services.AddSingleton<IAdminService, MockAdminService>();
            });
        }
    }
}
