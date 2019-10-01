using System;
using FrontEnd.Data;
using FrontEnd.Filter;
using FrontEnd.HealthChecks;
using FrontEnd.Infrastructure;
using FrontEnd.Pages;
using FrontEnd.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using static FrontEnd.Infrastructure.FriendlyUrlHelper;

namespace FrontEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(options => options.Filters.AddService<RequireLoginFilter>())
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Admin", "Admin");
                    options.Conventions.Add(
                        new PageRouteTransformerConvention(
                            new SlugifyParameterTransformer()));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ServiceUrl"]);
            });
            services.AddSingleton<IApiClientFactory, ApiClientFactory>();
            //.AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            //{
            //    TimeSpan.FromSeconds(1),
            //    TimeSpan.FromSeconds(5),
            //    TimeSpan.FromSeconds(10),
            //}));

            services.AddSingleton<IAdminService, AdminService>();

            services.AddTransient<RequireLoginFilter>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser()
                        .RequiredIsAdminClaim();
                });
            });

            services.AddHealthChecks()
                .AddCheck<BackendHealthChecks>("backend")
                .AddDbContextCheck<IdentityDbContext>();

            services.AddMemoryCache();
            services.AddSingleton<MemoryCacheSingleton>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
