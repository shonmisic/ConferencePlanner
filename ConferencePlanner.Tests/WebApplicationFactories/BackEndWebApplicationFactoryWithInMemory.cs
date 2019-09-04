using System;
using BackEnd.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ConferencePlanner.Tests.WebApplicationFactories
{
    public class BackEndWebApplicationFactoryWithInMemory : WebApplicationFactory<TestStartup>
    {
        private readonly InMemoryDatabaseRoot _databaseRoot = new InMemoryDatabaseRoot();
        private readonly string _connectionString = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.ConfigureServices(services =>
            {
                services
                    .AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(_connectionString, _databaseRoot);
                        options.UseInternalServiceProvider(services.BuildServiceProvider());
                    });
            });

        protected override IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder().UseStartup<TestStartup>();
    }
}
