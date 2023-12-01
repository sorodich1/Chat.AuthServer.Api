using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Auth.Data.Entityes;
using Server.Auth.Helpers;
using System.Threading.Tasks;

namespace Server.Auth.Services.Configure
{
    public static class ConfigureAuthServices
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration.GetSection("IdentityServer").GetValue<string>("Url");
            services.AddAuthentication()
                .AddIdentityServerAuthentication(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            Program.Logger.Info(accessToken);

                            var path = context.HttpContext.Request.Path;

                            Program.Logger.Info(path);

                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                            {
                                context.Token = accessToken;

                                Program.Logger.Info(context.Token);
                            }

                            Program.Logger.Info("Настройка токена завершена");
                            return Task.CompletedTask;
                        }
                    };
                    options.SupportedTokens = SupportedTokens.Jwt;
                    options.Authority = $"{url}";
                    options.EnableCaching = true;
                    options.RequireHttpsMetadata = false;
                });
            Program.Logger.Info("Настройка токена не удолась");

            services.AddIdentityServer(options =>
            {
                options.Authentication.CookieSlidingExpiration = true;
                options.IssuerUri = $"{url}";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.UserInteraction.LoginUrl = "/Authentication/Login";
                options.UserInteraction.LogoutUrl = "/Authentication/Logout";
            })
                .AddInMemoryPersistedGrants()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetResource())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResource())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddInMemoryApiScopes(IdentityServerConfig.GetScope())
                .AddAspNetIdentity<ApplicationUsers>()
                .AddJwtBearerClientAuthentication();
        }
    }
}
