using System;
using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static partial class StringExtensions
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static char FirstOrDefault(this string obj, char defaultValue = '\0')
            => StringHelper.IsNullOrEmpty(obj) ? defaultValue : obj[0];

        [Inline(InlineBehavior.Keep, export: true)]
        public static char LastOrDefault(this string obj, char defaultValue = '\0')
            => StringHelper.IsNullOrEmpty(obj) ? defaultValue : obj[obj.Length - 1];

#if !NET8_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool ContainsAny(this string obj, params char[] values) => StringHelper.ContainsAny(obj, values);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool ContainsAny(this string obj, string values) => StringHelper.ContainsAny(obj, values);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this string[] array, string value, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, 0, array.Length, comparison) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this string[] array, string value, int startIndex, int length, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, startIndex, length, comparison) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this string[] array, string value, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, 0, array.Length, comparison);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this string[] array, string value, int startIndex, int length, StringComparison comparison = StringComparison.Ordinal)
        {
            for (int i = startIndex; i < length; i++)
            {
                if (SequenceHelper.Equals(array[i], value, comparison))
                    return i;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWith(this string str, char c)
        {
            int count = str.Length;
            if (count > 0)
                return str[count - 1] == c;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWithAny(this string str, params char[] chars)
        {
            int count = str.Length;
            if (count > 0)
            {
                char c = str[count - 1];
                for (int i = 0, length = chars.Length; i < length; i++)
                {
                    if (c == chars[i])
                        return true;
                }
            }
            return false;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool StartsWith(this string str, char c)
            => !StringHelper.IsNullOrEmpty(str) && str[0] == c;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool StartsWithAny(this string str, params char[] chars)
        {
            if (StringHelper.IsNullOrEmpty(str))
                return false;
            return chars.Contains(str[0]);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static StringBase ToStringBase(this string _this)
            => StringBase.Create(_this);

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe StringBase ToStringBase(this string _this, StringCreateOptions options)
            => StringBase.Create(_this, options);

        public static string[] ToUpperAscii(this string[] array)
        {
            for (int i = 0, length = array.Length; i < length; i++)
                array[i] = ToUpperAscii(array[i]);
            return array;
        }

        public static string[] ToLowerAscii(this string[] array)
        {
            for (int i = 0, length = array.Length; i < length; i++)
                array[i] = ToLowerAscii(array[i]);
            return array;
        }

        public static unsafe string ToUpperAscii(this string value)
        {
            int length = value.Length;
            if (length <= 0)
                return string.Empty;
            fixed (char* ptr = value)
                return ToUpperAsciiCore(ptr, ptr + length) ?? value;
        }

        public static unsafe string ToLowerAscii(this string value)
        {
            int length = value.Length;
            if (length <= 0)
                return string.Empty;
            fixed (char* ptr = value)
                return ToLowerAsciiCore(ptr, ptr + length) ?? value;
        }

        private static unsafe string? ToUpperAsciiCore(char* ptr, char* ptrEnd)
            => ToLowerOrUpperAsciiCore(ptr, ptrEnd, isUpper: true);

        private static unsafe string? ToLowerAsciiCore(char* ptr, char* ptrEnd)
            => ToLowerOrUpperAsciiCore(ptr, ptrEnd, isUpper: false);

        public static string[] WithPrefix(this string[] array, string prefix)
        {
            for (int i = 0, length = array.Length; i < length; i++)
                array[i] = array[i].WithPrefix(prefix);
            return array;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static string WithPrefix(this string value, string prefix)
            => prefix + value;
    }
}
