/******************************************************************
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

        private static AssetBundleBuildEditor s_abBuilderWindow;
        private BuildAssetBundleConfig _buildAbConfig;

        private int _selectPlatformIndex;

        private int _selectChannelIndex;

        private bool _isShowHotfixConfig;
        private bool _isShowFolderConfig;
        private bool _isShowVersionConfig;
        private bool _isShowChannelConfig;
        private bool _isShowBuildConfig;
        #endregion

        #region Enter

        [MenuItem("Tools/Build AssetBundle")]
        private static void OpenBuildAssetBundleWindow()
        {
            s_abBuilderWindow = CreateWindow<AssetBundleBuildEditor>("AssetBundle Builder");
            s_abBuilderWindow._buildAbConfig = BuildAssetBundleConfig.Load();
            s_abBuilderWindow._selectPlatformIndex = Utility.Platform.GetRuntimePlatformIndex(Utility.Platform.CurrentRuntimePlatform);

            s_abBuilderWindow.Show();
        }

        private void OnDestroy()
        {
            _buildAbConfig.TrimData();
        }

        #endregion

        #region OnGUI

        private void OnGUI()
        {
            DrawBuildRule();

            DrawHotfixConfig();

            DrawFolderConfig();

            DrawVersion();

            DrawChannels();

            DrawBuildAb();
        }

        private void DrawBuildRule()
        {
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("You must know the rules before building!", MessageType.Error, true);
            if (GUILayout.Button("read build assetbundle rules"))
            {
                Application.OpenURL(Path.GetFullPath(@"Assets\Scripts\BMBaseCore\Module\Asset\Editor\BuildAssetBundleRules.txt"));
            }
            GUILayout.Space(10);
        }

        private void DrawHotfixConfig()
        {
            if (!(_isShowHotfixConfig = EditorGUILayout.Foldout(_isShowHotfixConfig, "Hotfix Config"))) { return; }

            _buildAbConfig.isHotfix = EditorGUILayout.ToggleLeft("Is need hotfix ?", _buildAbConfig.isHotfix);

            if (_buildAbConfig.isHotfix)
            {
                _buildAbConfig.isNeedReboot = EditorGUILayout.ToggleLeft("Is need reboot after hotfix ?", _buildAbConfig.isNeedReboot);
            }

            GUILayout.Space(10);
        }

        private void DrawFolderConfig()
        {
            if (!(_isShowFolderConfig = EditorGUILayout.Foldout(_isShowFolderConfig, "Assetbundle Folder Config"))) { return; }

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

                        if (index == -1) { return; }

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
                    _buildAbConfig.CheckFolderPath();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }
        private void DrawVersion()
        {
            if (!(_isShowVersionConfig = EditorGUILayout.Foldout(_isShowVersionConfig, "Version Config"))) { return; }

            GUILayout.Box(Utility.Text.Format("Version : {0}.{1}.{2}", _buildAbConfig.version1, _buildAbConfig.version2, _buildAbConfig.version3));

            _buildAbConfig.version1 = EditorGUILayout.IntField("Big Version:", _buildAbConfig.version1);
            _buildAbConfig.version2 = EditorGUILayout.IntField("CSharp Version:", _buildAbConfig.version2);
            _buildAbConfig.version3 = EditorGUILayout.IntField("Resource Version:", _buildAbConfig.version3);

            GUILayout.Space(10);
        }

        private void DrawChannels()
        {
            if (!_buildAbConfig.isHotfix || !(_isShowChannelConfig = EditorGUILayout.Foldout(_isShowChannelConfig, "Channels Config"))) { return; }

            _selectChannelIndex = EditorGUILayout.Popup("Channels :", _selectChannelIndex, _buildAbConfig.ToChannelNameString());

            if (_selectChannelIndex >= _buildAbConfig.channels.Count)
            {
                _buildAbConfig.channels.Add(new BuildChannel() { name = "new channel" });
                s_abBuilderWindow.Repaint();
            }

            _buildAbConfig.channels[_selectChannelIndex].name = EditorGUILayout.TextField("name:", _buildAbConfig.channels[_selectChannelIndex].name);
            _buildAbConfig.channels[_selectChannelIndex].webConfigUrl = EditorGUILayout.TextField("webConfigUrl:", _buildAbConfig.channels[_selectChannelIndex].webConfigUrl);

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Add"))
                {
                    _buildAbConfig.channels.Add(new BuildChannel() { name = "new channel" });
                    _selectChannelIndex = _buildAbConfig.channels.Count - 1;
                    s_abBuilderWindow.Repaint();
                }
                if (GUILayout.Button("Check") && !_buildAbConfig.CheckChannel(_selectChannelIndex))
                {
                    EditorUtility.DisplayDialog("Eorr", "channel config eorr", "Let me see");
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
        }

        private void DrawBuildAb()
        {
            if (!(_isShowBuildConfig = EditorGUILayout.Foldout(_isShowBuildConfig, "Build Config"))) { return; }


            _buildAbConfig.isZipAb = EditorGUILayout.ToggleLeft("Compress all assetbundles into one file ?", _buildAbConfig.isZipAb);
            _buildAbConfig.isEncryptAb = EditorGUILayout.ToggleLeft("Encrypt assetbundle?", _buildAbConfig.isEncryptAb);

            _selectPlatformIndex = EditorGUILayout.Popup("Build Target:", _selectPlatformIndex, Utility.Platform.RuntimePlatformNames);
            if (GUILayout.Button("Build"))
            {
                _buildAbConfig.CheckFolderPath();

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

                sb.AppendLine(Utility.Text.Format("{0} {1}", fileName, Utility.Verifier.GetCRC32(fileName)));

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
                // Debug.Log(abs[i]);
                EditorUtility.DisplayProgressBar("Move AssetBundle:", abs[i], 1);

                File.Copy(Path.Combine(destPath, abs[i]), Path.Combine(PathConst.AssetBundleDir, abs[i]));
            }
            EditorUtility.DisplayProgressBar("Move AssetBundle:", string.Empty, 1);
        }

        #endregion
    }
}
