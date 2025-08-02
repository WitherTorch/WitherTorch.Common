using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Text
{
    partial class StringBase
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBase Create(string str) => Create(str, WTCommon.StringCreateOptions);

        public static unsafe StringBase Create(string str, StringCreateOptions options)
        {
            int length = str.Length;
            if (length <= 0)
                return Empty;

            string? internedString = string.IsInterned(str);
            if (internedString is null)
            {
                if ((options & StringCreateOptions.UseLatin1Compression) == StringCreateOptions.UseLatin1Compression)
                {
                    fixed (char* ptr = str)
                    {
                        if (Latin1String.TryCreate(ptr, unchecked((nuint)length), out Latin1String? latin1String))
                            return latin1String;
                    }
                }
                if ((options & StringCreateOptions.UseUtf8Compression) == StringCreateOptions.UseUtf8Compression)
                {
                    fixed (char* ptr = str)
                    {
                        if (Utf8String.TryCreate(ptr, unchecked((nuint)length), options, out StringBase? utf8String))
                            return utf8String;
                    }
                }
            }
            else
            {
                str = internedString;

                if ((options & StringCreateOptions.ForceUseUtf8) == StringCreateOptions.ForceUseUtf8)
                {
                    fixed (char* ptr = str)
                    {
                        if (Utf8String.TryCreate(ptr, unchecked((nuint)length), options, out StringBase? utf8String))
                            return utf8String;
                    }
                }
            }

            return Utf16String.Create(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase Create(in ReadOnlySpan<char> span) => Create(span, WTCommon.StringCreateOptions);

        public static unsafe StringBase Create(in ReadOnlySpan<char> span, StringCreateOptions options)
        {
            int length = span.Length;
            if (length <= 0)
                return Empty;

            nuint castedLength = unchecked((nuint)length);
            fixed (char* ptr = span)
            {
                if ((options & StringCreateOptions.UseLatin1Compression) == StringCreateOptions.UseLatin1Compression)
                {
                    if (Latin1String.TryCreate(ptr, castedLength, out Latin1String? latin1String))
                        return latin1String;
                }
                if ((options & StringCreateOptions.UseUtf8Compression) == StringCreateOptions.UseUtf8Compression)
                {
                    if (Utf8String.TryCreate(ptr, castedLength, options, out StringBase? utf8String))
                        return utf8String;
                }
                return Utf16String.Create(ptr, castedLength);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase Create(char* ptr) => Create(ptr, WTCommon.StringCreateOptions);

        public static unsafe StringBase Create(char* ptr, StringCreateOptions options)
        {
            if (*ptr == '\0')
                return Empty;

            nuint length;
            for (length = 1; ptr[length] != '\0'; length++) ;

            if ((options & StringCreateOptions.UseLatin1Compression) == StringCreateOptions.UseLatin1Compression &&
                Latin1String.TryCreate(ptr, length, out Latin1String? latin1String))
                return latin1String;

            if ((options & StringCreateOptions.UseUtf8Compression) == StringCreateOptions.UseUtf8Compression &&
                Utf8String.TryCreate(ptr, length, options, out StringBase? utf8String))
                return utf8String;

            return Utf16String.Create(ptr, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase Create(char* ptr, int startIndex, int count) => Create(ptr, startIndex, count, WTCommon.StringCreateOptions);

        public static unsafe StringBase Create(char* ptr, int startIndex, int count, StringCreateOptions options)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            return Create(ptr, unchecked((nuint)startIndex), unchecked((nuint)count), options);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase Create(char* ptr, nuint startIndex, nuint count) => Create(ptr, startIndex, count, WTCommon.StringCreateOptions);

        public static unsafe StringBase Create(char* ptr, nuint startIndex, nuint count, StringCreateOptions options)
        {
            if (count == 0)
                return Empty;
            if ((options & StringCreateOptions.UseLatin1Compression) == StringCreateOptions.UseLatin1Compression)
            {
                if (Latin1String.TryCreate(ptr + startIndex, count, out Latin1String? latin1String))
                    return latin1String;
            }
            if ((options & StringCreateOptions.UseUtf8Compression) == StringCreateOptions.UseUtf8Compression)
            {
                if (Utf8String.TryCreate(ptr + startIndex, count, options, out StringBase? utf8String))
                    return utf8String;
            }
            return Utf16String.Create(ptr + startIndex, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateUtf16String(string str)
        {
            int length = str.Length;
            if (length <= 0)
                return Empty;
            return Utf16String.Create(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateUtf16String(in ReadOnlySpan<char> span)
        {
            int length = span.Length;
            if (length <= 0)
                return Empty;
            fixed (char* ptr = span)
                return Utf16String.Create(ptr, unchecked((nuint)length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateUtf16String(char* ptr)
        {
            if (*ptr == default)
                return Empty;
            return Utf16String.Create(ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateUtf16String(char* ptr, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            return CreateUtf16String(ptr, unchecked((nuint)startIndex), unchecked((nuint)count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateUtf16String(char* ptr, nuint startIndex, nuint count)
        {
            if (count == 0)
                return Empty;
            return Utf16String.Create(ptr + startIndex, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateLatin1String(in ReadOnlySpan<byte> span)
        {
            int length = span.Length;
            if (length <= 0)
                return Empty;
            fixed (byte* ptr = span)
                return Latin1String.Create(ptr, unchecked((nuint)length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateLatin1String(byte* ptr)
        {
            if (*ptr == default)
                return Empty;
            return Latin1String.Create(ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateLatin1String(byte* ptr, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            return CreateLatin1String(ptr, unchecked((nuint)startIndex), unchecked((nuint)count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateLatin1String(byte* ptr, nuint startIndex, nuint count)
        {
            if (count == 0)
                return Empty;
            return Latin1String.Create(ptr + startIndex, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateUtf8String(in ReadOnlySpan<byte> span)
        {
            int length = span.Length;
            if (length <= 0)
                return Empty;
            fixed (byte* ptr = span)
                return Utf8String.Create(ptr, unchecked((nuint)length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateUtf8String(byte* ptr)
        {
            if (*ptr == default)
                return Empty;
            return Utf8String.Create(ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateUtf8String(byte* ptr, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            return CreateUtf8String(ptr, unchecked((nuint)startIndex), unchecked((nuint)count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase CreateUtf8String(byte* ptr, nuint startIndex, nuint count)
        {
            if (count == 0)
                return Empty;
            return Utf8String.Create(ptr + startIndex, count);
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
