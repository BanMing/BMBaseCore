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
        [Serializable]
        public class Config
        {
            public string sourceFolderPath;
            public string targetFolderPath;
        }

        private const string PATH = @"Assets\BMBaseCore\Editor\Animation\SplitAnimationClipConfig.asset";

        public List<Config> configs = new List<Config>();

        public bool CheckFolderPath()
        {
            configs.RemoveAll(item =>
            {
                bool isSourceOk = string.IsNullOrEmpty(item.sourceFolderPath) ||
                item.sourceFolderPath.IndexOf(PathConst.AssetsPerfix) == -1 ||
                !Directory.Exists(Path.GetFullPath(item.sourceFolderPath));

                bool isTargetOk = string.IsNullOrEmpty(item.targetFolderPath) ||
              item.targetFolderPath.IndexOf(PathConst.AssetsPerfix) == -1 ||
              !Directory.Exists(Path.GetFullPath(item.targetFolderPath));

                return isSourceOk && isTargetOk;
            });
            return configs.Count > 0;
        }


        public void AddEmptyOne()
        {
            configs.Add(new Config());
        }

        public void DelOne(int index)
        {
            if (configs.Count <= index) { return; }

            configs.RemoveAt(index);
        }

        public static SplitAnimationClipConfig Load()
        {
            SplitAnimationClipConfig config = UnityEditor.AssetDatabase.LoadAssetAtPath<SplitAnimationClipConfig>(PATH);
            config.CheckFolderPath();
            return config;
        }
    }
}
