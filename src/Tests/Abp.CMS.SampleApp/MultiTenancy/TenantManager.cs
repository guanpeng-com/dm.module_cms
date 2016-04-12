using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.CMS.SampleApp.Editions;
using Abp.CMS.SampleApp.Roles;
using Abp.CMS.SampleApp.Users;

namespace Abp.CMS.SampleApp.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, Role, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            EditionManager editionManager) :
            base(tenantRepository, tenantFeatureRepository, editionManager)
        {
        }
    }
}
