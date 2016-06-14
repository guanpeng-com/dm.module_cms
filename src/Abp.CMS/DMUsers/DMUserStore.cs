using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNet.Identity;

namespace Abp.DMUsers
{
    /// <summary>
    /// Implements 'User Store' of ASP.NET Identity Framework.
    /// </summary>
    public abstract class DMUserStore<TDMUser> :
        IUserPasswordStore<TDMUser, long>,
        IUserEmailStore<TDMUser, long>,
        IUserLoginStore<TDMUser, long>,
        IQueryableUserStore<TDMUser, long>,

        ITransientDependency

        where TDMUser : DMUser<TDMUser>
    {
        private readonly IRepository<TDMUser, long> _userRepository;
        private readonly IRepository<DMUserLogin, long> _userLoginRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected DMUserStore(
            IRepository<TDMUser, long> userRepository,
            IRepository<DMUserLogin, long> userLoginRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual async Task CreateAsync(TDMUser user)
        {
            await _userRepository.InsertAsync(user);
        }

        public virtual async Task UpdateAsync(TDMUser user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public virtual async Task DeleteAsync(TDMUser user)
        {
            await _userRepository.DeleteAsync(user.Id);
        }

        public virtual async Task<TDMUser> FindByIdAsync(long userId)
        {
            return await _userRepository.FirstOrDefaultAsync(userId);
        }

        public virtual async Task<TDMUser> FindByNameAsync(string userName)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.UserName == userName
                );
        }

        public virtual async Task<TDMUser> FindByEmailAsync(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.EmailAddress == email
                );
        }

        /// <summary>
        /// Tries to find a user with user name or email address in current tenant.
        /// </summary>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>
        public virtual async Task<TDMUser> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => (user.UserName == userNameOrEmailAddress || user.EmailAddress == userNameOrEmailAddress)
                );
        }

        /// <summary>
        /// Tries to find a user with user name or email address in given tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>
        [UnitOfWork]
        public virtual async Task<TDMUser> FindByNameOrEmailAsync(int? tenantId, string userNameOrEmailAddress)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return await FindByNameOrEmailAsync(userNameOrEmailAddress);
            }
        }

        public virtual Task SetPasswordHashAsync(TDMUser user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public virtual Task<string> GetPasswordHashAsync(TDMUser user)
        {
            return Task.FromResult(user.Password);
        }

        public virtual Task<bool> HasPasswordAsync(TDMUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        public virtual Task SetEmailAsync(TDMUser user, string email)
        {
            user.EmailAddress = email;
            return Task.FromResult(0);
        }

        public virtual Task<string> GetEmailAsync(TDMUser user)
        {
            return Task.FromResult(user.EmailAddress);
        }

        public virtual Task<bool> GetEmailConfirmedAsync(TDMUser user)
        {
            return Task.FromResult(user.IsEmailConfirmed);
        }

        public virtual Task SetEmailConfirmedAsync(TDMUser user, bool confirmed)
        {
            user.IsEmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public virtual async Task AddLoginAsync(TDMUser user, UserLoginInfo login)
        {
            await _userLoginRepository.InsertAsync(
                new DMUserLogin
                {
                    TenantId = user.TenantId,
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = user.Id
                });
        }

        public virtual async Task RemoveLoginAsync(TDMUser user, UserLoginInfo login)
        {
            await _userLoginRepository.DeleteAsync(
                ul => ul.UserId == user.Id &&
                      ul.LoginProvider == login.LoginProvider &&
                      ul.ProviderKey == login.ProviderKey
                );
        }

        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(TDMUser user)
        {
            return (await _userLoginRepository.GetAllListAsync(ul => ul.UserId == user.Id))
                .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey))
                .ToList();
        }

        public virtual async Task<TDMUser> FindAsync(UserLoginInfo login)
        {
            var userLogin = await _userLoginRepository.FirstOrDefaultAsync(
                ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey
                );

            if (userLogin == null)
            {
                return null;
            }

            return await _userRepository.FirstOrDefaultAsync(u => u.Id == userLogin.UserId);
        }

        [UnitOfWork]
        public virtual Task<List<TDMUser>> FindAllAsync(UserLoginInfo login)
        {
            var query = from userLogin in _userLoginRepository.GetAll()
                        join user in _userRepository.GetAll() on userLogin.UserId equals user.Id
                        where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                        select user;

            return Task.FromResult(query.ToList());
        }

        public virtual Task<TDMUser> FindAsync(int? tenantId, UserLoginInfo login)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var query = from userLogin in _userLoginRepository.GetAll()
                            join user in _userRepository.GetAll() on userLogin.UserId equals user.Id
                            where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                            select user;

                return Task.FromResult(query.FirstOrDefault());
            }
        }


        public virtual IQueryable<TDMUser> Users
        {
            get { return _userRepository.GetAll(); }
        }

        public virtual void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
