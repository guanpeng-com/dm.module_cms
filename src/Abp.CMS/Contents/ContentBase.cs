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
    public class ContentBase : FullAuditedEntity<long>, IMustHaveApp, IMustHaveChannel, IMayHaveTenant
    {
        /// <summary>
        /// Maximum length of the <see cref="Title"/> property.
        /// </summary>
        public const int MaxTitleLength = 128;

        /// <summary>
        ///  默认字符串最大长度
        /// </summary>
        public const int MaxDefaultLength = 50;

        /// <summary>
        ///  最大内容组长度
        /// </summary>
        public const int MaxContentGroupNameCollectionLength = 256;

        /// <summary>
        /// TenantId
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
        /// 内容分组集合
        /// </summary>
        [StringLength(MaxContentGroupNameCollectionLength)]
        public virtual string ContentGroupNameCollection { get; set; }

        /// <summary>
        ///  内容标签
        /// </summary>
        [StringLength(MaxDefaultLength)]
        public virtual string Tags { get; set; }

        /// <summary>
        ///  是否审核
        /// </summary>
        public virtual bool IsChecked { get; set; }

        /// <summary>
        ///  审核等级
        /// </summary>
        public virtual int CheckedLevel { get; set; }

        /// <summary>
        ///  评论数量
        /// </summary>
        public virtual int Comments { get; set; }

        /// <summary>
        /// 点击数
        /// </summary>
        public virtual int Hits { get; set; }

        /// <summary>
        ///  日点击
        /// </summary>
        public virtual int HitsByDay { get; set; }

        /// <summary>
        ///  周点击
        /// </summary>
        public virtual int HitsByWeek { get; set; }

        /// <summary>
        ///  月点击
        /// </summary>
        public virtual int HitsByMonth { get; set; }

        /// <summary>
        ///  最后点击时间
        /// </summary>
        public virtual DateTime? LastHitsDate { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBase"/> class.
        /// </summary>
        public ContentBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBase"/> class.
        /// </summary>
        /// <param name="appId">应用ID</param>
        /// <param name="channelId">栏目ID</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public ContentBase(long appId, long channelId, string title, string content)
        {
            AppId = appId;
            ChannelId = channelId;
            Title = title;
            ContentText = content;
        }
    }
}
