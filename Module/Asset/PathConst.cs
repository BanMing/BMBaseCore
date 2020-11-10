/******************************************************************
** PathConst.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午4:17:05
** @Description  : 
*******************************************************************/

using UnityEngine;

namespace BMBaseCore
{
    public class PathConst
    {

        /// <summary>
        /// folder of asset in project
        /// </summary>
        public static string ResRootUnityPath
        {
            get { return Utility.Text.Format("{0}/Res/", Application.dataPath); }
        }

        /// <summary>
        ///  directory:  Application.streamingAssetsPath /bundles/
        /// </summary>
        public static string AssetBundleDir
        {
            get { return Utility.Text.Format("{0}/bundles/", Application.streamingAssetsPath); }
        }


        public static string AssetBundleManifestPath
        {
            get
            {
                //Application.platform.
                //string 
                return "";
            }
        }

        /// <summary>
        /// directory: Application.dataPath/../Build/
        /// </summary>
        public static string AssetBundleBuildRoot
        {
            get { return Utility.Text.Format("{0}/../Build/", Application.dataPath); }
        }

        public static string FileListName
        {
            get { return "filelist.file"; }
        }
    }
}
