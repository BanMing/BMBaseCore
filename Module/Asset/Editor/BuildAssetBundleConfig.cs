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
        public static readonly string PATH = @"Assets\BMBaseCore\Module\Asset\Editor\FolderAssetBundleConfig.asset";
        public List<string> folderPaths = new List<string>();

        public int version1;
        public int version2;
        public int version3;

        public List<BuildChannel> channels;

        public bool isHotfix;

        public bool isNeedReboot;

        public bool isZipAb;

        public bool isEncryptAb;


        public void TrimData()
        {
            CheckFolderPath();

            channels.RemoveAll(item =>
            {
                return string.IsNullOrEmpty(item.name) || string.IsNullOrEmpty(item.webConfigUrl);
            });
        }

        public void CheckFolderPath()
        {
            folderPaths.RemoveAll(item =>
            {
                return item.IndexOf("Assets/") == -1 || string.IsNullOrEmpty(item) || !Directory.Exists(Path.GetFullPath(item));
            });
        }

        public bool CheckChannel(int selectChannelIndex)
        {
            if (selectChannelIndex >= channels.Count)
            {
                return false;
            }
            Application.OpenURL(channels[selectChannelIndex].webConfigUrl);

            return !string.IsNullOrEmpty(channels[selectChannelIndex].name) && !string.IsNullOrEmpty(channels[selectChannelIndex].webConfigUrl);
        }
        public string[] ToChannelNameString()
        {
            string[] res = new string[channels.Count + 1];
            res[channels.Count] = "new channel";
            for (int i = 0; i < channels.Count; i++)
            {
                res[i] = channels[i].name;
            }
            return res;
        }

#if UNITY_EDITOR
        public static BuildAssetBundleConfig Load()
        {
            BuildAssetBundleConfig config = UnityEditor.AssetDatabase.LoadAssetAtPath<BuildAssetBundleConfig>(PATH);
            config.TrimData();
            return config;
        }
#endif
    }
}
