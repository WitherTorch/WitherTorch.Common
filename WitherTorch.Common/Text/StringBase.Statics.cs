using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class StringBase
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBase Create(string str) => Create(str, WTCommon.AllowLatin1StringCompression);

        public static unsafe StringBase Create(string str, bool tryLatin1Compress)
        {
            int length = str.Length;
            if (length <= 0)
                return Empty;

            if (tryLatin1Compress)
            {
                fixed (char* ptr = str)
                {
                    if (Latin1String.TryCreate(ptr, unchecked((nuint)length), out Latin1String? latin1String))
                        return latin1String;
                }
            }

            return Utf16String.Create(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase Create(char* ptr) => Create(ptr, WTCommon.AllowLatin1StringCompression);

        public static unsafe StringBase Create(char* ptr, bool tryLatin1Compress)
        {
            if (*ptr == '\0')
                return Empty;

            if (tryLatin1Compress && Latin1String.TryCreate(ptr, out Latin1String? latin1String))
                return latin1String;

            return Utf16String.Create(ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase Create(char* ptr, int startIndex, int count) => Create(ptr, startIndex, count, WTCommon.AllowLatin1StringCompression);

        public static unsafe StringBase Create(char* ptr, int startIndex, int count, bool tryLatin1Compress)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count <= 0)
                return Empty;
            if (tryLatin1Compress)
            {
                if (Latin1String.TryCreate(ptr + startIndex, unchecked((nuint)count), out Latin1String? latin1String))
                    return latin1String;
            }
            return Utf16String.Create(ptr + startIndex, unchecked((nuint)count));
        }

        public static unsafe StringBase CreateUtf16String(char* ptr)
        {
            if (*ptr == default)
                return Empty;

            return Utf16String.Create(ptr);
        }

        public static unsafe StringBase CreateUtf16String(char* ptr, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count <= 0)
                return Empty;
            return Utf16String.Create(ptr + startIndex, unchecked((nuint)count));
        }

        public static unsafe StringBase CreateLatin1String(byte* ptr)
        {
            if (*ptr == default)
                return Empty;
            return Latin1String.Create(ptr);
        }

        public static unsafe StringBase CreateLatin1String(byte* ptr, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count <= 0)
                return Empty;
            return Latin1String.Create(ptr + startIndex, unchecked((nuint)count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(StringBase str) => str is null || str.Length <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrWhiteSpace(StringBase str) => str is null || str.IsFullyWhitespaced();

        public static bool operator ==(StringBase left, StringBase right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(StringBase left, StringBase right)
        {
            if (ReferenceEquals(left, right))
                return false;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }
    }
}
