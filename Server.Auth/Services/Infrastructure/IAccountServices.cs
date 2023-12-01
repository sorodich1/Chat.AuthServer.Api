using AutoMapper;

using Calabonga.OperationResults;
using Server.Auth.Data.Entityes;
using Server.Auth.ViewModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Auth.Services.Infrastructure
{
    public interface IAccountServices
    {
        /// <summary>
        /// Возвращает коллекцию пользователей по email
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUsers>> GetUsersEmailAsync(IEnumerable<string> emails);

        /// <summary>
        /// Получает идентификатор пользователя из HttpContext
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
        /// <param name="identifer"></param>
        /// <returns></returns>
        Task<OperationResult<UserProfileViewModel>> GetProfileAsync(string identifer);

        /// <summary>
        /// Возвращает пользователя по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationUsers> GetByIdAsync(Guid id);

        /// <summary>
        /// Возвращает ClaimPrincipal по идентификатору пользователя
        /// </summary>
        /// <param name="identifer"></param>
        /// <returns></returns>
        Task<ClaimsPrincipal> GetUsersClaimsAsync(string identifer);

        /// <summary>
        /// Возвращает текущую информацию об учетной записи пользователя или null, если пользователь не вошел в систему
        /// </summary>
        /// <returns></returns>
        Task<ApplicationUsers> GetCurrentUserAsync();

        /// <summary>
        ///  Возвращает всех системных администраторов, зарегистрированных в системе
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUsers>> GetUsersInRoleAsync(string roleName);
    }
}
