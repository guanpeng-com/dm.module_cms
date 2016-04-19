using Abp.Channels;
using System.Linq;

namespace Abp.CMS.EntityFramework.Migrations.Seed
{
    public class DefaultAppCreator
    {
        private readonly AbpCMSDbContext _context;

        public DefaultAppCreator(AbpCMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateApps();
        }

        private void CreateApps()
        {
            var defaultApp = _context.Apps.FirstOrDefault(e => e.Id > 0);
            if (defaultApp == null)
            {
                defaultApp = new Apps.App { AppName = "cms", AppDir = "cms", AppUrl = "/cms" };
                _context.Apps.Add(defaultApp);
                _context.SaveChanges();

                //TODO: Add desired features to the standard Channel, if wanted!
            }
        }
    }
}