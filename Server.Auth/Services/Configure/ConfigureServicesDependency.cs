using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Server.Auth.Data.DbContext;
using Server.Auth.Data.Entityes;
using Server.Auth.Services.Infrastructure;

namespace Server.Auth.Services.Configure
{
    public partial class ConfigureServicesDependency
    {
        public static void Common(IServiceCollection services)
        {
            services.AddTransient<ApplicationUserStore>();
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ApplicationClaimsPrincipalFactory>();
            services.AddTransient<IAccountServices, AccountServices>();
            services.AddTransient<IProfileService, IdentityProfileService>();
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<ICorsPolicyService, IdentityServerCorsPolicy>();
        }
    }
}
