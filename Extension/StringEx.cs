/******************************************************************
** StringEx.cs
** @Author       : BanMing 
** @Date         : 9/28/2020 5:40:01 PM
** @Description  : 
*******************************************************************/

using System;
using System.Collections.Generic;

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
}
