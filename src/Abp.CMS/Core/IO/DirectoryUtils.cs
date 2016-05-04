using Abp.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Core.IO
{
    /// <summary>
    /// 封装操作文件夹代码的类
    /// </summary>
    public class DirectoryUtils
    {
        public class aspnet_client
        {
            public const string DirectoryName = "aspnet_client";
        }

        public class Bin
        {
            public const string DirectoryName = "Bin";
        }

        public class obj
        {
            public const string DirectoryName = "obj";
        }

        public class WebConfig
        {
            public const string DirectoryName = "Web.config";
        }

        public static char DirectorySeparatorChar = Path.DirectorySeparatorChar;

        public static void CreateDirectoryIfNotExists(string path)
        {
            string directoryPath = GetDirectoryPath(path);

            if (!IsDirectoryExists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch
                {
                    //Scripting.FileSystemObject fso = new Scripting.FileSystemObjectClass();
                    //string[] directoryNames = directoryPath.Split('\\');
                    //string thePath = directoryNames[0];
                    //for (int i = 1; i < directoryNames.Length; i++)
                    //{
                    //    thePath = thePath + "\\" + directoryNames[i];
                    //    if (StringUtils.Contains(thePath.ToLower(), ConfigUtils.Instance.PhysicalApplicationPath.ToLower()) && !IsDirectoryExists(thePath))
                    //    {
                    //        fso.CreateFolder(thePath);
                    //    }
                    //}                    
                }
            }
        }

        public static void Copy(string sourcePath, string targetPath)
        {
            Copy(sourcePath, targetPath, true);
        }

        public static void Copy(string sourcePath, string targetPath, bool isOverride)
        {
            if (Directory.Exists(sourcePath))
            {
                DirectoryUtils.CreateDirectoryIfNotExists(targetPath);
                DirectoryInfo directoryInfo = new DirectoryInfo(sourcePath);
                if (directoryInfo.GetFileSystemInfos() != null)
                {
                    foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
                    {
                        string destPath = Path.Combine(targetPath, fileSystemInfo.Name);
                        if (fileSystemInfo is FileInfo)
                        {
                            FileUtils.CopyFile(fileSystemInfo.FullName, destPath, isOverride);
                        }
                        else if (fileSystemInfo is DirectoryInfo)
                        {
                            Copy(fileSystemInfo.FullName, destPath, isOverride);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 验证此字符串是否合作作为文件夹名称
        /// </summary>
        public static bool IsDirectoryNameCompliant(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName)) return false;
            if (-1 != directoryName.IndexOfAny(PathUtils.InvalidPathChars))
            {
                return false;
            }
            //for (int i = 0; i < directoryName.Length; i++)
            //{
            //    if (StringUtils.IsTwoBytesChar(directoryName[i]))
            //    {
            //        return false;
            //    }
            //}
            return true;
        }

        /// <summary>
        /// 获取文件的文件夹路径，如果path为文件夹，返回自身。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetDirectoryPath(string path)
        {
            string directoryPath;
            string ext = Path.GetExtension(path);
            if (!string.IsNullOrEmpty(ext))		//path为文件路径
            {
                directoryPath = Path.GetDirectoryName(path);
            }
            else									//path为文件夹路径
            {
                directoryPath = path;
            }
            return directoryPath;
        }


        public static bool IsDirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public static bool IsInDirectory(string parentDirectoryPath, string path)
        {
            if (string.IsNullOrEmpty(parentDirectoryPath) || string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException();
            }

            parentDirectoryPath = parentDirectoryPath.Trim().ToLower();
            path = path.Trim().ToLower();

            char ch1 = parentDirectoryPath[parentDirectoryPath.Length - 1];
            if (ch1 == Path.DirectorySeparatorChar)
            {
                parentDirectoryPath = parentDirectoryPath.Substring(0, parentDirectoryPath.Length - 1);
            }

            char ch2 = path[path.Length - 1];
            if (ch2 == Path.DirectorySeparatorChar)
            {
                path = path.Substring(0, path.Length - 1);
            }

            return path.StartsWith(parentDirectoryPath);
        }

        public static void MoveDirectory(string srcDirectoryPath, string destDirectoryPath, bool isOverride)
        {
            //如果提供的路径中不存在末尾分隔符，则添加末尾分隔符。
            if (!srcDirectoryPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                srcDirectoryPath += Path.DirectorySeparatorChar;
            }
            if (!destDirectoryPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                destDirectoryPath += Path.DirectorySeparatorChar;
            }

            //如果目标目录不存在，则予以创建。 
            DirectoryUtils.CreateDirectoryIfNotExists(destDirectoryPath);

            //从当前父目录中获取目录列表。 
            foreach (string srcDir in Directory.GetDirectories(srcDirectoryPath))
            {
                string directoryName = PathUtils.GetDirectoryName(srcDir);

                DirectoryInfo srcDirInfo = new DirectoryInfo(srcDir);

                string destDir = destDirectoryPath + directoryName;
                //如果该目录不存在，则创建该目录。 
                DirectoryUtils.CreateDirectoryIfNotExists(destDir);
                //由于我们处于递归模式下，因此还要复制子目录
                MoveDirectory(srcDir, destDir, isOverride);
            }

            //从当前父目录中获取文件。
            foreach (string srcFile in Directory.GetFiles(srcDirectoryPath))
            {
                FileInfo srcFileInfo = new FileInfo(srcFile);
                FileInfo destFileInfo = new FileInfo(srcFile.Replace(srcDirectoryPath, destDirectoryPath));
                //如果文件不存在，则进行复制。 
                bool isExists = destFileInfo.Exists;
                if (isOverride)
                {
                    if (isExists)
                    {
                        FileUtils.DeleteFileIfExists(destFileInfo.FullName);
                    }
                    FileUtils.CopyFile(srcFileInfo.FullName, destFileInfo.FullName);
                }
                else if (!isExists)
                {
                    FileUtils.CopyFile(srcFileInfo.FullName, destFileInfo.FullName);
                }
            }
        }


        public static string[] GetDirectoryNames(string directoryPath)
        {
            string[] directorys = Directory.GetDirectories(directoryPath);
            string[] retval = new string[directorys.Length];
            int i = 0;
            foreach (string directory in directorys)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                retval[i++] = directoryInfo.Name;
            }
            return retval;
        }

        public static ArrayList GetLowerDirectoryNames(string directoryPath)
        {
            ArrayList arraylist = new ArrayList();
            string[] directorys = Directory.GetDirectories(directoryPath);
            foreach (string directory in directorys)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                arraylist.Add(directoryInfo.Name.ToLower());
            }
            return arraylist;
        }

        public static string[] GetFileNames(string directoryPath)
        {
            string[] filePaths = Directory.GetFiles(directoryPath);
            string[] retval = new string[filePaths.Length];
            int i = 0;
            foreach (string filePath in filePaths)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                retval[i++] = fileInfo.Name;
            }
            return retval;
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="directoryPath">文件夹路径</param>
        /// <returns>删除过程中是否出错</returns>
        public static bool DeleteDirectoryIfExists(string directoryPath)
        {
            bool retval = true;
            try
            {
                if (DirectoryUtils.IsDirectoryExists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }
            }
            catch
            {
                retval = false;
            }
            return retval;
        }

        public static void DeleteFilesSync(string rootDirectoryPath, string syncDirectoryPath)
        {
            if (DirectoryUtils.IsDirectoryExists(syncDirectoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(syncDirectoryPath);
                if (directoryInfo.GetFileSystemInfos() != null)
                {
                    foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
                    {
                        string fileSystemPath = PathUtils.Combine(rootDirectoryPath, fileSystemInfo.Name);
                        if (fileSystemInfo is FileInfo)
                        {
                            try
                            {
                                FileUtils.DeleteFileIfExists(fileSystemPath);
                            }
                            catch { }
                        }
                        else if (fileSystemInfo is DirectoryInfo)
                        {
                            DeleteFilesSync(fileSystemPath, fileSystemInfo.FullName);
                            DirectoryUtils.DeleteEmptyDirectory(fileSystemPath);
                        }
                    }
                }
            }
        }

        public static void DeleteEmptyDirectory(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            if (directoryInfo != null && directoryInfo.Exists)
            {
                if (directoryInfo.GetFileSystemInfos() == null || directoryInfo.GetFileSystemInfos().Length == 0)
                {
                    try
                    {
                        DirectoryUtils.DeleteDirectoryIfExists(directoryPath);
                    }
                    catch { }
                }
            }
        }

        //public static void DeleteChildrens(string directoryPath)
        //{
        //    string[] filePaths = DirectoryUtils.GetFilePaths(directoryPath);
        //    FileUtils.DeleteFilesIfExists(filePaths);

        //    string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(directoryPath);
        //    foreach (string subDirectoryPath in directoryPaths)
        //    {
        //        DirectoryUtils.DeleteDirectoryIfExists(subDirectoryPath);
        //    }
        //}

        public static void CreateUrlRedirectDirectories(string sourceUrlRedirectFilePath, string directoryPath, ArrayList directoryNameArrayList)
        {
            if (directoryNameArrayList != null && directoryNameArrayList.Count > 0)
            {
                foreach (string directoryName in directoryNameArrayList)
                {
                    DirectoryUtils.CreateUrlRedirectDirectory(sourceUrlRedirectFilePath, PathUtils.Combine(directoryPath, directoryName));
                }
            }
        }


        public static void CreateUrlRedirectDirectory(string sourceUrlRedirectFilePath, string directoryPath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            string filePath = PathUtils.Combine(directoryPath, "index.html");
            if (!FileUtils.IsFileExists(filePath))
            {
                FileUtils.CopyFile(sourceUrlRedirectFilePath, filePath);
            }
        }

        public static string[] GetDirectoryPaths(string directoryPath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return Directory.GetDirectories(directoryPath);
        }

        public static string[] GetDirectoryPaths(string directoryPath, string searchPattern)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
            return Directory.GetDirectories(directoryPath, searchPattern);
        }

        public static string[] GetFilePaths(string directoryPath)
        {
            return Directory.GetFiles(directoryPath);
        }

        public static long GetDirectorySize(string directoryPath)
        {
            long size = 0;
            string[] filePaths = DirectoryUtils.GetFilePaths(directoryPath);
            //通过GetFiles方法,获取目录中所有文件的大小
            foreach (string filePath in filePaths)
            {
                FileInfo info = new FileInfo(filePath);
                size += info.Length;
            }
            string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(directoryPath);
            //获取目录下所有文件夹大小,并存到一个新的对象数组中
            foreach (string path in directoryPaths)
            {
                size += GetDirectorySize(path);
            }
            return size;
        }

        public static bool IsSystemDirectory(string directoryName)
        {
            if (StringUtils.EqualsIgnoreCase(directoryName, DirectoryUtils.aspnet_client.DirectoryName)
                || StringUtils.EqualsIgnoreCase(directoryName, DirectoryUtils.Bin.DirectoryName)
                || StringUtils.EqualsIgnoreCase(directoryName, "obj")
                || StringUtils.EqualsIgnoreCase(directoryName, "Properties"))
            {
                return true;
            }
            return false;
        }
    }
}
