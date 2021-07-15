using System;
using System.Diagnostics;

namespace BMBaseCore
{
    /// <summary>
    /// Helper class for asserting in a consistent way across the whole application
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Call when an assertion fails.
        /// </summary>
        public static event AssertHandler OnAssert;
        public delegate void AssertHandler(string message, StackTrace stackTrace, bool debug);

        private const bool kStackIncluded = true;
        private const bool kStackIncludeFileInfo = true;
        private const int kStackSkipFrames = 2; // skip stack frame(s) from the Assert mechanics to get back to the asserting code

        private const string kAssertDebugMessageDefault = "---- DEBUG ASSERT FAILED ----";

        /// <summary>
        /// Condition has already been checked, just display the assert message
        /// </summary>
        public static void DebugFail(string message = kAssertDebugMessageDefault)
        {
            DoAssert(message, true);
        }

        /// <summary>
        /// Condition has already been checked, just display the assert message
        /// </summary>
        public static void DebugFail<T1>(string format, T1 arg1)
        {
            DoAssert(Utility.Text.Format(format, arg1), true);
        }
        

        /// <summary>
        /// Condition has already been checked, just display the assert message
        /// </summary>
        public static void DebugFail<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            DoAssert(Utility.Text.Format(format, arg1, arg2), true);
        }

        /// <summary>
        /// Condition has already been checked, just display the assert message
        /// </summary>
        public static void DebugFail<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            DoAssert(Utility.Text.Format(format, arg1, arg2, arg3), true);
        }

        /// <summary>
        /// Condition has already been checked, just display the assert message
        /// </summary>
        public static void DebugFail<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            DoAssert(Utility.Text.Format(format, arg1, arg2, arg3, arg4), true);
        }

        /// <summary>
        /// Verify a condition that should be met.
        /// </summary>
        public static void Debug(bool condition, string message = kAssertDebugMessageDefault)
        {
            if (!condition)
            {
                DoAssert(message, true);
            }
        }

        /// <summary>
        /// Verify a condition that should be met and display a formatted message if condition fails
        /// (1 formal argument version)
        /// </summary>
        public static void Debug<T1>(bool condition, string format, T1 arg1)
        {
            if (!condition)
            {
                DoAssert(Utility.Text.Format(format, arg1), true);
            }
        }

        /// <summary>
        /// Verify a condition that should be met and display a formatted message if condition fails
        /// (2 formal arguments version)
        /// </summary>
        public static void Debug<T1, T2>(bool condition, string format, T1 arg1, T2 arg2)
        {
            if (!condition)
            {
                DoAssert(Utility.Text.Format(format, arg1, arg2), true);
            }
        }

        /// <summary>
        /// Verify a condition that should be met and display a formatted message if condition fails
        /// (3 formal arguments version)
        /// </summary>

        public static void Debug<T1, T2, T3>(bool condition, string format, T1 arg1, T2 arg2, T3 arg3)
        {
            if (!condition)
            {
                DoAssert(Utility.Text.Format(format, arg1, arg2, arg3), true);
            }
        }

        /// <summary>
        /// Verify a condition that should be met and display a formatted message if condition fails
        /// (4 formal arguments version)
        /// </summary>
        public static void Debug<T1, T2, T3, T4>(bool condition, string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (!condition)
            {
                DoAssert(Utility.Text.Format(format, arg1, arg2, arg3, arg4), true);
            }
        }

        /// <summary>
        /// Verify a condition that should be met and display a formatted message if condition fails
        /// (N formal arguments version)
        /// </summary>
        public static void Debug(bool condition, string format, params object[] args)
        {
            if (!condition)
            {
                DoAssert(Utility.Text.Format(format, args), true);
            }
        }

        private static void DoAssert(string message, bool debug)
        {
            if (OnAssert == null)
            {
                return;
            }

            StackTrace stackTrace = (kStackIncluded ? new StackTrace(kStackSkipFrames, kStackIncludeFileInfo) : null);
            OnAssert(message, stackTrace, debug);
        }
    }
}
