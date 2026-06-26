using System.Runtime.CompilerServices;

using InlineMethod;

using RiceTea.Core.Helpers;

namespace RiceTea.Core.Text;

public static partial class AsciiEncodingHelper
{
    // ASCII 編碼邊界
    public const char AsciiEncodingLimit = '\u007F';
    public const byte AsciiEncodingLimit_InByte = 0x007F;

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetWorstCaseForEncodeLength(int length) => length;

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetWorstCaseForEncodeLength(uint length) => length;

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint GetWorstCaseForEncodeLength(nuint length) => length;

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetWorstCaseForDecodeLength(int length) => length;

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetWorstCaseForDecodeLength(uint length) => length;

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint GetWorstCaseForDecodeLength(nuint length) => length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe byte* ReadFromUtf16Buffer(char* source, nuint sourceLength, byte* destination, nuint destinationLength)
    {
        if (sourceLength == 0 || destinationLength == 0)
            return destination;
        nuint length = MathHelper.Min(sourceLength, destinationLength);
        if (SequenceHelper.ContainsGreaterThan(source, length, AsciiEncodingLimit))
            return ReadFromUtf16BufferCore_OutOfAsciiRange(source, destination, length);
        return ReadFromUtf16BufferCore(source, destination, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe byte* ReadFromUtf16Buffer(char* source, char* sourceEnd, byte* destination, byte* destinationEnd)
    {
        if (sourceEnd <= source || destinationEnd <= destination)
            return destination;
        nuint length = MathHelper.Min(unchecked((nuint)(sourceEnd - source)), unchecked((nuint)(destinationEnd - destination)));
        if (SequenceHelper.ContainsGreaterThan(source, length, AsciiEncodingLimit))
            return ReadFromUtf16BufferCore_OutOfAsciiRange(source, destination, length);
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
