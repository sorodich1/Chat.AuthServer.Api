using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.SignalR.ConfigureServices.Extensions;
using Server.SignalR.Data.EntityDbBase;
using Server.SignalR.ViewModel;
using System;

namespace Server.SignalR.ConfigureServices.Collection
{
    public class ConfigureServicesBase
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            configuration.Bind("ConnectionString", new Config());

            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseMySql(Config.ApplicationContext, new MySqlServerVersion(new Version(8, 0, 32)));
            });

            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            services.AddUnitOfWork<ApplicationDbContext>();
            services.AddMemoryCache();
            services.AddRouting();
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddOptions();
            services.Configure<CurrentAppSettings>(configuration.GetSection(nameof(CurrentAppSettings)));
            services.Configure<MvcOptions>(options => options.UseRouteSlugifi());
            services.AddLocalization();
            services.AddHttpContextAccessor();
            services.AddResponseCaching();
        }
    }
}
