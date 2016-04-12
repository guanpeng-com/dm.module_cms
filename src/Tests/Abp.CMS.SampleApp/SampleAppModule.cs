using System.Reflection;
using Abp.Modules;
using Abp.CMS.SampleApp.Authorization;
using Abp.CMS.SampleApp.Configuration;
using Abp.CMS.SampleApp.Features;
using Abp.CMS.Configuration;

namespace Abp.CMS.SampleApp
{
    [DependsOn(typeof(AbpCMSCoreModule))]
    public class SampleAppModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.CMS().LanguageManagement.EnableDbLocalization();

            Configuration.Features.Providers.Add<AppFeatureProvider>();

            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
            Configuration.Settings.Providers.Add<AppSettingProvider>();
            Configuration.MultiTenancy.IsEnabled = true;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
