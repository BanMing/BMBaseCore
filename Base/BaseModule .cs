/******************************************************************
** BaseModule.cs
** @Author       : BanMing 
** @Date         : 2020/9/27 下午6:41:05
** @Description  : 基础模块
*******************************************************************/

namespace BMBaseCore
{
    public abstract class BaseModule : BaseObject
    {
        /// <summary>
        /// 获取游戏框架模块优先级
        /// 轮询先后
        /// </summary>
        internal virtual int Priority { get { return 0; } }

        /// <summary>
        /// 游戏框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位</param>
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 关闭并清理游戏框架模块
        /// </summary>
        internal abstract void Shutdown();
    }
}