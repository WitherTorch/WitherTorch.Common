using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    public static class NullSafetyHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
        public static T ThrowIfNull<T>(T? obj, [CallerArgumentExpression(nameof(obj))] string? argumentName = null) where T : class
#else
        public static T ThrowIfNull<T>(T? obj, string? argumentName = null) where T : class
#endif
        {
            if (obj is null)
                throw new ArgumentNullException(argumentName ?? nameof(obj));
            return obj;
        }
    }
}
