﻿/******************************************************************
** PathConst.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午4:17:05
** @Description  : 
*******************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BMBaseCore
{
    public class PathConst
    {

        public static string AssetBundleDir
        {
            get { return $"{Application.streamingAssetsPath}//bundles//"; }
        }

        public static string AssetBundleExtension
        {
            get

            {
                
                //if (Application.platform)
                //{

                //}
                return "";
            }
        }

        public static string AssetBundleManifestPath
        {
            get
            {
                //Application.platform.
                //string 
                return "";
            }
        }
    }
}