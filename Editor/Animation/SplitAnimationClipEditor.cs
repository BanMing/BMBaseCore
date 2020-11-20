/******************************************************************
** SplitAnimationClipEditor.cs
** @Author       : BanMing 
** @Date         : 11/19/2020 4:56:22 PM
** @Description  : 
*******************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace BMBaseCore
{
    public class SplitAnimationClipEditor : EditorWindow
    {
        private float windowWith9;

        private SplitAnimationClipConfig _config;

        private void OnEnable()
        {
            _config = SplitAnimationClipConfig.Load();
            windowWith9 = position.width / 9;
        }

        private void OnDestroy()
        {
            _config.CheckFolderPath();
        }

        #region GUI

        private void OnGUI()
        {
            windowWith9 = position.width / 9;

            DrawTargetPath();
            DrawSourcePaths();

            if (GUILayout.Button("Split"))
            {
                Split();
            }
        }

        private void DrawSourcePaths()
        {
            GUILayout.Space(10);

            for (int i = 0; i < _config.sourceFolders.Count; i++)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label($"Source Path {i + 1}", GUILayout.Width(windowWith9));
                    GUILayout.Box(_config.sourceFolders[i], GUILayout.Width(5.5f * windowWith9));
                    if (GUILayout.Button("...", GUILayout.Width(windowWith9)))
                    {
                        string folderPath = EditorUtility.OpenFolderPanel("select folder", "", "");

                        int index = folderPath.IndexOf("Assets/");

                        if (index == -1) { return; }

                        _config.sourceFolders[i] = folderPath.Substring(index);
                    }
                    if (GUILayout.Button("del", GUILayout.Width(windowWith9)))
                    {
                        _config.sourceFolders.RemoveAt(i);
                    }
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(20);


            if (GUILayout.Button("Add Source Path"))
            {
                _config.sourceFolders.Add(string.Empty);
            }

            GUILayout.Space(10);
        }

        private void DrawTargetPath()
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label($"Target Top Path", GUILayout.Width(windowWith9));
                GUILayout.Box(_config.targetTopFolder, GUILayout.Width(5.5f * windowWith9));
                if (GUILayout.Button("...", GUILayout.Width(windowWith9 * 2)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("select folder", "", "");

                    int index = folderPath.IndexOf("Assets/");

                    if (index == -1) { return; }

                    _config.targetTopFolder = folderPath.Substring(index);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }

        #endregion


        private void Split()
        {

            if (!_config.CheckFolderPath())
            {
                EditorUtility.DisplayDialog("Error", "Config is not right","got it");
                return;
            }


        }

        private AnimationClip CopyAnimationClip(AnimationClip sourceClip)
        {
            AnimationClip newClip = new AnimationClip();
            var editorCurves = AnimationUtility.GetCurveBindings(sourceClip);

            for (int i = 0; i < editorCurves.Length; i++)
            {
                AnimationUtility.SetEditorCurve(newClip, editorCurves[i], AnimationUtility.GetEditorCurve(sourceClip, editorCurves[i]));
            }

            return newClip;
        }

    }
}