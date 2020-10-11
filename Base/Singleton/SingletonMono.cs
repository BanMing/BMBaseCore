//////////////////////////////////////////////////////////////////////////////////////////
//// SingletonMono.cs
//// time:2019/4/1 上午11:55:36 				
//// author:BanMing   
//// des:unity 的单列类
////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace BMBaseCore
{
    public class SingletonMono<T> : BaseMonoObject where T : BaseMonoObject
    {

        private static T instance;
        public static T Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindObjectOfType<T>();
                    if (!instance)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        instance = go.AddComponent<T>();
                        instance.Init();
                    }
                }
                return instance;
            }
        }
    }
}