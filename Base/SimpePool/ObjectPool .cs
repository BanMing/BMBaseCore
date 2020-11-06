/******************************************************************
** ObjectPool.cs
** @Author       : BanMing 
** @Date         : 2020/11/6 pm 3:46:49
** @Description  : 
*******************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BMBaseCore
{
    public class ObjectPool<T> where T : new()
    {
        private readonly Stack<T> _stack = new Stack<T>();

        private readonly Action<T> _actionOnGet;

        private readonly Action<T> _actionOnRelease;

        public int CountAll { get; private set; }

        public int CountInactive { get { return _stack.Count; } }

        public int CountActive { get { return CountAll - CountInactive; } }

        public ObjectPool(Action<T> actionOnGet, Action<T> actionOnRelease)
        {
            _actionOnGet = actionOnGet;
            _actionOnRelease = actionOnRelease;
            CountAll = 0;
        }

        public T Get()
        {
            T t;
            if (_stack.Count == 0)
            {
                t = Activator.CreateInstance<T>();
                this.CountAll++;
            }
            else
            {
                t = this._stack.Pop();
            }

            _actionOnGet?.Invoke(t);

            return t;
        }

        public void Release(T element)
        {
            if (element == null)
            {
                return;

            }

            _actionOnRelease?.Invoke(element);
            this._stack.Push(element);
        }
    }
}