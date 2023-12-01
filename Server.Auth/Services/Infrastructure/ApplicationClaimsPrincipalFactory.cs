using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Server.Auth.Data.Entityes;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Auth.Services.Infrastructure
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUsers, ApplicationRoles>
    {
        public ApplicationClaimsPrincipalFactory(UserManager<ApplicationUsers> userManager, RoleManager<ApplicationRoles> roleManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor) 
        {

        }

        /// <summary>
        /// Асинхронно создает <see cref="T:ClaimsPrincipal" /> от пользователя.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="user">Пользователь для создания <see cref="T:ClaimsPrincipal" /> от.</param>
        /// <returns><see cref="T:Task" />, который представляет операцию асинхронного создания, содержащую созданный <see cref="T:ClaimsPrincipal"</returns>
        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUsers user)
        {
            var principal = await base.CreateAsync(user);
            //if(user.ApplicationUserProfile.Permissions != null)
            //{
            //     var permissions = user.ApplicationUserProfile.Permissions.ToList();
            //    if(permissions.Any())
            //    {
            //        permissions.ForEach(x => ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(x.PolicyName, JwtClaimTypes.Role)));
            //    }
            //}
            if(!string.IsNullOrWhiteSpace(user.FirstName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            }
            if(!string.IsNullOrWhiteSpace(user.LastName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
            }

            return principal;
        }
    }
}
