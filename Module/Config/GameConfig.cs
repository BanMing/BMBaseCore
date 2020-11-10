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

        public string Version { get; private set; }

        #endregion

        #region Public Method
        public void Init()
        {
            //Version = Utility.Text.Format("{0}.{1}.{2}", version1, version2, version3);
        }
        #endregion
    }
}