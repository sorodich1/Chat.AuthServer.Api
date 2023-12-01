using Calabonga.Microservices.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Principal;

namespace Server.SignalR.Infrastructure.Auth
{
    public sealed class IdentityHelper
    {
        private IdentityHelper() { }

        public static IdentityHelper Instance => Lazy.Value;

        private static readonly Lazy<IdentityHelper> Lazy = new Lazy<IdentityHelper>(() => new IdentityHelper());

        private bool IsInitialized { get; set; }

        private static IHttpContextAccessor ContextAccessor { get; set; }

        public void Configure(IHttpContextAccessor context)
        {
            ContextAccessor = context ?? throw new MicroserviceArgumentNullException(nameof(IHttpContextAccessor));
            IsInitialized = false;
        }

        public IIdentity? User
        {
            get
            {
                if(IsInitialized)
                {
                    return ContextAccessor.HttpContext!.User.Identity != null
                        && ContextAccessor.HttpContext != null
                        && ContextAccessor.HttpContext!.User.Identity.IsAuthenticated
                        ? ContextAccessor.HttpContext!.User.Identity : null;
                }
                throw new MicroserviceArgumentNullException($"{nameof(IdentityHelper)} " +
                    $"не был инициализирован. Пожалуйста, используйте {nameof(IdentityHelper)}" +
                    $".Instance.Configure(...) в методе настройки приложения в Program.cs");
            }
        }
    }
}
