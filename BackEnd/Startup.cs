using BackEnd.Data;
using BackEnd.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatabaseServices(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new Info { Title = "Conference Planner API", Version = "v1" })
            );

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddTransient<IAttendeesRepository, AttendeesRepository>();
            services.AddTransient<ISessionsRepository, SessionsRepository>();
            services.AddTransient<IImagesRepository, ImagesRepository>();

            if (Environment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddDistributedSqlServerCache(options =>
                {
                    options.ConnectionString = Configuration.GetConnectionString("DistCache_Connection");
                    options.SchemaName = "dbo";
                    options.TableName = "ConferenceDistCache";
                });
            }
        }

        // We have to override this method in our TestStartup, because we want to inject our custom database services
        protected virtual void ConfigureDatabaseServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                    //options.UseNpgsql(Configuration.GetConnectionString("PostgreSQLConnection"));
                    //options.UseMySQL(Configuration.GetConnectionString("MySQLConnection"));
                }
                else
                {
                    options.UseSqlite("Data Source=conferences.db");
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (env.IsStaging())
            {
                var context = app.ApplicationServices.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Conference Planner API v1")
            );

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseMvc();

            app.Run(context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });
        }
    }
}
