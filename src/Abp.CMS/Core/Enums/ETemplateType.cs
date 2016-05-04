using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Core.Enums
{
    /// <summary>
    /// 模板类型枚举
    /// </summary>
    public enum ETemplateType
    {
        /// <summary>
        /// 首页模板
        /// </summary>
        Index,
        /// <summary>
        /// 栏目模板
        /// </summary>
        Channel,
        /// <summary>
        /// 内容模板
        /// </summary>
        Content,
        /// <summary>
        /// 单页模板
        /// </summary>
        File
    }

    /// <summary>
    /// 枚举工具类
    /// </summary>
    public class ETemplateTypeUtils
    {
        /// <summary>
        /// 根据枚举类型获取信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetValue(ETemplateType type)
        {
            switch (type)
            {
                case ETemplateType.Index:
                    return "Index";
                case ETemplateType.Channel:
                    return "Channel";
                case ETemplateType.Content:
                    return "Content";
                case ETemplateType.File:
                    return "File";
                default:
                    return "File";
            }
        }

        /// <summary>
        /// 根据枚举类型获取展示信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetText(ETemplateType type)
        {
            switch (type)
            {
                case ETemplateType.Index:
                    return GetValue(ETemplateType.Index);
                case ETemplateType.Channel:
                    return GetValue(ETemplateType.Channel);
                case ETemplateType.Content:
                    return GetValue(ETemplateType.Content);
                case ETemplateType.File:
                    return GetValue(ETemplateType.File);
                default:
                    return GetValue(ETemplateType.File);
            }
        }

        /// <summary>
        ///  根据字符串获取枚举类型
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static ETemplateType GetEnum(string typeStr)
        {
            switch (typeStr)
            {
                case "Index":
                    return ETemplateType.Index;
                case "Channel":
                    return ETemplateType.Channel;
                case "Content":
                    return ETemplateType.Content;
                case "File":
                    return ETemplateType.File;
                default:
                    return ETemplateType.File;
            }
        }

        /// <summary>
        /// 是否相同
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static bool Equals(ETemplateType type, string typeStr)
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
        public static bool Equals(string typeStr, ETemplateType type)
        {
            return Equals(type, typeStr);
        }

        /// <summary>
        /// 获取HTML字符串
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static string GetCtrlStr(ETemplateType? selected)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ETemplateType.Index), GetText(ETemplateType.Index), ETemplateType.Index == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ETemplateType.Channel), GetText(ETemplateType.Channel), ETemplateType.Channel == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ETemplateType.Content), GetText(ETemplateType.Content), ETemplateType.Content == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ETemplateType.File), GetText(ETemplateType.File), ETemplateType.File == selected ? "selected='true'" : string.Empty);
            return sb.ToString();
        }
    }
}
