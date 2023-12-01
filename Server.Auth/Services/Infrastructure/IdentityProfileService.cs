using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Auth.Services.Infrastructure
{
    /// <summary>
    /// Пользовательская реализация IProfileService
    /// </summary>
    public class IdentityProfileService : IProfileService
    {
        private readonly IAccountServices _accountServices;
        public IdentityProfileService(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        /// <summary>
        /// метод вызывается всякий раз, когда запрашиваются утверждения о 
        /// пользователе (например, во время создания токена или через конечную
        /// точку информации о пользователе)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var identifier = context.Subject.GetSubjectId();
            var profile = await _accountServices.GetUsersClaimsAsync(identifier);
            Program.Logger.Info($"установлен claims для - {profile.Identity.Name}");
            context.IssuedClaims = profile.Claims.ToList();
        }

        /// <summary>
        /// Возвращает IsActive 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var identifier = context.Subject.GetSubjectId();
            var user = await _accountServices.GetByIdAsync(new Guid(identifier));
            Program.Logger.Info($"установлен пользователь - {user.LastName}");
            context.IsActive = user != null;
        }
    }
}
