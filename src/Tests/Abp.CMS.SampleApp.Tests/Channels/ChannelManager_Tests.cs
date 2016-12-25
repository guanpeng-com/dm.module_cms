using System.Linq;
using System.Threading.Tasks;
using Abp.Channels;
using Abp.UI;
using Shouldly;
using Xunit;
using Abp.Apps;

namespace Abp.CMS.SampleApp.Tests.Channel
{
    public class ChannelManager_Tests : SampleAppTestBase
    {
        private readonly ChannelManager _ChannelManager;
        private readonly AppManager _AppManager;

        public ChannelManager_Tests()
        {
            _ChannelManager = Resolve<ChannelManager>();
            _AppManager = Resolve<AppManager>();
        }

        [Fact]
        public async Task Should_Create_Default_Channel()
        {
            var defaultApp = await _AppManager.FindDefaultAsync();

            await _ChannelManager.CreateDefaultChannel(defaultApp.Id);

            var defaultChannel = _ChannelManager.FindDefaultAsync();

            defaultChannel.Id.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Create_Root_CH()
        {
            //default app
            var deraultApp = await _AppManager.FindDefaultAsync();

            //Act
            await _ChannelManager.CreateAsync(new Channels.Channel(deraultApp.Id, "Root 1"));

            //Assert
            var root1 = GetCHOrNull("Root 1");
            root1.ShouldNotBeNull();
            root1.Code.ShouldBe(Channels.Channel.CreateCode(1));
        }

        [Fact]
        public async Task Should_Create_Child_CH()
        {
            //default app
            var deraultApp = _AppManager.FindDefault();

            //Arrange
            await _ChannelManager.CreateAsync(new Channels.Channel(deraultApp.Id, "CH1"));
            var ou1 = GetCH("CH1");

            //Act
            await _ChannelManager.CreateAsync(new Channels.Channel(deraultApp.Id, "CH1 New Child", ou1.Id));

            //Assert
            var newChild = GetCHOrNull("CH1 New Child");
            newChild.ShouldNotBeNull();
            newChild.ParentId.ShouldBe(ou1.Id);
            newChild.Code.ShouldBe(Channels.Channel.CreateCode(1, 1));
        }

        private Channels.Channel GetCH(string diplayName)
        {
            var Channel = UsingDbContext(context => context.Channels.FirstOrDefault(ou => ou.DisplayName == diplayName));
            Channel.ShouldNotBeNull();
            return Channel;
        }

        private Channels.Channel GetCHOrNull(string diplayName)
        {
            return UsingDbContext(context => context.Channels.FirstOrDefault(ou => ou.DisplayName == diplayName));
        }
    }
}
