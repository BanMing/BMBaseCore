/******************************************************************
** @File         : ToolbarEx.cs
** @Author       : BanMing 
** @Date         : 11/24/2020 8:41:34 AM
** @Description  :  
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BMBaseCore
{
    [InitializeOnLoad]
    public static class ToolbarEx
    {
        private static int s_toolCount;
        private static GUIStyle s_commandStyle = null;

        public static readonly List<Action> LeftToolbarGUI = new List<Action>();
        public static readonly List<Action> RightToolbarGUI = new List<Action>();

        static ToolbarEx()
        {
            Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");

#if UNITY_2019_1_OR_NEWER
            string fieldName = "k_ToolCount";
#else
            string fieldName = "s_ShownToolIcons";
#endif

            FieldInfo toolIcons = toolbarType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

#if UNITY_2019_1_OR_NEWER
            s_toolCount = toolIcons != null ? ((int)toolIcons.GetValue(null)) : 7;
#elif UNITY_2018_1_OR_NEWER
            s_toolCount = toolIcons != null ? ((Array) toolIcons.GetValue(null)).Length : 6;
#else
            s_toolCount = toolIcons != null ? ((Array) toolIcons.GetValue(null)).Length : 5;
#endif

            ToolbarCallback.OnToolbarGUI -= OnGUI;
            ToolbarCallback.OnToolbarGUI += OnGUI;
        }

        private static void OnGUI()
        {

            //根据需求，这里面的style跟Rect可以随便改
            // Create two containers, left and right
            // Screen is whole toolbar

            if (s_commandStyle == null)
            {
                s_commandStyle = new GUIStyle("CommandLeft");
            }

            var screenWidth = EditorGUIUtility.currentViewWidth;

            // Following calculations match code reflected from Toolbar.OldOnGUI()
            float playButtonsPosition = (screenWidth - 100) / 2;

            Rect leftRect = new Rect(0, 0, screenWidth, Screen.height);
            leftRect.xMin += 10; // Spacing left
            leftRect.xMin += 32 * s_toolCount; // Tool buttons
            leftRect.xMin += 20; // Spacing between tools and pivot
            leftRect.xMin += 64 * 2; // Pivot buttons
            leftRect.xMax = playButtonsPosition;

            Rect rightRect = new Rect(0, 0, screenWidth, Screen.height);
            rightRect.xMin = playButtonsPosition;
            rightRect.xMin += s_commandStyle.fixedWidth * 3; // Play buttons
            rightRect.xMax = screenWidth;
            rightRect.xMax -= 10; // Spacing right
            rightRect.xMax -= 80; // Layout
            rightRect.xMax -= 10; // Spacing between layout and layers
            rightRect.xMax -= 80; // Layers
            rightRect.xMax -= 20; // Spacing between layers and account
            rightRect.xMax -= 80; // Account
            rightRect.xMax -= 10; // Spacing between account and cloud
            rightRect.xMax -= 32; // Cloud
            rightRect.xMax -= 10; // Spacing between cloud and collab
            rightRect.xMax -= 78; // Colab

            // Add spacing around existing controls
            leftRect.xMin += 10;
            leftRect.xMax -= 10;
            rightRect.xMin += 10;
            rightRect.xMax -= 10;

            // Add top and bottom margins
            leftRect.y = 5;
            leftRect.height = 24;
            rightRect.y = 5;
            rightRect.height = 24;

            if (leftRect.width > 0)
            {
                GUILayout.BeginArea(leftRect);
                GUILayout.BeginHorizontal();
                foreach (var handler in LeftToolbarGUI)
                {
                    handler();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }

            if (rightRect.width > 0)
            {
                GUILayout.BeginArea(rightRect);
                GUILayout.BeginHorizontal();
                foreach (var handler in RightToolbarGUI)
                {
                    handler();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }
    }
}
