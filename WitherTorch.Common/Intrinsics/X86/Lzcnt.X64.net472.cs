#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Intrinsics.X86
{
    partial class Lzcnt
    {
        unsafe partial class X64
        {
            private static readonly bool _isSupported;
            private static readonly void* _lzcntFunc;

            unsafe static X64()
            {
                if (!CheckIsSupported())
                    return;
                _isSupported = true;
                _lzcntFunc = BuildLzcntAsm();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool CheckIsSupported()
            {
                if (!X86Base.X64.IsSupported)
                    return false;
                const int LzcntMask = 1 << 5;
                return (X86Base.CpuId(unchecked((int)0x80000001), 0).Ebx & LzcntMask) == LzcntMask;
            }

            public static partial bool IsSupported
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _isSupported;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static partial ulong LeadingZeroCount(ulong value) => ((delegate* unmanaged[Cdecl]<ulong, ulong>)_lzcntFunc)(value);

            private static partial class StoreAsArray { }

            private static partial class StoreAsSpan { }
        }
    }
}
#endif