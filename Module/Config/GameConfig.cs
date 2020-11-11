/******************************************************************
** GameConfig.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午4:52:27
** @Description  : config of game
*******************************************************************/

using System;
using UnityEngine;

namespace BMBaseCore
{
    [Serializable]
    public class GameConfig
    {
        #region Attriube

        public bool isAssertBundle;

        #endregion

        #region Perproty

        public string Version { get; set; }

        #endregion

        #region Public Method

        #endregion
    }
}