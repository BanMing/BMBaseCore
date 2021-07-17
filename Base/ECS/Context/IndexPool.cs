using System;
using System.Collections;
using System.Collections.Generic;

namespace BMBaseCore.ECS
{
    // Singed integer;negatives are used to indicate free slots.
    using IndexType = Int16;

    /// <summary>
    /// An allocation-free pool of indices("slots")
    /// </summary>
    public sealed class IndexPool
    {
        private readonly IndexType _capacity;

        /// <summary>
        /// Array of indices stores linked list of free and linked list of used nodes.
        /// </summary>
        private readonly IndexType[] indices;
        private IndexType count;
        private IndexType firstFree;
        private IndexType firstUsed;

        /// <summary>
        /// Used for change detection while iterating.
        /// </summary>
        public int Version { get; private set; }

        public IndexType Count
        {
            get { return count; }
        }

        /// <summary>
        /// Create a new struct pool with the specified capacity
        /// </summary>
        /// <param name="maxCapacity"></param>
        /// <param name="name"></param>
        public IndexPool(IndexType maxCapacity, string name = "IndexPool")
        {
            _capacity = maxCapacity;
            indices = new IndexType[maxCapacity];
            Version = 0;

            ReleaseAll();
        }
        /// <summary>
        /// 
        /// Reserve a new item from the pool.
        /// </summary>
        /// <returns>Returns the index of the reserved item.</returns>
        public IndexType Reserve()
        {
            if (firstFree < _capacity)
            {
                // Grab next available slot of the free list
                IndexType newIndex = firstFree;
                // Negate because it`s a free slot
                firstFree = (short)-indices[firstFree];

                if (newIndex < firstUsed)
                {
                    // New entry ahead of all current used entries -- insert it on the front of the used list.
                    indices[newIndex] = (IndexType)firstUsed;
                    firstUsed = newIndex;
                }
                else
                {
                    // New entry after first used entry. Note that the first free entry must have immediately
                    // followed a used entry,so can link in by updating preceding entry.
                    indices[newIndex] = indices[newIndex - 1];
                    indices[newIndex - 1] = (IndexType)newIndex;
                }

                ++count;
                ++Version;
                
                return newIndex;
            }

            // Uh-oh,pool is full!
            return -1;
        }

        /// <summary>
        /// Release a previously reserved item back into the pool
        /// </summary>
        /// <param name="indexForFree"></param>
        public void Release(IndexType indexForFree)
        {
            if (indexForFree >= _capacity) { return; }
            IndexType nextUsed = indices[indexForFree];

            // If slot contents negative then it has already been released.
            if (nextUsed < 0) { return; }

            IndexType prevUsed = -1;

            // Are we freeing the first used item?
            if (indexForFree == firstUsed)
            {
                // Then first used is now the next used.
                firstUsed = nextUsed;
            }
            else
            {
                // Search back for preceding used item, the one that links to the item being freed.
                prevUsed = indexForFree;
                do
                {
                    --prevUsed;
                }
                while (indices[prevUsed] != indexForFree);

                // The hookup happens below, just before we exit the function.
                // Can`t do it now or it breaks the free list maintenance.
            }

            // Are we freeing an item in front of the start of the free list?
            if (indexForFree < firstFree)
            {
                // If so,then link freed item to start of free list.
                // Negate to indicate free slot.
                indices[indexForFree] = (IndexType)(-firstFree);
                firstFree = indexForFree;
            }
            else
            {
                // Search back for preceding free item,which will be the first item that
                // links past the item being freed.
                int prevFree = indexForFree;
                do
                {
                    --prevFree;
                }
                while (-indices[prevFree] <= indexForFree); // Negate free slot index.

                // Link freed item into the list
                indices[indexForFree] = indices[prevFree];
                indices[prevFree] = (IndexType)(-indexForFree);
            }

            // Finish up used list maintenance.Doing this earlier will break the search for prevFree.
            if (prevUsed >= 0)
            {
                indices[prevUsed] = (IndexType)nextUsed;
            }

            --count;
            ++Version;
        }

        public void ReleaseAll()
        {
            // Each entry links to the next. Negative indices indicate slots are free.
            for (int i = 0; i < _capacity; i++)
            {
                indices[i] = (IndexType)(-(i + 1));
            }

            count = 0;
            firstFree = 0;
            firstUsed = _capacity;
            ++Version;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Enumerates the in-use slots of the pool.
        /// </summary>
        public struct Enumerator : IEnumerator<int>
        {
            private IndexPool _pool;
            private int _current;
            private int _next;
            private int _iteratingVersion;

            public Enumerator(IndexPool pool)
            {
                _pool = pool;
                _current = -1;
                _next = pool.firstUsed;
                _iteratingVersion = pool.Version;
            }

            public int Current => _current;

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                _pool = null;
                _current = 0;
                _next = 0;
                _iteratingVersion = 0;
            }

            public bool MoveNext()
            {
                if ((_iteratingVersion == _pool.Version) && (_next < _pool._capacity))
                {
                    _current = _next;
                    _next = _pool.indices[_next];
                    return true;
                }

                return MoveNextRare();
            }

            internal bool MoveNextRare()
            {
                if (_iteratingVersion != _pool.Version)
                {
                    // If version has changed then we need to see if _next is now pointing to a free slot
                    // i.e. negative index, and search forward for the next reserved slot.
                    // Since we always iterate in-order, this works
                    while ((_next < _pool._capacity) && (_pool.indices[_next] < 0))
                    {
                        ++_next;
                    }

                    // If we found a used slot then we can now do the default behaviour of MoveNext,as above.
                    if (_next < _pool._capacity)
                    {
                        _current = _next;
                        _next = _pool.indices[_next];

                        return true;
                    }
                }

                // Must be end of list
                return false;
            }

            public void Reset()
            {
                _current = -1;
                _next = _pool.firstUsed;
            }
        }
    }
}
