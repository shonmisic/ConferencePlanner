using BackEnd;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConferencePlanner.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration, IHostingEnvironment env) : base(configuration, env)
        {
        }

        protected override void ConfigureDatabaseServices(IServiceCollection services)
        {
            services.AddTransient<TestDataSeeder>();
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            base.Configure(app, env);

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var seeder = serviceScope.ServiceProvider.GetService<TestDataSeeder>();
                seeder.SeedToDoItems();
            }
        }
    }
}