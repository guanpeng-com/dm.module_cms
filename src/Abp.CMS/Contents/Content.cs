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

namespace Abp.Contents
{
    /// <summary>
    /// Represents an Content.
    /// </summary>
    [Table("dm_Contents")]
    public class Content : FullAuditedEntity<long>, IMustHaveApp, IMustHaveChannel
    {
        /// <summary>
        /// Maximum length of the <see cref="Title"/> property.
        /// </summary>
        public const int MaxTitleLength = 128;

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
        ///  栏目ID
        /// </summary>
        public virtual long ChannelId { get; set; }

        /// <summary>
        /// 栏目
        /// </summary>
        [ForeignKey("ChannelId")]
        public virtual Channel Channel { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(MaxTitleLength)]
        public virtual string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string ContentText { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class.
        /// </summary>
        public Content()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class.
        /// </summary>
        /// <param name="appId">应用ID</param>
        /// <param name="channelId">栏目ID</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public Content(long appId, long channelId, string title, string content)
        {
            AppId = appId;
            ChannelId = channelId;
            Title = title;
            ContentText = content;
        }
    }
}
