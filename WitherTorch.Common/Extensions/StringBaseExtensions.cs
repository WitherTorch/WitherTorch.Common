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
            public static StringBase Create(in Span<char> span)
                => StringBase.Create(span, WTCommon.StringCreateOptions);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase Create(in Span<char> span, StringCreateOptions options)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (char* ptr = span)
                    return StringBase.Create(ptr, 0u, unchecked((nuint)length), options);
            }

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
                    return StringBase.Create(ptr, 0u, unchecked((nuint)length), options);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateUtf16String(in Span<char> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (char* ptr = span)
                    return StringBase.CreateUtf16String(ptr, 0u, unchecked((nuint)length));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateUtf16String(in ReadOnlySpan<char> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (char* ptr = span)
                    return StringBase.CreateUtf16String(ptr, 0u, unchecked((nuint)length));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateAsciiString(in Span<byte> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (byte* ptr = span)
                    return StringBase.CreateAsciiString(ptr, 0u, unchecked((nuint)length));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateAsciiString(in ReadOnlySpan<byte> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (byte* ptr = span)
                    return StringBase.CreateAsciiString(ptr, 0u, unchecked((nuint)length));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateLatin1String(in Span<byte> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (byte* ptr = span)
                    return StringBase.CreateLatin1String(ptr, 0u, unchecked((nuint)length));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateLatin1String(in ReadOnlySpan<byte> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (byte* ptr = span)
                    return StringBase.CreateLatin1String(ptr, 0u, unchecked((nuint)length));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateUtf8String(in Span<byte> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (byte* ptr = span)
                    return StringBase.CreateUtf8String(ptr, 0u, unchecked((nuint)length));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe StringBase CreateUtf8String(in ReadOnlySpan<byte> span)
            {
                int length = span.Length;
                if (length <= 0)
                    return StringBase.Empty;
                fixed (byte* ptr = span)
                    return StringBase.CreateUtf8String(ptr, 0u, unchecked((nuint)length));
            }
        }
    }
}
