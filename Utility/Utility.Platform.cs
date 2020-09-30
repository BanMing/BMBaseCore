/******************************************************************
** Utility.Platform.cs
** @Author       : BanMing
** @Date         : 9/29/2020 9:53:46 AM
** @Description  :
*******************************************************************/

using UnityEngine;

namespace BMBaseCore
{
    public static partial class Utility
    {
        public static class Platform
        {
            public static string[] runtimePlatformNames = new string[] { "Android", "IOS", "OSX", "Windows", "Unkonwn" };
            public static string[] platformAssetBundleExs = new string[] { "a", "i", "o", "w", "u" };

            public static RuntimePlatform CurrentRuntimePlatform
            {
                get
                {
#if UNITY_STANDALONE_OSX
                    return RuntimePlatform.OSXPlayer;
#elif UNITY_STANDALONE_WIN
                    return RuntimePlatform.WindowsPlayer;
#elif UNITY_ANDROID
                    return RuntimePlatform.Android;
#elif UNITY_ANDROID
                    return RuntimePlatform.IPhonePlayer;
#else
                    return RuntimePlatform.OSXPlayer;
#endif
                }
            }

            public static bool IsMobile
            {
                get
                {
                    if (Application.platform == RuntimePlatform.Android ||
                        Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        return true;
                    }

                    return false;
                }
            }

            public static bool IsPC
            {
                get
                {
                    if (Application.platform == RuntimePlatform.WindowsPlayer ||
                        Application.platform == RuntimePlatform.OSXPlayer ||
                        Application.platform == RuntimePlatform.LinuxPlayer)
                    {
                        return true;
                    }

                    return false;
                }
            }

            public static bool IsEditor
            {
                get
                {
                    if (Application.platform == RuntimePlatform.OSXEditor ||
                        Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        return true;
                    }

                    return false;
                }
            }

            /// <summary>
            /// { "Android", "IOS", "OSX", "Windows", "Unkonwn" };
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public static int GetRuntimePlatformIndex(RuntimePlatform p)
            {
                switch (p)
                {
                    case RuntimePlatform.Android:
                        return 0;

                    case RuntimePlatform.IPhonePlayer:
                        return 1;

                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                        return 2;

                    case RuntimePlatform.WindowsPlayer:
                    case RuntimePlatform.WindowsEditor:
                        return 3;

                    default:
                        return 4;
                }
            }

            /// <summary>
            /// { "Android", "IOS", "OSX", "Windows", "Unkonwn" };
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public static string GetRuntimePlatformIndexName(RuntimePlatform p)
            {
                return runtimePlatformNames[GetRuntimePlatformIndex(p)];
            }

            /// <summary>
            /// { "a", "i", "o", "w", "u" };
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public static string GetPlatformAssetBundleEx(RuntimePlatform p)
            {
                return platformAssetBundleExs[GetRuntimePlatformIndex(p)];
            }
        }
    }
}