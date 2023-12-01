using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using IdentityModel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Auth.Data;
using Server.Auth.Data.DbContext;
using Server.Auth.Data.Entityes;
using Server.Auth.Helpers;
using Server.Auth.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Auth.Services.Infrastructure
{
    public class AccountServices : IAccountServices
    {
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly ILogger<AccountServices> _logger;
        private readonly ApplicationClaimsPrincipalFactory _claimsFactory;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<ApplicationRoles> _roleManager;

        public AccountServices(
            IUserStore<ApplicationUsers> userStore, 
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUsers> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUsers>> userValidator,
            IEnumerable<IPasswordValidator<ApplicationUsers>> passwordValidator,
            ILookupNormalizer keyNormolizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<RoleManager<ApplicationRoles>> loggerRole,
            IEnumerable<IRoleValidator<ApplicationRoles>> roleValidator,
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<AccountServices> logger,
            ILogger<UserManager<ApplicationUsers>> loggerUser,
            ApplicationClaimsPrincipalFactory claimsFactory,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _claimsFactory = claimsFactory;
            _httpContext = httpContext;
            _mapper = mapper;

            _userManager = new UserManager<ApplicationUsers>(userStore, optionsAccessor, passwordHasher, userValidator, passwordValidator, keyNormolizer, errors, services, loggerUser);
            var roleStore = new RoleStore<ApplicationRoles, ApplicationDbContext, Guid>(_unitOfWork.DbContext);
            _roleManager = new RoleManager<ApplicationRoles>(roleStore,roleValidator,keyNormolizer, errors, loggerRole);
        }

        /// <summary>
        /// Возвращает пользователя по его идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ApplicationUsers> GetByIdAsync(Guid id)
        {
            var userManager = _userManager;
            Program.Logger.Info($"Получен пользователь с именем {userManager.Users.FirstOrDefault(x => x.Id == id)}");
            return userManager.FindByIdAsync(id.ToString());
        }

        /// <summary>
        /// Возвращает текущую информацию об учетной записи пользователя или ноль, если пользователь не вошел в систему
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationUsers> GetCurrentUserAsync()
        {
            var userManager = _userManager;
            var userId = GetCurrentUserId().ToString();
            var user = await userManager.FindByIdAsync(userId);
            Program.Logger.Info($"Получен пользователь с именем {user.LastName}");
            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Guid GetCurrentUserId()
        {
            var identity = _httpContext.HttpContext?.User.Identity;
            var identitySub = identity.GetSubjectId();
            Program.Logger.Info($"Получен пользователь с именем {identitySub}");
            return new Guid(identitySub);
        }

        /// <summary>
        /// Возвращает профиль пользователя
        /// </summary>
        /// <param name="identifer"></param>
        /// <returns></returns>
        public async Task<OperationResult<UserProfileViewModel>> GetProfileAsync(string identifer)
        {
            var operation = OperationResult.CreateResult<UserProfileViewModel>();
            var claimsPrincipal = await GetUsersClaimsAsync(identifer);
            operation.Result = _mapper.Map<UserProfileViewModel>(claimsPrincipal.Identity);
            return await Task.FromResult(operation);
        }

        /// <summary>
        /// Возвращает ClaimPrincipal по идентификатору пользователя
        /// </summary>
        /// <param name="identifer"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ClaimsPrincipal> GetUsersClaimsAsync(string identifer)
        {
            if(string.IsNullOrEmpty(identifer))
            {
                throw new Exception();
            }
            var userManager = _userManager;
            var user = await userManager.FindByIdAsync(identifer);
            if(user == null)
            {
                throw new Exception();
            }
            var defaultClaims = await _claimsFactory.CreateAsync(user);
            return defaultClaims;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationUsers>> GetUsersEmailAsync(IEnumerable<string> emails)
        {
            var userManager = _userManager;
            var result = new List<ApplicationUsers>();
            foreach (var email in emails)
            {
                var user = await userManager.FindByEmailAsync(email);
                if(user != null && !result.Contains(user))
                {
                    result.Add(user);
                }
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationUsers>> GetUsersInRoleAsync(string roleName)
        {
            var userManager = _userManager;
            return await userManager.GetUsersInRoleAsync(roleName);
        }

        public async Task<OperationResult<UserProfileViewModel>> RegisterAsync(RegisterViewModel model)
        {
            var operation = OperationResult.CreateResult<UserProfileViewModel>();
            try
            {
                var user = _mapper.Map<ApplicationUsers>(model);

                user.UserName = model.FirstName;
                user.Email = model.Email;

                user.UserName = user.UserName.Replace(" ", "");

                await using var transaction = await _unitOfWork.BeginTransactionAsync();
                var result = await _userManager.CreateAsync(user, model.Password);

                const string role = "Manager";

                if(result.Succeeded)
                {
                    //if(await _roleManager.FindByIdAsync(role) == null)
                    //{
                    //    operation.Exception = new Exception();
                    //    operation.AddError("Пользователь не зарегистрирован в системе");
                    //    return await Task.FromResult(operation);
                    //}

                    //await _userManager.AddToRoleAsync(user, role);

                    await AddClaimsToUser(_userManager, user, role);
                    //var profile = _mapper.Map<ApplicationUserProfile>(model);
                    //var profileRepository = _unitOfWork.GetRepository<ApplicationUserProfile>();

                    //await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                    //await profileRepository.InsertAsync(profile);
                    try
                    {
                        await _unitOfWork.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                    
                    if(_unitOfWork.LastSaveChangesResult.IsOk)
                    {
                        var principal = await _claimsFactory.CreateAsync(user);
                        operation.Result = _mapper.Map<UserProfileViewModel>(principal.Identity);
                        operation.AddSuccess("Пользователь успешно зарегистрирован");
                        _logger.LogInformation(operation.GetMetadataMessages());
                        transaction.Commit();
                        _logger.MicroserviceUserRegistration(model.Email, operation.Exception);
                        return await Task.FromResult(operation);
                    }
                }
                var errors = result.Errors.Select(x => $"{x.Code}: {x.Description}");
                operation.AddError(string.Join(", ", errors));
                operation.Exception = _unitOfWork.LastSaveChangesResult.Exception;
                transaction.Rollback();
                _logger.MicroserviceUserRegistration(model.Email, operation.Exception);
            }
            catch(Exception ex) 
            {
                Program.Logger.Error(ex.Message);
            }
            return await Task.FromResult(operation);
        }

        private async Task AddClaimsToUser(UserManager<ApplicationUsers> userManager, ApplicationUsers user, string role)
        {
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, user.UserName));
            await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.GivenName, user.FirstName));
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, user.LastName));
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
        }
    }
}