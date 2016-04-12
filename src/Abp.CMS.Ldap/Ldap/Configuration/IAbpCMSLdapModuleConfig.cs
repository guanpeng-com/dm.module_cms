using System;

namespace Abp.CMS.Ldap.Configuration
{
    public interface IAbpCMSLdapModuleConfig
    {
        bool IsEnabled { get; }

        Type AuthenticationSourceType { get; }

        void Enable(Type authenticationSourceType);
    }
}