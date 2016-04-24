using Abp.CMS.EntityFramework.Migrations.Seed;
using System.Data.Entity.Migrations;

namespace Abp.CMS.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AbpCMSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AbpCMSTemplate";
        }

        protected override void Seed(AbpCMSDbContext context)
        {
            new InitialDbBuilder(context).Create();
        }
    }
}
