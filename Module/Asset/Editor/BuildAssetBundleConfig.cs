/******************************************************************
** @File         : FolderAssetBundleConfig.cs
** @Author       : BanMing 
** @Date         : 11/4/2020 10:11:24 PM
** @Description  :  
*******************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BMBaseCore
{
    //[CreateAssetMenu(fileName = "FolderAssetBundleConfig")]
    [Serializable]
    public class BuildAssetBundleConfig : ScriptableObject
    {
        public const string PATH = @"Assets\Scripts\BMBaseCore\Module\Asset\Editor\FolderAssetBundleConfig.asset";
        public List<string> folderPaths = new List<string>();

        public int version1;
        public int version2;
        public int version3;

        public void Check()
        {
            folderPaths.RemoveAll(item =>
            {
                return item.IndexOf("Assets/") == -1 || string.IsNullOrEmpty(item) || !Directory.Exists(Path.GetFullPath(item));
            });
        }

        public static BuildAssetBundleConfig Load()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<BuildAssetBundleConfig>(PATH);
        }
    }
}
