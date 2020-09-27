/******************************************************************
** EditorAssetLoader.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午5:39:57
** @Description  : 
*******************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Threading.Tasks;

namespace BMBaseCore
{
    public class EditorAssetLoader : IAssetLoader
    {
        #region Attriube
        #endregion

        #region Public Method
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            callback?.Invoke(AssetDatabase.LoadAssetAtPath<T>(path));
        }

        #endregion

        #region Local Method

        #endregion

    }

}
