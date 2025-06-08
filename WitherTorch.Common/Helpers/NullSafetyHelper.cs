using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    public static class NullSafetyHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNull<T>(T? obj, [CallerArgumentExpression(nameof(obj))] string? argumentName = null) where T : class
        {
            if (obj is null)
                throw new ArgumentNullException(argumentName ?? nameof(obj));
            return obj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNull<T>(T? obj, [CallerArgumentExpression(nameof(obj))] string? argumentName = null) where T : struct
        {
            if (!obj.HasValue)
                throw new ArgumentNullException(argumentName ?? nameof(obj));
            return obj.Value;
        }
    }
}
