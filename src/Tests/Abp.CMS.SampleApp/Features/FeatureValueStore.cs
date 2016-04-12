using Abp.Application.Features;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.CMS.SampleApp.Roles;
using Abp.CMS.SampleApp.Users;

namespace Abp.CMS.SampleApp.Features
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(TenantManager tenantManager)
            : base(tenantManager)
        {

        }
    }
}