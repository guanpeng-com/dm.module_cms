using System.Data.Common;
using System.Data.Entity;
using Abp.BackgroundJobs;
using Abp.EntityFramework;
using Abp.EntityFramework.Extensions;
using Abp.Notifications;
using Abp.Channels;
using Abp.Apps;
using Abp.Dependency;
using Abp.Contents;
using Abp.Templates;
using Abp.DMUsers;
using Abp.Zero.EntityFramework;
using Abp.MultiTenancy;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;

namespace Abp.CMS.EntityFramework
{
    /// <summary>
    /// DbContext for ABP CMS.
    /// </summary>
    public abstract class AbpCMSDbContext<TTenant, TRole, TUser, TDMUser> : AbpZeroDbContext<TTenant, TRole, TUser>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
        where TDMUser : DMUser<TDMUser>
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
        /// Contents.
        /// </summary>
        public virtual IDbSet<Content> Contents { get; set; }

        /// <summary>
        /// Goods.
        /// </summary>
        public virtual IDbSet<Good> Goods { get; set; }

        /// <summary>
        /// Template.
        /// </summary>
        public virtual IDbSet<Template> Templates { get; set; }

        /// <summary>
        /// 前台用户
        /// </summary>
        public virtual IDbSet<TDMUser> DMUsers { get; set; }

        /// <summary>
        /// 前台用户登陆
        /// </summary>
        public virtual IDbSet<DMUserLogin> DMUserLogins { get; set; }

        /// <summary>
        /// 前台用户登陆记录
        /// </summary>
        public virtual IDbSet<DMUserLoginAttempt> DMUserLoginAttempts { get; set; }

        /// <summary>
        /// Default constructor.
        /// Do not directly instantiate this class. Instead, use dependency injection!
        /// </summary>
        public AbpCMSDbContext()
            : base("Default")
        {

        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or a name in connection strings in configuration file</param>
        public AbpCMSDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        /// <summary>
        /// This constructor can be used for unit tests.
        /// </summary>
        public AbpCMSDbContext(DbConnection dbConnection, bool contextOwnsConnection)
            : base(dbConnection, contextOwnsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
