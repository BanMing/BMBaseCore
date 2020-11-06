/******************************************************************
** Utility.DeviceInfo.cs
** @Author       : BanMing 
** @Date         : 2020/11/6 下午3:38:14
** @Description  : 
*******************************************************************/

using UnityEngine;
using System.Collections;
using System;

namespace BMBaseCore
{
    public static partial class Utility
    {
        public static class DeviceInfo
        {
            public static int ScreenWidth
            {
                get
                {
                    return Screen.width;
                }
            }

            public static int ScreenHeight
            {
                get
                {
                    return Screen.height;
                }
            }

            public static string DeviceIdentifier
            {
                get
                {
                    return SystemInfo.deviceUniqueIdentifier;
                }
            }

            public static string DeviceName
            {
                get
                {
                    return SystemInfo.deviceName;
                }
            }
        }
    }
}