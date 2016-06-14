using Abp.CMS.SampleApp.Authorization.Users;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.DMUsers;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.CMS.SampleApp.DMUsers
{
    public class DMUserManager : DMUserManager<Tenant, User, DMUser>
    {
        public DMUserManager(
            DMUserStore userStore,
            IRepository<Tenant> tenantRepository,
            IMultiTenancyConfig multiTenancyConfig,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IIocResolver iocResolver,
            ICacheManager cacheManager,
            IRepository<DMUserLoginAttempt, long> userLoginAttemptRepository
            )
            : base(
                  userStore,
                 tenantRepository,
                 multiTenancyConfig,
                 unitOfWorkManager,
                 settingManager,
                 iocResolver,
                 cacheManager,
                 userLoginAttemptRepository)
        {

        }
    }
}
