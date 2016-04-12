using Abp.Authorization;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.CMS.SampleApp.Roles;
using Abp.CMS.SampleApp.Users;

namespace Abp.CMS.SampleApp.Authorization
{
    public class AppPermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public AppPermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
