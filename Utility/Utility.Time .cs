/******************************************************************
** Utility.Time.cs
** @Author       : BanMing 
** @Date         : 2020/11/6 pm 3:19:34
** @Description  : 
*******************************************************************/

using System;
namespace BMBaseCore
{
    public static partial class Utility
    {
        public static class Time
        {
            private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

            public static long ClientNow()
            {
                return (DateTime.UtcNow.Ticks - epoch) / 10000L;
            }

            public static long ClientNowSeconds()
            {
                return (DateTime.UtcNow.Ticks - epoch) / 10000000L;
            }
        }
    }
}