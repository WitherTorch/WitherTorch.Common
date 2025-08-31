#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace WitherTorch.Common
{
    partial class WTCommon
    {
        private static readonly Lazy<bool> _systemBuffersExistsLazy = new Lazy<bool>(CheckSystemBuffersExists, LazyThreadSafetyMode.PublicationOnly);

        public static partial bool SystemBuffersExists => _systemBuffersExistsLazy.Value;

        private static bool CheckSystemBuffersExists()
        {
            try
            {
                return SystemBufferChecker.CheckSpan() && SystemBufferChecker.CheckMemory() && SystemBufferChecker.CheckArrayPool();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static class SystemBufferChecker
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static bool CheckSpan() => ReadOnlySpan<byte>.Empty.Length == 0;

            [MethodImpl(MethodImplOptions.NoInlining)]
            public static bool CheckMemory() => ReadOnlyMemory<byte>.Empty.Length == 0;

            [MethodImpl(MethodImplOptions.NoInlining)]
            public static bool CheckArrayPool() => System.Buffers.ArrayPool<byte>.Shared is not null;
        }
    }
}
#endif