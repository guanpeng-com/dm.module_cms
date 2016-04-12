using System.Reflection;
using Abp.Localization.Dictionaries.Xml;
using Abp.Localization.Sources;
using Abp.Modules;
using Abp.CMS.Ldap.Configuration;

namespace Abp.CMS.Ldap
{
    /// <summary>
    /// This module extends module CMS to add LDAP authentication.
    /// </summary>
    [DependsOn(typeof (AbpCMSCoreModule))]
    public class AbpCMSLdapModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IAbpCMSLdapModuleConfig, AbpCMSLdapModuleConfig>();

            Configuration.Localization.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(
                    AbpCMSConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Abp.CMS.Ldap.Localization.Source")
                    )
                );

            Configuration.Settings.Providers.Add<LdapSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
