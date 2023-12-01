using Calabonga.Microservices.Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Auth.Helpers
{
    /// <summary>
    /// Обработчик разрешений для пользовательских реализаций авторизации
    /// </summary>
    public class MicroservicePermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var identity = (ClaimsIdentity?)context.User.Identity;

            var claim = ClaimsHelper.GetValue<string>(identity, requirement.PermissionName);

            Program.Logger.Info($"Получены разрешения для реализации интерфейса {claim}");

            if (claim == null)
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
