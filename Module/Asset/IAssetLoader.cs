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
        T Load<T>(string path) where T : UnityEngine.Object;
        T Load<T>(string path, string assetName) where T : UnityEngine.Object;

        void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object;
        void LoadAsync<T>(string path, Action<T> callback, string assetName) where T : UnityEngine.Object;

        void Release();
    }
}