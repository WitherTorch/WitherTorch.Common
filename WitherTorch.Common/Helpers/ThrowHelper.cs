using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static class ThrowHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static void ThrowExceptionForHR(int errorCode)
            => Marshal.ThrowExceptionForHR(errorCode);

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void ThrowExceptionForHR(int errorCode, void* resultPointer)
        {
            ThrowExceptionForHR(errorCode);
            if (resultPointer is null)
                throw new InvalidOperationException("The result pointer is null.");
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void ResetPointerForHR(int errorCode, ref void* resultPointer)
        {
            if (errorCode >= 0)
                return;
            resultPointer = null;
        }
    }
}
