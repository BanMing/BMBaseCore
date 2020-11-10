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
        private static StringBuilder s_CacheSb = null;

        public static string ToItemString<T>(this List<T> list)
        {
            if (s_CacheSb == null)
            {
                s_CacheSb = new StringBuilder();
            }
            s_CacheSb.Clear();
            s_CacheSb.Length = 0;

            for (int i = 0; i < list.Count; i++)
            {
                s_CacheSb.AppendLine($"[{i}] = {list[i].ToString()}");
            }

            return s_CacheSb.ToString();
        }
    }
}
