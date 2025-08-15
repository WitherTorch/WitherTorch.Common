#if NET8_0_OR_GREATER
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

using InlineMethod;

using WitherTorch.Common.Helpers;

using RuntimeX64Base = System.Runtime.Intrinsics.X86.X86Base.X64;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class X86Base
    {
        unsafe partial class X64
        {
            private static readonly delegate* managed<ulong, ulong> _bsfFunc, _bsrFunc;

            static X64()
            {
                if (!IsSupported)
                    return;
                Type type = typeof(RuntimeX64Base);
                _bsfFunc = (delegate* managed<ulong, ulong>)ReflectionHelper.GetMethodPointer(type, "BitScanForward",
                    [typeof(ulong)], typeof(ulong), BindingFlags.NonPublic | BindingFlags.Static);
                _bsrFunc = (delegate* managed<ulong, ulong>)ReflectionHelper.GetMethodPointer(type, "BitScanReverse",
                    [typeof(ulong)], typeof(ulong), BindingFlags.NonPublic | BindingFlags.Static);
            }

            public static partial bool IsSupported
            {
                [Inline(InlineBehavior.Keep, export: true)]
                get => RuntimeX64Base.IsSupported;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static partial uint BitScanForward(ulong value) => unchecked((uint)_bsfFunc(value));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static partial uint BitScanReverse(ulong value) => unchecked((uint)_bsrFunc(value));

            [RequiresPreviewFeatures]
            [Inline(InlineBehavior.Keep, export: true)]
            public static partial (long Quotient, long Remainder) DivRem(ulong lower, long upper, long divisor)
                => RuntimeX64Base.DivRem(lower, upper, divisor);

            [RequiresPreviewFeatures]
            [Inline(InlineBehavior.Keep, export: true)]
            public static partial (ulong Quotient, ulong Remainder) DivRem(ulong lower, ulong upper, ulong divisor)
                => RuntimeX64Base.DivRem(lower, upper, divisor);
        }
    }
}
#endif