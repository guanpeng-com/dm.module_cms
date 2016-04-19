using Abp.Channels;
using System.Linq;

namespace Abp.CMS.EntityFramework.Migrations.Seed
{
    public class DefaultChannelCreator
    {
        private readonly AbpCMSDbContext _context;

        public DefaultChannelCreator(AbpCMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateChannels();
        }

        private void CreateChannels()
        {
            var defaultChannel = _context.Channels.FirstOrDefault(e => e.Id > 0);
            if (defaultChannel == null)
            {
                var defaultApp = _context.Apps.FirstOrDefault(a => a.Id > 0);
                defaultChannel = new Channel { ParentId = ChannelManager.DefaultParentId, DisplayName = ChannelManager.DefaultChannelName, AppId = defaultApp.Id };
                _context.Channels.Add(defaultChannel);
                _context.SaveChanges();

                //TODO: Add desired features to the standard Channel, if wanted!
            }
        }
    }
}