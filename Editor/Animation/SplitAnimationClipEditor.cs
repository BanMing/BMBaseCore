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
        private const string ANIMATION_CLIP_ACCRACY = "f3";

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
            EditorUtility.SetDirty(_config);
        }

        #region GUI

        private void OnGUI()
        {
            windowWith9 = position.width / 9;

            DrawSourcePaths();

            if (GUILayout.Button("Split All"))
            {
                Split();
            }
        }

        private void DrawSourcePaths()
        {
            GUILayout.Space(10);

            for (int i = 0; i < _config.configs.Count; i++)
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.Label($"Animation Clip Path {i + 1}");
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label($"sourceFolderPath :", GUILayout.Width(windowWith9));
                        GUILayout.Box(_config.configs[i].sourceFolderPath, GUILayout.Width(5.5f * windowWith9));
                        if (GUILayout.Button("...", GUILayout.Width(windowWith9)))
                        {
                            string folderPath = EditorUtility.OpenFolderPanel("select folder", "", "");

                            int index = folderPath.IndexOf("Assets/");

                            if (index == -1) { return; }

                            //_config.sourceFolders[i] = folderPath.Substring(index);
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label($"sourceFolderPath :");
                        GUILayout.Box(_config.configs[i].sourceFolderPath, GUILayout.Width(5.5f * windowWith9));
                        if (GUILayout.Button("...", GUILayout.Width(windowWith9)))
                        {
                            string folderPath = EditorUtility.OpenFolderPanel("select folder", "", "");

                            int index = folderPath.IndexOf("Assets/");

                            if (index == -1) { return; }

                            //_config.sourceFolders[i] = folderPath.Substring(index);
                        }
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("del"))
                    {
                        //_config.sourceFolders.RemoveAt(i);
                    }
                }
                GUILayout.EndVertical();
            }

            GUILayout.Space(20);


            if (GUILayout.Button("Add Source Path"))
            {
                //_config.sourceFolders.Add(string.Empty);
            }

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

            //string[] files = Directory.GetFiles(Path.GetFullPath(_config.sourceFolders[0]));


            //for (int i = 0; i < files.Length; i++)
            //{
            //    if (files[i].EndsWith(".meta"))
            //    {
            //        continue;
            //    }

            //    string path = files[i].Substring(files[i].IndexOf(@"Assets\"));
            //    Debug.Log(path);

            //    AnimationClip animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            //    AnimationClip singleClip = CopyAnimationClip(animationClip);

            //    string clipName = Path.GetFileNameWithoutExtension(path);
            //    //AssetDatabase.CreateAsset(singleClip, _config.targetTopFolder + "/" + animationClip.name + ".anim");
            //}

        }

        private AnimationClip CopyAnimationClip(AnimationClip sourceClip)
        {
            AnimationClip newClip = new AnimationClip();
            var editorCurves = AnimationUtility.GetCurveBindings(sourceClip);

            for (int i = 0; i < editorCurves.Length; i++)
            {
                // reduce animation accuracy 
                var animationCurve = AnimationUtility.GetEditorCurve(sourceClip, editorCurves[i]);
                for (int j = 0; j < animationCurve.keys.Length; j++)
                {
                    var key = animationCurve.keys[j];
                    key.value = float.Parse(key.value.ToString(ANIMATION_CLIP_ACCRACY));
                    key.inTangent = float.Parse(key.inTangent.ToString(ANIMATION_CLIP_ACCRACY));
                    key.outTangent = float.Parse(key.outTangent.ToString(ANIMATION_CLIP_ACCRACY));
                }

                AnimationUtility.SetEditorCurve(newClip, editorCurves[i], animationCurve);
            }

            return newClip;
        }

    }
}