using Abp.Localization.Dictionaries;

namespace Abp.CMS.Configuration
{
    /// <summary>
    /// Used to configure language management.
    /// </summary>
    public interface ILanguageManagementConfig
    {
        /// <summary>
        /// Enables the database localization.
        /// Replaces all <see cref="IDictionaryBasedLocalizationSource"/> localization sources
        /// with database based localization source.
        /// </summary>
        void EnableDbLocalization();
    }
}