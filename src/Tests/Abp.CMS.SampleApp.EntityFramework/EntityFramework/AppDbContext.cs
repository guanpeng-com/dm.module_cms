using System.Data.Common;
using Abp.CMS.EntityFramework;

namespace Abp.CMS.SampleApp.EntityFramework
{
    public class AppDbContext : AbpCMSDbContext
    {
        public AppDbContext(DbConnection connection)
            : base(connection, true)
        {

        }
    }
}