using Abp.CMS;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Localization;
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
        IndexTemplate,
        /// <summary>
        /// 栏目模板
        /// </summary>
        ChannelTemplate,
        /// <summary>
        /// 内容模板
        /// </summary>
        ContentTemplate,
        /// <summary>
        /// 单页模板
        /// </summary>
        FileTemplate
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
                case ETemplateType.IndexTemplate:
                    return "IndexTemplate";
                case ETemplateType.ChannelTemplate:
                    return "ChannelTemplate";
                case ETemplateType.ContentTemplate:
                    return "ContentTemplate";
                case ETemplateType.FileTemplate:
                    return "FileTemplate";
                default:
                    return "FileTemplate";
            }
        }

        /// <summary>
        /// 根据枚举类型获取展示信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetText(ETemplateType type)
        {
            return L(GetValue(type));
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
                case "IndexTemplate":
                    return ETemplateType.IndexTemplate;
                case "ChannelTemplate":
                    return ETemplateType.ChannelTemplate;
                case "ContentTemplate":
                    return ETemplateType.ContentTemplate;
                case "FileTemplate":
                    return ETemplateType.FileTemplate;
                default:
                    return ETemplateType.FileTemplate;
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
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ETemplateType.IndexTemplate), GetText(ETemplateType.IndexTemplate), ETemplateType.IndexTemplate == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ETemplateType.ChannelTemplate), GetText(ETemplateType.ChannelTemplate), ETemplateType.ChannelTemplate == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ETemplateType.ContentTemplate), GetText(ETemplateType.ContentTemplate), ETemplateType.ContentTemplate == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(ETemplateType.FileTemplate), GetText(ETemplateType.FileTemplate), ETemplateType.FileTemplate == selected ? "selected='true'" : string.Empty);
            return sb.ToString();
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
