/******************************************************************
** GameConfig.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午4:52:27
** @Description  : config of game
*******************************************************************/

using UnityEngine;
using System.Collections;
using System;

//[CreateAssetMenu(fileName = "GameConfig")]
[Serializable]
public class GameConfig : ScriptableObject
{
    #region Attriube

    public const string AssetPath = "Assets\\Res\\GameConfig.asset";

    public bool isAssertBundle;

    #endregion

    #region Public Method
    #endregion

    #region Local Method

    #endregion
}