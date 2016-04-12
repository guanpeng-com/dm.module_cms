using Abp.CMS.SampleApp.EntityFramework;
using Abp.CMS.SampleApp.MultiTenancy;

namespace Abp.CMS.SampleApp.Tests.TestDatas
{
    public class InitialTenantsBuilder
    {
        private readonly AppDbContext _context;

        public InitialTenantsBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            CreateTenants();
        }

        private void CreateTenants()
        {
            _context.Tenants.Add(new Tenant(Tenant.DefaultTenantName, Tenant.DefaultTenantName));
            _context.SaveChanges();
        }
    }
}