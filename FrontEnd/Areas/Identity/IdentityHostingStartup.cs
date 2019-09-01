using System;
using FrontEnd.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FrontEnd.Areas.Identity.IdentityHostingStartup))]
namespace FrontEnd.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IdentityDbContext>(options =>
                    //options.UseSqlServer(
                    //    context.Configuration.GetConnectionString("IdentityDbContextConnection")));
                    options.UseNpgsql(
                        context.Configuration.GetConnectionString("PostgreSQLConnection")));

                services.AddDefaultIdentity<User>(options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 1;
                        options.Password.RequiredUniqueChars = 0;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                    })
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<IdentityDbContext>()
                    .AddClaimsPrincipalFactory<ClaimsPrincipalFactory>();

                services.AddAuthentication().AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = context.Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = context.Configuration["Authentication:Facebook:AppSecret"];
                });
            });
        }
    }
}