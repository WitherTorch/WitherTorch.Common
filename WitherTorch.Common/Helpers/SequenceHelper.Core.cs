using System;
using System.Numerics;

using InlineMethod;

#if NET472_OR_GREATER
using LocalsInit;
#else
using System.Runtime.CompilerServices;
#endif

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        private static unsafe partial class Core { }

#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#else
        [LocalsInit(false)]
#endif
        private static unsafe partial class Core<T> where T : unmanaged
        {
            [Inline(InlineBehavior.Remove)]
            private static bool CheckTypeCanBeVectorized()
#if NET8_0_OR_GREATER
                => Vector<T>.IsSupported;
#else
                => (typeof(T) == typeof(byte)) ||
                       (typeof(T) == typeof(short)) ||
                       (typeof(T) == typeof(int)) ||
                       (typeof(T) == typeof(long)) ||
                       (typeof(T) == typeof(sbyte)) ||
                       (typeof(T) == typeof(ushort)) ||
                       (typeof(T) == typeof(uint)) ||
                       (typeof(T) == typeof(ulong)) ||
                       (typeof(T) == typeof(float)) ||
                       (typeof(T) == typeof(double));
#endif

            [Inline(InlineBehavior.Remove)]
            private static bool IsUnsigned()
                => (typeof(T) == typeof(byte)) ||
                       (typeof(T) == typeof(ushort)) ||
                       (typeof(T) == typeof(uint)) ||
                       (typeof(T) == typeof(ulong));
        }
    }
}
