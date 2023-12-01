using Calabonga.UnitOfWork;
using Chat.AuthServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebChatAPI.Infrastructure;
using WebChatAPI.Infrastructure.Extensions;
using WebChatAPI.Models;

namespace WebChatAPI.Services
{
    public static class ConfigureServicesBase
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                         .AddUserStore<ApplicationUserStore>()
                         .AddEntityFrameworkStores<ApplicationDbContext>()
                         .AddDefaultTokenProviders();

            configuration.Bind("ConnectionStrings", new Config());

            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseMySql(Config.ApplicationDbContext, new MySqlServerVersion(new Version(8, 0, 32)));
            });

            services.AddAutoMapper(typeof(Program));

            services.AddControllers();

            services.AddMvc();

            services.AddUnitOfWork<ApplicationDbContext>();

            services.AddMemoryCache();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false; 
                options.Password.RequireLowercase = false; 
                options.Password.RequiredLength = 6; 
                options.Password.RequireUppercase = false; 
                options.Password.RequireNonAlphanumeric = false; 
                options.Password.RequiredUniqueChars = 0;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
                options.User.AllowedUserNameCharacters = null;
                options.User.RequireUniqueEmail = true;
            });

            services.AddOptions();

            services.Configure<CurrentAppSettings>(configuration.GetSection(nameof(CurrentAppSettings)));

            services.Configure<MvcOptions>(options => options.UseRouteSlugify());

            services.AddLocalization();

            services.AddHttpContextAccessor();

            services.AddResponseCaching();


        }
    }
}
