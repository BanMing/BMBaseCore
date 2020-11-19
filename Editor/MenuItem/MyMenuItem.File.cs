using UnityEditor;
using UnityEngine;


namespace BMBaseCore
{
    public partial class MyMenuItem
    {
        private class File
        {
            [MenuItem("Tools/File/Delete Empty Directory In Project")]
            private static void RemoveProjectEmptyDirectory()
            {
                Utility.File.RemoveEmptyDirectory(Application.dataPath + "/");
                AssetDatabase.Refresh();
                Debug.Log("Clear Done!");
            }

            [MenuItem("Tools/Animation/Split Animation Clip Form FBX")]
            private static void OpenSplitAnimationEditor()
            {
                EditorWindow.CreateWindow<SplitAnimationClipEditor>().Show();
            }
        }
    }
}
