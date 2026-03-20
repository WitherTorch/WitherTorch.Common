using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        [LocalsInit(false)]
        private static partial class FastCore { }

        [LocalsInit(false)]
        private static partial class FastCore<T> where T : unmanaged
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

            [Inline(InlineBehavior.Remove)]
            public static nuint GetLimitForVectorizing()
            {
#if NET8_0_OR_GREATER
                if (Limits.UseVector64())
                    return unchecked((nuint)System.Runtime.Intrinsics.Vector64<T>.Count) - 1;
                if (Limits.UseVector128())
                    return unchecked((nuint)System.Runtime.Intrinsics.Vector128<T>.Count) - 1;
                if (Limits.UseVector256())
                    return unchecked((nuint)System.Runtime.Intrinsics.Vector256<T>.Count) - 1;
                if (Limits.UseVector512())
                    return unchecked((nuint)System.Runtime.Intrinsics.Vector512<T>.Count) - 1;
#else
                if (Limits.UseVector())
                    return unchecked((nuint)System.Numerics.Vector<T>.Count) - 1;
#endif
                return UnsafeHelper.GetMaxValue<nuint>(); // Don't let program go vectorize!
            }
        }

        [LocalsInit(false)]
        private static partial class SlowCore { }

        [LocalsInit(false)]
        private static partial class SlowCore<T> { }
    }
}
