using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Auth.Data.DbContext;
using Server.Auth.Data.Entityes;
using Server.Auth.Helpers.AppData;
using System;
using System.Linq;

namespace Server.Auth.Data.HelperInitializer
{
    public static class DaBaseInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            var roles = AppData.Roles.ToArray();


            foreach (var role in roles)
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRoles>>();

                if (!context!.Roles.Any(x => x.Name == role))
                {
                    await roleManager.CreateAsync(new ApplicationRoles { Name = role, NormalizedName = role.ToUpper() });
                }
            }

            #region development
            var user = UserHelper.GetUsers("sorodich@gmail.com");
            await UserHelper.AddUserWithRoles(context!, scope, user, roles);

            var user1 = UserHelper.GetUsers("sorodich1@gmail.com");
            await UserHelper.AddUserWithRoles(context!, scope, user, roles);
            #endregion

            await context!.SaveChangesAsync();
        }
    }
}
