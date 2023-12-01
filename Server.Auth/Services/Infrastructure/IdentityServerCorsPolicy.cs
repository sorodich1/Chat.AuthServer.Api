using IdentityServer4.Services;
using System.Threading.Tasks;

namespace Server.Auth.Services.Infrastructure
{
    public class IdentityServerCorsPolicy : ICorsPolicyService
    {
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            return Task.FromResult(true);
        }
    }
}
