using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.AuthServer
{
    public static class UserHelpers
    {
        public static ApplicationUser GetUser(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            return new ApplicationUser
            {
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = email,
                FirstName = "Microservice",
                LastName = "Administrator",
                NormalizedUserName = email.ToUpper(),
                PhoneNumber = "89113933661",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Enabled = false,
                ApplicationUserProfile = new ApplicationUserProfile
                {
                    CreatedAt = DateTime.Now,
                    
                    CreatedBy = "SEED",
                    Permission = new List<MicroservicePermission>
                    {
                        new MicroservicePermission()
                        {
                            CreatedAt = DateTime.Now,
                            
                            CreatedBy = "SEED",
                            PolicyName = "Журналы: Роли пользователей: Просмотр",
                            Description = "Политика доступа для представления пользователя контроллера журналов"
                        }
                    }
                }
            };
        }

        public static async Task AddUserWithRoles(ApplicationDbContext context, IServiceScope scope, ApplicationUser user, string[] roles)
        {
            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "123qwe!@#");
                user.PasswordHash = hashed;
                var userStore = scope.ServiceProvider.GetService<ApplicationUserStore>();
                var result = await userStore!.CreateAsync(user);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Ошибка создания учётной записи");
                }
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
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
    }
}
