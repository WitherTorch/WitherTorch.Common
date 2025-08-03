using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    public static partial class Latin1EncodingHelper
    {
        // ASCII 編碼邊界
        public const char AsciiEncodingLimit = '\u007F';
        public const byte AsciiEncodingLimit_InByte = 0x007F;

        // Latin-1 編碼邊界
        public const char Latin1EncodingLimit = '\u00FF';
        public const byte Latin1EncodingLimit_InByte = 0x00FF;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe int GetWorstCaseForEncodeLength(int length) => length;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe uint GetWorstCaseForEncodeLength(uint length) => length;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe nuint GetWorstCaseForEncodeLength(nuint length) => length;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe int GetWorstCaseForDecodeLength(int length) => length;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe uint GetWorstCaseForDecodeLength(uint length) => length;

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe nuint GetWorstCaseForDecodeLength(nuint length) => length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* ReadFromUtf16Buffer(char* source, nuint sourceLength, byte* destination, nuint destinationLength)
        {
            if (sourceLength == 0 || destinationLength == 0)
                return destination;
            nuint length = MathHelper.Min(sourceLength, destinationLength);
            if (SequenceHelper.ContainsGreaterThan(source, length, Latin1EncodingLimit))
                return ReadFromUtf16BufferCore_OutOfLatin1Range(source, destination, length);
            return ReadFromUtf16BufferCore(source, destination, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe byte* ReadFromUtf16Buffer(char* source, char* sourceEnd, byte* destination, byte* destinationEnd)
        {
            if (sourceEnd <= source || destinationEnd <= destination)
                return destination;
            nuint length = MathHelper.Min(unchecked((nuint)(sourceEnd - source)), unchecked((nuint)(destinationEnd - destination)));
            if (SequenceHelper.ContainsGreaterThan(source, length, Latin1EncodingLimit))
                return ReadFromUtf16BufferCore_OutOfLatin1Range(source, destination, length);
            return ReadFromUtf16BufferCore(source, destination, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe char* WriteToUtf16Buffer(byte* source, nuint sourceLength, char* destination, nuint destinationLength)
        {
            if (sourceLength == 0 || destinationLength == 0)
                return destination;
            nuint length = MathHelper.Min(sourceLength, destinationLength);
            return WriteToUtf16BufferCore(source, destination, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe char* WriteToUtf16Buffer(byte* source, byte* sourceEnd, char* destination, char* destinationEnd)
        {
            if (sourceEnd <= source || destinationEnd <= destination)
                return destination;
            nuint length = MathHelper.Min(unchecked((nuint)(sourceEnd - source)), unchecked((nuint)(destinationEnd - destination)));
            return WriteToUtf16BufferCore(source, destination, length);
        }
    }
}
