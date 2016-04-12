using Abp.CMS.NHibernate.EntityMappings;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.CMS.SampleApp.Roles;
using Abp.CMS.SampleApp.Users;

namespace Abp.CMS.SampleApp.NHibernate.Mappings
{
    public class RoleMap : AbpRoleMap<Tenant, Role, User>
    {
        
    }
}