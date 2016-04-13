using System.Linq;
using System.Threading.Tasks;
using Abp.Channels;
using Abp.UI;
using Abp.CMS.SampleApp.NHibernate;
using NHibernate.Linq;
using Shouldly;
using Xunit;

namespace Abp.CMS.SampleApp.Channel
{
    //public class ChannelManager_NHibernate_Tests : NHibernateTestBase
    //{
    //    private readonly ChannelManager _ChannelManager;

    //    public ChannelManager_NHibernate_Tests()
    //    {
    //        AbpSession.TenantId = GetDefaultTenant().Id;

    //        _ChannelManager = Resolve<ChannelManager>();
    //    }

    //    [Fact]
    //    public async Task Should_Create_Root_OU()
    //    {
    //        //Act
    //        await _ChannelManager.CreateAsync(new Channel(AbpSession.TenantId, "Root 1"));

    //        //Assert
    //        var root1 = GetOUOrNull("Root 1");
    //        root1.ShouldNotBeNull();
    //        root1.Code.ShouldBe(Channel.CreateCode(3));
    //    }

    //    [Fact]
    //    public async Task Should_Create_Child_OU()
    //    {
    //        //Arrange
    //        var ou11 = GetOU("OU11");

    //        //Act
    //        await _ChannelManager.CreateAsync(new Channel(AbpSession.TenantId, "OU11 New Child", ou11.Id));

    //        //Assert
    //        var newChild = GetOUOrNull("OU11 New Child");
    //        newChild.ShouldNotBeNull();
    //        newChild.ParentId.ShouldBe(ou11.Id);
    //        newChild.Code.ShouldBe(Channel.CreateCode(1, 1, 3));
    //    }
        
    //    [Fact]
    //    public async Task Should_Not_Create_OU_With_Same_Name_In_Same_Level()
    //    {
    //        //Arrange
    //        var ou11 = GetOU("OU11");

    //        //Act & Assert
    //        await Assert.ThrowsAsync<UserFriendlyException>(
    //            () => _ChannelManager.CreateAsync(
    //                new Channel(AbpSession.TenantId, "OU112", ou11.Id)
    //                )
    //            );
    //    }

    //    [Fact]
    //    public async Task Should_Delete_UO_With_Children()
    //    {
    //        //Arrange
    //        var ou11 = GetOU("OU11");

    //        //Act
    //        await _ChannelManager.DeleteAsync(ou11.Id);

    //        //Assert
    //        GetOU("OU11").IsDeleted.ShouldBeTrue();
    //        GetOU("OU111").IsDeleted.ShouldBeTrue();
    //        GetOU("OU112").IsDeleted.ShouldBeTrue();
    //    }

    //    /* Moves UO1 under OU2 (with all children) */
    //    [Fact]
    //    public async Task Should_Move_OU_Under_Other_OU_With_Children()
    //    {
    //        //Arrange
    //        var ou1 = GetOU("OU1");
    //        var ou2 = GetOU("OU2");

    //        //Act
    //        await _ChannelManager.MoveAsync(ou1.Id, ou2.Id);

    //        //Assert
    //        ou1 = GetOU("OU1");
    //        ou1.ParentId.ShouldBe(ou2.Id);
    //        ou1.Code.ShouldBe(Channel.CreateCode(2, 2));

    //        var ou11 = GetOU("OU11");
    //        ou11.ParentId.ShouldBe(ou1.Id);
    //        ou11.Code.ShouldBe(Channel.CreateCode(2, 2, 1));

    //        var ou111 = GetOU("OU111");
    //        ou111.ParentId.ShouldBe(ou11.Id);
    //        ou111.Code.ShouldBe(Channel.CreateCode(2, 2, 1, 1));

    //        var ou112 = GetOU("OU112");
    //        ou112.ParentId.ShouldBe(ou11.Id);
    //        ou112.Code.ShouldBe(Channel.CreateCode(2, 2, 1, 2));

    //        var ou12 = GetOU("OU12");
    //        ou12.ParentId.ShouldBe(ou1.Id);
    //        ou12.Code.ShouldBe(Channel.CreateCode(2, 2, 2));
    //    }

    //    /* Moves UO11 to ROOT (with all children) */
    //    [Fact]
    //    public async Task Should_Move_OU_To_Root_With_Children()
    //    {
    //        //Arrange
    //        var ou11 = GetOU("OU11");

    //        //Act
    //        await _ChannelManager.MoveAsync(ou11.Id, null);

    //        //Assert
    //        ou11 = GetOU("OU11");
    //        ou11.ParentId.ShouldBe(null);
    //        ou11.Code.ShouldBe(Channel.CreateCode(3));

    //        var ou111 = GetOU("OU111");
    //        ou111.ParentId.ShouldBe(ou11.Id);
    //        ou111.Code.ShouldBe(Channel.CreateCode(3, 1));

    //        var ou112 = GetOU("OU112");
    //        ou112.ParentId.ShouldBe(ou11.Id);
    //        ou112.Code.ShouldBe(Channel.CreateCode(3, 2));
    //    }

    //    private Channel GetOU(string diplayName)
    //    {
    //        var Channel = UsingSession(session => session.Query<Channel>().FirstOrDefault(ou => ou.DisplayName == diplayName));
    //        Channel.ShouldNotBeNull();
    //        return Channel;
    //    }

    //    private Channel GetOUOrNull(string diplayName)
    //    {
    //        return UsingSession(session => session.Query<Channel>().FirstOrDefault(ou => ou.DisplayName == diplayName));
    //    }
    //}
}
