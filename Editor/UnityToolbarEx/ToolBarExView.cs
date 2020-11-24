/******************************************************************
** @File         : ToolBarExView.cs
** @Author       : BanMing 
** @Date         : 11/24/2020 8:48:39 AM
** @Description  :  
*******************************************************************/

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BMBaseCore
{
    static class ToolbarStyles
    {
        public static readonly GUIStyle ToolBarExtenderBtnStyle;

        static ToolbarStyles()
        {
            ToolBarExtenderBtnStyle = new GUIStyle("Command")
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Normal,
                fixedWidth = 60
            };
        }
    }

    [InitializeOnLoad]
    public static class ToolBarExView
    {
        static ToolBarExView()
        {
            //ToolbarEx.LeftToolbarGUI.Add(OnToolbarGUILeft);
            //ToolbarEx.RightToolbarGUI.Add(OnToolbarGUIRight);
        }

        static void OnToolbarGUILeft()
        {
            //下面只是Demo，可随便更改
            GUILayout.FlexibleSpace();
            string finalStr = "当前是：主干项目/***分支";
            GUILayout.Label(finalStr, new GUIStyle("WarningOverlay"));
            if (GUILayout.Button(new GUIContent("入口场景", "Start [startup] Scene"), ToolbarStyles.ToolBarExtenderBtnStyle))
            {
                //SceneHelper.StartScene("Assets/***.unity");
            }
        }

        static void OnToolbarGUIRight()
        {
            //下面只是Demo，可随便更改
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("SVN更新", "更新当前的客户端"), ToolbarStyles.ToolBarExtenderBtnStyle))
            {
                Debug.Log("TODO : SVN更新");
            }
            if (GUILayout.Button(new GUIContent("SVN提交", "提交客户端"), ToolbarStyles.ToolBarExtenderBtnStyle))
            {
                Debug.Log("TODO : SVN提交");
            }
            Time.timeScale = GUILayout.HorizontalSlider(Time.timeScale, 0, 10, new GUIStyle("MiniSliderHorizontal"), new GUIStyle("MinMaxHorizontalSliderThumb"), GUILayout.MinWidth(200), GUILayout.MinHeight(20));
        }
    }

}
