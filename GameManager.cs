/******************************************************************
** GameManager.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午4:17:05
** @Description  : Game Loop Center
*******************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BMBaseCore
{
    public class GameManager : SingletonMono<GameManager>
    {
        #region Attriube

        private List<BaseModule> _modules;

        [SerializeField]
        private GameConfig _gameConfig = new GameConfig();

        #endregion

        #region Property
        public GameConfig GameConfig { get { return _gameConfig; } }

        #endregion

        #region Unity Event

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            DestroyModules();
        }

        #endregion

        #region Public Method

        public override void Init()
        {
            base.Init();

            _gameConfig.Init();

            //InitModules();
        }

        #endregion

        #region Init Method

        /// <summary>
        /// init all modules
        /// </summary>
        private void InitModules()
        {
            if (_modules != null)
            {
                DestroyModules();
            }
            _modules = new List<BaseModule>();

            // 1. asset
            AssetModule assetModule = new AssetModule();
            _modules.Add(assetModule);

            // 2. config

        }
        #endregion

        #region Destory Method

        /// <summary>
        /// unload all modules
        /// </summary>
        private void DestroyModules()
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

        #region Editor Method
#if UNITY_EDITOR

#endif

        #endregion
    }
}