using System.Collections.Generic;

namespace Abp.CMS.Configuration
{
    public interface IRoleManagementConfig
    {
        List<StaticRoleDefinition> StaticRoles { get; }
    }
}