using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.NHibernate;

namespace Abp.CMS.NHibernate
{
    /// <summary>
    /// Startup class for ABP CMS NHibernate module.
    /// </summary>
    [DependsOn(typeof(AbpCMSCoreModule), typeof(AbpNHibernateModule))]
    public class AbpCMSNHibernateModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpNHibernate().FluentConfiguration
                .Mappings(
                    m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
