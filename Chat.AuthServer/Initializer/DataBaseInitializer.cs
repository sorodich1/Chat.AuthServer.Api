using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Chat.AuthServer.Initializer
{
    public static class DataBaseInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var roles = AppData.Roles.ToArray();

            foreach(var role in roles)
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                if(!context!.Roles.Any(r => r.Name == role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role, NormalizedName = role.ToUpper() });
                }
            }
            #region developer

            #endregion
        }
    }
}
