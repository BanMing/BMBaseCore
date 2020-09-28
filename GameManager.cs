/******************************************************************
** GameManager.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午4:17:05
** @Description  : Game Loop Center
*******************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace BMBaseCore
{
    public class GameManager : SingletonMono<GameManager>
    {
        #region Attriube

        private List<BaseModule> _modules;

        #endregion

        #region Property

        public GameConfig GameConfig { get; private set; }

        #endregion

        #region Unity Event

        private void Awake()
        {


        }

        private void OnDestroy()
        {
            destroyModules();
            CoroutineTool.Instance.StopAllCoroutines();
        }

        #endregion

        #region Public Method

        public override void Init()
        {
            base.Init();
            initModules();
        }

        #endregion

        #region Local Method

        /// <summary>
        /// init all modules
        /// </summary>
        private void initModules()
        {
            if (_modules != null)
            {
                destroyModules();
            }
            _modules = new List<BaseModule>();

            // 1. asset
            AssetModule assetModule = new AssetModule();
            _modules.Add(assetModule);

            // 2. config
            GameConfig = assetModule.Load<GameConfig>(GameConfig.AssetPath);

        }

        /// <summary>
        /// unload all modules
        /// </summary>
        private void destroyModules()
        {
            if (_modules == null)
            {
                return;
            }

            for (int i = 0; i < _modules.Count; i++)
            {
                _modules[i].Destroy();
            }

            _modules.Clear();
            _modules = null;
        }

        #endregion
    }
}