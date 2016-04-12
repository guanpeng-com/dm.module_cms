using Abp.CMS.NHibernate.EntityMappings;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.CMS.SampleApp.Users;

namespace Abp.CMS.SampleApp.NHibernate.Mappings
{
    public class UserMap : AbpUserMap<Tenant, User>
    {

    }
}