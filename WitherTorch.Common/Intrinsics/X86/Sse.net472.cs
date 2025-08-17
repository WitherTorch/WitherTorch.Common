#if NET472_OR_GREATER
using System.Runtime.CompilerServices;
using System.Security;

namespace WitherTorch.Common.Intrinsics.X86
{
    [SuppressUnmanagedCodeSecurity]
    unsafe partial class Sse
    {
        private static readonly bool _isSupported;
        private static readonly void* _movmskpsFunc;

        unsafe static Sse()
        {
            if (!CheckIsSupported())
                return;
            _isSupported = true;
            _movmskpsFunc = BuildMovmskpsAsm();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckIsSupported()
        {
            if (!X86Base.IsSupported)
                return false;
            const int SseMask = 1 << 25;
            return (X86Base.CpuId(1).Ebx & SseMask) == SseMask;
        }

        public static partial bool IsSupported
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _isSupported;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<float> value) => ((delegate* unmanaged[Cdecl]<Register128, int>)_movmskpsFunc)(value._register);

        private static partial class StoreAsArray { }

        private static partial class StoreAsSpan { }
    }
}
#endif