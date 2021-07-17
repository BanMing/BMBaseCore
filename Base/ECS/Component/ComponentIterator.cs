using System;
using System.Collections.Generic;
using BMBaseCore.Collections;

namespace BMBaseCore.ECS
{
    public struct ComponentIterator : IEnumerator<object>
    {
        private const int kMaxComponentFlagIndex = 127;

        private readonly BoolArray128 _componentFlags;
        private readonly object[] _components;
        private readonly int _startingOffset;
        private int _componentFlagIndex;

        public object Current => _components[_startingOffset + _componentFlagIndex];

        public ComponentIterator(BoolArray128 componentFlags, object[] components, int startingOffset)
        {
            _componentFlags = componentFlags;
            _components = components;
            _startingOffset = startingOffset;
            _componentFlagIndex = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            while (_componentFlagIndex < kMaxComponentFlagIndex)
            {
                _componentFlagIndex++;
                if (_componentFlags[_componentFlagIndex])
                {
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            _componentFlagIndex = -1;
        }
    }
}
