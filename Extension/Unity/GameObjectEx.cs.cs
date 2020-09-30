/******************************************************************
** GameObjectEx.cs
** @Author       : BanMing
** @Date         : 2020/9/27 下午4:17:05
** @Description  :
*******************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace BMBaseCore
{
    public static class GameObjectEx
    {
        private static List<Transform> s_CacheTransforms = new List<Transform>();

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        /// <summary>
        /// cheack gameobject is in scene
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static bool InScene(this GameObject gameObject)
        {
            return gameObject.scene.name != null;
        }

        /// <summary>
        /// set gameobject layer by recurisvely
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="layer"></param>
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.GetComponentsInChildren(true, s_CacheTransforms);

            for (int i = 0; i < s_CacheTransforms.Count; i++)
            {
                s_CacheTransforms[i].gameObject.layer = layer;
            }

            s_CacheTransforms.Clear();
        }
    }
}