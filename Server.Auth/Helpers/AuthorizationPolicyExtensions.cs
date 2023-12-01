using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Auth.Helpers
{
    /// <summary>
    /// Регистрация политики авторизации
    /// </summary>
    public static class AuthorizationPolicyExtensions
    {
        /// <summary>
        /// Регистрирует пользовательский <see cref="IAuthorizationHandler"/> для возможности использования авторизации по политике.
        /// </summary>
        /// <param name="collection"></param>
        public static void UseMicroserviceAuthorizationPolicy(this IServiceCollection collection)
        {
            collection.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            collection.AddSingleton<IAuthorizationHandler, MicroservicePermissionHandler>();
        }
    }
}
