/******************************************************************
** @File         : SplitAnimationClipConfig.cs
** @Author       : BanMing 
** @Date         : 11/20/2020 8:14:36 AM
** @Description  :  
*******************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BMBaseCore
{
    //[CreateAssetMenu(fileName = "SplitAnimationClipConfig")]
    [Serializable]
    public class SplitAnimationClipConfig : ScriptableObject
    {

        private const string PATH = @"Assets\Scripts\BMBaseCore\Editor\Animation\SplitAnimationClipConfig.asset";

        public List<string> sourceFolders = new List<string>();

        public string targetTopFolder;

        public bool CheckFolderPath()
        {
            bool isCanSplit = targetTopFolder.IndexOf("Assets/") != -1 && !string.IsNullOrEmpty(targetTopFolder) && Directory.Exists(Path.GetFullPath(targetTopFolder));

            sourceFolders.RemoveAll(item =>
            {
                return item.IndexOf("Assets/") == -1 || string.IsNullOrEmpty(item) || !Directory.Exists(Path.GetFullPath(item));
            });

            return isCanSplit && sourceFolders.Count > 0;
        }

        public static SplitAnimationClipConfig Load()
        {
            SplitAnimationClipConfig config = UnityEditor.AssetDatabase.LoadAssetAtPath<SplitAnimationClipConfig>(PATH);
            config.CheckFolderPath();
            return config;
        }
    }
}
