/******************************************************************
** Utility.File.cs
** @Author       : BanMing
** @Date         : 9/28/2020 4:37:49 PM
** @Description  :
*******************************************************************/

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BMBaseCore
{
    public static partial class Utility
    {
        public static class File
        {

            private static readonly char[] UrlSafeChars = new[] { '.', '_', '-', ';', '/', '?', '\\', ':' };

            /// <summary>
            /// check exist path ,if not ,creat
            /// </summary>
            /// <param name="path"></param>
            public static void CheckDirectory(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    UnityEngine.Debug.LogError("path is invalid !");
                    return;
                }

                string direct = Path.GetDirectoryName(path);
                if (!Directory.Exists(direct))
                {
                    Directory.CreateDirectory(direct);
                }
            }

            /// <summary>
            /// remove empty directory
            /// </summary>
            /// <param name="dirctoryName"></param>
            /// <returns></returns>
            public static bool RemoveEmptyDirectory(string dirctoryName)
            {
                if (string.IsNullOrEmpty(dirctoryName))
                {
                    UnityEngine.Debug.LogError("dirctoryName is invalid !");
                    return false;
                }
                try
                {
                    if (!Directory.Exists(dirctoryName))
                    {
                        return false;
                    }

                    // don`t use SearchOption.AllDirctories ,to delete more files when have a exception
                    string[] subDirNames = Directory.GetDirectories(dirctoryName, "*");
                    int subDirCount = subDirNames.Length;
                    for (int i = 0; i < subDirNames.Length; i++)
                    {
                        if (RemoveEmptyDirectory(subDirNames[i]))
                        {
                            subDirCount--;
                        }
                    }
                    if (subDirCount > 0)
                    {
                        return false;
                    }

                    string[] files = Directory.GetFiles(dirctoryName, "*");
                    int fileCount = 0;
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (!Path.GetExtension(files[i]).EndsWith("meta") || !Path.GetFileName(files[i]).Equals(".DS_Store"))
                        {
                            fileCount++;
                        }
                    }
                    if (fileCount > 0)
                    {
                        return false;
                    }

                    Directory.Delete(dirctoryName);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            private static string TrimPath(string filePath)
            {
                // Remove . in filePath

                while (filePath.Contains("/./"))
                    filePath = filePath.Replace("/./", "/");

                while (filePath.Contains(@"\.\"))
                    filePath = filePath.Replace(@"\.\", @"\");

                filePath = Regex.Replace(filePath, @"^\.(\/|\\)", string.Empty);

                // Remove .. in filePath

                filePath = Regex.Replace(filePath, @"[^\/\\]+(\/|\\)\.\.(\/|\\)", string.Empty);

                return filePath;
            }

            internal static string UrlEncode(string url)
            {
                var encoder = new UTF8Encoding();
                var safeline = new StringBuilder(encoder.GetByteCount(url) * 3);

                foreach (var c in url)
                {
                    if ((c >= 48 && c <= 57) || (c >= 65 && c <= 90) || (c >= 97 && c <= 122) || Array.IndexOf(UrlSafeChars, c) != -1)
                        safeline.Append(c);
                    else
                    {
                        var bytes = encoder.GetBytes(c.ToString());

                        foreach (var num in bytes)
                        {
                            safeline.Append("%");
                            safeline.Append(num.ToString("X"));
                        }
                    }
                }

                return safeline.ToString();
            }
        }
    }
}