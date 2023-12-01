using Calabonga.UnitOfWork.Controllers.DependencyContainer;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using Server.SignalR.ConfigureServices.Application;
using Server.SignalR.ConfigureServices.Collection;
using Server.SignalR.Data;
using Server.SignalR.Infrastructure;
using System;
using System.Threading.Tasks;

internal class Program
{
    public static Logger Logger { get; set; }
    public static async Task<int> Main(string[] args)
    {
        Logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            ConfigureServicesBase.ConfigureServices(builder.Services, builder.Configuration);
            ConfigureServicesAuthentication.ConfigureServices(builder.Services, builder.Configuration);
            ConfigureServicesSwagger.ConfigureServices(builder.Services, builder.Configuration);
            ConfigureServicesCors.ConfigureServices(builder.Services, builder.Configuration);

            builder.Services.AddControllers();

            ConfigureServicesMediator.ConfigureServices(builder.Services);

            builder.Services.AddSignalR();

            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            ConfigureServicesDependency.ConfigureServices(builder.Services);
            NimbleDependencyContainer.ConfigureServices(builder.Services);

            var app = builder.Build();


            ConfigureCommon.Configure(app, app.Environment);
            ConfigureEndpoint.Configure(app);


            var serviceProvider = builder.Services.BuildServiceProvider();

            using(var host = serviceProvider.CreateScope())
            {
                DatabaseInitializer.Seed(host.ServiceProvider);
            }

            //app

            await app.RunAsync();

            return 0;
        }
        catch(Exception ex)
        {
            Logger.Error(ex);
            return 1;
        }
        finally
        {
            LogManager.Shutdown();
        }


    }
}