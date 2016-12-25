using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Apps
{
    /// <summary>
    /// 应用
    /// </summary>
    [Table("cms_Apps")]
    public class App : FullAuditedEntity<long>, IMayHaveTenant
    {
        public const string DefaultName = "Main";
        public const string DefaultDir = "Main";

        /// <summary>
        /// <see cref="AppName" />的最大长度
        /// </summary>
        public const int MaxAppNameLength = 128;

        /// <summary>
        /// <see cref="DisplayName" />的最大长度
        /// </summary>
        public const int MaxDisplayNameLength = 20;

        /// <summary>
        /// <see cref="AppDir" />的最大长度
        /// </summary>
        public const int MaxAppDirLength = 128;

        /// <summary>
        /// <see cref="AppUrl" />的最大长度
        /// </summary>
        public const int MaxAppUrlLength = 1024;

        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        ///  app名称
        /// </summary>
        [Required]
        [StringLength(MaxAppNameLength)]
        public virtual string AppName { get; set; }

        /// <summary>
        /// 展示名称
        /// </summary>
        [StringLength(MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        /// <summary>
        /// app文件夹
        /// </summary>
        [Required]
        [StringLength(MaxAppDirLength)]
        public string AppDir { get; set; }

        /// <summary>
        /// app地址
        /// </summary>
        [Required]
        [StringLength(MaxAppUrlLength)]
        public string AppUrl { get; set; }

        /// <summary>
        ///  构造函数<see cref="App"/>
        /// </summary>
        public App()
        { }

        /// <summary>
        ///  构造函数<see cref="App"/>
        /// </summary>
        public App(int? tenantId, string appName, string displayName, string appDir)
        {
            this.TenantId = tenantId;
            this.AppName = appName;
            this.DisplayName = displayName;
            this.AppDir = appDir;
            this.AppUrl = "/" + appDir;
        }

    }
}
