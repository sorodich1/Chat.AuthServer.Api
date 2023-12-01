using Chat.AuthServer;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WebChatAPI.Infrastructure.Extensions;
using WebChatAPI.Services.Identity;

namespace WebChatAPI.Services
{
    public static class ConfigureServicesAuthentication
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration.GetSection("IdentityServer").GetValue<string>("Url");

            services.AddAuthentication()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var patch = context.HttpContext.Request.Path;
                            if(!string.IsNullOrEmpty(accessToken) && patch.StartsWithSegments("/chat"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                    options.SupportedTokens = SupportedTokens.Jwt;
                    options.Authority = $"{url}";
                    options.EnableCaching = true;
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AtLeast", policy =>
                policy.Requirements.Add(new PermissionRequirement("111")));
            });

            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, MicroservicePermissionHandler>();

            services.AddIdentityServer(options =>
            {
                options.Authentication.CookieSlidingExpiration = true;
                options.IssuerUri = url;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.UserInteraction.LogoutUrl = "/Authentication/Login";
                options.UserInteraction.LogoutUrl = "/Authentication/Logout";
            })
                          .AddInMemoryPersistedGrants()
                          .AddDeveloperSigningCredential()
                          .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                          .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                          .AddInMemoryClients(IdentityServerConfig.GetClients())
                          .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
                          .AddAspNetIdentity<ApplicationUser>()
                          .AddJwtBearerClientAuthentication()
                          .AddProfileService<IdentityProfileService>();

        }
    }
}
