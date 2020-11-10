﻿/******************************************************************
** @File         : AssetBundleBuildEditor.cs
** @Author       : BanMing 
** @Date         : 11/4/2020 10:11:24 PM
** @Description  :  
*******************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BMBaseCore
{
    public class AssetBundleBuildEditor : EditorWindow
    {
        #region Attriube
        public const string TIPS = @"build assetbundle rules:
1.The config of Folder AssetBundle,the directory does not contain any sub directory.All assets in the folder (Folder of AssetBundle ) will build into one assetbundle.
2.except the folder of config,other assets each one will build into itself assetbundle.
3.version: big version,csharp version,resourece version.
4.big version: project version.
5.csharp version: increasing one when build a new player.
6.resource version: increasing one when build assetbuild.";

        private static AssetBundleBuildEditor s_abBuilderWindow;
        private BuildAssetBundleConfig _buildAbConfig;

        private int _selectPlatformIndex;

        #endregion

        #region Enter

        [MenuItem("Tools/Build AssetBundle")]
        private static void OpenBuildAssetBundleWindow()
        {
            s_abBuilderWindow = CreateWindow<AssetBundleBuildEditor>();
            s_abBuilderWindow._buildAbConfig = BuildAssetBundleConfig.Load();
            s_abBuilderWindow._selectPlatformIndex = Utility.Platform.GetRuntimePlatformIndex(Utility.Platform.CurrentRuntimePlatform);

            s_abBuilderWindow.Show();
        }

        #endregion

        #region OnGUI

        private void OnGUI()
        {
            EditorGUILayout.HelpBox(TIPS, MessageType.Error, true);
            GUILayout.Space(10);

            DrawFolderConfig();
            GUILayout.Space(10);

            DrawVersion();
            GUILayout.Space(10);

            DrawBuildAb();
        }

        private void DrawFolderConfig()
        {
            GUILayout.Box($"Folder AssetBundle:{_buildAbConfig.folderPaths.Count}");
            float windowWith8 = s_abBuilderWindow.position.width / 9;
            for (int i = 0; i < _buildAbConfig.folderPaths.Count; i++)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label($"Path {i + 1}", GUILayout.Width(windowWith8));
                    GUILayout.Box(_buildAbConfig.folderPaths[i], GUILayout.Width(5.5f * windowWith8));
                    if (GUILayout.Button("...", GUILayout.Width(windowWith8)))
                    {
                        string folderPath = EditorUtility.OpenFolderPanel("select folder", "", "");

                        int index = folderPath.IndexOf("Assets/");
                        if (index == -1)
                            return;

                        _buildAbConfig.folderPaths[i] = folderPath.Substring(index);
                    }
                    if (GUILayout.Button("del", GUILayout.Width(windowWith8)))
                    {
                        _buildAbConfig.folderPaths.RemoveAt(i);
                    }
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Add"))
                {
                    _buildAbConfig.folderPaths.Add(string.Empty);
                }
                if (GUILayout.Button("Save"))
                {
                    _buildAbConfig.Check();
                }
            }
            GUILayout.EndHorizontal();

        }
        private void DrawVersion()
        {
            GUILayout.Box(Utility.Text.Format("Version : {0}.{1}.{2}", _buildAbConfig.version1, _buildAbConfig.version2, _buildAbConfig.version3));

            _buildAbConfig.version1 = EditorGUILayout.IntField("Big Version:", _buildAbConfig.version1);
            _buildAbConfig.version2 = EditorGUILayout.IntField("CSharp Version:", _buildAbConfig.version2);
            _buildAbConfig.version3 = EditorGUILayout.IntField("Resource Version:", _buildAbConfig.version3);
        }

        private void DrawBuildAb()
        {
            _selectPlatformIndex = EditorGUILayout.Popup("Build Target:", _selectPlatformIndex, Utility.Platform.RuntimePlatformNames);
            if (GUILayout.Button("Build"))
            {
                _buildAbConfig.Check();

                bool isBuild = EditorUtility.DisplayDialog("tips", Utility.Text.Format("would you build assetbuild?\nplatform:{0}\nversion:{1}.{2}.{3}\nIt will delete the folder of streamingAssts in project!",
                    Utility.Platform.RuntimePlatformNames[_selectPlatformIndex], _buildAbConfig.version1, _buildAbConfig.version2, _buildAbConfig.version3), "Ok", "Cancel");

                if (!isBuild)
                {
                    return;
                }

                BuildResources(Utility.Platform.GetRuntimePlatformByIndex(_selectPlatformIndex).TranslateToRuntimePlatform());

                s_abBuilderWindow.Close();
                AssetDatabase.Refresh();

            }
        }

        #endregion

        #region build assetbuild 
        /// <summary>
        /// step 1. check dest path
        /// step 2. set build version 
        /// step 3. add all asset to AssetbundleBuild list
        /// step 4. build assetbundle
        /// step 5. save file list
        /// step 6. move assetbundle`s folder to streamingAssets Path
        /// </summary>
        /// <param name="buildTarget"></param>
        private void BuildResources(BuildTarget buildTarget)
        {
            EditorUtility.DisplayProgressBar("Start Build Resources", string.Empty, 0);

            // clean unuse assetbundle name
            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetBundle.UnloadAllAssetBundles(true);

            string destPath = CheckBuildDir(buildTarget);

            SetBuildVersion(buildTarget);

            List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
            GetAllAssetBundleBuilds(assetBundleBuilds, PathConst.ResRootUnityPath, buildTarget);

            BuildAssetBundle(buildTarget, destPath, assetBundleBuilds);

            SaveFileList(destPath);

            MoveToStreamingAssetsFolder(destPath);

            EditorUtility.ClearProgressBar();
        }

        // step 1
        private string CheckBuildDir(BuildTarget buildTarget)
        {
            EditorUtility.DisplayProgressBar("Check Build Directory", string.Empty, 0);

            //check build root folder
            if (!Directory.Exists(PathConst.AssetBundleBuildRoot))
            {
                Directory.CreateDirectory(PathConst.AssetBundleBuildRoot);
            }

            // check platform folder
            string platformPath = Utility.Text.Format("{0}{1}/", PathConst.AssetBundleBuildRoot, Utility.Platform.GetRuntimePlatformIndexName(buildTarget.TranslateToBuildTarget()));
            if (!Directory.Exists(platformPath))
            {
                Directory.CreateDirectory(platformPath);
            }

            // check version folder
            string versionPath = Utility.Text.Format("{0}{1}.{2}.{3}/", platformPath, _buildAbConfig.version1, _buildAbConfig.version2, _buildAbConfig.version3);
            if (Directory.Exists(versionPath))
            {
                Directory.Delete(versionPath, true);
            }
            Directory.CreateDirectory(versionPath);

            EditorUtility.DisplayProgressBar("Check Build Directory", string.Empty, 0);

            return versionPath;
        }

        // step 2
        private void SetBuildVersion(BuildTarget buildTarget)
        {
            EditorUtility.DisplayProgressBar("Set Build Version", string.Empty, 0);

            if (buildTarget == BuildTarget.Android)
            {
                PlayerSettings.Android.bundleVersionCode = _buildAbConfig.version2;
            }

            //string gameMgrPerfabPath = "Assets/Res/Perfabs/GameManager.prefab";
            //GameManager gameManager = AssetDatabase.LoadAssetAtPath<GameManager>(gameMgrPerfabPath);

            //gameManager.SetVersion(_buildAbConfig.version1, _buildAbConfig.version2, _buildAbConfig.version3);
            //Debug.Log(gameManager.GameConfig.version1);
            EditorUtility.DisplayProgressBar("Set Build Version", string.Empty, 0);
        }

        // step 3
        private void GetAllAssetBundleBuilds(List<AssetBundleBuild> assetBundleBuilds, string dirRoot, BuildTarget buildTarget)
        {
            string[] dirs = Directory.GetDirectories(dirRoot);
            for (int i = 0; i < dirs.Length; i++)
            {
                string inProjectPath = dirs[i].Substring(dirs[i].IndexOf("Assets/")).Replace("\\", "/");
                if (_buildAbConfig.folderPaths.Contains(inProjectPath))
                {
                    AddOneAbBuild(assetBundleBuilds, inProjectPath, buildTarget);
                }
                else
                {
                    GetAllAssetBundleBuilds(assetBundleBuilds, dirs[i], buildTarget);
                }
            }

            string[] files = Directory.GetFiles(dirRoot);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(".meta") || files[i].EndsWith(".DS_Store"))
                {
                    continue;
                }
                AddOneAbBuild(assetBundleBuilds, files[i].Substring(files[i].IndexOf("Assets/")), buildTarget);
            }
        }

        // step 3
        private void AddOneAbBuild(List<AssetBundleBuild> assetBundleBuilds, string inProjectPath, BuildTarget buildTarget)
        {
            //Debug.Log($"add assetbundle build :{inProjectPath}");
            EditorUtility.DisplayProgressBar("Add Asset:", inProjectPath, 0);

            AssetBundleBuild bundleBuild = new AssetBundleBuild();
            //string inProjectPath = assetPath.Substring(assetPath.IndexOf("Assets/"));
            bundleBuild.assetBundleName = TrimAssetBundleName(inProjectPath, buildTarget);
            bundleBuild.assetNames = new string[] { inProjectPath };

            assetBundleBuilds.Add(bundleBuild);

            EditorUtility.DisplayProgressBar("Add Asset:", inProjectPath, 1);
        }

        // step 3
        // Trim about starting with : "Assets/Res/test.png" or "Assets/Res/test/"
        // return "assets.res.test.png.a" or "assets.res.test.a"
        private string TrimAssetBundleName(string assetName, BuildTarget buildTarget)
        {
            assetName = assetName.Replace("\\", "/").Replace("/", ".").Replace(" ", "");
            string adExStr = Utility.Platform.GetPlatformAssetBundleEx(buildTarget.TranslateToBuildTarget());
            return Utility.Text.Format("{0}.{1}", assetName, adExStr).ToLower();
        }

        // step 4
        private void BuildAssetBundle(BuildTarget buildTarget, string destPath, List<AssetBundleBuild> assetBundleBuilds)
        {
            BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression
                                                       | BuildAssetBundleOptions.DeterministicAssetBundle
                                                       | BuildAssetBundleOptions.DisableWriteTypeTree;

            BuildPipeline.BuildAssetBundles(destPath, assetBundleBuilds.ToArray(), assetBundleOptions, buildTarget);
        }

        // step 5
        // filelist format :
        // version
        // assetbundle file name - crc32
        private void SaveFileList(string destPath)
        {
            EditorUtility.DisplayProgressBar("Save File List:", string.Empty, 0);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Utility.Text.Format("{0}.{1}.{2}", _buildAbConfig.version1, _buildAbConfig.version2, _buildAbConfig.version3));

            string[] filePaths = Directory.GetFiles(destPath, "*.a");
            for (int i = 0; i < filePaths.Length; i++)
            {
                string fileName = Path.GetFileName(filePaths[i]);

                sb.AppendLine(Utility.Text.Format("{0} {1}", fileName, CRC32Tool.GetCRC32(fileName)));

                EditorUtility.DisplayProgressBar("Save File List:", string.Empty, (i * 1.0f) / filePaths.Length);
            }

            File.WriteAllText(Path.Combine(destPath, PathConst.FileListName), sb.ToString());

            EditorUtility.DisplayProgressBar("Save File List:", string.Empty, 1);
        }

        // step 6
        private void MoveToStreamingAssetsFolder(string destPath)
        {
            EditorUtility.DisplayProgressBar("Move AssetBundle:", string.Empty, 0);

            if (Directory.Exists(PathConst.AssetBundleDir))
            {
                Directory.Delete(PathConst.AssetBundleDir, true);
            }
            Directory.CreateDirectory(PathConst.AssetBundleDir);

            string versionStr = Utility.Text.Format("{0}.{1}.{2}", _buildAbConfig.version1, _buildAbConfig.version2, _buildAbConfig.version3);
            string manifestPath = Path.Combine(destPath, versionStr);

            AssetBundle manifestBundle = AssetBundle.LoadFromFile(manifestPath);
            AssetBundleManifest assetBundleManifest = manifestBundle.LoadAsset<AssetBundleManifest>(manifestBundle.GetAllAssetNames()[0]);

            File.Copy(manifestPath, Path.Combine(PathConst.AssetBundleDir, versionStr));

            string[] abs = assetBundleManifest.GetAllAssetBundles();
            for (int i = 0; i < abs.Length; i++)
            {
                Debug.Log(abs[i]);
                EditorUtility.DisplayProgressBar("Move AssetBundle:", abs[i], 1);

                File.Copy(Path.Combine(destPath, abs[i]), Path.Combine(PathConst.AssetBundleDir, abs[i]));
            }
            EditorUtility.DisplayProgressBar("Move AssetBundle:", string.Empty, 1);
        }

        #endregion
    }
}
