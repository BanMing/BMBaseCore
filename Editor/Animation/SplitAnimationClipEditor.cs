/******************************************************************
** SplitAnimationClipEditor.cs
** @Author       : BanMing 
** @Date         : 11/19/2020 4:56:22 PM
** @Description  : 
*******************************************************************/

using System;
using System.IO;
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
                EditorUtility.DisplayDialog("Error", "Config is not right", "got it");
                return;
            }

            string[] files = Directory.GetFiles(Path.GetFullPath(_config.sourceFolders[0]));


            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".meta"))
                {
                    continue;
                }

                string path = files[i].Substring(files[i].IndexOf(@"Assets\"));
                Debug.Log(path);

                AnimationClip animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                AnimationClip singleClip = CopyAnimationClip(animationClip);
                AssetDatabase.CreateAsset(singleClip, _config.targetTopFolder + "/" + animationClip.name + ".anim");
            }

        }

        static private void CompressAnim()
        {
            UnityEngine.Object folder = Selection.objects[0];
            if (folder == null || folder.GetType() != typeof(DefaultAsset)) { return; }

            string folderPath = AssetDatabase.GetAssetPath(folder.GetInstanceID());
            // UnityEngine.Debug.Log(folderPath);
            folderPath = Path.GetFullPath(folderPath);
            string[] animPaths = Directory.GetFiles(folderPath, "*.anim");
            for (int j = 0; j < animPaths.Length; j++)
            {
                string animPath = animPaths[j].Substring(animPaths[j].IndexOf("Assets\\"));
                AnimationClip theAnimation = AssetDatabase.LoadAssetAtPath<AnimationClip>(animPath);
                // AnimationClip theAnimation = Selection.objects[0] as AnimationClip;
                if (theAnimation == null) return;

                //浮点数精度压缩到f3
                AnimationClipCurveData[] curves = null;
                curves = AnimationUtility.GetAllCurves(theAnimation);
                Keyframe key;
                Keyframe[] keyFrames;
                for (int ii = 0; ii < curves.Length; ++ii)
                {
                    AnimationClipCurveData curveDate = curves[ii];
                    if (curveDate.curve == null || curveDate.curve.keys == null)
                    {
                        continue;
                    }
                    keyFrames = curveDate.curve.keys;
                    for (int i = 0; i < keyFrames.Length; i++)
                    {
                        key = keyFrames[i];
                        key.value = float.Parse(key.value.ToString("f3"));
                        key.inTangent = float.Parse(key.inTangent.ToString("f3"));
                        key.outTangent = float.Parse(key.outTangent.ToString("f3"));
                        keyFrames[i] = key;
                    }
                    curveDate.curve.keys = keyFrames;
                    theAnimation.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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