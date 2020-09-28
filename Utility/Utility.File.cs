/******************************************************************
** Utility.File.cs
** @Author       : BanMing 
** @Date         : 9/28/2020 4:37:49 PM
** @Description  : 
*******************************************************************/

using System.IO;

namespace BMBaseCore
{
    public static partial class Utility
    {
        public static class File
        {
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
                        if (!Path.GetExtension(files[i]).EndsWith("meta"))
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

        }
    }
}