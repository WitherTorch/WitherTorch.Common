using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static class ThrowHelper
    {
#if B64_ARCH
        private static readonly bool _bmi1X64Supported = Bmi1.X64.IsSupported;
#endif
        private static readonly bool _bmi1Supported = Bmi1.IsSupported;

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowExceptionForHR(int errorCode)
            => Marshal.ThrowExceptionForHR(errorCode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ThrowExceptionForHR(int errorCode, void* resultPointer)
        {
            ThrowExceptionForHR(errorCode);
            if (resultPointer is null)
                Throw();

            static void Throw() => throw new NullReferenceException("The result pointer is null.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void ResetPointerForHR(int errorCode, ref void* resultPointer)
        {
            const int SignBitShift = sizeof(int) * 8 - 1;

            errorCode >>= SignBitShift;
            if (_bmi1X64Supported)
                resultPointer = (void*)Bmi1.X64.AndNot((ulong)errorCode, (ulong)resultPointer);
            else if (_bmi1Supported)
                resultPointer = (void*)Bmi1.AndNot((uint)errorCode, (uint)resultPointer);
            else
                resultPointer = (void*)(~(nuint)errorCode & (nuint)resultPointer);
        }
    }
}
