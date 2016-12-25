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
using Abp.Core.Enums;

namespace Abp.Channels
{
    /// <summary>
    /// Represents an Channel.
    /// </summary>
    [Table("cms_Channels")]
    public class Channel : FullAuditedEntity<long>, IMustHaveApp, IMayHaveTenant
    {
        /// <summary>
        /// Maximum length of the <see cref="DisplayName"/> property.
        /// </summary>
        public const int MaxDisplayNameLength = 128;

        /// <summary>
        /// Maximum depth of an channel hierarchy.
        /// </summary>
        public const int MaxDepth = 16;

        /// <summary>
        /// Length of a code unit between dots.
        /// </summary>
        public const int CodeUnitLength = 5;

        /// <summary>
        /// Maximum length of the <see cref="Code"/> property.
        /// </summary>
        public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;

        /// <summary>
        ///  栏目组最大长度
        /// </summary>
        public const int MaxChannelGroupNameCollectionLength = 256;

        /// <summary>
        ///  栏目图片最大长度
        /// </summary>
        public const int MaxImageUrlLength = 256;

        /// <summary>
        ///  栏目路径最大长度
        /// </summary>
        public const int MaxFilePathLength = 256;

        /// <summary>
        ///  栏目链接最大长度
        /// </summary>
        public const int MaxLinkUrlLength = 256;

        /// <summary>
        /// 通用string长度
        /// </summary>
        public const int MaxDefaultLength = 50;

        /// <summary>
        /// TenantId
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// AppId of this entity.
        /// </summary>
        public virtual long AppId { get; set; }

        /// <summary>
        /// Parent <see cref="Channel"/>.
        /// Null, if this OU is root.
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual Channel Parent { get; set; }

        /// <summary>
        /// Parent <see cref="Channel"/> Id.
        /// Null, if this OU is root.
        /// </summary>
        public virtual long? ParentId { get; set; }

        /// <summary>
        /// Hierarchical Code of this organization unit.
        /// Example: "00001.00042.00005".
        /// This is a unique code for a Tenant.
        /// It's changeable if OU hierarch is changed.
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string Code { get; set; }

        /// <summary>
        /// Display name of this role.
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// 栏目模板ID
        /// </summary>
        [Required]
        public virtual long ChannelTemplateId { get; set; }

        /// <summary>
        /// 内容模板ID
        /// </summary>
        [Required]
        public virtual long ContentTemplateId { get; set; }

        /// <summary>
        /// 是否首页栏目
        /// </summary>
        public virtual bool IsIndex { get; set; }

        /// <summary>
        ///  内容模型类型 <see cref="EModelTypeType"/>
        /// </summary>
        [StringLength(MaxDefaultLength)]
        public virtual string ModelType { get; set; }

        /// <summary>
        ///  栏目组集合
        /// </summary>
        [StringLength(MaxChannelGroupNameCollectionLength)]
        public virtual string ChannelGroupNameCollection { get; set; }

        /// <summary>
        ///  栏目图片
        /// </summary>
        [StringLength(MaxImageUrlLength)]
        public virtual string ImageUrl { get; set; }

        /// <summary>
        ///  栏目地址（自定义）
        /// </summary>
        [StringLength(MaxFilePathLength)]
        public virtual string FilePath { get; set; }

        /// <summary>
        ///  栏目链接
        /// </summary>
        [StringLength(MaxLinkUrlLength)]
        public virtual string LinkUrl { get; set; }

        /// <summary>
        ///  栏目链接类型
        /// </summary>
        [StringLength(MaxDefaultLength)]
        public virtual string LinkType { get; set; }

        /// <summary>
        ///  栏目内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [StringLength(MaxDefaultLength)]
        public virtual string Keywords { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(MaxDefaultLength)]
        public virtual string Description { get; set; }

        /// <summary>
        ///  栏目下内容数量
        /// </summary>
        public virtual int ContentNum { get; set; }




        /// <summary>
        /// Children of this OU.
        /// </summary>
        public virtual ICollection<Channel> Children { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        public Channel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="appId">AppId's Id or null for app.</param>
        /// <param name="displayName">Display name.</param>
        /// <param name="parentId">Parent's Id or null if OU is a root.</param>
        public Channel(long appId, string displayName, long? parentId = null)
        {
            AppId = appId;
            DisplayName = displayName;
            ParentId = parentId;
        }

        /// <summary>
        /// Creates code for given numbers.
        /// Example: if numbers are 4,2 then returns "00004.00002";
        /// </summary>
        /// <param name="numbers">Numbers</param>
        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers.Select(number => number.ToString(new string('0', CodeUnitLength))).JoinAsString(".");
        }

        /// <summary>
        /// Appends a child code to a parent code. 
        /// Example: if parentCode = "00001", childCode = "00042" then returns "00001.00042".
        /// </summary>
        /// <param name="parentCode">Parent code. Can be null or empty if parent is a root.</param>
        /// <param name="childCode">Child code.</param>
        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException("childCode", "childCode can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        /// <summary>
        /// Gets relative code to the parent.
        /// Example: if code = "00019.00055.00001" and parentCode = "00019" then returns "00055.00001".
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="parentCode">The parent code.</param>
        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException("code", "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            if (code.Length == parentCode.Length)
            {
                return null;
            }

            return code.Substring(parentCode.Length + 1);
        }

        /// <summary>
        /// Calculates next code for given code.
        /// Example: if code = "00019.00055.00001" returns "00019.00055.00002".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException("code", "code can not be null or empty.");
            }

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        /// <summary>
        /// Gets the last unit code.
        /// Example: if code = "00019.00055.00001" returns "00001".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException("code", "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        /// <summary>
        /// Gets parent code.
        /// Example: if code = "00019.00055.00001" returns "00019.00055".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException("code", "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
        }
    }
}
