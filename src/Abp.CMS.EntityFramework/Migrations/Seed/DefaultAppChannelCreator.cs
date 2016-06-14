using Abp.Apps;
using Abp.Channels;
using Abp.CMS.SampleApp.Authorization.Roles;
using Abp.CMS.SampleApp.Authorization.Users;
using Abp.CMS.SampleApp.DMUsers;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.DMUsers;
using System.Linq;

namespace Abp.CMS.EntityFramework.Migrations.Seed
{
    public class DefaultAppChannelCreator
    {
        private readonly AbpCMSDbContext<Tenant, Role, User, DMUser> _context;

        public DefaultAppChannelCreator(AbpCMSDbContext<Tenant, Role, User, DMUser> context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateAppAndChannel();
        }

        private void CreateAppAndChannel()
        {
            var defaultApp = _context.Apps.FirstOrDefault(e => e.Id > 0);
            if (defaultApp == null)
            {
                //var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
                defaultApp = new App
                {
                    AppName = App.DefaultName,
                    AppDir = App.DefaultDir,
                    AppUrl = "/" + App.DefaultDir,
                    TenantId = 0
                };
                _context.Apps.Add(defaultApp);
                _context.SaveChanges();
            }


            var defaultChannel = _context.Channels.FirstOrDefault(e => e.Id > 0);
            if (defaultChannel == null)
            {
                defaultChannel = new Channel
                {
                    ParentId = null,
                    DisplayName = ChannelManager.DefaultChannelName,
                    AppId = defaultApp.Id,
                    Code = Channel.CreateCode(0),
                    Parent = null,
                    IsIndex = true
                };

                _context.Channels.Add(defaultChannel);
                _context.SaveChanges();
            }
        }
    }
}