using System.Numerics;
using System.Runtime.CompilerServices;

using InlineMethod;

using LocalsInit;

#pragma warning disable CS8500

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        [LocalsInit(false)]
        private static unsafe partial class FastCore { }

        [LocalsInit(false)]
        private static unsafe partial class FastCore<T> where T : unmanaged
        {
            [Inline(InlineBehavior.Remove)]
            public static bool CheckTypeCanBeVectorized()
#if NET8_0_OR_GREATER
                => System.Numerics.Vector<T>.IsSupported;
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
        }

        [LocalsInit(false)]
        private static unsafe partial class SlowCore { }

        [LocalsInit(false)]
        private static unsafe partial class SlowCore<T> { }
    }
}
