using Microsoft.Extensions.DependencyInjection;
using Server.SignalR.ConfigureServices.Extensions;
using Server.SignalR.Data.EntityDbBase;
using Server.SignalR.Hubs;

namespace Server.SignalR.Infrastructure
{
    public static class ConfigureServicesDependency
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<ICacheService, CacheService>();

            services.AddSingleton<ChatManager>();
        }
    }
}
