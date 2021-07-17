using System;
using System.Threading;

namespace BMBaseCore.Threading
{
    /// <summary>
    /// Used to confirm code is called from the appropriate thread.
    /// </summary>
    public class ThreadAffinity
    {
        /// <summary>
        /// Thrown when RegisterWorkerThread is called in a new thread when max configured threads are already registerd.
        /// </summary>
        public sealed class TooManyWorkerThreadsException : Exception
        {
            public TooManyWorkerThreadsException(int maxThreads) : base($"ThreadAffinity worker capcity exceeded ({maxThreads} threads)")
            {

            }
        }

        // There is no predefined invalid thread ID in C# ,but it`s 0 according to 
        // https://github.com/dotnet/coreclr/blob/master/src/vm/threads.h around line 5683
        public const int kInvalidThreadID = 0;
        public const int kInvalidSlotID = -1;
        public const int kDefaultMaxWorkerThreads = 8;

        /// <summary>
        /// Create a new thread checker,initially affiliated with the calling thread.
        /// </summary>
        /// <returns></returns>
        public static ThreadAffinity Create()
        {
            return new ThreadAffinity();
        }

        internal int OwnerThreadID = kInvalidThreadID;
        internal string OwnerThreadName = string.Empty;

        private readonly int _maxWorkerThreads;
        private readonly int[] _registeredWorkerThreadIDs;

        private ThreadAffinity(int maxWorkerThreads = kDefaultMaxWorkerThreads)
        {
            _maxWorkerThreads = maxWorkerThreads;
            _registeredWorkerThreadIDs = new int[maxWorkerThreads];

            for (int i = 0; i < maxWorkerThreads; i++)
            {
                _registeredWorkerThreadIDs[i] = kInvalidThreadID;
            }

            this.SetAffinity();
        }

        internal void RegisterWorkerThreadInternal(int threadID)
        {
            if (threadID == OwnerThreadID)
            {
                return;
            }

            int slotIndex = FindWorkerThreadIndex(threadID);

            if (slotIndex == kInvalidSlotID)
            {
                int slotThreadID = threadID;
                for (int i = 0; i < _maxWorkerThreads; i++)
                {
                    slotThreadID = Interlocked.CompareExchange(ref _registeredWorkerThreadIDs[i], threadID, kInvalidThreadID);
                    if (slotThreadID == kInvalidThreadID)
                    {
                        slotIndex = i;
                        break;
                    }
                }
            }

            if (slotIndex == kInvalidSlotID)
            {
                throw new TooManyWorkerThreadsException(_maxWorkerThreads);
            }
        }

        internal void UnregisterWorkerThreadInternal(int threadID)
        {
            int slotIndex = FindWorkerThreadIndex(threadID);
            if (slotIndex != kInvalidSlotID)
            {
                Interlocked.Exchange(ref _registeredWorkerThreadIDs[slotIndex], kInvalidThreadID);
            }
        }

        internal bool IsWorkerThreadRegisteredInternal(int threadID)
        {
            int slotIndex = FindWorkerThreadIndex(threadID);
            return slotIndex == kInvalidSlotID;
        }

        private int FindWorkerThreadIndex(int currentThreadID)
        {
            int slotIndex = kInvalidSlotID;
            for (int i = 0; i < _maxWorkerThreads; i++)
            {
                if (_registeredWorkerThreadIDs[i] == currentThreadID)
                {
                    slotIndex = i;
                    break;
                }
            }

            return slotIndex;
        }
    }

    /// <summary>
    /// Extension methods so they can run on null instances of ThreadAffinity
    /// </summary>
    public static class ThreadAffinityExtensions
    {
        /// <summary>
        /// Validate that current code has the desired thread affinity
        /// </summary>
        /// <param name="affinity"></param>
        public static void Validate(this ThreadAffinity affinity)
        {
            if (affinity != null)
            {
                // Careful,Thread.Name allocates in Mono,so don`t access it until there`s a problem to report
                Thread currentThread = Thread.CurrentThread;
                int currentThreadID = currentThread.ManagedThreadID;
                if (currentThreadID != affinity.OwnerThreadID && !affinity.IsWorkerThreadRegisteredInternal(currentThreadID))
                {
                    Assert.DebugFail($"Invoked on wrong thread;expected thread \"{affinity.OwnerThreadName}\"(ID {affinity.OwnerThreadID}) or registered worker thread but was thrad \"{currentThread.Name}\"(ID {currentThread.ManagedThreadID})");
                }
            }
        }

        /// <summary>
        /// Set the affinity to the current thread.
        /// </summary>
        /// <param name="affinity"></param>
        public static void SetAffinity(this ThreadAffinity affinity)
        {
            if (affinity != null)
            {
                int threadID = Thread.CurrentThread.ManagedThreadID;
                affinity.OwnerThreadID = threadID;
                affinity.OwnerThreadName = Thread.CurrentThread.Name;
                affinity.UnregisterWorkerThreadInternal(threadID);
            }
        }

        /// <summary>
        /// Add a permitted worker thread
        /// </summary>
        /// <param name="affinity"></param>
        /// <param name="threadID"></param>
        public static void RegisterWorkerThread(this ThreadAffinity affinity, int threadID)
        {
            if (affinity != null)
            {
                affinity.RegisterWorkerThreadInternal(threadID);
            }
        }

        /// <summary>
        /// Remove a permitted worker thread
        /// </summary>
        /// <param name="affinity"></param>
        /// <param name="threadID"></param>
        public static void UnregisterWorkerThread(this ThreadAffinity affinity, int threadID)
        {
            if (affinity != null)
            {
                affinity.UnregisterWorkerThread(threadID);
            }
        }
    }
}
