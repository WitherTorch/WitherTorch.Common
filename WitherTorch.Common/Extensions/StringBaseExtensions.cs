using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static class StringBaseExtensions
    {
        extension(StringBase)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static StringBase Create(in ReadOnlySpan<char> span)
                => StringBase.Create(span, WTCommon.StringCreateOptions);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase Create(in ReadOnlySpan<char> span, StringCreateOptions options)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (char* ptr = span)
                    return StringBase.CreateCore(ptr, unchecked((nuint)length), options);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateUtf16String(in ReadOnlySpan<char> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (char* ptr = span)
                    return Utf16String.Create(ptr, unchecked((nuint)length));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateAsciiString(in ReadOnlySpan<byte> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (byte* ptr = span)
                    return AsciiString.Create(ptr, unchecked((nuint)length));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateLatin1String(in ReadOnlySpan<byte> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (byte* ptr = span)
                    return Latin1String.Create(ptr, unchecked((nuint)length));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateUtf8String(in ReadOnlySpan<byte> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (byte* ptr = span)
                    return Utf8String.Create(ptr, unchecked((nuint)length));
            }
        }
    }
}
