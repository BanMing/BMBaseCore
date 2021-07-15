/******************************************************************
** BaseMonoObject.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午6:44:47
** @Description  : 
*******************************************************************/

using System;
using System.Diagnostics;
using UnityEngine;

namespace BMBaseCore
{
    public class BaseMonoObject : MonoBehaviour
    {
        #region Attriube

        /// <summary>
        /// 类型名
        /// </summary>
        [NonSerialized]
        private string _typeName;

        /// <summary>
        /// 是否启用日志
        /// </summary>
        [NonSerialized]
        protected bool m_logEnable;

        #endregion

        #region Property

        /// <summary>
        /// 类型名
        /// </summary>
        internal string TypeName
        {
            get
            {
                if (_typeName == null)
                {
                    _typeName = this.GetType().Name;
                }
                return _typeName;
            }
        }

        /// <summary>
        /// 是否启用日志
        /// </summary>
        internal virtual bool IsLogEnable
        {
            get
            {
                return m_logEnable;
            }
        }

        #endregion

        #region Life Cycle

        public virtual void Init() { }

        public virtual void Destroy()
        {
            _typeName = null;
        }

        #endregion

        #region Log Method

        /// <summary>
        /// 日志打印
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [Conditional("ENABLE_LOG")]
        protected void Log(string format, params object[] args)
        {
            if (IsLogEnable)
            {
                string newFormat = Utility.Text.Format("{0} : {1}", TypeName, format);
                BMBaseCore.Log.Info(Utility.Text.Format(newFormat, args));
            }
        }


        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        protected void LogError(string format, params object[] args)
        {
            string newFormat = Utility.Text.Format("{0} : {1}", TypeName, format);
            BMBaseCore.Log.Error(Utility.Text.Format(newFormat, args));
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [Conditional("ENABLE_LOG")]
        protected void LogWarning(string format, params object[] args)
        {
            string newFormat = Utility.Text.Format("{0} : {1}", TypeName, format);
            BMBaseCore.Log.Warning(Utility.Text.Format(newFormat, args));
        }

        /// <summary>
        /// 写日志到本地文件
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        [Conditional("WRITE_LOG_TO_FILE")]
        protected void WirteLog(string format, params object[] args)
        {
            string newFormat = Utility.Text.Format("{0} : {1}", TypeName, format);
            BMBaseCore.Log.WriteLogToFile(Utility.Text.Format(newFormat, args));
        }


        #endregion

    }
}