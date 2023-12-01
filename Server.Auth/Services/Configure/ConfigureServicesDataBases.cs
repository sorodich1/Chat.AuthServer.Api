using Calabonga.UnitOfWork;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Auth.Data.DbContext;
using Server.Auth.Data.Entityes;
using Server.Auth.TokenSettings;
using Server.Auth.ViewModel;
using System;
using System.Configuration;

namespace Server.Auth.Services.Configure
{
    public static class ConfigureServicesDataBases
    {
        public static void ConfigureServices(IServiceCollection  services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUsers, ApplicationRoles>()
                .AddUserStore<ApplicationUserStore>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            Program.Logger.Info("IdentityService - поделючен");

            configuration.Bind("ConnectionStrings", new Config());


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(Config.ApplicationContext, new MySqlServerVersion(new Version(8, 0, 32))/*, x => x.MigrationsAssembly("Server.Auth.Data")*/);
            });

            Program.Logger.Info("DbContext - поделючен");

            services.AddUnitOfWork<ApplicationDbContext>();

            services.AddAutoMapper(typeof(Program));

            services.AddMemoryCache();

            services.AddRouting(options => options.LowercaseUrls = true);

            Program.Logger.Info("Routing - поделючен");

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.Configure<IdentityOptions>(options =>
            {
                //настройки пароля
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                //настройки блокировки
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                //настройки пользователя
                options.User.AllowedUserNameCharacters = null;
                options.User.RequireUniqueEmail = true;
            });

            Program.Logger.Info("Configure - поделючен");

            services.AddOptions();

            services.Configure<MvcOptions>(options => options.UseRouteSlugify());

            Program.Logger.Info("MvcOptions - поделючен");

            services.AddLocalization();
            services.AddHttpContextAccessor();
            services.AddResponseCaching();

            Program.Logger.Info("Connect - поделючен");
        }
    }
}
