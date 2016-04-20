using Abp.CMS.SampleApp.EntityFramework;

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
            var CH1 = CreateCH("CH1", Channels.Channel.CreateCode(1));
            var CH11 = CreateCH("CH11", Channels.Channel.CreateCode(1, 1), CH1.Id);
            var CH111 = CreateCH("CH111", Channels.Channel.CreateCode(1, 1, 1), CH11.Id);
            var CH112 = CreateCH("CH112", Channels.Channel.CreateCode(1, 1, 2), CH11.Id);
            var CH12 = CreateCH("CH12", Channels.Channel.CreateCode(1, 2), CH1.Id);
            var CH2 = CreateCH("CH2", Channels.Channel.CreateCode(2));
            var CH21 = CreateCH("CH21", Channels.Channel.CreateCode(2, 1), CH2.Id);
        }

        private Channels.Channel CreateCH(string displayName, string code, long? parentId = null)
        {
            var CH = _context.Channels.Add(new Channels.Channel(0, displayName, parentId) { Code = code });
            _context.SaveChanges();
            return CH;
        }
    }
}
