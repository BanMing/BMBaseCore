/******************************************************************
** GameLogLevel.cs
** @Author       : BanMing 
** @Date         : 9/30/2020 5:31:53 PM
** @Description  : 
*******************************************************************/

namespace BMBaseCore
{

        /// <summary>
        /// 游戏日志等级
        /// </summary>
        public enum LogLevel : byte
        {
            /// <summary>
            /// 调试
            /// </summary>
            Debug = 0,

            /// <summary>
            /// 信息
            /// </summary>
            Info,

            /// <summary>
            /// 警告
            /// </summary>
            Warning,

            /// <summary>
            /// 错误
            /// </summary>
            Error,

            /// <summary>
            /// 严重错误
            /// </summary>
            Fatal
        }
}