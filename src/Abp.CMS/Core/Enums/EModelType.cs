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
    public enum EModelType
    {
        /// <summary>
        /// 文章
        /// </summary>
        Content,
        /// <summary>
        /// 商品
        /// </summary>
        Good
    }

    /// <summary>
    /// 枚举工具类
    /// </summary>
    public class EModelTypeUtils
    {
        /// <summary>
        /// 根据枚举类型获取信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetValue(EModelType type)
        {
            switch (type)
            {
                case EModelType.Content:
                    return "Content";
                case EModelType.Good:
                    return "Good";
                default:
                    return "Content";
            }
        }

        /// <summary>
        /// 根据枚举类型获取展示信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetText(EModelType type)
        {
            return L(GetValue(type));
        }

        /// <summary>
        ///  根据字符串获取枚举类型
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static EModelType GetEnum(string typeStr)
        {
            switch (typeStr)
            {
                case "Content":
                    return EModelType.Content;
                case "Good":
                    return EModelType.Good;
                default:
                    return EModelType.Content;
            }
        }

        /// <summary>
        /// 是否相同
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static bool Equals(EModelType type, string typeStr)
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
        public static bool Equals(string typeStr, EModelType type)
        {
            return Equals(type, typeStr);
        }

        /// <summary>
        /// 获取HTML字符串
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static string GetCtrlStr(EModelType? selected)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(EModelType.Content), GetText(EModelType.Content), EModelType.Content == selected ? "selected='true'" : string.Empty);
            sb.AppendFormat("<option value='{0}' {2}>{1}</option>", GetValue(EModelType.Good), GetText(EModelType.Good), EModelType.Good == selected ? "selected='true'" : string.Empty);
            return sb.ToString();
        }

        /// <summary>
        /// 装在枚举集合
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static ArrayList LoadList(ArrayList array)
        {
            array.Add(new { Key = GetValue(EModelType.Content), Value = GetText(EModelType.Content) });
            array.Add(new { Key = GetValue(EModelType.Good), Value = GetText(EModelType.Good) });
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
