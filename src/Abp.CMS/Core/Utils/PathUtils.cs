using Abp.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Abp.Core.Utils
{
    public class PathUtils
    {
        public const char SeparatorChar = '\\';
        public static readonly char[] InvalidPathChars = Path.InvalidPathChars;

        public static string Combine(params string[] paths)
        {
            string retval = string.Empty;
            if (paths != null && paths.Length > 0)
            {
                retval = (paths[0] != null) ? paths[0].Replace(PageUtils.SeparatorChar, PathUtils.SeparatorChar).TrimEnd(PathUtils.SeparatorChar) : string.Empty;
                for (int i = 1; i < paths.Length; i++)
                {
                    string path = (paths[i] != null) ? paths[i].Replace(PageUtils.SeparatorChar, PathUtils.SeparatorChar).Trim(PathUtils.SeparatorChar) : string.Empty;
                    retval = Path.Combine(retval, path);
                }
            }
            return retval;
        }

        //public static string MapPath(string virtualPath)
        //{
        //    string retval = string.Empty;
        //    if (!string.IsNullOrEmpty(virtualPath))
        //    {
        //        if (virtualPath.StartsWith("~"))
        //        {
        //            virtualPath = virtualPath.Substring(1);
        //        }
        //        virtualPath = PageUtils.Combine("~", virtualPath);
        //    }
        //    HttpContext context = HttpContext.Current;
        //    if(context != null)
        //    {
        //        retval = context.Server.MapPath(virtualPath);
        //    }
        //    else
        //    {
        //        if (virtualPath != null)
        //        {
        //            virtualPath = virtualPath.Substring(2);
        //        }
        //        else
        //        {
        //            virtualPath = string.Empty;
        //        }
        //        retval = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  virtualPath);
        //    }
        //    if (retval == null) retval = string.Empty;
        //    return retval.Replace("/", "\\");
        //}


        //获取当前页面不带后缀的文件名
        public static string GetCurrentFileNameWithoutExtension()
        {
            if (HttpContext.Current != null)
            {
                return Path.GetFileNameWithoutExtension(HttpContext.Current.Request.PhysicalPath);
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据路径扩展名判断是否为文件夹路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectoryPath(string path)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(path))
            {
                string ext = Path.GetExtension(path);
                if (!string.IsNullOrEmpty(ext))		//path为文件路径
                {
                    retval = false;
                }
                else									//path为文件夹路径
                {
                    retval = true;
                }
            }
            return retval;
        }

        public static string GetExtension(string path)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Trim('/', '\\').Trim();
                try
                {
                    retval = Path.GetExtension(path);
                }
                catch { }
            }
            return retval;
        }

        public static string RemoveExtension(string fileName)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                int index = fileName.LastIndexOf('.');
                if (index != -1)
                {
                    retval = fileName.Substring(0, index);
                }
                else
                {
                    retval = fileName;
                }
            }
            return retval;
        }

        public static string RemoveParentPath(string path)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                retval = path.Replace("../", string.Empty);
                retval = retval.Replace("./", string.Empty);
            }
            return retval;
        }

        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public static string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public static string GetDirectoryName(string directoryPath)
        {
            if (!string.IsNullOrEmpty(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                return directoryInfo.Name;
            }
            return string.Empty;
        }

        public static string GetDirectoryDifference(string rootDirectoryPath, string path)
        {
            string directoryPath = DirectoryUtils.GetDirectoryPath(path);
            if (!string.IsNullOrEmpty(directoryPath) && StringUtils.StartsWithIgnoreCase(directoryPath, rootDirectoryPath))
            {
                string retval = directoryPath.Substring(rootDirectoryPath.Length, directoryPath.Length - rootDirectoryPath.Length);
                return retval.Trim('/', '\\');
            }
            return string.Empty;
        }

        public static string GetPathDifference(string rootPath, string path)
        {
            if (!string.IsNullOrEmpty(path) && StringUtils.StartsWithIgnoreCase(path, rootPath))
            {
                string retval = path.Substring(rootPath.Length, path.Length - rootPath.Length);
                return retval.Trim('/', '\\');
            }
            return string.Empty;
        }

        public static string AddVirtualToPath(string path)
        {
            string resolvedPath = path;
            if (!string.IsNullOrEmpty(path))
            {
                path.Replace("../", string.Empty);
                if (!path.StartsWith("~"))
                {
                    resolvedPath = "~" + path;
                }
            }
            return resolvedPath;
        }

        public static string GetCurrentPagePath()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.PhysicalPath;
            }
            return string.Empty;
        }

        public static string RemovePathInvalidChar(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return filePath;
            string invalidChars = new string(Path.GetInvalidPathChars());
            string invalidReStr = string.Format("[{0}]", Regex.Escape(invalidChars));
            return Regex.Replace(filePath, invalidReStr, "");
        }

        public static string MapPath(string virtualPath)
        {
            virtualPath = PathUtils.RemovePathInvalidChar(virtualPath);
            string retval;
            if (!string.IsNullOrEmpty(virtualPath))
            {
                if (virtualPath.StartsWith("~"))
                {
                    virtualPath = virtualPath.Substring(1);
                }
                virtualPath = PageUtils.Combine("~", virtualPath);
            }
            else
            {
                virtualPath = "~/";
            }
            if (HttpContext.Current != null)
            {
                retval = HttpContext.Current.Server.MapPath(virtualPath);
            }
            else
            {
                string rootPath = HttpContext.Current.Request.PhysicalApplicationPath;

                if (!string.IsNullOrEmpty(virtualPath))
                {
                    virtualPath = virtualPath.Substring(2);
                }
                else
                {
                    virtualPath = string.Empty;
                }
                retval = PathUtils.Combine(rootPath, virtualPath);
            }

            if (retval == null) retval = string.Empty;
            return retval.Replace("/", "\\");
        }

        public static bool IsFileExtenstionAllowed(string sAllowedExt, string sExt)
        {
            bool allow = false;
            if (sExt != null && sExt.StartsWith("."))
            {
                sExt = sExt.Substring(1, sExt.Length - 1);
            }
            sAllowedExt = sAllowedExt.Replace("|", ",");
            string[] aExt = sAllowedExt.Split(',');
            for (int i = 0; i < aExt.Length; i++)
            {
                if (StringUtils.EqualsIgnoreCase(sExt, aExt[i]))
                {
                    allow = true;
                    break;
                }
            }
            return allow;
        }

        public static bool IsFileExtenstionNotAllowed(string sNotAllowedExt, string sExt)
        {
            bool allow = true;
            if (sExt != null && sExt.StartsWith("."))
            {
                sExt = sExt.Substring(1, sExt.Length - 1);
            }
            sNotAllowedExt = sNotAllowedExt.Replace("|", ",");
            string[] aExt = sNotAllowedExt.Split(',');
            for (int i = 0; i < aExt.Length; i++)
            {
                if (StringUtils.EqualsIgnoreCase(sExt, aExt[i]))
                {
                    allow = false;
                    break;
                }
            }
            return allow;
        }
       

    }
}
