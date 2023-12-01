using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WebChatAPI.Services.Identity;

namespace WebChatAPI.Services.Settings
{
    public static class AuthorizationPolicyExtensions
    {
        public static void UseMicroserviceAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, MicroservicePermissionHandler>();
        }
    }
}
