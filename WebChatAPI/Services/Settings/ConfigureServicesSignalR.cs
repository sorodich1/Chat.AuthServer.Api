using Microsoft.Extensions.DependencyInjection;

namespace WebChatAPI.Services.Settings
{
    public class ConfigureServicesSignalR
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }
    }
}
