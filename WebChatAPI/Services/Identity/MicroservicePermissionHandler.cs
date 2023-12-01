using Calabonga.Microservices.Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebChatAPI.Services.Identity
{
    public class MicroservicePermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var identity = (ClaimsIdentity)context.User.Identity;
            var claim = ClaimsHelper.GetValue<string>(identity, requirement.Name);
            if (claim == null)
            {
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
