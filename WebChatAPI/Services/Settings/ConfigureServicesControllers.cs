using Microsoft.Extensions.DependencyInjection;

namespace WebChatAPI.Services.Settings
{
    public static class ConfigureServicesControllers
    {
        public static void ConfigureServices(IServiceCollection services) => services.AddControllers();
    }
}
