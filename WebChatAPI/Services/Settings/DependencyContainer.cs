using Chat.AuthServer;
using Chat.AuthServer.Interfaces;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WebChatAPI.Hubs;
using WebChatAPI.Infrastructure.Auth;
using WebChatAPI.Services.Identity;

namespace WebChatAPI.Services.Settings
{
    public partial class DependencyContainer
    {
        public static void Common(IServiceCollection services)
        {
            services.AddTransient<ApplicationUserStore>();
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ApplicationClaimsPrincipalFactory>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IProfileService, IdentityProfileService>();
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<ICorsPolicyService, IdentityServerCorsePolicy>();

            services.AddSingleton<ChatManager>();
        }
    }
    public class IdentityServerCorsePolicy : ICorsPolicyService
    {
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            return Task.FromResult(true);
        }
    }
}
