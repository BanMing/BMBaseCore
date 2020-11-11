/******************************************************************
** AssetModule.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午6:33:31
** @Description  : 
*******************************************************************/

using System;

namespace BMBaseCore
{
    public class AssetModule : BaseModule, IAssetLoader
    {
        #region Attriube

        private IAssetLoader _assetLoader;


        #endregion

        #region Life Cycle

        public AssetModule()
        {

#if UNITY_EDITOR
            if (GameManager.Instance.GameConfig.isAssertBundle)
            {
                _assetLoader = new AssetBundleLoader();
            }
            else
            {
                _assetLoader = new EditorAssetLoader();
            }
#else
            _assetLoader = new AssetBundleLoader();
#endif

            
        }


        public override void Destroy()
        {
            base.Destroy();
            if (_assetLoader != null)
            {

                (_assetLoader as BaseObject).Destroy();
            }
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            Release();
        }

        internal override void Shutdown()
        {
            Destroy();
        }

        #endregion

        #region Public Method

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            if (!System.IO.File.Exists(path))
            {
                return null;
            }

            return _assetLoader.Load<T>(path);
        }

        public void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            if (!System.IO.File.Exists(path))
            {
                callback?.Invoke(null);
                return;
            }

            _assetLoader.LoadAsync<T>(path, callback);
        }

        public void Release()
        {
            _assetLoader?.Release();
        }

        public T Load<T>(string path, string assetName = null) where T : UnityEngine.Object
        {
            throw new NotImplementedException();
        }

        public void LoadAsync<T>(string path, string assetName = null, Action<T> callback = null) where T : UnityEngine.Object
        {
            throw new NotImplementedException();
        }

        public void LoadAsync<T>(string path, Action<T> callback, string assetName = null) where T : UnityEngine.Object
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Local Method
        #endregion
    }
}