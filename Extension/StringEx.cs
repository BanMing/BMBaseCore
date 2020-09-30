/******************************************************************
** StringEx.cs
** @Author       : BanMing 
** @Date         : 9/28/2020 5:40:01 PM
** @Description  : 
*******************************************************************/

using System;
using System.Collections.Generic;

namespace BMBaseCore
{

    public static class StringEx
    {
        public static int IntValue(this string s)
        {
            int result;
            if (int.TryParse(s, out result))
            {
                return result;
            }

            return 0;
        }

        public static uint UIntValue(this string s)
        {
            uint result;
            if (uint.TryParse(s, out result))
            {
                return result;
            }

            return 0;
        }

        public static long Int64Value(this string s)
        {
            long result;
            if (long.TryParse(s, out result))
            {
                return result;
            }

            return 0;
        }

        public static string ToFirstLower(this string s)
        {
            return Utility.Text.Format("{0}{1}", char.ToLower(s[0]), s.Substring(1));
        }

        public static string ToFirstUpper(this string s)
        {
            return Utility.Text.Format("{0}{1}", char.ToUpper(s[0]), s.Substring(1));
        }

        /// <summary>
        /// add string to length ('0' default) ,fill left
        /// </summary>
        public static string FillLeft(this string str, int length, char c = '0')
        {
            while (str.Length < length)
            {
                str = c + str;
            }

            return str;
        }

        /// <summary>
        /// add string to length ('0' default) ,fill right
        /// </summary>
        public static string FillRight(this string str, int length, char c = '0')
        {
            while (str.Length < length)
            {
                str = str + c;
            }

            return str;
        }
    }
}
