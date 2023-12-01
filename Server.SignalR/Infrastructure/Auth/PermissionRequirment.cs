using Microsoft.AspNetCore.Authorization;

namespace Server.SignalR.Infrastructure.Auth
{
    public class PermissionRequirment : IAuthorizationRequirement
    {
        public PermissionRequirment(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; set; }
    }
}
