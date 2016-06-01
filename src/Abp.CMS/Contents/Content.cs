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
    public class Content : ContentBase
    {
        /// <summary>
        /// 子标题最大长度
        /// </summary>
        public const int MaxSubTitleLength = 128;

        /// <summary>
        /// 外链最大长度
        /// </summary>
        public const int MaxLinkUrlLength = 256;

        /// <summary>
        /// 简介最大长度
        /// </summary>
        public const int MaxSummaryLength = 256;

        /// <summary>
        /// 图片最大长度
        /// </summary>
        public const int MaxImageUrlLength = 256;

        /// <summary>
        /// 视频最大长度
        /// </summary>
        public const int MaxVideoUrlLength = 256;

        /// <summary>
        /// 附件最大长度
        /// </summary>
        public const int MaxFileUrlLength = 256;

        /// <summary>
        ///  默认字符串最大长度
        /// </summary>
        public const int MaxDefaultLength = 50;

        /// <summary>
        /// 子标题
        /// </summary>
        [StringLength(MaxSubTitleLength)]
        public virtual string SubTitle { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [StringLength(MaxImageUrlLength)]
        public virtual string ImageUrl { get; set; }

        /// <summary>
        /// 视频
        /// </summary>
        [StringLength(MaxVideoUrlLength)]
        public virtual string VideoUrl { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        [StringLength(MaxFileUrlLength)]
        public virtual string FileUrl { get; set; }

        /// <summary>
        /// 外链
        /// </summary>
        [StringLength(MaxLinkUrlLength)]
        public virtual string LinkUrl { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [StringLength(MaxSummaryLength)]
        public virtual string Summary { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [StringLength(MaxDefaultLength)]
        public virtual string Author { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [StringLength(MaxDefaultLength)]
        public virtual string Source { get; set; }

        /// <summary>
        /// 置顶
        /// </summary>
        public virtual bool IsTop { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        public virtual bool IsRecommend { get; set; }

        /// <summary>
        /// 热点
        /// </summary>
        public virtual bool IsHot { get; set; }

        /// <summary>
        /// 高亮
        /// </summary>
        public virtual bool IsColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class.
        /// </summary>
        public Content()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBase"/> class.
        /// </summary>
        /// <param name="appId">应用ID</param>
        /// <param name="channelId">栏目ID</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public Content(long appId, long channelId, string title, string content)
            : base(appId, channelId, title, content)
        {

        }

    }
}
