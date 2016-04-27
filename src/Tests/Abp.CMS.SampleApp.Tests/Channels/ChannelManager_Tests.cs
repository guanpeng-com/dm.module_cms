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
        public async Task Should_Create_Root_CH()
        {
            //default app
            var deraultApp =await _AppManager.FindDefaultAsync();

            //Act
            await _ChannelManager.CreateAsync(new Channels.Channel(deraultApp.Id, "Root 1"));

            //Assert
            var root1 = GetCHOrNull("Root 1");
            root1.ShouldNotBeNull();
            root1.Code.ShouldBe(Channels.Channel.CreateCode(3));
        }

        [Fact]
        public async Task Should_Create_Child_CH()
        {
            //default app
            var deraultApp = _AppManager.FindDefault();

            //Arrange
            var ou11 = GetCH("CH11");

            //Act
            await _ChannelManager.CreateAsync(new Channels.Channel(deraultApp.Id, "CH11 New Child", ou11.Id));

            //Assert
            var newChild = GetCHOrNull("CH11 New Child");
            newChild.ShouldNotBeNull();
            newChild.ParentId.ShouldBe(ou11.Id);
            newChild.Code.ShouldBe(Channels.Channel.CreateCode(1, 1, 3));
        }
        
        [Fact]
        public async Task Should_Not_Create_CH_With_Same_Name_In_Same_Level()
        {
            //default app
            var deraultApp = _AppManager.FindDefault();

            //Arrange
            var ou11 = GetCH("CH11");

            //Act & Assert
            await Assert.ThrowsAsync<UserFriendlyException>(
                () => _ChannelManager.CreateAsync(
                    new Channels.Channel(deraultApp.Id, "CH112", ou11.Id)
                    )
                );
        }

        [Fact]
        public async Task Should_Delete_UO_With_Children()
        {
            //Arrange
            var ou11 = GetCH("CH11");

            //Act
            await _ChannelManager.DeleteAsync(ou11.Id);

            //Assert
            GetCH("CH11").IsDeleted.ShouldBeTrue();
            GetCH("CH111").IsDeleted.ShouldBeTrue();
            GetCH("CH112").IsDeleted.ShouldBeTrue();
        }

        /* Moves UO1 under CH2 (with all children) */
        [Fact]
        public async Task Should_Move_CH_Under_Other_CH_With_Children()
        {
            //Arrange
            var ou1 = GetCH("CH1");
            var ou2 = GetCH("CH2");

            //Act
            await _ChannelManager.MoveAsync(ou1.Id, ou2.Id);

            //Assert
            ou1 = GetCH("CH1");
            ou1.ParentId.ShouldBe(ou2.Id);
            ou1.Code.ShouldBe(Channels.Channel.CreateCode(2, 2));

            var ou11 = GetCH("CH11");
            ou11.ParentId.ShouldBe(ou1.Id);
            ou11.Code.ShouldBe(Channels.Channel.CreateCode(2, 2, 1));

            var ou111 = GetCH("CH111");
            ou111.ParentId.ShouldBe(ou11.Id);
            ou111.Code.ShouldBe(Channels.Channel.CreateCode(2, 2, 1, 1));

            var ou112 = GetCH("CH112");
            ou112.ParentId.ShouldBe(ou11.Id);
            ou112.Code.ShouldBe(Channels.Channel.CreateCode(2, 2, 1, 2));

            var ou12 = GetCH("CH12");
            ou12.ParentId.ShouldBe(ou1.Id);
            ou12.Code.ShouldBe(Channels.Channel.CreateCode(2, 2, 2));
        }

        /* Moves UO11 to ROOT (with all children) */
        [Fact]
        public async Task Should_Move_CH_To_Root_With_Children()
        {
            //Arrange
            var ou11 = GetCH("CH11");

            //Act
            await _ChannelManager.MoveAsync(ou11.Id, null);

            //Assert
            ou11 = GetCH("CH11");
            ou11.ParentId.ShouldBe(null);
            ou11.Code.ShouldBe(Channels.Channel.CreateCode(3));

            var ou111 = GetCH("CH111");
            ou111.ParentId.ShouldBe(ou11.Id);
            ou111.Code.ShouldBe(Channels.Channel.CreateCode(3, 1));

            var ou112 = GetCH("CH112");
            ou112.ParentId.ShouldBe(ou11.Id);
            ou112.Code.ShouldBe(Channels.Channel.CreateCode(3, 2));
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
