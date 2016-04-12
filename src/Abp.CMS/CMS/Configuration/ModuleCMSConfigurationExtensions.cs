using Abp.Configuration.Startup;

namespace Abp.CMS.Configuration
{
    /// <summary>
    /// Extension methods for module CMS configurations.
    /// </summary>
    public static class ModuleCMSConfigurationExtensions
    {
        /// <summary>
        /// Used to configure module CMS.
        /// </summary>
        /// <param name="moduleConfigurations"></param>
        /// <returns></returns>
        public static IAbpCMSConfig CMS(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration
                .GetOrCreate("AbpCMSConfig",
                    () => moduleConfigurations.AbpConfiguration.IocManager.Resolve<IAbpCMSConfig>()
                );
        }
    }
}