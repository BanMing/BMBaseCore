/******************************************************************
** AssetBundleLoader.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午5:43:59
** @Description  : 
*******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMBaseCore
{
    // to do : take a load queue 
    public class AssetBundleLoader : BaseObject, IAssetLoader
    {
        #region Load Task
        public class AsyncLoadTask
        {
            public Coroutine coroutine;

            public Action<UnityEngine.Object> callback;
        }

        #endregion

        #region Attriube

        private AssetBundleManifest _bundleManifest;

        private Dictionary<int, AsyncLoadTask> _loadingAsset;

        private Dictionary<int, AssetBundleInfo> _curAllBundleInfos;

        #endregion

        #region Life Cycle

        /// <summary>
        /// step 1. initialize attriube
        /// step 2. load file list (hotfix)
        /// step 3. load manifest
        /// </summary>
        public AssetBundleLoader()
        {
            base.Init();

            _loadingAsset = new Dictionary<int, AsyncLoadTask>();
            _curAllBundleInfos = new Dictionary<int, AssetBundleInfo>();

            LoadFileList();

            LoadAssetBundleManifest();
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_loadingAsset != null)
            {
                while (_loadingAsset.GetEnumerator().MoveNext())
                {
                    CoroutineTool.Instance.StopCoroutine(_loadingAsset.GetEnumerator().Current.Value.coroutine);
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

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return Load<T>(path, null);
        }

        /// <summary>
        /// load asset sync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T Load<T>(string path, string assetName) where T : UnityEngine.Object
        {
            int pathHashCode = path.GetHashCode();

            if (_loadingAsset.ContainsKey(pathHashCode))
            {
                //CoroutineTool.Instance.StopCoroutine(_loadingAsset[pathHashCode]);
                //_loadingAsset.Remove(pathHashCode);
                Debug.LogError("load assetbundle path is loading async!not loading by sync!");
                return null;
            }


            AssetBundleInfo bundleInfo = LoadAssetBundleInfo<T>(path);
            if (bundleInfo == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                return bundleInfo.As<T>();
            }
            else
            {
                return bundleInfo.GetOneAsset<T>(assetName);
            }
        }


        public void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            LoadAsync<T>(path, callback, null);
        }

        public void LoadAsync<T>(string path, Action<T> callback, string assetName) where T : UnityEngine.Object
        {
            if (_loadingAsset.ContainsKey(path.GetHashCode()))
            {
                _loadingAsset[path.GetHashCode()].callback += (Action<UnityEngine.Object>)callback;
                return;
            }

            AsyncLoadTask asyncLoadTask = new AsyncLoadTask();
            asyncLoadTask.callback += (Action<UnityEngine.Object>)callback;

            Coroutine coroutine = CoroutineTool.Instance.StartCoroutine(LoadAsync<T>(path, assetName, asyncLoadTask));
            asyncLoadTask.coroutine = coroutine;

            //_loadingAsset.Add(path.GetHashCode(), coroutine);
        }


        public void Release()
        {
            using (var e = _curAllBundleInfos.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    AssetBundleInfo assetBundleInfo = e.Current.Value;
                    if (assetBundleInfo.refCount < 1)
                    {
                        assetBundleInfo.Destroy();
                        _curAllBundleInfos.Remove(e.Current.Key);
                    }
                }
            }
        }

        #endregion

        #region Local Init Method

        // step 2
        private void LoadFileList()
        {
            GameManager gameManager = GameManager.Instance;
            HTTPTool.GetText(PathConst.FileListPath, (text) =>
            {
                string[] strs = text.Split('\n');
                gameManager.SetGameVersion(strs[0]);
            });
        }

        // step 3
        private void LoadAssetBundleManifest()
        {
            HTTPTool.GetAssetBundle(PathConst.AssetBundleManifestPath, ab =>
            {
                if (ab == null)
                {
                    Debug.LogError($"mainfest is null in {PathConst.AssetBundleManifestPath}");
                }

                _bundleManifest = ab.LoadAllAssets<AssetBundleManifest>()?[0];
                if (_bundleManifest == null)
                {
                    Debug.LogError($"mainfest asset is null in {PathConst.AssetBundleManifestPath}");
                }
            });
        }

        #endregion

        #region Local Load Method
        /// <summary>
        /// load one asset bundle by sync
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private AssetBundleInfo LoadAssetBundleInfo<T>(string path) where T : UnityEngine.Object
        {
            int pathHashCode = path.GetHashCode();

            // cache 
            AssetBundleInfo assetBundleInfo = null;
            if (_curAllBundleInfos.TryGetValue(pathHashCode, out assetBundleInfo))
            {
                return assetBundleInfo;
            }

            AssetBundleInfo bundleInfo = new AssetBundleInfo() { pathHash = pathHashCode };

            string[] dependecies = _bundleManifest.GetAllDependencies(path);
            bundleInfo.dependecies = new AssetBundleInfo[dependecies.Length];

            for (int i = 0; i < dependecies.Length; i++)
            {
                bundleInfo.dependecies[i] = LoadAssetBundleInfo<T>(dependecies[i]);
            }

            AssetBundle ab = AssetBundle.LoadFromFile(path);
            if (ab == null)
            {
                Debug.LogError($"there don`t exits assetbundle in {path}");
                return null;
            }

            bundleInfo.bundle = ab;

            T[] assets = bundleInfo.bundle.LoadAllAssets<T>();
            bundleInfo.assets = assets;
            if (assets == null || assets.Length < 1)
            {
                Debug.LogError($"there don`t exits assrt in assetbundle where {path}");
                return null;
            }

            _curAllBundleInfos.Add(pathHashCode, bundleInfo);
            return bundleInfo;
        }

        private IEnumerator LoadAsync<T>(string path, string assetName, AsyncLoadTask asyncLoadTask) where T : UnityEngine.Object
        {
            yield return LoadOneAsync<T>(path);

            AssetBundleInfo assetBundleInfo;
            if (_curAllBundleInfos.TryGetValue(path.GetHashCode(), out assetBundleInfo))
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    asyncLoadTask.callback?.Invoke(assetBundleInfo.As<T>());
                }
                else
                {
                    asyncLoadTask.callback?.Invoke(assetBundleInfo.GetOneAsset<T>(assetName));
                }
            }
            else
            {
                asyncLoadTask.callback?.Invoke(null);
            }

            _loadingAsset.Remove(path.GetHashCode());
        }

        private IEnumerator LoadOneAsync<T>(string path)
        {
            AssetBundleInfo bundleInfo = new AssetBundleInfo() { pathHash = path.GetHashCode() };

            string[] dependecies = _bundleManifest.GetAllDependencies(path);
            bundleInfo.dependecies = new AssetBundleInfo[dependecies.Length];

            for (int i = 0; i < dependecies.Length; i++)
            {
                yield return LoadOneAsync<T>(path);
                bundleInfo.dependecies[i] = _curAllBundleInfos[path.GetHashCode()];
            }

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
            bundleInfo.assets = abReq.allAssets;

            _curAllBundleInfos.Add(bundleInfo.pathHash, bundleInfo);
        }

        #endregion

    }
}