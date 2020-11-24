/******************************************************************
** @File         : ToolbarCallback.cs
** @Author       : BanMing 
** @Date         : 11/24/2020 8:18:02 AM
** @Description  :  
*******************************************************************/

using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif

namespace BMBaseCore
{
    public static class ToolbarCallback
    {
        private static Type s_toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");

        private static Type s_guiViewType = typeof(Editor).Assembly.GetType("UnityEditor.GUIView");

        private static PropertyInfo s_viewVisualTree = s_guiViewType.GetProperty("visualTree", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private static FieldInfo s_imguiContainerOnGui = typeof(IMGUIContainer).GetField("m_OnGUIHandler", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private static ScriptableObject s_currentToolbar;

        /// <summary>
        /// Callback for toolbar OnGUI method.
        /// </summary>
        public static Action OnToolbarGUI;

        static ToolbarCallback()
        {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            // Relying on the fact that toolbar is ScriptableObject and gets deleted when layout changes
            if (s_currentToolbar != null) { return; }

            // Find toolbar
            var toolbars = Resources.FindObjectsOfTypeAll(s_toolbarType);
            s_currentToolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;

            if (s_currentToolbar == null) { return; }

            // Get it`s visual tree
            var visualTree = (VisualElement)s_viewVisualTree.GetValue(s_currentToolbar, null);

            // Get first child which 'happens' to be toolbar IMGUIContainer
            var container = (IMGUIContainer)visualTree[0];

            // (Re)attach handler
            var handler = (Action)s_imguiContainerOnGui.GetValue(container);
            handler -= OnGUI;
            handler += OnGUI;
            s_imguiContainerOnGui.SetValue(container, handler);
        }

        private static void OnGUI()
        {
            OnToolbarGUI?.Invoke();
        }
    }
}
