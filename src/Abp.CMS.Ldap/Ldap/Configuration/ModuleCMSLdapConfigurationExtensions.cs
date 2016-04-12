using Abp.Configuration.Startup;

namespace Abp.CMS.Ldap.Configuration
{
    /// <summary>
    /// Extension methods for module CMS configurations.
    /// </summary>
    public static class ModuleCMSLdapConfigurationExtensions
    {
        /// <summary>
        /// Configures ABP CMS LDAP module.
        /// </summary>
        /// <returns></returns>
        public static IAbpCMSLdapModuleConfig CMSLdap(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration
                .GetOrCreate("AbpCMSLdapConfig",
                    () => moduleConfigurations.AbpConfiguration.IocManager.Resolve<IAbpCMSLdapModuleConfig>()
                );
        }
    }
}
