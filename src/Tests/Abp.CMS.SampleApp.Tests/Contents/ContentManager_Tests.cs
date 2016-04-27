using System.Linq;
using System.Threading.Tasks;
using Abp.Contents;
using Abp.UI;
using Shouldly;
using Xunit;
using Abp.Apps;
using Abp.Channels;

namespace Abp.CMS.SampleApp.Tests.Content
{
    public class ContentManager_Tests : SampleAppTestBase
    {
        private readonly ContentManager _contentManager;
        private readonly AppManager _appManager;
        private readonly ChannelManager _channelManager;

        public ContentManager_Tests()
        {
            _contentManager = Resolve<ContentManager>();
            _appManager = Resolve<AppManager>();
            _channelManager = Resolve<ChannelManager>();
        }

        [Fact]
        public async Task Should_Create()
        {
            //default app
            var deraultApp = await _appManager.FindDefaultAsync();
            var defaultChannel = await _channelManager.FindDefaultAsync();
            //Act
            await _contentManager.CreateAsync(new Contents.Content(deraultApp.Id, defaultChannel.Id, "test", "test"));

            //Assert
            var root1 = GetContentOrNull("test");
            root1.ShouldNotBeNull();
        }

        private Contents.Content GetContentOrNull(string title)
        {
            return UsingDbContext(context => context.Contents.FirstOrDefault(c => c.Title == title));
        }
    }
}
