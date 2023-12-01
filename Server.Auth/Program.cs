using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using Server.Auth.Data.HelperInitializer;
using Server.Auth.Services.Application;
using Server.Auth.Services.Configure;
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
            Logger.Info("Запуск приложения");

            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            ConfigureServicesDataBases.ConfigureServices(builder.Services, builder.Configuration);
            ConfigureAuthServices.ConfigureServices(builder.Services, builder.Configuration);
            ConfigureServiceSwagger.ConfigureServices(builder.Services);
            ConfigureServicesCors.ConfigureServices(builder.Services, builder.Configuration);
            ConfigureServicesDependency.Common(builder.Services);
            builder.Services.AddControllersWithViews();


            var app = builder.Build();

            ConfigureCommon.Configure(app, app.Environment);
            ConfigureAuthentication.Configure(app);
            app.MapDefaultControllerRoute();

            var serviceProvider = builder.Services.BuildServiceProvider();

            using(var host = serviceProvider.CreateScope())
            {
                DaBaseInitializer.Seed(host.ServiceProvider);
            }

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