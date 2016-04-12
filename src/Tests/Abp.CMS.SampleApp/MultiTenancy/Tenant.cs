using Abp.MultiTenancy;
using Abp.CMS.SampleApp.Users;

namespace Abp.CMS.SampleApp.MultiTenancy
{
    public class Tenant : AbpTenant<Tenant, User>
    {
        protected Tenant()
        {

        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}