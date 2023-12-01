using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace WebChatAPI.Services.Identity
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { _options = options.Value; }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policyExist = await base.GetPolicyAsync(policyName);
            if(policyExist != null)
            {
                return policyExist;
            }
            policyExist = new AuthorizationPolicyBuilder().AddRequirements(new PermissionRequirement(policyName)).Build();
            _options.AddPolicy(policyName, policyExist);
            return policyExist;
        }

    }
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string name) { Name = name; }
        public string Name { get; }
    }
}
