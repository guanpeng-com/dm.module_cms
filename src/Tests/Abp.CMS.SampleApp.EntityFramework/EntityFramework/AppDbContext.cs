using Abp.Authorization.Users;
using Abp.CMS.EntityFramework;
using Abp.CMS.SampleApp.Authorization.Roles;
using Abp.CMS.SampleApp.Authorization.Users;
using Abp.CMS.SampleApp.DMUsers;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.DMUsers;
using Abp.MultiTenancy;
using System.Data.Common;
using System.Data.Entity;

namespace Abp.CMS.SampleApp.EntityFramework
{
    public class AppDbContext : AbpCMSDbContext<Tenant, Role, User, DMUser>
    {
        public AppDbContext(DbConnection connection)
            : base(connection, true)
        {

        }

    }
}