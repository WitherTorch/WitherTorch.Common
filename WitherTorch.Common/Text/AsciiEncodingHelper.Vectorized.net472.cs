#if NET472_OR_GREATER
using System.Numerics;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class AsciiEncodingHelper
    {
        internal static unsafe partial byte* ReadFromUtf16BufferCore_OutOfAsciiRange(char* source, byte* destination, nuint length)
        {
            const byte ReplaceCharacter = 0x003F;

            char* sourceEnd = source + length;
            if (Limits.UseVector())
            {
                Vector<ushort>* sourceLimit = ((Vector<ushort>*)source) + 2;
                if (sourceLimit < sourceEnd)
                {
                    Vector<byte>* destinationLimit = ((Vector<byte>*)destination) + 1;
                    Vector<ushort> filterVector = new Vector<ushort>(AsciiEncodingLimit);
                    Vector<ushort> replaceVector = new Vector<ushort>(ReplaceCharacter);
                    do
                    {
                        Vector<ushort> sourceVectorLow = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source);
                        Vector<ushort> sourceVectorHigh = UnsafeHelper.ReadUnaligned<Vector<ushort>>(((Vector<ushort>*)source) + 1);
                        sourceVectorLow = Vector.ConditionalSelect(
                            condition: Vector.LessThanOrEqual(sourceVectorLow, filterVector),
                            left: sourceVectorLow,
                            right: replaceVector);
                        sourceVectorHigh = Vector.ConditionalSelect(
                            condition: Vector.LessThanOrEqual(sourceVectorHigh, filterVector),
                            left: sourceVectorLow,
                            right: replaceVector);
                        UnsafeHelper.WriteUnaligned(destination, Vector.Narrow(sourceVectorLow, sourceVectorHigh));
                        source = (char*)sourceLimit;
                        destination = (byte*)destinationLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            for (; source < sourceEnd; source++, destination++)
            {
                char c = *source;
                if (c > AsciiEncodingLimit)
                    *destination = ReplaceCharacter;
                else
                    *destination = unchecked((byte)c);
            }

        Result:
            return destination;
        }

        internal static unsafe partial byte* ReadFromUtf16BufferCore(char* source, byte* destination, nuint length)
        {
            char* sourceEnd = source + length;
            if (Limits.UseVector())
            {
                Vector<ushort>* sourceLimit = ((Vector<ushort>*)source) + 2;
                if (sourceLimit < sourceEnd)
                {
                    Vector<byte>* destinationLimit = ((Vector<byte>*)destination) + 1;
                    do
                    {
                        Vector<ushort> sourceVectorLow = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source);
                        Vector<ushort> sourceVectorHigh = UnsafeHelper.ReadUnaligned<Vector<ushort>>(((Vector<ushort>*)source) + 1);
                        UnsafeHelper.WriteUnaligned(destination, Vector.Narrow(sourceVectorLow, sourceVectorHigh));
                        source = (char*)sourceLimit;
                        destination = (byte*)destinationLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            for (; source < sourceEnd; source++, destination++)
                *destination = unchecked((byte)*source);

            Result:
            return destination;
        }

        internal static unsafe partial char* WriteToUtf16BufferCore(byte* source, char* destination, nuint length)
        {
            byte* sourceEnd = source + length;
            if (Limits.UseVector())
            {
                Vector<byte>* sourceLimit = ((Vector<byte>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector<ushort>* destinationLimit = ((Vector<ushort>*)destination) + 2;
                    do
                    {
                        Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(source);
                        Vector.Widen(sourceVector, out Vector<ushort> destVectorLow, out Vector<ushort> destVectorHigh);
                        UnsafeHelper.WriteUnaligned(destination, destVectorLow);
                        UnsafeHelper.WriteUnaligned(((Vector<ushort>*)destination) + 1, destVectorHigh);
                        source = (byte*)sourceLimit;
                        destination = (char*)destinationLimit;
                        destinationLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            for (; source < sourceEnd; source++, destination++)
                *destination = unchecked((char)*source);

            Result:
            return destination;
        }
    }
}
#endif