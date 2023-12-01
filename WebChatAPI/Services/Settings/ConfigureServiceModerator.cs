using Calabonga.AspNetCore.Controllers.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WebChatAPI.Mediator.Behaviors;

namespace WebChatAPI.Services.Settings
{
    public static class ConfigureServiceModerator
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddCommandAndQueries(typeof(Program).Assembly);
        }
    }
}
