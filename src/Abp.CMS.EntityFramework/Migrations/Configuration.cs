using Abp.CMS.EntityFramework;
using Abp.CMS.EntityFramework.Migrations.Seed;
using Abp.DMUsers;
using System.Data.Entity.Migrations;

namespace Abp.CMS.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AbpCMSDbContext<Tenant, Role, User, DMUser>>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AbpCMSTemplate";
        }

        protected override void Seed(AbpCMSDbContext<Tenant, Role, User, DMUser> context)
        {
            new InitialDbBuilder(context).Create();
        }
    }
}
