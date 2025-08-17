#if NET472_OR_GREATER
using System.Runtime.CompilerServices;
using System.Security;

namespace WitherTorch.Common.Intrinsics.X86
{
    [SuppressUnmanagedCodeSecurity]
    unsafe partial class Avx
    {
        private static readonly bool _isSupported;
        private static readonly void* _vpmovmskbFunc, _vmovmskpsFunc_128, _vmovmskpsFunc_256, _vmovmskpdFunc_128, _vmovmskpdFunc_256;

        unsafe static Avx()
        {
            if (!CheckIsSupported())
                return;
            _isSupported = true;
            _vpmovmskbFunc = BuildVpmovmskbAsm();
            _vmovmskpsFunc_128 = BuildVmovmskpsAsm_128();
            _vmovmskpsFunc_256 = BuildVmovmskpsAsm_256();
            _vmovmskpdFunc_128 = BuildVmovmskpdAsm_128();
            _vmovmskpdFunc_256 = BuildVmovmskpdAsm_256();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckIsSupported()
        {
            if (!X86Base.IsSupported)
                return false;
            const int AvxMask = 1 << 28;
            return (X86Base.CpuId(1).Ecx & AvxMask) == AvxMask;
        }

        public static partial bool IsSupported
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _isSupported;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<float> value) => ((delegate* unmanaged[Cdecl]<Register128, int>)_vmovmskpsFunc_128)(value._register);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M256<float> value) => ((delegate* unmanaged[Cdecl]<Register256, int>)_vmovmskpsFunc_256)(value._register);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<double> value) => ((delegate* unmanaged[Cdecl]<Register128, int>)_vmovmskpdFunc_128)(value._register);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M256<double> value) => ((delegate* unmanaged[Cdecl]<Register256, int>)_vmovmskpdFunc_256)(value._register);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<byte> value) => ((delegate* unmanaged[Cdecl]<Register128, int>)_vpmovmskbFunc)(value._register);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M128<sbyte> value) => ((delegate* unmanaged[Cdecl]<Register128, int>)_vpmovmskbFunc)(value._register);

        private static partial class StoreAsArray { }

        private static partial class StoreAsSpan { }
    }
}
#endif