/******************************************************************
** EventPool.cs
** @Author       : BanMing 
** @Date         : 9/30/2020 6:08:00 PM
** @Description  : 
*******************************************************************/

using System;
using System.Collections.Generic;

namespace BMBaseCore
{
    /// <summary>
    /// 事件池
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    internal sealed partial class EventPool<T> where T : class
    {
        private readonly Dictionary<int, LinkedListNode<EventHandler<T>>> _eventHandlers;
        private readonly Queue<Event> _events;
        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> _cachedNodes;
        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> _tempNodes;
        private readonly EventPoolMode _eventPoolMode;
        private EventHandler<T> _defaultHandler;

    }
}