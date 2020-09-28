//////////////////////////////////////////////////////////////////////////////////////////
//// Singleton.cs
//// time:2019/4/1 下午1:02:52 				
//// author:BanMing   
//// des:不继承与mono的单例类
////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;

public class Singleton<T> where T : BaseObject
{
    public static T Instance
    {
        get { return Nest.instance; }
    }

    private class Nest
    {
        static Nest()
        {
            instance.Init();
        }
        internal static readonly T instance = Activator.CreateInstance<T>();
    }
}