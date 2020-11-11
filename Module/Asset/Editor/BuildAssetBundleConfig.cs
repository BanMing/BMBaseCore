﻿/******************************************************************
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

        public List<BuildChannel> channels;

        public bool isHotfix;

        public bool isNeedReboot;

        public bool isZipAb;

        public bool isEncryptAb;

        public void Check()
        {
            folderPaths.RemoveAll(item =>
            {
                return item.IndexOf("Assets/") == -1 || string.IsNullOrEmpty(item) || !Directory.Exists(Path.GetFullPath(item));
            });
        }

        public string[] ToChannelNameString()
        {
            if (channels == null || channels.Count < 1)
            {
                return new string[] { };
            }
            string[] res = new string[channels.Count];
            for (int i = 0; i < channels.Count; i++)
            {
                res[i] = channels[i].name;
            }
            return res;
        }

        public static BuildAssetBundleConfig Load()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<BuildAssetBundleConfig>(PATH);
        }
    }
}