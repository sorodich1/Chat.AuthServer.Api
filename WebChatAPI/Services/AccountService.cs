using AutoMapper;
using Calabonga.Microservices.Core.Extensions;
using Calabonga.Microservices.Core.Validators;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Chat.AuthServer;
using IdentityModel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebChatAPI.Infrastructure.Auth;
using WebChatAPI.ViewModel;

namespace WebChatAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly ILogger<AccountService> _logger;
        private readonly ApplicationClaimsPrincipalFactory _claimsFactory;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountService(IUserStore<ApplicationUser> userStore,
            IOptions<IdentityOptions> options,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer lookupNormalizer,
            IdentityErrorDescriber errorDescriber,
            IServiceProvider serviceProvider,
            ILogger<RoleManager<ApplicationRole>> loggerRole,
            IEnumerable<IRoleValidator<ApplicationRole>> roleValidator,
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<AccountService> logger,
            ILogger<UserManager<ApplicationUser>> loggerUser,
            ApplicationClaimsPrincipalFactory principalFactory,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _claimsFactory = principalFactory;
            _mapper = mapper;

            _userManager = new UserManager<ApplicationUser>(userStore, options, passwordHasher, userValidators, passwordValidators, lookupNormalizer, errorDescriber, serviceProvider, loggerUser);
            var roleStore = new RoleStore<ApplicationRole, ApplicationDbContext, Guid>(_unitOfWork.DbContext);
            _roleManager = new RoleManager<ApplicationRole>(roleStore, roleValidator, lookupNormalizer, errorDescriber, loggerRole);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            var userManager = _userManager;
            return userManager.FindByIdAsync(id.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userManager = _userManager;
            var userId = GetCurrentUserId().ToString();
            var user = await userManager.FindByIdAsync(userId);
            return user;
        }

        /// <summary>
        /// Guid Id
        /// </summary>
        /// <returns></returns>
        public Guid GetCurrentUserId()
        {
            var identity = _httpContext.HttpContext?.User.Identity;
            var identitySub = identity.GetSubjectId();
            return identitySub.ToGuid();
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public async Task<OperationResult<UserProfileViewModel>> GetProfileAsync(string identifier)
        {
            var operation = OperationResult.CreateResult<UserProfileViewModel>();
            var claimsPrincipal = await GetUserClaimsAsync(identifier);
            operation.Result = _mapper.Map<UserProfileViewModel>(claimsPrincipal.Identity);
            return await Task.FromResult(operation);
        }

        /// <summary>
        /// создание нового пользователя
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ClaimsPrincipal> GetUserClaimsAsync(string identifier)
        {
            Program.Logger.Info($"Идентификатор пользователя входящего в систему  {identifier}");
            if (string.IsNullOrEmpty(identifier))
            {
                throw new Exception();
            }
            var userManager = _userManager;
            var user = await userManager.FindByIdAsync(identifier);
            if (user == null)
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
        public async Task<IEnumerable<ApplicationUser>> GetUsersByEmailsAsync(IEnumerable<string> emails)
        {
            var userManager = _userManager;
            var result = new List<ApplicationUser>();
            foreach (var email in emails)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user != null && !result.Contains(user))
                {
                    result.Add(user);
                }
            }
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            var userManager = _userManager;
            return await userManager.GetUsersInRoleAsync(roleName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleNames"></param>
        /// <returns></returns>
        public async Task<PermissionValidationResult> IsInRolesAsync(string[] roleNames)
        {
            var userManager = _userManager;
            var userId = GetCurrentUserId().ToString();
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                var resultUserNotFound = new PermissionValidationResult();
            //    resultUserNotFound.AddError(AppData.Exception.UnauthorizedException);
                return await Task.FromResult(resultUserNotFound);
            }
            foreach (var role in roleNames)
            {
                var ok = await userManager.IsInRoleAsync(user, role);
                if (ok)
                {
                    return new PermissionValidationResult();
                }
            }
            var result = new PermissionValidationResult();
           // result.AddError(AppData.Exception.UnauthorizedException);
            return result;
        }

        /// <summary>
        /// Регистрация пользователей
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<OperationResult<UserProfileViewModel>> RegisterAsync(RegisterViewModel model)
        {
            var operation = OperationResult.CreateResult<UserProfileViewModel>();
            var user = _mapper.Map<ApplicationUser>(model);
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            var result = await _userManager.CreateAsync(user, model.Password);
            const string role = AppData.ManagerRoleName;

            if (result.Succeeded)
            {
                if (await _roleManager.FindByNameAsync(role) == null)
                {
                    operation.Exception = new Exception();
                    operation.AddError("Пользователь не зарегистрирован в системе");
                    return await Task.FromResult(operation);
                }
                await _userManager.AddToRoleAsync(user, role);
                await AddClaimsToUser(_userManager, user, role);
                var profile = _mapper.Map<ApplicationUserProfile>(model);
                var profileRepository = _unitOfWork.GetRepository<ApplicationUserProfile>();

                await profileRepository.InsertAsync(profile);
                await _unitOfWork.SaveChangesAsync();
                if (_unitOfWork.LastSaveChangesResult.IsOk)
                {
                    var principal = await _claimsFactory.CreateAsync(user);
                    operation.Result = _mapper.Map<UserProfileViewModel>(principal.Identity);
                   operation.AddSuccess("Пользователь успешно зарегистрирован в системе");
                    _logger.LogInformation(operation.GetMetadataMessages());
                    transaction.Commit();
                    return await Task.FromResult(operation);
                }
            }
            var errors = result.Errors.Select(x => $"{x.Code}: {x.Description}");
            operation.AddError(string.Join(", ", errors));
            transaction.Rollback();
            return await Task.FromResult(operation);
        }

        private async Task AddClaimsToUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string role)
        {
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, user.UserName));
            await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.GivenName, user.FirstName));
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, user.LastName));
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
        }
    }
}
