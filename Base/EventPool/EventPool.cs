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
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        private readonly Dictionary<int, LinkedList<EventHandler<T>>> _eventHandlers;

        private readonly Queue<Event> _events;

        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> _cachedNodes;

        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> _tempNodes;

        private readonly EventPoolMode _eventPoolMode;

        private EventHandler<T> _defaultHandler;

        /// <summary>
        /// 初始化事件池的新实例
        /// </summary>
        /// <param name="mode"></param>
        public EventPool(EventPoolMode mode)
        {
            _eventHandlers = new Dictionary<int, LinkedList<EventHandler<T>>>();
            _events = new Queue<Event>();
            _cachedNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
            _tempNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
            _eventPoolMode = mode;
            _defaultHandler = null;
        }

        /// <summary>
        /// 获取事件处理函数的数量
        /// </summary>
        public int EventHandlerCount
        {
            get { return _eventHandlers.Count; }
        }

        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int EventCount
        {
            get
            {
                return _events.Count;
            }
        }

        /// <summary>
        /// 轮询事件池
        /// </summary>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {

            while (_events.Count > 0)
            {
                Event eventNode = null;
                lock (_events)
                {
                    eventNode = _events.Dequeue();
                    HandleEvent(eventNode.Sender, eventNode.EventArgs);
                }

                ReferencePool.Release(eventNode);
            }
        }

        /// <summary>
        /// 关闭并处理事件池
        /// </summary>
        public void Shutdown()
        {
            Clear();
            _eventHandlers.Clear();
            _cachedNodes.Clear();
            _tempNodes.Clear();
            _defaultHandler = null;
        }

        /// <summary>
        /// 清理还未执行事件
        /// </summary>
        public void Clear()
        {
            lock (_events)
            {
                _events.Clear();
            }
        }

        /// <summary>
        /// 获得事件处理函数的数量
        /// </summary>
        /// <param name="id">事件id</param>
        /// <returns></returns>
        public int Count(int id)
        {
            LinkedList<EventHandler<T>> handlers = null;
            if (_eventHandlers.TryGetValue(id, out handlers))
            {
                return handlers.Count;
            }
            return 0;
        }

        /// <summary>
        /// 检查是否存在事件处理函数
        /// </summary>
        /// <param name="id">事件id</param>
        /// <param name="handler">处理函数</param>
        /// <returns></returns>
        public bool Check(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                return false;
            }

            LinkedList<EventHandler<T>> handlers = null;

            if (_eventHandlers.TryGetValue(id, out handlers))
            {
                return handlers.Contains(handler);
            }

            return false;
        }

        /// <summary>
        /// 订阅事件处理函数
        /// </summary>
        /// <param name="id">事件id</param>
        /// <param name="handler">事件处理函数</param>
        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                Log.Error("Subscribe event {0} handler is null!", id.ToString());
            }

            LinkedList<EventHandler<T>> handlers = null;
            if (!_eventHandlers.TryGetValue(id, out handlers))
            {
                handlers = new LinkedList<EventHandler<T>>();
                handlers.AddLast(handler);
                _eventHandlers.Add(id, handlers);
            }
            else if ((_eventPoolMode & EventPoolMode.AllowMultiHandler) == 0)
            {
                Log.Error(Utility.Text.Format("Event '{0}' not allow multi handler.", id.ToString()));
            }
            else if ((_eventPoolMode & EventPoolMode.AllowDuplicateHandler) == 0 && Check(id, handler))
            {
                Log.Error(Utility.Text.Format("Event '{0}' not allow duplicate handler.", id.ToString()));
            }
            else
            {
                handlers.AddLast(handler);
            }
        }

        /// <summary>
        /// 取消订阅事件处理函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        public void Unsubscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                Log.Error("Unsubscribe event {0} handler is null!", id.ToString());
            }

            LinkedList<EventHandler<T>> handlers = null;
            if (!_eventHandlers.TryGetValue(id, out handlers))
            {
                Log.Error("event {0} doesn`t have any handler!", id.ToString());
            }

            if (_cachedNodes.Count > 0)
            {
                foreach (KeyValuePair<object, LinkedListNode<EventHandler<T>>> cachedNode in _cachedNodes)
                {
                    if (cachedNode.Value != null && cachedNode.Value.Value == handler)
                    {
                        _tempNodes.Add(cachedNode.Key, cachedNode.Value.Next);
                    }
                }

                if (_tempNodes.Count > 0)
                {
                    foreach (KeyValuePair<object, LinkedListNode<EventHandler<T>>> tempNode in _tempNodes)
                    {
                        _cachedNodes[tempNode.Key] = tempNode.Value;
                    }

                    _tempNodes.Clear();
                }
            }

            if (!handlers.Remove(handler))
            {
                Log.Error("Event '{0}' not exists specified handler.", id.ToString());
            }

        }

        /// <summary>
        /// 设置默认事件处理函数
        /// </summary>
        /// <param name="handler">处理函数</param>
        public void SetDefaultHandler(EventHandler<T> handler)
        {
            _defaultHandler = handler;
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，
        /// 也可以保证在主线程种回调事件处理函数，但事件会在抛出后的下一帧分发
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void Fire(object sender, T e)
        {
            Event eventNode = Event.Create(sender, e);
            lock (_events)
            {
                _events.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 立即执行事件
        /// 线程不安全
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FireNow(object sender, T e)
        {
            HandleEvent(sender, e);
        }

        /// <summary>
        /// 处理事件节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleEvent(object sender, T e)
        {
            int eventId = e.Id;
            bool noHandleException = false;
            LinkedList<EventHandler<T>> handlers = null;
            if (_eventHandlers.TryGetValue(eventId, out handlers) && handlers.Count > 0)
            {
                LinkedListNode<EventHandler<T>> current = handlers.First;
                while (current != null)
                {
                    _cachedNodes[e] = current.Next;
                    current.Value(sender, e);
                    current = _cachedNodes[e];
                }
                _cachedNodes.Remove(e);
            }
            else if (_defaultHandler != null)
            {
                _defaultHandler(sender, e);
            }
            else if ((_eventPoolMode & EventPoolMode.AllowNoHandler) == 0)
            {
                noHandleException = true;
            }

            ReferencePool.Release(e);

            if (noHandleException)
            {
                Log.Error("Event '{0}' not allow no handler.", eventId.ToString());
            }
        }

    }
}