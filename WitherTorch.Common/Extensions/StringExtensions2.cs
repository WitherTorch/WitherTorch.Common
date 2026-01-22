using System;
using System.Runtime.CompilerServices;
using System.Security;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Extensions
{
    public static class StringExtensions2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static unsafe bool StartsWithAny(this string str, params ReadOnlySpan<char> chars)
        {
            if (StringHelper.IsNullOrEmpty(str) || chars.Length <= 0)
                return false;
            fixed (char* source = str, compare = chars)
                return SequenceHelper.Contains(compare, unchecked((nuint)chars.Length), source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool EndsWithAny(this string str, params ReadOnlySpan<char> chars)
        {
            if (StringHelper.IsNullOrEmpty(str) || chars.Length <= 0)
                return false;
            fixed (char* source = str, compare = chars)
                return SequenceHelper.Contains(compare, unchecked((nuint)chars.Length), source[str.Length - 1]);
        }

#if NET8_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool ContainsAny(this string obj, params ReadOnlySpan<char> values) => StringHelperExtensions.ContainsAny(obj, values);
#endif
    }
}
