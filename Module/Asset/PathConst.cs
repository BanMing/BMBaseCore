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
        /// Assets/
        /// </summary>
        public const string AssetsPerfix = "Assets/";

        /// <summary>
        /// folder of asset in project
        /// </summary>
        public static string ResRootUnityPath
        {
            get { return Utility.Text.Format("{0}/Res/", Application.dataPath); }
        }

        /// <summary>
        ///  directory:  {Application.streamingAssetsPath} /bundles/
        /// </summary>
        public static string AssetBundleDir
        {
            get { return Utility.Text.Format("{0}/bundles/", Application.streamingAssetsPath); }
        }

        /// <summary>
        /// directory:  {Application.streamingAssetsPath} /bundles/{version}
        /// </summary>
        public static string AssetBundleManifestPath
        {
            get
            {
                return Utility.Text.Format("{0}{1}", AssetBundleDir, GameManager.Instance.GameConfig.Version);
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

        public static string FileListPath
        {
            get { return Utility.Text.Format("{0}{1}", AssetBundleDir, FileListName); }
        }
    }
}
