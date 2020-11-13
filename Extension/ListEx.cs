/******************************************************************
** ListEx.cs
** @Author       : BanMing 
** @Date         : 9/30/2020 11:26:27 AM
** @Description  : 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace BMBaseCore
{
    public static class ListEx
    {
        [ThreadStatic]
        private static StringBuilder s_CacheSb;
        private static StringBuilder S_CacheSb
        {
            get
            {
                if (s_CacheSb == null) { s_CacheSb = new StringBuilder(); }

                return s_CacheSb;
            }
        }

        public static string ToItemString<T>(this List<T> list)
        {
            S_CacheSb.Clear();
            S_CacheSb.Length = 0;

            for (int i = 0; i < list.Count; i++)
            {
                S_CacheSb.AppendLine($"[{i}] = {list[i].ToString()}");
            }

            return S_CacheSb.ToString();
        }
    }
}
