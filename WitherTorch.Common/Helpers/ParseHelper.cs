using InlineMethod;

using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    public static unsafe partial class ParseHelper
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static int ParseToInt32(string input) => int.Parse(input);

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
            int length = input.Length;
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = input)
                return ParseToInt32(ptr + startIndex, count);
#endif
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
            switch (errors)
            {
                case ConvertionErrors.Format:
                    throw new FormatException(nameof(input) + " is not a valid number string!");
                case ConvertionErrors.Overflow:
                    throw new OverflowException();
                default:
                    return result;
            }
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
