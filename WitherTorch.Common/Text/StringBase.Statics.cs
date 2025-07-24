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
                return EmptyString.Instance;

            if (tryLatin1Compress)
            {
                fixed (char* ptr = str)
                {
                    if (Latin1String.TryCreate(ptr, unchecked((nuint)length), out Latin1String? latin1String))
                        return latin1String;
                }
            }

            return new Utf16String(str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase Create(char* ptr) => Create(ptr, WTCommon.AllowLatin1StringCompression);

        public static unsafe StringBase Create(char* ptr, bool tryLatin1Compress)
        {
            if (*ptr == '\0')
                return EmptyString.Instance;

            if (tryLatin1Compress && Latin1String.TryCreate(ptr, out Latin1String? latin1String))
                return latin1String;

            return new Utf16String(new string(ptr));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase Create(char* ptr, int startIndex, int length) => Create(ptr, startIndex, length, WTCommon.AllowLatin1StringCompression);

        public static unsafe StringBase Create(char* ptr, int startIndex, int length, bool tryCompress)
        {
            if (length <= 0)
                return EmptyString.Instance;

            if (tryCompress)
            {
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                if (Latin1String.TryCreate(ptr + startIndex, unchecked((nuint)length), out Latin1String? latin1String))
                    return latin1String;
            }

            return new Utf16String(new string(ptr, startIndex, length));
        }

        public static unsafe StringBase CreateLatin1String(byte* ptr, int startIndex, int length)
        {
            if (length <= 0)
                return EmptyString.Instance;

            byte[] buffer = new byte[length + 1];
            fixed (byte* ptrBuffer = buffer)
                UnsafeHelper.CopyBlockUnaligned(ptrBuffer, ptr, unchecked((uint)length * sizeof(byte)));
            return new Latin1String(buffer);
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
