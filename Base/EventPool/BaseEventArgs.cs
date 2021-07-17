using System;

namespace BMBaseCore
{
    /// <summary>
    /// 事件基类。
    /// </summary>
    public abstract class BaseEventArgs : EventArgs, IReference
    {
        /// <summary>
        /// 获取类型编号。
        /// </summary>
        public abstract int ID
        {
            get;
        }

        /// <summary>
        /// 清理引用
        /// </summary>
        public abstract void Clear();
    }
}
