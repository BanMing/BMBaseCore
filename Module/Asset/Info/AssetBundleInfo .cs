/******************************************************************
** AssetBundleInfo.cs
** @Author       : BanMing 
** @Date         : 2020/9/28 上午11:22:55
** @Description  : 
*******************************************************************/

using UnityEngine;

namespace BMBaseCore
{
    public class AssetBundleInfo : BaseObject
    {
        #region Attriube

        public int pathHash;

        public AssetBundle bundle;

        public UnityEngine.Object[] assets;

        public int refCount;

        public AssetBundleInfo[] dependecies;

        #endregion

        #region Public Method

        public override void Destroy()
        {
            base.Destroy();

            pathHash = default;
            refCount = default;
            dependecies = null;
            assets = null;
            bundle.Unload(true);
        }

        public T As<T>() where T : UnityEngine.Object
        {
            if (assets == null || assets.Length < 1 || assets[0] == null)
            {
                return null;
            }
            return assets[0] as T;
        }

        public T GetOneAsset<T>(string assetName) where T : UnityEngine.Object
        {
            if (assets == null || assets.Length < 1)
            {
                return null;
            }

            for (int i = 0; i < assets.Length; i++)
            {
                if (assets[i].name.Equals(assetName))
                {
                    return assets[i] as T;
                }
            }

            return null;
        }

        #endregion

        #region Local Method
        #endregion
    }
}