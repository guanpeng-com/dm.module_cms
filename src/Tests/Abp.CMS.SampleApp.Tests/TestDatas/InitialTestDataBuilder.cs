using Abp.CMS.SampleApp.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Abp.CMS.SampleApp.Tests.TestDatas
{
    public class InitialTestDataBuilder
    {
        private readonly AppDbContext _context;

        public InitialTestDataBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            _context.DisableAllFilters();

            new InitialTestChannelsBuilder(_context).Build();

            _context.SaveChanges();
        }
    }
}