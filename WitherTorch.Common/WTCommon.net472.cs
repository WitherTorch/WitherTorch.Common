#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common
{
    partial class WTCommon
    {
        private static readonly bool _systemBuffersExists = CheckSystemBuffersExists();
        private static readonly bool _systemMemoryExists = CheckSystemMemoryExists();

        public static partial bool SystemBuffersExists
        {
            get => _systemBuffersExists;
        }

        public static partial bool SystemMemoryExists
        {
            get => _systemMemoryExists;
        }

        private static bool CheckSystemBuffersExists()
        {
            try
            {
                return SystemBufferChecker.CheckArrayPool();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool CheckSystemMemoryExists()
        {
            try
            {
                return SystemMemoryChecker.CheckSpan() && SystemMemoryChecker.CheckMemory();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static class SystemBufferChecker
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static bool CheckArrayPool() => System.Buffers.ArrayPool<byte>.Shared is not null;
        }

        private static class SystemMemoryChecker
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static bool CheckSpan() => ReadOnlySpan<byte>.Empty.Length == 0;

            [MethodImpl(MethodImplOptions.NoInlining)]
            public static bool CheckMemory() => ReadOnlyMemory<byte>.Empty.Length == 0;
        }
    }
}
#endif