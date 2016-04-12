using System.Data.Common;
using Abp.CMS.EntityFramework;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.CMS.SampleApp.Roles;
using Abp.CMS.SampleApp.Users;

namespace Abp.CMS.SampleApp.EntityFramework
{
    public class AppDbContext : AbpCMSDbContext<Tenant, Role, User>
    {
        public AppDbContext(DbConnection connection)
            : base(connection, true)
        {

        }
    }
}