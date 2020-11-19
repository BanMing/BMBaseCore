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

public class SplitAnimationClipEditor : EditorWindow
{
    public List<string> paths = new List<string>();


    private void OnGUI()
    {
        for (int i = 0; i < paths.Count; i++)
        {
            GUILayout.Label(paths[i]);
        }
        if (GUILayout.Button("Add"))
        {
            string folderPath = EditorUtility.OpenFolderPanel("select folder", "", "");

            int index = folderPath.IndexOf("Assets/");

            if (index == -1) { return; }

            paths.Add(folderPath.Substring(index));
            
            //_buildAbConfig.folderPaths[i] = folderPath.Substring(index);
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
