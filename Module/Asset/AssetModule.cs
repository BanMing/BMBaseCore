/******************************************************************
** AssetModule.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午6:33:31
** @Description  : 
*******************************************************************/

using UnityEngine;
using System.Collections;
using System;

namespace BMBaseCore
{
    public class AssetModule : IAssetLoader
    {
        #region Attriube

        private IAssetLoader _assetLoader;

        #endregion

        #region Public Method

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            throw new NotImplementedException();
        }

        public void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Local Method
        #endregion
    }
}