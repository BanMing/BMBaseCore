/******************************************************************
** IAssetLoader.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午5:32:36
** @Description  : interface of Asset loader
*******************************************************************/

using System;

namespace BMBaseCore
{
    public interface IAssetLoader
    {
        T Load<T>(string path, string assetName = null) where T : UnityEngine.Object;

        void LoadAsync<T>(string path, Action<T> callback, string assetName = null) where T : UnityEngine.Object;

        void Release();
    }
}