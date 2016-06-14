using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using Abp.Zero;

namespace Abp.CMS.EntityFramework
{
    /// <summary>
    /// Entity framework integration module for ASP.NET Boilerplate CMS.
    /// </summary>
    [DependsOn(typeof(AbpCMSCoreModule), typeof(AbpZeroEntityFrameworkModule))]
    public class AbpCMSEntityFrameworkModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
