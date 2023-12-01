using Calabonga.UnitOfWork.Controllers.DependencyContainer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.AuthServer;
using WebChatAPI.Services;
using WebChatAPI.Services.Application;
using WebChatAPI.Services.Settings;
using WebChatAPI.Services.TestServices;

namespace WebChatAPI
{
    public class Program
    {
        public static Logger Logger { get; set; }
        public static List<ApplicationUser> Users = new List<ApplicationUser>();    
        public static string ttt { get; set; }
        public static async Task<int> Main(string[] args)
        {
            Logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            Logger.Debug("init main");
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();
                ConfigureServicesBase.ConfigureServices(builder.Services, builder.Configuration);



                ConfigureServicesAuthentication.ConfigureServices(builder.Services, builder.Configuration);
                ConfigureServicesSwapper.ConfigureServices(builder.Services, builder.Configuration);
                ConfigureServicesCors.ConfigureServices(builder.Services, builder.Configuration);
                ConfigureServicesControllers.ConfigureServices(builder.Services);
                ConfigureServiceModerator.ConfigureServices(builder.Services);
                ConfigureServicesSignalR.ConfigureServices(builder.Services);
                ConfigureServicesValidators.ConfigureServices(builder.Services);

                DependencyContainer.Common(builder.Services);
                NimbleDependencyContainer.ConfigureServices(builder.Services);

                var app = builder.Build();

                GlobalDiagnosticsContext.Set("configDir", "D:\\git\\Heron\\ChatAuth.API\\sorodich1\\Chat.AuthServer\\Logs");
                GlobalDiagnosticsContext.Set("connectionString", app.Configuration.GetConnectionString("DefaultConnection"));

                ConfigureCommon.Configure(app, app.Environment);
                ConfigureAuthentication.Configure(app);
                ConfigureEndpoints.Configure(app);

                var serviceProvaider = builder.Services.BuildServiceProvider();

                using (var host = serviceProvaider.CreateScope())
                {
                    DatabaseInitializer.Seed(host.ServiceProvider);
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
}