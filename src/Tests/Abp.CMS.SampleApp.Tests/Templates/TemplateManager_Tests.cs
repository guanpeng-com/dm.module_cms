using System.Linq;
using System.Threading.Tasks;
using Abp.Templates;
using Abp.UI;
using Shouldly;
using Xunit;
using Abp.Apps;

namespace Abp.CMS.SampleApp.Tests.Template
{
    public class TemplateManager_Tests : SampleAppTestBase
    {
        private readonly TemplateManager _templateManager;
        private readonly AppManager _appManager;

        public TemplateManager_Tests()
        {
            _templateManager = Resolve<TemplateManager>();
            _appManager = Resolve<AppManager>();
        }

        [Fact]
        public async Task Should_Create()
        {
            //default app
            var deraultApp = await _appManager.FindDefaultAsync();
            //Act
            await _templateManager.CreateAsync(new Templates.Template(deraultApp.Id, "test", "T_test", Templates.Template.Type_File, Templates.Template.DefaultExtension));

            //Assert
            var root1 = GetTemplateOrNull("test");
            root1.ShouldNotBeNull();
        }

        private Templates.Template GetTemplateOrNull(string title)
        {
            return UsingDbContext(context => context.Templates.FirstOrDefault(c => c.Title == title));
        }
    }
}
