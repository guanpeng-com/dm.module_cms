namespace Abp.CMS.Configuration
{
    internal class AbpCMSConfig : IAbpCMSConfig
    {
        public IRoleManagementConfig RoleManagement
        {
            get { return _roleManagementConfig; }
        }
        private readonly IRoleManagementConfig _roleManagementConfig;

        public IUserManagementConfig UserManagement
        {
            get { return _userManagementConfig; }
        }
        private readonly IUserManagementConfig _userManagementConfig;

        public ILanguageManagementConfig LanguageManagement
        {
            get { return _languageManagement; }
        }
        private readonly ILanguageManagementConfig _languageManagement;


        public AbpCMSConfig(
            IRoleManagementConfig roleManagementConfig, 
            IUserManagementConfig userManagementConfig,
            ILanguageManagementConfig languageManagement)
        {
            _roleManagementConfig = roleManagementConfig;
            _userManagementConfig = userManagementConfig;
            _languageManagement = languageManagement;
        }
    }
}