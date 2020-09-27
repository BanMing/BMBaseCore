/******************************************************************
** AssetBundleLoader.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午5:43:59
** @Description  : 
*******************************************************************/

using UnityEngine;
using System.Collections;
using System;
using System.Threading.Tasks;

namespace BMBaseCore
{
    // to do : take a load queue 
    public class AssetBundleLoader : IAssetLoader
    {
        #region Attriube

        //private Queue<>
        #endregion

        #region Public Method

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            if (ab == null)
            {
                Debug.LogError($"there don`t exits assetbundle in {path}");
                return null;
            }

            T[] assets = ab.LoadAllAssets<T>();
            if (assets == null || assets.Length < 1)
            {
                Debug.LogError($"there don`t exits assrt in assetbundle where {path}");
                return null;
            }

            return assets[0];
        }

        public void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            Coroutine coroutine = CoroutineTool.Instance.StartCoroutine(loadAsync<T>(path, null));
        }

        #endregion

        #region Local Method

        private IEnumerator loadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            AssetBundleCreateRequest abCreateReq = AssetBundle.LoadFromFileAsync(path);
            yield return abCreateReq.isDone;

            if (abCreateReq.assetBundle == null)
            {
                Debug.LogError($"there don`t exits assetbundle in {path}");
                yield break;
            }

            AssetBundleRequest abReq = abCreateReq.assetBundle.LoadAllAssetsAsync<T>();
            yield return abReq.isDone;

            if (abReq.asset == null)
            {
                Debug.LogError($"there don`t exits assrt in assetbundle where {path}");
                yield break;
            }

            callback?.Invoke(abReq.asset as T);
        }


        #endregion

    }
}