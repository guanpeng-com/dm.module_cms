using Abp.CMS.SampleApp.DMUsers;
using Abp.Configuration.Startup;
using Abp.DMUsers;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Abp.CMS.SampleApp.Tests.DMUsers
{
    public class DMUserManager_Tests : SampleAppTestBase
    {
        private readonly DMUserManager _userManager;

        public DMUserManager_Tests()
        {
            UsingDbContext(DMUserLoginHelper.CreateLoginUsers);
            _userManager = LocalIocManager.Resolve<DMUserManager>();
        }

        [Fact]
        public async Task Should_Login_With_Correct_Values_Without_MultiTenancy()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = false;//单租户
            AbpSession.TenantId = 1;

            var loginResult = await _userManager.LoginAsync("user1", "123qwe");
            loginResult.Result.ShouldBe(DMLoginResultType.Success);
            loginResult.User.Name.ShouldBe("User");
            loginResult.Identity.ShouldNotBe(null);

            UsingDbContext(context =>
            {
                context.DMUserLoginAttempts.Count().ShouldBe(1);
                context.DMUserLoginAttempts.FirstOrDefault(a =>
                    a.TenantId == AbpSession.TenantId &&
                    a.UserId == loginResult.User.Id
                ).ShouldNotBe(null);
            });

        }
    }
}
