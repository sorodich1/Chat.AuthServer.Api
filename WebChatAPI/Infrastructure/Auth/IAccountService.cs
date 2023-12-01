using Calabonga.Microservices.Core.Validators;
using Calabonga.OperationResults;
using Chat.AuthServer;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using WebChatAPI.ViewModel;

namespace WebChatAPI.Infrastructure.Auth
{
    public interface IAccountService
    {
        /// <summary>
        /// Возврощает коллекцию <see cref="ApplicationUser"> emails
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUser>> GetUsersByEmailsAsync(IEnumerable<string> emails);

        /// <summary>
        /// Получаем идентификатор пользователя
        /// </summary>
        /// <returns></returns>
        Guid GetCurrentUserId();
        /// <summary>
        /// Возвращает экземпляр <see cref="ApplicationUser"/> после успешной регистрации
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<OperationResult<UserProfileViewModel>> RegisterAsync(RegisterViewModel model);

        /// <summary>
        /// Возвращает профиль пользователя
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        Task<OperationResult<UserProfileViewModel>> GetProfileAsync(string identifier);

        /// <summary>
        /// Возвращает пользователя по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationUser> GetByIdAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        Task<ClaimsPrincipal> GetUserClaimsAsync(string identifier);

        /// <summary>
        /// Возвращает текущую информацию об учетной записи пользователя или null, если пользователь не вошел в систему
        /// </summary>
        /// <returns></returns>
        Task<ApplicationUser> GetCurrentUserAsync();

        /// <summary>
        /// Проверка роли для текущего пользователя
        /// </summary>
        /// <param name="roleNames"></param>
        /// <returns></returns>
        Task<PermissionValidationResult> IsInRolesAsync(string[] roleNames);

        /// <summary>
        /// Возвращает всех системных администраторов, зарегистрированных в системе
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName);
    }
}
