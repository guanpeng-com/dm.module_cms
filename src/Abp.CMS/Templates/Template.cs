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

namespace Abp.Templates
{
    /// <summary>
    /// Represents an Template.
    /// </summary>
    [Table("dm_Templates")]
    public class Template : FullAuditedEntity<long>, IMustHaveApp
    {
        public const string IndexDefaultName = "T_默认首页模板";
        public const string IndexDefaultTitle = "默认首页模板";
        public const string ChannelDefaultName = "T_默认栏目模板";
        public const string ChannelDefaultTitle = "默认栏目模板";
        public const string ContentDefaultName = "T_默认内容模板";
        public const string ContentDefaultTitle = "默认内容模板";

        public const string DefaultExtension = ".html";

        public const string Type_Index = "Index";
        public const string Type_Channel = "Channle";
        public const string Type_Content = "Content";
        public const string Type_File = "File";

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
        /// Initializes a new instance of the <see cref="Template"/> class.
        /// </summary>
        public Template()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class.
        /// </summary>
        public Template(long appId, string title, string name, string type, string extension)
        {
            AppId = appId;
            Title = title;
            Name = name;
            Extension = extension;
        }

        /// <summary>
        /// 初始化首页模板
        /// </summary>
        /// <param name="appId"></param>
        public void InitDefaultIndexTemplate(long appId)
        {
            this.Title = IndexDefaultTitle;
            this.Name = IndexDefaultName;
            this.Type = Type_Index;
            this.AppId = appId;
            this.Extension = DefaultExtension;
        }

        /// <summary>
        /// 初始化栏目模板
        /// </summary>
        /// <param name="appId"></param>
        public void InitDefaultChannelTemplate(long appId)
        {
            this.Title = ChannelDefaultTitle;
            this.Name = ChannelDefaultName;
            this.Type = Type_Channel;
            this.AppId = appId;
            this.Extension = DefaultExtension;
        }

        /// <summary>
        /// 初始化首页模板
        /// </summary>
        /// <param name="appId"></param>
        public void InitDefaultContentTemplate(long appId)
        {
            this.Title = ContentDefaultTitle;
            this.Name = ContentDefaultName;
            this.Type = Type_Content;
            this.AppId = appId;
            this.Extension = DefaultExtension;
        }
    }
}
