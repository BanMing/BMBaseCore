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

        private Dictionary<Type, BaseModule> _moduleDict;

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

            InitModules();
        }

        #endregion

        #region Init Method

        /// <summary>
        /// init all modules
        /// </summary>
        private void InitModules()
        {
            if (_moduleDict != null)
            {
                DestroyModules();
            }
            _moduleDict = new Dictionary<Type, BaseModule>();

            // 1. asset
            AssetModule assetModule = new AssetModule();
            _moduleDict.Add(typeof(AssetModule), assetModule);

            // 2. config

        }


        #endregion

        #region Destory Method

        /// <summary>
        /// unload all modules
        /// </summary>
        private void DestroyModules()
        {
            if (_moduleDict == null)
            {
                return;
            }

            foreach (var item in _moduleDict)
            {
                item.Value.Destroy();
            }


            _moduleDict.Clear();
            _moduleDict = null;
        }

        #endregion

        #region Getter

        public T GetMoudle<T>() where T : BaseModule
        {
            BaseModule module;
            _moduleDict.TryGetValue(typeof(T), out module);

            return module.As<T>();
        }

        #endregion

        #region Setter

        public void SetGameVersion(string version)
        {
            _gameConfig.Version = version;
        }
        #endregion

        #region Editor Method
#if UNITY_EDITOR

#endif

        #endregion
    }
}