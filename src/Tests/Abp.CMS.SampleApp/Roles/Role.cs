using Abp.Authorization.Roles;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.CMS.SampleApp.Users;

namespace Abp.CMS.SampleApp.Roles
{
    public class Role : AbpRole<Tenant, User>
    {
        public Role()
        {

        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {

        }
    }
}