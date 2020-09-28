/******************************************************************
** AssetBundleInfo.cs
** @Author       : BanMing 
** @Date         : 2020/9/28 上午11:22:55
** @Description  : 
*******************************************************************/

using UnityEngine;
using System.Collections;
using System;

namespace BMBaseCore
{
    public class AssetBundleInfo : BaseObject
    {
        #region Attriube

        public int pathHash;

        public AssetBundle bundle;

        public int refCount;

        #endregion

        #region Public Method

        public override void Destroy()
        {
            base.Destroy();

            bundle.Unload(true);
            pathHash = default(int);
            refCount = default(int);
        }

        #endregion

        #region Local Method
        #endregion
    }
}