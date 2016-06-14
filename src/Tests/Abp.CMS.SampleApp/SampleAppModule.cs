using System.Reflection;
using Abp.Modules;
using Abp.CMS.Configuration;
using Abp.Zero;
using Abp.Zero.Configuration;
using Abp.CMS.SampleApp.Authorization.Roles;

namespace Abp.CMS.SampleApp
{
    [DependsOn(typeof(AbpCMSCoreModule))]
    public class SampleAppModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.MultiTenancy.IsEnabled = true;

            //Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
