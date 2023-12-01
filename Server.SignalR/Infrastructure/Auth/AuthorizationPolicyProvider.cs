using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Server.SignalR.Infrastructure.Auth
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policyExists = await base.GetPolicyAsync(policyName);
            if (policyExists != null)
            {
                return policyExists;
            }

            policyExists = new AuthorizationPolicyBuilder().AddRequirements(new PermissionRequirement(policyName)).Build();
            _options.AddPolicy(policyName, policyExists);
            return policyExists;
        }
    }

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; set; }
    }
}
