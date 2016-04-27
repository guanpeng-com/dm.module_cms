using Abp.Apps;
using Abp.Channels;
using Abp.CMS.EntityFramework;
using Abp.CMS.SampleApp.EntityFramework;
using System.Linq;

namespace Abp.CMS.SampleApp.Tests.TestDatas
{
    /* Creates CH tree for default tenant as shown below:
     * 
     * - CH1
     *   - CH11
     *     - CH111
     *     - CH112
     *   - CH12
     * - CH2
     *   - CH21
     */
    public class InitialTestChannelsBuilder
    {
        private readonly AppDbContext _context;

        public InitialTestChannelsBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            CreateCHs();
        }

        private void CreateCHs()
        {
            var defaultApp = _context.Apps.FirstOrDefault(e => e.Id > 0);
            if (defaultApp == null)
            {

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
                defaultChannel = new Channels.Channel
                {
                    ParentId = null,
                    DisplayName = ChannelManager.DefaultChannelName,
                    AppId = defaultApp.Id,
                    Code = Channels.Channel.CreateCode(0),
                    Parent = null,
                };

                _context.Channels.Add(defaultChannel);
                _context.SaveChanges();
            }
        }


    }
}
