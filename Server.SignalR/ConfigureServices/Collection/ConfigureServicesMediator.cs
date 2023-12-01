using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Server.SignalR.ConfigureServices.Extensions;

namespace Server.SignalR.ConfigureServices.Collection
{
    public static class ConfigureServicesMediator
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Program).Assembly));
            //services.AddCommandAndQueries(typeof(Program).Assembly);
        }
    }
}
