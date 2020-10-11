/******************************************************************
** GameLog.ILogHelper.cs
** @Author       : BanMing 
** @Date         : 9/30/2020 5:29:46 PM
** @Description  : 
*******************************************************************/


namespace BMBaseCore
{
    public static partial class GameLog
    {
        /// <summary>
        /// 游戏日志辅助器日志
        /// </summary>
        public interface ILogHelper
        {
            /// <summary>
            /// 记录日志
            /// </summary>
            /// <param name="level">日志等级</param>
            /// <param name="message">日志内容</param>
            void Log(GameLogLevel level, object message);

            /// <summary>
            /// 将日志内容写入本地文件
            /// </summary>
            /// <param name="msg"></param>
            void WriteLogToFile(string msg);
        }

    }
}
