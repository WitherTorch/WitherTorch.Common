#if NET472_OR_GREATER
using System.Runtime.CompilerServices;
using System.Security;

namespace WitherTorch.Common.Intrinsics.X86
{
    [SuppressUnmanagedCodeSecurity]
    unsafe partial class Sse2
    {
        private static readonly bool _isSupported;
        private static readonly void* _movmskpdFunc, _pmovmskbFunc;

        unsafe static Sse2()
        {
            if (!CheckIsSupported())
                return;
            _isSupported = true;
            _movmskpdFunc = BuildMovmskpdAsm();
            _pmovmskbFunc = BuildPmovmskbAsm();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckIsSupported()
        {
            if (!X86Base.IsSupported)
                return false;
            const int Sse2Mask = 1 << 26;
            return (X86Base.CpuId(1).Ebx & Sse2Mask) == Sse2Mask;
        }

        public static partial bool IsSupported
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _isSupported;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<double> value) => ((delegate* unmanaged[Cdecl]<Register128, int>)_movmskpdFunc)(value._register);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<byte> value) => ((delegate* unmanaged[Cdecl]< Register128, int>)_pmovmskbFunc)(value._register);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<sbyte> value) => ((delegate* unmanaged[Cdecl]<Register128, int>)_pmovmskbFunc)(value._register);

        private static partial class StoreAsArray { }

        private static partial class StoreAsSpan { }
    }
}
#endif