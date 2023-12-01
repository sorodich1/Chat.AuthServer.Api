using Calabonga.Microservices.Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.SignalR.Infrastructure.Auth
{
    public class MicroservicePermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var identity = (ClaimsIdentity)context.User.Identity!;
            var claim = ClaimsHelper.GetValue<string>(identity, requirement.PermissionName);
            if (claim == null)
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
