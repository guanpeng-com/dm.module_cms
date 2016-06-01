using Abp.CMS;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Core.Enums
{
    /// <summary>
    /// 内容模型类型枚举
    /// </summary>
    public enum ELinkType
    {
        /// <summary>
        /// 默认
        /// </summary>
        LinkNoRelatedToChannelAndContent,
        /// <summary>
        /// 无内容时不可链接
        /// </summary>
        NoLinkIfContentNotExists,
        /// <summary>
        /// 仅一条内容时链接到此内容
        /// </summary>
        LinkToOnlyOneContent,
        /// <summary>
        /// 无内容时不可链接，仅一条内容时链接到此内容
        /// </summary>
        NoLinkIfContentNotExistsAndLinkToOnlyOneContent,
        /// <summary>
        /// 链接到第一条内容
        /// </summary>
        LinkToFirstContent,
        /// <summary>
        /// 无内容时不可链接，有内容时链接到第一条内容
        /// </summary>
        NoLinkIfContentNotExistsAndLinkToFirstContent,
        /// <summary>
        /// 无栏目时不可链接
        /// </summary>
        NoLinkIfChannelNotExists,
        /// <summary>
        /// 链接到最近增加的子栏目
        /// </summary>
        LinkToLastAddChannel,
        /// <summary>
        /// 链接到第一个子栏目
        /// </summary>
        LinkToFirstChannel,
        /// <summary>
        /// 无栏目时不可链接，有栏目时链接到最近增加的子栏目
        /// </summary>
        NoLinkIfChannelNotExistsAndLinkToLastAddChannel,
        /// <summary>
        /// 无栏目时不可链接，有栏目时链接到第一个子栏目
        /// </summary>
        NoLinkIfChannelNotExistsAndLinkToFirstChannel,
        /// <summary>
        /// 不可链接
        /// </summary>
        NoLink
    }

    /// <summary>
    /// 枚举工具类
    /// </summary>
    public class ELinkTypeUtils
    {
        /// <summary>
        /// 根据枚举类型获取信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetValue(ELinkType type)
        {
            if (type == ELinkType.LinkNoRelatedToChannelAndContent)
            {
                return "LinkNoRelatedToChannelAndContent";
            }
            else if (type == ELinkType.NoLinkIfContentNotExists)
            {
                return "NoLinkIfContentNotExists";
            }
            else if (type == ELinkType.LinkToOnlyOneContent)
            {
                return "LinkToOnlyOneContent";
            }
            else if (type == ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent)
            {
                return "NoLinkIfContentNotExistsAndLinkToOnlyOneContent";
            }
            else if (type == ELinkType.LinkToFirstContent)
            {
                return "LinkToFirstContent";
            }
            else if (type == ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent)
            {
                return "NoLinkIfContentNotExistsAndLinkToFirstContent";
            }
            else if (type == ELinkType.NoLinkIfChannelNotExists)
            {
                return "NoLinkIfChannelNotExists";
            }
            else if (type == ELinkType.LinkToLastAddChannel)
            {
                return "LinkToLastAddChannel";
            }
            else if (type == ELinkType.LinkToFirstChannel)
            {
                return "LinkToFirstChannel";
            }
            else if (type == ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel)
            {
                return "NoLinkIfChannelNotExistsAndLinkToLastAddChannel";
            }
            else if (type == ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel)
            {
                return "NoLinkIfChannelNotExistsAndLinkToFirstChannel";
            }
            else if (type == ELinkType.NoLink)
            {
                return "NoLink";
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 根据枚举类型获取展示信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetText(ELinkType type)
        {
            return L(GetValue(type));
        }

        /// <summary>
        ///  根据字符串获取枚举类型
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static ELinkType GetEnum(string typeStr)
        {
            ELinkType retval = ELinkType.LinkNoRelatedToChannelAndContent;

            if (Equals(ELinkType.NoLinkIfContentNotExists, typeStr))
            {
                retval = ELinkType.NoLinkIfContentNotExists;
            }
            else if (Equals(ELinkType.LinkToOnlyOneContent, typeStr))
            {
                retval = ELinkType.LinkToOnlyOneContent;
            }
            else if (Equals(ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent, typeStr))
            {
                retval = ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent;
            }
            else if (Equals(ELinkType.LinkToFirstContent, typeStr))
            {
                retval = ELinkType.LinkToFirstContent;
            }
            else if (Equals(ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent, typeStr))
            {
                retval = ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent;
            }
            else if (Equals(ELinkType.NoLinkIfChannelNotExists, typeStr))
            {
                retval = ELinkType.NoLinkIfChannelNotExists;
            }
            else if (Equals(ELinkType.LinkToLastAddChannel, typeStr))
            {
                retval = ELinkType.LinkToLastAddChannel;
            }
            else if (Equals(ELinkType.LinkToFirstChannel, typeStr))
            {
                retval = ELinkType.LinkToFirstChannel;
            }
            else if (Equals(ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel, typeStr))
            {
                retval = ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel;
            }
            else if (Equals(ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel, typeStr))
            {
                retval = ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel;
            }
            else if (Equals(ELinkType.NoLink, typeStr))
            {
                retval = ELinkType.NoLink;
            }
            else if (Equals(ELinkType.LinkNoRelatedToChannelAndContent, typeStr))
            {
                retval = ELinkType.LinkNoRelatedToChannelAndContent;
            }

            return retval;
        }

        /// <summary>
        /// 是否相同
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static bool Equals(ELinkType type, string typeStr)
        {
            if (String.IsNullOrEmpty(typeStr))
                return false;
            if (GetEnum(typeStr) == type)
                return true;
            return false;
        }

        /// <summary>
        /// 是否相同
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static bool Equals(string typeStr, ELinkType type)
        {
            return Equals(type, typeStr);
        }

        /// <summary>
        /// 获取HTML字符串
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static string GetCtrlStr(ELinkType? selected)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.LinkNoRelatedToChannelAndContent), GetText(ELinkType.LinkNoRelatedToChannelAndContent), ELinkType.LinkNoRelatedToChannelAndContent == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.NoLinkIfContentNotExists), GetText(ELinkType.NoLinkIfContentNotExists), ELinkType.NoLinkIfContentNotExists == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.LinkToOnlyOneContent), GetText(ELinkType.LinkToOnlyOneContent), ELinkType.LinkToOnlyOneContent == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent), GetText(ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent), ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.LinkToFirstContent), GetText(ELinkType.LinkToFirstContent), ELinkType.LinkToFirstContent == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent), GetText(ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent), ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.NoLinkIfChannelNotExists), GetText(ELinkType.NoLinkIfChannelNotExists), ELinkType.NoLinkIfChannelNotExists == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.LinkToLastAddChannel), GetText(ELinkType.LinkToLastAddChannel), ELinkType.LinkToLastAddChannel == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.LinkToFirstChannel), GetText(ELinkType.LinkToFirstChannel), ELinkType.LinkToFirstChannel == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel), GetText(ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel), ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel), GetText(ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel), ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ELinkType.NoLink), GetText(ELinkType.NoLink), ELinkType.NoLink == selected ? "selected='true'" : string.Empty);
            return sb.ToString();
        }

        /// <summary>
        /// 装在枚举集合
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static ArrayList LoadList(ArrayList array)
        {
            array.Add(new { Key = GetValue(ELinkType.LinkNoRelatedToChannelAndContent), Value = GetText(ELinkType.LinkNoRelatedToChannelAndContent) });
            array.Add(new { Key = GetValue(ELinkType.LinkToFirstChannel), Value = GetText(ELinkType.LinkToFirstChannel) });
            array.Add(new { Key = GetValue(ELinkType.LinkToFirstContent), Value = GetText(ELinkType.LinkToFirstContent) });
            array.Add(new { Key = GetValue(ELinkType.LinkToLastAddChannel), Value = GetText(ELinkType.LinkToLastAddChannel) });
            array.Add(new { Key = GetValue(ELinkType.LinkToOnlyOneContent), Value = GetText(ELinkType.LinkToOnlyOneContent) });
            array.Add(new { Key = GetValue(ELinkType.NoLink), Value = GetText(ELinkType.NoLink) });
            array.Add(new { Key = GetValue(ELinkType.NoLinkIfChannelNotExists), Value = GetText(ELinkType.NoLinkIfChannelNotExists) });
            array.Add(new { Key = GetValue(ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel), Value = GetText(ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel) });
            array.Add(new { Key = GetValue(ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel), Value = GetText(ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel) });
            array.Add(new { Key = GetValue(ELinkType.NoLinkIfContentNotExists), Value = GetText(ELinkType.NoLinkIfContentNotExists) });
            array.Add(new { Key = GetValue(ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent), Value = GetText(ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent) });
            array.Add(new { Key = GetValue(ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent), Value = GetText(ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent) });
            return array;
        }

        /// <summary>
        /// 本地化
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string L(string name)
        {
            ILocalizationManager localizationManager = IocManager.Instance.Resolve<ILocalizationManager>();
            return localizationManager.GetString(AbpCMSConsts.LocalizationSourceName, name);
        }
    }
}
