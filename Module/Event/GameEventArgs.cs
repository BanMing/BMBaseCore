// ******************************************************************
// @File         : GameEventArgs.cs
// @Author       : BanMing 
// @Date         : 10/11/2020 2:30 PM
// @Description  : 
// *******************************************************************

namespace BMBaseCore
{
    /// <summary>
    /// 游戏逻辑事件基类。
    /// </summary>
    public abstract class GameEventArgs<T> : BaseEventArgs where T : BaseEventArgs
    {
        public BaseEventArgs GetEvent<T>()
        {
            return this;
        }

        public void Fire()
        {
        }
    }
}
