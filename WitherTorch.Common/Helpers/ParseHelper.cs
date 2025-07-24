using InlineMethod;

using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Helpers
{
    public static unsafe partial class ParseHelper
    {
#if NET5_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int ParseToInt32(string input)
        {
#if NET5_0_OR_GREATER
            return int.Parse(input);
#else
            fixed (char* ptr = input)
                return ParseToInt32(ptr, input.Length);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ParseToInt32(StringBase input) => input.StringType switch
        {
            StringType.Utf16 => ParseToInt32(input.ToString()),
            _ => ParseToInt32_Other(input),
        };

#if NET5_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int ParseToInt32(string input, int startIndex, int count)
        {
#if NET5_0_OR_GREATER
            return int.Parse(input.AsSpan(startIndex, count));
#else
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = input.Length;
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = input)
                return ParseToInt32(ptr + startIndex, count);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ParseToInt32(StringBase input, int startIndex, int count) => input.StringType switch
        {
            StringType.Utf16 => ParseToInt32(input.ToString(), startIndex, count),
            _ => ParseToInt32_Other(input, startIndex, count),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ParseToInt32_Other(StringBase input)
        {
            int length = input.Length;
            if (length <= 0)
                throw new FormatException(nameof(input) + " is not a valid number string!");
            return ParseToInt32_OtherChecked(input, 0, unchecked((nuint)length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ParseToInt32_Other(StringBase input, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int length = input.Length;
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            return ParseToInt32_OtherChecked(input, unchecked((nuint)startIndex), unchecked((nuint)count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ParseToInt32_OtherChecked(StringBase input, nuint startIndex, nuint length)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(input.Length);
            try
            {
                fixed (char* ptr = buffer)
                {
                    input.CopyToCore(ptr, 0, length);
                    return ParseToInt32(ptr, (int)length);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

#if NET5_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static int ParseToInt32(char* input, int length)
        {
#if NET5_0_OR_GREATER
            return int.Parse(new ReadOnlySpan<char>(input, length));
#else
            ConvertionErrors errors = ParseToInt32Core(input, length, out int result);
            return errors switch
            {
                ConvertionErrors.Format => throw new FormatException(nameof(input) + " is not a valid number string!"),
                ConvertionErrors.Overflow => throw new OverflowException(),
                _ => result,
            };
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool TryParseToInt32(string input, out int result) => int.TryParse(input, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseToInt32(string input, int startIndex, int count, out int result)
        {
            int length = input.Length;
            if (startIndex + count > length)
            {
                result = 0;
                return false;
            }
#if NET5_0_OR_GREATER
            return int.TryParse(input.AsSpan().Slice(startIndex, count), out result);
#else
            fixed (char* ptr = input)
                return TryParseToInt32(ptr + startIndex, count, out result);
#endif
        }

#if NET5_0_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
#endif
        public static bool TryParseToInt32(char* input, int length, out int result)
        {
#if NET5_0_OR_GREATER
            return int.TryParse(new ReadOnlySpan<char>(input, length), out result);
#else
            return ParseToInt32Core(input, length, out result) == ConvertionErrors.None;
#endif
        }
    }
}
