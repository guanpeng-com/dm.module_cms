using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization.Roles;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Timing;
using Microsoft.AspNet.Identity;
using Abp.CMS;
using Abp.CMS.Configuration;
using Abp.Authorization.Users;

namespace Abp.DMUsers
{
    //TODO: Extract Login operations to AbpLoginManager and remove TTenant generic parameter.

    /// <summary>
    /// Extends <see cref="UserManager{TDMUser,TKey}"/> of ASP.NET Identity Framework.
    /// </summary>
    public abstract class DMUserManager<TTenant, TUser, TDMUser>
        : UserManager<TDMUser, long>,
        IDomainService
        where TTenant : AbpTenant<TUser>
        where TUser : AbpUser<TUser>
        where TDMUser : DMUser<TDMUser>
    {

        public ILocalizationManager LocalizationManager { get; set; }

        public IAbpSession AbpSession { get; set; }

        public IAuditInfoProvider AuditInfoProvider { get; set; }

        public FeatureDependencyContext FeatureDependencyContext { get; set; }

        protected ISettingManager SettingManager { get; private set; }

        protected DMUserStore<TDMUser> DMUserStore { get; private set; }

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IIocResolver _iocResolver;
        private readonly IRepository<TTenant> _tenantRepository;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<DMUserLoginAttempt, long> _userLoginAttemptRepository;

        protected DMUserManager(
            DMUserStore<TDMUser> userStore,
            IRepository<TTenant> tenantRepository,
            IMultiTenancyConfig multiTenancyConfig,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IIocResolver iocResolver,
            ICacheManager cacheManager,
            IRepository<DMUserLoginAttempt, long> userLoginAttemptRepository)
            : base(userStore)
        {
            DMUserStore = userStore;
            SettingManager = settingManager;
            _tenantRepository = tenantRepository;
            _multiTenancyConfig = multiTenancyConfig;
            _unitOfWorkManager = unitOfWorkManager;
            _iocResolver = iocResolver;
            _cacheManager = cacheManager;
            _userLoginAttemptRepository = userLoginAttemptRepository;

            LocalizationManager = NullLocalizationManager.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public override async Task<IdentityResult> CreateAsync(TDMUser user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            var tenantId = GetCurrentTenantId();
            if (tenantId.HasValue && !user.TenantId.HasValue)
            {
                user.TenantId = tenantId.Value;
            }

            return await base.CreateAsync(user);
        }

        public virtual async Task<TDMUser> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await DMUserStore.FindByNameOrEmailAsync(userNameOrEmailAddress);
        }

        public virtual Task<List<TDMUser>> FindAllAsync(UserLoginInfo login)
        {
            return DMUserStore.FindAllAsync(login);
        }

        [UnitOfWork]
        public virtual async Task<DMLoginResult> LoginAsync(UserLoginInfo login, string tenancyName = null)
        {
            var result = await LoginAsyncInternal(login, tenancyName);
            await SaveLoginAttempt(result, tenancyName, login.ProviderKey + "@" + login.LoginProvider);
            return result;
        }

        private async Task<DMLoginResult> LoginAsyncInternal(UserLoginInfo login, string tenancyName)
        {
            if (login == null || login.LoginProvider.IsNullOrEmpty() || login.ProviderKey.IsNullOrEmpty())
            {
                throw new ArgumentException("login");
            }

            //Get and check tenant
            TTenant tenant = null;
            if (!_multiTenancyConfig.IsEnabled)
            {
                tenant = await GetDefaultTenantAsync();
            }
            else if (!string.IsNullOrWhiteSpace(tenancyName))
            {
                tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                if (tenant == null)
                {
                    return new DMLoginResult(DMLoginResultType.InvalidTenancyName);
                }
            }

            int? tenantId = tenant == null ? (int?)null : tenant.Id;
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var user = await DMUserStore.FindAsync(tenantId, login);
                if (user == null)
                {
                    return new DMLoginResult(DMLoginResultType.UnknownExternalLogin, tenant);
                }

                return await CreateLoginResultAsync(user, tenant);
            }
        }

