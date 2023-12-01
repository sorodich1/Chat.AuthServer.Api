using Calabonga.Microservices.Core.Extensions;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;
using WebChatAPI.Infrastructure.Auth;

namespace WebChatAPI.Services.Identity
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IAccountService _accountService;

        public IdentityProfileService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var idintifier = context.Subject.GetSubjectId();
            Program.ttt = idintifier;
            var profile = await _accountService.GetUserClaimsAsync(idintifier);
            context.IssuedClaims.AddRange(profile.Claims);
            Program.Logger.Info($"Активация профиля клиента в системе для клиента");

           
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var idintifier = context.Subject.GetSubjectId();
            var user = await _accountService.GetByIdAsync(idintifier.ToGuid());

            Program.Users.Add(user);

            //success - добиться проверки идентификации
            context.IsActive = user != null;
            Program.Logger.Info($"Активация клиента {user.LastName} в системе {context.Client.ClientId}");

            if(context.IsActive)
            {
                Program.Logger.Info($"Активация клиента {user.LastName} в системе {context.Client.ClientId}");
            }

        }
    }
}
