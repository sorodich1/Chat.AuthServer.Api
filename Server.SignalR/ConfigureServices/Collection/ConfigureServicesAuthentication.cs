using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.SignalR.Infrastructure.Auth;
using System.Threading.Tasks;

namespace Server.SignalR.ConfigureServices.Collection
{
    public static class ConfigureServicesAuthentication
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = options =>
                        {
                            var accessToken = options.HttpContext.Request.Query["access_token"];
                            var path = options.HttpContext.Request.Path;
                            if(!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                            {
                                options.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                    options.SupportedTokens = SupportedTokens.Jwt;
                    options.Authority = "https://localhost:10001";
                    options.EnableCaching = true;
                    options.RequireHttpsMetadata = false;
                });
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, MicroservicePermissionHandler>();
        }
    }
}
