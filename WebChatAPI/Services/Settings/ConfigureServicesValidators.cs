using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace WebChatAPI.Services.Settings
{
    public static class ConfigureServicesValidators
    {
        public static void ConfigureServices(IServiceCollection services) => services.AddValidatorsFromAssembly(typeof(Program).Assembly);
    }
}
