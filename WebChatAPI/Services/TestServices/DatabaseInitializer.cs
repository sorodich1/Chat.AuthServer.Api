using Chat.AuthServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace WebChatAPI.Services.TestServices
{
    public static class DatabaseInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var roles = AppData.Roles.ToArray();

            foreach(var role in roles)
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                foreach (var rolett in roleManager.Roles)
                {
                    var tt = rolett;
                }

                if (!context!.Roles.Any(r => r.Name == role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role, NormalizedName = role.ToUpper() });
                }
            }

            #region developer
            var user1 = UserHelpers.GetUser("user1@sorodich.com");
            await UserHelpers.AddUserWithRoles(context, scope, user1, roles);

            var user2 = UserHelpers.GetUser("user2@sorodich.com");
            await UserHelpers.AddUserWithRoles(context, scope, user2, roles);

            //var user3 = UserHelpers.GetUser("user3@sorodich.com");
            //await UserHelpers.AddUserWithRoles(context, scope, user3, roles);

            //var user4 = UserHelpers.GetUser("user4@sorodich.com");
            //await UserHelpers.AddUserWithRoles(context, scope, user4, roles);
            #endregion

            await context!.SaveChangesAsync();
        }
    }
}
