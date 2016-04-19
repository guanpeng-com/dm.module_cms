using System.Data.Common;
using System.Data.Entity;
using Abp.BackgroundJobs;
using Abp.EntityFramework;
using Abp.EntityFramework.Extensions;
using Abp.Notifications;
using Abp.Channels;
using Abp.Apps;

namespace Abp.CMS.EntityFramework
{
    /// <summary>
    /// DbContext for ABP CMS.
    /// </summary>
    public abstract class AbpCMSDbContext : AbpDbContext
    {
        /// <summary>
        /// Apps
        /// </summary>
        public virtual IDbSet<App> Apps { get; set; }

        /// <summary>
        /// Channels.
        /// </summary>
        public virtual IDbSet<Channel> Channels { get; set; }

        /// <summary>
        /// Default constructor.
        /// Do not directly instantiate this class. Instead, use dependency injection!
        /// </summary>
        protected AbpCMSDbContext()
            : base("Default")
        {

        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or a name in connection strings in configuration file</param>
        protected AbpCMSDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        /// <summary>
        /// This constructor can be used for unit tests.
        /// </summary>
        protected AbpCMSDbContext(DbConnection dbConnection, bool contextOwnsConnection)
            : base(dbConnection, contextOwnsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
