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

        private float windowWith8;

        private SplitAnimationClipConfig _config;

        private void OnEnable()
        {
            _config = SplitAnimationClipConfig.Load();
            windowWith8 = position.width / 8;
        }

        private void OnDestroy()
        {
            _config.CheckFolderPath();
            EditorUtility.SetDirty(_config);
        }

        #region GUI

        private void OnGUI()
        {
            windowWith8 = position.width / 9;

            DrawSourcePaths();

            if (GUILayout.Button("Split All"))
            {
                SplitAll();
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

                    DrawOneFolder("source", ref _config.configs[i].sourceFolderPath);
                    DrawOneFolder("target", ref _config.configs[i].targetFolderPath);

                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("del"))
                        {
                            _config.DelOne(i);
                        }
                        if (GUILayout.Button("split one"))
                        {
                            SplitOnePath(_config.configs[i].sourceFolderPath, _config.configs[i].targetFolderPath);
                            AssetDatabase.Refresh();
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }

            GUILayout.Space(20);


            if (GUILayout.Button("Add Config Path"))
            {
                _config.AddEmptyOne();
            }

            GUILayout.Space(10);
        }

        private void DrawOneFolder(string tag, ref string path)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label($"{tag} :", GUILayout.Width(windowWith8));
                GUILayout.Box(path, GUILayout.Width(6.5f * windowWith8));
                if (GUILayout.Button("...", GUILayout.Width(windowWith8)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel($"select {tag} folder", "", "");

                    int index = folderPath.IndexOf(PathConst.AssetsPerfix);

                    if (index == -1) { return; }

                    path = folderPath.Substring(index);
                }
            }
            GUILayout.EndHorizontal();
        }

        #endregion

        private void SplitOnePath(string sourcePath, string targetPath)
        {
            string fullPath = Path.GetFullPath(sourcePath);
            string[] files = Directory.GetFiles(fullPath);

            string targetFullPath = Path.GetFullPath(targetPath);
            if (!Directory.Exists(targetFullPath))
            {
                Directory.CreateDirectory(targetFullPath);
            }

            Debug.Log($"targetFullPath:{targetFullPath}");
            string[] subDirs = Directory.GetDirectories(fullPath);
            for (int i = 0; i < subDirs.Length; i++)
            {
                string subTargetPath = Path.Combine(targetPath, Path.GetFileName(subDirs[i])).Replace("/", "\\");
                Debug.Log($"subTargetPath:{subTargetPath} | targetPath:{targetPath}");

                SplitOnePath(subDirs[i], subTargetPath);
            }

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

                string clipName = Path.GetFileNameWithoutExtension(path);
                string newClipPath = string.Format("{0}\\{1}.anim", targetPath, clipName).Replace("/", "\\");
                newClipPath = newClipPath.Substring(newClipPath.IndexOf(@"Assets\"));
                Debug.Log($"newClipPath :{newClipPath}");

                AssetDatabase.CreateAsset(singleClip, newClipPath);
            }
        }


        private void SplitAll()
        {

            if (!_config.CheckFolderPath())
            {
                EditorUtility.DisplayDialog("Error", "Config is not right", "got it");
                return;
            }

            for (int i = 0; i < _config.configs.Count; i++)
            {
                SplitOnePath(_config.configs[i].sourceFolderPath, _config.configs[i].targetFolderPath);
            }

            AssetDatabase.Refresh();
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