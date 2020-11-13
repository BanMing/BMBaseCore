/******************************************************************
** EditorAssetLoader.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午5:39:57
** @Description  : 
*******************************************************************/
#if UNITY_EDITOR
using System;
using UnityEditor;

namespace BMBaseCore
{
    public class EditorAssetLoader : BaseObject, IAssetLoader
    {
        #region Attriube
        #endregion

        #region Public Method
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public T Load<T>(string path, string assetName) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            callback?.Invoke(AssetDatabase.LoadAssetAtPath<T>(path));
        }

        public void LoadAsync<T>(string path, Action<T> callback, string assetName) where T : UnityEngine.Object
        {
            callback?.Invoke(AssetDatabase.LoadAssetAtPath<T>(path));
        }

        public void Release()
        {

        }

        #endregion

        #region Local Method

        #endregion

    }

}
#endif
