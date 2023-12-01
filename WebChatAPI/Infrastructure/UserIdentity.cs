using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace WebChatAPI.Infrastructure
{
    public sealed class UserIdentity
    {
        private UserIdentity() { }
       public static UserIdentity Instance => Lazy.Value;

       public static readonly Lazy<UserIdentity> Lazy = new Lazy<UserIdentity>(() => new UserIdentity());

        private static IHttpContextAccessor ContextAccessor { get; set; } = null;
        private bool IsInitialized { get; set; }

        public void Configure(IHttpContextAccessor httpContextAccessor)
        {
            ContextAccessor = httpContextAccessor ?? throw new Exception(nameof(IHttpContextAccessor));
            IsInitialized = true;
        }

        public IIdentity? User
        {
            get
            {
                if(IsInitialized)
                {
                    return ContextAccessor.HttpContext!.User.Identity != null
                        && ContextAccessor.HttpContext != null
                        && ContextAccessor.HttpContext.User.Identity.IsAuthenticated
                        ? ContextAccessor.HttpContext.User.Identity : null;
                }
                throw new Exception($"{nameof(UserIdentity)} не был инициализирован. Пожалуйста, используйте {nameof(UserIdentity)} в методе Main класса Program.cs");
            }
        }

        public IEnumerable<Claim> Claims
        {
            get
            {
                if(User != null)
                {
                    return ContextAccessor.HttpContext!.User.Claims;
                }
                return null;
            }
        }

    }
}
