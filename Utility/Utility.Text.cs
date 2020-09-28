/******************************************************************
** NewBehaviourScript1.cs
** @Author       : BanMing 
** @Date         : 9/28/2020 3:07:30 PM
** @Description  : 
*******************************************************************/

using System;
using System.Text;

namespace BMBaseCore
{
    public static partial class Utility
    {
        public static class Text
        {
            [ThreadStatic]
            private static StringBuilder s_CacheSb = null;

            public static string Format(string format, params object[] args)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CacheSb.Clear();
                s_CacheSb.Length = 0;
                s_CacheSb.AppendFormat(format, args);
                return s_CacheSb.ToString();
            }

            private static void CheckCachedStringBuilder()
            {
                if (s_CacheSb == null)
                {
                    s_CacheSb = new StringBuilder();
                }
            }

        }
    }
}