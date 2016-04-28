using Abp.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Abp.CMS.EntityFramework.Migrations.Seed
{
    public class InitialDbBuilder
    {
        private readonly AbpCMSDbContext _context;

        public InitialDbBuilder(AbpCMSDbContext context)
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
