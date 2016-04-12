using System;
using Abp.CMS.Configuration;

namespace Abp.CMS.Ldap.Configuration
{
    public class AbpCMSLdapModuleConfig : IAbpCMSLdapModuleConfig
    {
        public bool IsEnabled { get; private set; }

        public Type AuthenticationSourceType { get; private set; }

        private readonly IAbpCMSConfig _cmsConfig;

        public AbpCMSLdapModuleConfig(IAbpCMSConfig cmsConfig)
        {
            _cmsConfig = cmsConfig;
        }

        public void Enable(Type authenticationSourceType)
        {
            AuthenticationSourceType = authenticationSourceType;
            IsEnabled = true;

            _cmsConfig.UserManagement.ExternalAuthenticationSources.Add(authenticationSourceType);
        }
    }
}