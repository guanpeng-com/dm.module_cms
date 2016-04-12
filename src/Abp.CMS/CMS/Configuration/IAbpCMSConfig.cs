namespace Abp.CMS.Configuration
{
    /// <summary>
    /// Configuration options for CMS module.
    /// </summary>
    public interface IAbpCMSConfig
    {
        /// <summary>
        /// Gets role management config.
        /// </summary>
        IRoleManagementConfig RoleManagement { get; }

        /// <summary>
        /// Gets user management config.
        /// </summary>
        IUserManagementConfig UserManagement { get; }

        ILanguageManagementConfig LanguageManagement { get; }
    }
}