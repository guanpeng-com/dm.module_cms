using Abp.CMS.SampleApp.Authorization.Roles;
using Abp.CMS.SampleApp.Authorization.Users;
using Abp.CMS.SampleApp.DMUsers;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.DMUsers;
using Abp.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Abp.CMS.EntityFramework.Migrations.Seed
{
    public class InitialDbBuilder
    {
        private readonly AbpCMSDbContext<Tenant, Role, User, DMUser> _context;

        public InitialDbBuilder(AbpCMSDbContext<Tenant, Role, User, DMUser> context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultAppChannelCreator(_context).Create();
            new DefaultTemplateCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
