using ConferencePlanner.Tests.Mocks;
using FrontEnd;
using FrontEnd.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace ConferencePlanner.Tests.WebApplicationFactories
{
    public class FrontEndWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IApiClient>(new MockApiClient());
                services.AddSingleton<IAdminService>(new MockAdminService());
            });

        protected override IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder().UseStartup<Startup>();
    }
}
