using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;
using Abp.Apps;
using Abp.Dependency;
using Abp.Channels;
using Abp.IO;
using System.IO;
using Abp.Core.Enums;

namespace Abp.Templates
{
    /// <summary>
    /// Represents an Template.
    /// </summary>
    [Table("cms_Templates")]
    public class Template : FullAuditedEntity<long>, IMustHaveApp, IMayHaveTenant
    {
        public const string IndexDefaultName = "T_默认首页模板";
        public const string IndexDefaultTitle = "默认首页模板";
        public const string ChannelDefaultName = "T_默认栏目模板";
        public const string ChannelDefaultTitle = "默认栏目模板";
        public const string ContentDefaultName = "T_默认内容模板";
        public const string ContentDefaultTitle = "默认内容模板";

        public const string DefaultExtension = ".html";

        /// <summary>
        /// Maximum length of the <see cref="Extension"/> property.
        /// </summary>
        public const int MaxExtensionLength = 64;

        /// <summary>
        /// Maximum length of the <see cref="Type"/> property.
        /// </summary>
        public const int MaxTypeLength = 64;

        /// <summary>
        /// Maximum length of the <see cref="Title"/> property.
        /// </summary>
        public const int MaxTitleLength = 128;

        /// <summary>
        /// Maximum length of the <see cref="Name"/> property.
        /// </summary>
        public const int MaxNameLength = 128;

        /// <summary>
        /// 应用ID
        /// </summary>
        public virtual int? TenantId { get; set; }


        /// <summary>
        /// 应用ID
        /// </summary>
        public virtual long AppId { get; set; }

        /// <summary>
        /// 应用
        /// </summary>
        [ForeignKey("AppId")]
        public virtual App App { get; set; }

        /// <summary>
        /// 模板标题
        /// </summary>
        [Required]
        [StringLength(MaxTitleLength)]
        public virtual string Title { get; set; }

        /// <summary>
        /// 模板文件名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        ///  模板类型
        /// </summary>
        [Required]
        [StringLength(MaxTypeLength)]
        public virtual string Type { get; set; }

        /// <summary>
        ///  模板文件后缀
        /// </summary>
        [Required]
        [StringLength(MaxExtensionLength)]
        public virtual string Extension { get; set; }

        /// <summary>
        /// 是否是默认模板
        /// </summary>
        [Required]
        public virtual bool IsDefault { get; set; }

        /// <summary>
        ///  模板内容
        ///  保存在文件中，不在数据库
        /// </summary>
        [NotMapped]
        public string TemplateContent { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class.
        /// </summary>
        public Template()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class.
        /// </summary>
        public Template(long appId, string title, string name, string type, string extension, bool isDefault)
        {
            AppId = appId;
            Title = title;
            Name = name;
            Extension = extension;
            Type = type;
            IsDefault = isDefault;
        }

        /// <summary>
        /// 初始化首页模板
        /// </summary>
        /// <param name="appId"></param>
        public void InitDefaultIndexTemplate(long appId)
        {
            this.Title = IndexDefaultTitle;
            this.Name = IndexDefaultName;
            this.Type = ETemplateTypeUtils.GetValue(ETemplateType.IndexTemplate);
            this.AppId = appId;
            this.Extension = DefaultExtension;
            this.IsDefault = true;
        }

        /// <summary>
        /// 初始化栏目模板
        /// </summary>
        /// <param name="appId"></param>
        public void InitDefaultChannelTemplate(long appId)
        {
            this.Title = ChannelDefaultTitle;
            this.Name = ChannelDefaultName;
            this.Type = ETemplateTypeUtils.GetValue(ETemplateType.ChannelTemplate);
            this.AppId = appId;
            this.Extension = DefaultExtension;
            this.IsDefault = true;
        }

        /// <summary>
        /// 初始化首页模板
        /// </summary>
        /// <param name="appId"></param>
        public void InitDefaultContentTemplate(long appId)
        {
            this.Title = ContentDefaultTitle;
            this.Name = ContentDefaultName;
            this.Type = ETemplateTypeUtils.GetValue(ETemplateType.ContentTemplate);
            this.AppId = appId;
            this.Extension = DefaultExtension;
            this.IsDefault = true;
        }
    }
}
