using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using InlineMethod;

namespace RiceTea.Core.Helpers;

public static class ThrowHelper
{
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
        resultPointer = (void*)(~(nuint)(errorCode >>= SignBitShift) & (nuint)resultPointer);
    }
}
