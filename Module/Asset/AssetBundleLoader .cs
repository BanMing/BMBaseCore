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
using System.Collections.Generic;
using UnityEngine.Networking;

namespace BMBaseCore
{
    // to do : take a load queue 
    public class AssetBundleLoader : BaseObject, IAssetLoader
    {
        #region Attriube

        private AssetBundleManifest _bundleManifest;

        private Dictionary<int, Coroutine> _loadingAsset;

        private Dictionary<int, AssetBundleInfo> _curAllBundleInfos;

        #endregion

        #region Life Cycle
        public override void Init()
        {
            base.Init();

            _loadingAsset = new Dictionary<int, Coroutine>();
            _curAllBundleInfos = new Dictionary<int, AssetBundleInfo>();

            loadAssetBundleManifest();
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_loadingAsset != null)
            {
                while (_loadingAsset.GetEnumerator().MoveNext())
                {
                    CoroutineTool.Instance.StopCoroutine(_loadingAsset.GetEnumerator().Current.Value);
                }
            }
            _loadingAsset.Clear();
            _loadingAsset = null;

            if (_curAllBundleInfos != null)
            {
                while (_curAllBundleInfos.GetEnumerator().MoveNext())
                {
                    _curAllBundleInfos.GetEnumerator().Current.Value.Destroy();
                }
            }

            _curAllBundleInfos.Clear();
            _curAllBundleInfos = null;
        }
        #endregion

        #region Public Method

        /// <summary>
        /// load asset sync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            int pathHashCode = path.GetHashCode();

            if (_loadingAsset.ContainsKey(pathHashCode))
            {
                CoroutineTool.Instance.StopCoroutine(_loadingAsset[pathHashCode]);
                _loadingAsset.Remove(pathHashCode);
            }

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

            addAssetBundleInfo(pathHashCode, ab);
            return assets[0];
        }

        public void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            Coroutine coroutine = CoroutineTool.Instance.StartCoroutine(loadAsync<T>(path, null));
            _loadingAsset.Add(path.GetHashCode(), coroutine);
        }

        #endregion

        #region Local Method

        private void loadAssetBundleManifest()
        {
            HTTPTool.GetAssetBundle(PathConst.AssetBundleManifestPath, ab =>
            {
                _bundleManifest = ab.LoadAllAssets<AssetBundleManifest>()[0];
            });
        }

        private IEnumerator loadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            AssetBundleCreateRequest abCreateReq = AssetBundle.LoadFromFileAsync(path);
            yield return abCreateReq.isDone;

            if (abCreateReq.assetBundle == null)
            {
                Debug.LogError($"there don`t exits assetbundle in {path}");
                yield break;
            }

            //abCreateReq.assetBundle.get

            AssetBundleRequest abReq = abCreateReq.assetBundle.LoadAllAssetsAsync<T>();
            yield return abReq.isDone;

            if (abReq.asset == null)
            {
                Debug.LogError($"there don`t exits assrt in assetbundle where {path}");
                yield break;
            }

            _loadingAsset.Remove(path.GetHashCode());
            addAssetBundleInfo(path.GetHashCode(), abCreateReq.assetBundle);
            callback?.Invoke(abReq.asset as T);
        }

        private void addAssetBundleInfo(int pathHash, AssetBundle assetBundle)
        {
            if (_curAllBundleInfos.ContainsKey(pathHash))
            {
                _curAllBundleInfos[pathHash].refCount++;
            }
            else
            {
                _curAllBundleInfos.Add(pathHash, new AssetBundleInfo()
                {
                    pathHash = pathHash,
                    refCount = 1,
                    bundle = assetBundle,
                });
            }
        }

        private void checkAssetBundleLoaded(int pathHash) { }

        #endregion

    }
}