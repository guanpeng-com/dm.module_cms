using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;

namespace Abp.CMS.EntityFramework
{
    /// <summary>
    /// Entity framework integration module for ASP.NET Boilerplate CMS.
    /// </summary>
    [DependsOn(typeof(AbpCMSCoreModule), typeof(AbpEntityFrameworkModule))]
    public class AbpCMSEntityFrameworkModule : AbpModule
    {
        public override void PreInitialize()
        {
            //web.config (or app.config for non-web projects) file should containt a connection string named "Default".
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
