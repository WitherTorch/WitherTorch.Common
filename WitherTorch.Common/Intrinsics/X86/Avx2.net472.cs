#if NET472_OR_GREATER
using System.Runtime.CompilerServices;
using System.Security;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Intrinsics.X86
{
    [SuppressUnmanagedCodeSecurity]
    unsafe partial class Avx2
    {
        private static readonly bool _isSupported;
        private static readonly void* _vpmovmskbFunc;

        unsafe static Avx2()
        {
            if (!CheckIsSupported())
                return;
            _isSupported = true;
            _vpmovmskbFunc = BuildVpmovmskbAsm();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckIsSupported()
        {
            if (!X86Base.IsSupported)
                return false;
            const int Avx2Mask = 1 << 5;
            return (X86Base.CpuId(7, 0).Ebx & Avx2Mask) == Avx2Mask;
        }

        public static partial bool IsSupported
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _isSupported;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M256<byte> value)
        {
            return ((delegate* unmanaged[Cdecl]<void*, int>)_vpmovmskbFunc)(UnsafeHelper.AsPointerIn(in value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial int MoveMask(in M256<sbyte> value)
        {
            return ((delegate* unmanaged[Cdecl]<void*, int>)_vpmovmskbFunc)(UnsafeHelper.AsPointerIn(in value));
        }

        private static partial class StoreAsArray { }

        private static partial class StoreAsSpan { }
    }
}
#endif