        [UnitOfWork]
        public virtual async Task<DMLoginResult> LoginAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null)
        {
            var result = await LoginAsyncInternal(userNameOrEmailAddress, plainPassword, tenancyName);
            await SaveLoginAttempt(result, tenancyName, userNameOrEmailAddress);
            return result;
        }

        private async Task<DMLoginResult> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, string tenancyName = null)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException("userNameOrEmailAddress");
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException("plainPassword");
            }

            //Get and check tenant
            TTenant tenant = null;
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                if (!_multiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new DMLoginResult(DMLoginResultType.InvalidTenancyName);
                    }
                }
            }

            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {

                var user = await DMUserStore.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                if (user == null)
                {
                    return new DMLoginResult(DMLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                }
                var verificationResult = new PasswordHasher().VerifyHashedPassword(user.Password, plainPassword);
                if (verificationResult != PasswordVerificationResult.Success)
                {
                    return new DMLoginResult(DMLoginResultType.InvalidPassword, tenant, user);
                }

                return await CreateLoginResultAsync(user, tenant);
            }
        }

        private async Task<DMLoginResult> CreateLoginResultAsync(TDMUser user, TTenant tenant = null)
        {
            if (!user.IsActive)
            {
                return new DMLoginResult(DMLoginResultType.UserIsNotActive);
            }

            if (await IsEmailConfirmationRequiredForLoginAsync(user.TenantId) && !user.IsEmailConfirmed)
            {
                return new DMLoginResult(DMLoginResultType.UserEmailIsNotConfirmed);
            }

            user.LastLoginTime = Clock.Now;

            await Store.UpdateAsync(user);

            await _unitOfWorkManager.Current.SaveChangesAsync();

            return new DMLoginResult(tenant, user, await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie));
        }

        private async Task SaveLoginAttempt(DMLoginResult loginResult, string tenancyName, string userNameOrEmailAddress)
        {
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                var tenantId = loginResult.Tenant != null ? loginResult.Tenant.Id : (int?)null;
                using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var loginAttempt = new DMUserLoginAttempt
                    {
                        TenantId = tenantId,
                        TenancyName = tenancyName,

                        UserId = loginResult.User != null ? loginResult.User.Id : (long?)null,
                        UserNameOrEmailAddress = userNameOrEmailAddress,

                        Result = loginResult.Result,
                    };

                    //TODO: We should replace this workaround with IClientInfoProvider when it's implemented in ABP (https://github.com/aspnetboilerplate/aspnetboilerplate/issues/926)
                    if (AuditInfoProvider != null)
                    {
                        var auditInfo = new AuditInfo();
                        AuditInfoProvider.Fill(auditInfo);
                        loginAttempt.BrowserInfo = auditInfo.BrowserInfo;
                        loginAttempt.ClientIpAddress = auditInfo.ClientIpAddress;
                        loginAttempt.ClientName = auditInfo.ClientName;
                    }

                    await _userLoginAttemptRepository.InsertAsync(loginAttempt);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }

                await uow.CompleteAsync();
            }
        }

        /// <summary>
        /// Gets a user by given id.
        /// Throws exception if no user found with given id.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        /// <exception cref="AbpException">Throws exception if no user found with given id</exception>
        public virtual async Task<TDMUser> GetDMUserByIdAsync(long userId)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
            {
                throw new AbpException("There is no user with id: " + userId);
            }

            return user;
        }

        public async override Task<ClaimsIdentity> CreateIdentityAsync(TDMUser user, string authenticationType)
        {
            var identity = await base.CreateIdentityAsync(user, authenticationType);
            if (user.TenantId.HasValue)
            {
                identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString(CultureInfo.InvariantCulture)));
            }

            return identity;
        }

        public async override Task<IdentityResult> UpdateAsync(TDMUser user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            var oldUserName = (await GetDMUserByIdAsync(user.Id)).UserName;
            if (oldUserName == DMUser<TDMUser>.AdminUserName && user.UserName != DMUser<TDMUser>.AdminUserName)
            {
                return AbpIdentityResult.Failed(string.Format(L("CanNotRenameAdminUser"), DMUser<TDMUser>.AdminUserName));
            }

            return await base.UpdateAsync(user);
        }

        public async override Task<IdentityResult> DeleteAsync(TDMUser user)
        {
            if (user.UserName == DMUser<TDMUser>.AdminUserName)
            {
                return AbpIdentityResult.Failed(string.Format(L("CanNotDeleteAdminUser"), DMUser<TDMUser>.AdminUserName));
            }

            return await base.DeleteAsync(user);
        }

        public virtual async Task<IdentityResult> ChangePasswordAsync(TDMUser user, string newPassword)
        {
            var result = await PasswordValidator.ValidateAsync(newPassword);
            if (!result.Succeeded)
            {
                return result;
            }

            await DMUserStore.SetPasswordHashAsync(user, PasswordHasher.HashPassword(newPassword));
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(long? expectedUserId, string userName, string emailAddress)
        {
            var user = (await FindByNameAsync(userName));
            if (user != null && user.Id != expectedUserId)
            {
                return AbpIdentityResult.Failed(string.Format(L("Identity.DuplicateName"), userName));
            }

            user = (await FindByEmailAsync(emailAddress));
            if (user != null && user.Id != expectedUserId)
            {
                return AbpIdentityResult.Failed(string.Format(L("Identity.DuplicateEmail"), emailAddress));
            }

            return IdentityResult.Success;
        }


        private async Task<bool> IsEmailConfirmationRequiredForLoginAsync(int? tenantId)
        {
            if (tenantId.HasValue)
            {
                return await SettingManager.GetSettingValueForTenantAsync<bool>(AbpCMSSettingNames.DMUserManagement.IsEmailConfirmationRequiredForLogin, tenantId.Value);
            }

            return await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpCMSSettingNames.DMUserManagement.IsEmailConfirmationRequiredForLogin);
        }

        private async Task<TTenant> GetDefaultTenantAsync()
        {
            var tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == "Default");
            if (tenant == null)
            {
                throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
            }

            return tenant;
        }

        private string L(string name)
        {
            return LocalizationManager.GetString(AbpCMSConsts.LocalizationSourceName, name);
        }

        public class DMLoginResult
        {
            public DMLoginResultType Result { get; private set; }

            public TTenant Tenant { get; private set; }

            public TDMUser User { get; private set; }

            public ClaimsIdentity Identity { get; private set; }

            public DMLoginResult(DMLoginResultType result, TTenant tenant = null, TDMUser user = null)
            {
                Result = result;
                Tenant = tenant;
                User = user;
            }

            public DMLoginResult(TTenant tenant, TDMUser user, ClaimsIdentity identity)
                : this(DMLoginResultType.Success, tenant)
            {
                User = user;
                Identity = identity;
            }
        }

        private int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }
    }
}