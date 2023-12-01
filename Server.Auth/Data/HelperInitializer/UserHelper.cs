using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Server.Auth.Data.DbContext;
using Server.Auth.Data.Entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Auth.Data.HelperInitializer
{
    public static class UserHelper
    {
        public static ApplicationUsers GetUsers(string email)
        {
            if (email == null)
            {
                throw new Exception("Пользователь не обнаружен");
            }
            return new ApplicationUsers
            {
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = email,
                FirstName = "Microservice",
                LastName = "Administrator",
                NormalizedUserName = email.ToUpper(),
                PhoneNumber = "+79113933661",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ApplicationUserProfile = new ApplicationUserProfile
                {
                    CreateAt = DateTime.Now,
                    CreateBy = "SEED",
                    Permissions = new List<MicroservicePermission>
                    {
                        new MicroservicePermission()
                        {
                            CreateAt = DateTime.Now,
                            CreateBy = "SEED",
                            PolicyName = "Logs:UserRoles:View",
                            Description = "Политика доступа для пользовательского представления контроллера журналов"
                        }
                    }
                }
            };
        }
        public static async Task AddUserWithRoles(ApplicationDbContext context, IServiceScope scope, ApplicationUsers user, string[] roles)
        {
            try
            {
                if (!context.Users.Any(x => x.UserName == user.UserName))
                {
                    var password = new PasswordHasher<ApplicationUsers>();
                    var hashed = password.HashPassword(user, "123qwe!@#");
                    user.PasswordHash = hashed;
                    var userStore = scope.ServiceProvider.GetService<ApplicationUserStore>();
                    var result = await userStore!.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Не могу создать учетную запись");
                    }
                    var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUsers>>();
                    foreach (var role in roles)
                    {
                        var roleAddet = await userManager!.AddToRoleAsync(user, role);
                        if (roleAddet.Succeeded)
                        {
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex.Message);
            }
        }
    }
}
