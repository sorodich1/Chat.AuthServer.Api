using Chat.AuthServer;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebChatAPI.Infrastructure.Auth
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public ApplicationClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options) { }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {

            Program.Logger.Info($"Вход пользователя в систему с именем  {user.UserName} роль пользователя {user.LastName}");
            var principal = await base.CreateAsync(user);
            if(user.ApplicationUserProfile?.Permission != null)
            {
                var permissions = user.ApplicationUserProfile.Permission.ToList();
                if(permissions.Any())
                {
                    permissions.ForEach(x => ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(x.PolicyName, JwtClaimTypes.Role)));
                }
                if(!string.IsNullOrEmpty(user.FirstName))
                {
                    ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
                }
            }
            return principal;
        }
    }
}
