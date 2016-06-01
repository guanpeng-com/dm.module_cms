using System.Reflection;
using Abp.Dependency;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.CMS.Configuration;

namespace Abp.CMS
{
    /// <summary>
    /// ABP CMS core module.
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpCMSCoreModule : AbpModule
    {
        /// <summary>
        /// Current version of the CMS module.
        /// </summary>
        public const string CurrentVersion = "1.0.0.6";

        /// <summary>
        /// 预加载
        /// </summary>
        public override void PreInitialize()
        {
            IocManager.Register<IAbpCMSConfig, AbpCMSConfig>();

            Configuration.Settings.Providers.Add<AbpCMSSettingProvider>();

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    AbpCMSConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(), "Abp.CMS.Localization.Source"
                        )));
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
