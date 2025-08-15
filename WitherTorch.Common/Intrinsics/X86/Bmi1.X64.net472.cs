#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Bmi1
    {
        unsafe partial class X64
        {
            private static readonly bool _isSupported;
            private static readonly void* _tzcntFunc;

            unsafe static X64()
            {
                if (!CheckIsSupported())
                    return;
                _isSupported = true;
                _tzcntFunc = BuildTzcntAsm();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool CheckIsSupported()
            {
                if (!X86Base.X64.IsSupported)
                    return false;
                const int Bmi1Mask = 1 << 3;
                return (X86Base.CpuId(7, 0).Ebx & Bmi1Mask) == Bmi1Mask;
            }

            public static partial bool IsSupported
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _isSupported;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static partial ulong TrailingZeroCount(ulong value) => ((delegate* unmanaged[Cdecl]<ulong, ulong>)_tzcntFunc)(value);

            private static partial class StoreAsArray { }

            private static partial class StoreAsSpan { }
        }
    }
}
#endif