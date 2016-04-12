using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Runtime.Caching;
using Abp.CMS.Configuration;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.CMS.SampleApp.Users;

namespace Abp.CMS.SampleApp.Roles
{
    public class RoleManager : AbpRoleManager<Tenant, Role, User>
    {
        public RoleManager(
            RoleStore store, 
            IPermissionManager permissionManager, 
            IRoleManagementConfig roleManagementConfig, 
            ICacheManager cacheManager)
            : base(
            store, 
            permissionManager, 
            roleManagementConfig, 
            cacheManager)
        {
        }
    }
}