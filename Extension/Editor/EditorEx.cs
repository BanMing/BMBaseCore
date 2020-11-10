/******************************************************************
** @File         : EditorEx.cs
** @Author       : BanMing 
** @Date         : 11/8/2020 9:14:29 PM
** @Description  :  
*******************************************************************/

using UnityEditor;
using UnityEngine;

namespace BMBaseCore
{
    public static class EditorEx
    {
        public static BuildTarget TranslateToRuntimePlatform(this RuntimePlatform p)
        {
            switch (p)
            {
                case RuntimePlatform.Android:
                    return UnityEditor.BuildTarget.Android;

                case RuntimePlatform.IPhonePlayer:
                    return UnityEditor.BuildTarget.iOS;

                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return UnityEditor.BuildTarget.StandaloneOSX;

                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return UnityEditor.BuildTarget.StandaloneWindows;

                default:
                    return UnityEditor.BuildTarget.StandaloneWindows;
            }
        }

        public static RuntimePlatform TranslateToBuildTarget(this BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneOSX:
                    return RuntimePlatform.OSXPlayer;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return RuntimePlatform.WindowsPlayer;
                case BuildTarget.iOS:
                    return RuntimePlatform.IPhonePlayer;
                case BuildTarget.Android:
                    return RuntimePlatform.Android;
                default:
                    return RuntimePlatform.WindowsPlayer;
            }
        }
    }
}
