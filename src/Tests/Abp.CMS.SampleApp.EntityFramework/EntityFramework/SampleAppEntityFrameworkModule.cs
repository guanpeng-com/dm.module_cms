using System.Reflection;
using Abp.Modules;
using Abp.CMS.EntityFramework;

namespace Abp.CMS.SampleApp.EntityFramework
{
    [DependsOn(typeof(AbpCMSEntityFrameworkModule))]
    public class SampleAppEntityFrameworkModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
