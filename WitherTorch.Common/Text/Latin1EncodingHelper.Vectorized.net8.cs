#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace WitherTorch.Common.Text
{
    partial class Latin1EncodingHelper
    {
        internal static unsafe partial byte* ReadFromUtf16BufferCore_OutOfLatin1Range(char* source, byte* destination, nuint length)
        {
            const byte ReplaceCharacter = 0x003F;

            char* sourceEnd = source + length;
            if (Limits.UseVector512())
            {
                Vector512<ushort>* sourceLimit = ((Vector512<ushort>*)source) + 2;
                if (sourceLimit < sourceEnd)
                {
                    Vector512<byte>* destinationLimit = ((Vector512<byte>*)destination) + 1;
                    Vector512<ushort> filterVector = Vector512.Create<ushort>(Latin1EncodingLimit);
                    Vector512<ushort> replaceVector = Vector512.Create<ushort>(ReplaceCharacter);
                    do
                    {
                        Vector512<ushort> sourceVectorLow = Vector512.Load((ushort*)source);
                        Vector512<ushort> sourceVectorHigh = Vector512.Load((ushort*)(((Vector512<ushort>*)source) + 1));
                        sourceVectorLow = Vector512.ConditionalSelect(
                            condition: Vector512.LessThanOrEqual(sourceVectorLow, filterVector),
                            left: sourceVectorLow,
                            right: replaceVector);
                        sourceVectorHigh = Vector512.ConditionalSelect(
                            condition: Vector512.LessThanOrEqual(sourceVectorHigh, filterVector),
                            left: sourceVectorLow,
                            right: replaceVector);
                        Vector512.Narrow(sourceVectorLow, sourceVectorHigh).Store(destination);
                        source = (char*)sourceLimit;
                        destination = (byte*)destinationLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<ushort>* sourceLimit = ((Vector256<ushort>*)source) + 2;
                if (sourceLimit < sourceEnd)
                {
                    Vector256<byte>* destinationLimit = ((Vector256<byte>*)destination) + 1;
                    Vector256<ushort> filterVector = Vector256.Create<ushort>(Latin1EncodingLimit);
                    Vector256<ushort> replaceVector = Vector256.Create<ushort>(ReplaceCharacter);
                    do
                    {
                        Vector256<ushort> sourceVectorLow = Vector256.Load((ushort*)source);
                        Vector256<ushort> sourceVectorHigh = Vector256.Load((ushort*)(((Vector256<ushort>*)source) + 1));
                        sourceVectorLow = Vector256.ConditionalSelect(
                            condition: Vector256.LessThanOrEqual(sourceVectorLow, filterVector),
                            left: sourceVectorLow,
                            right: replaceVector);
                        sourceVectorHigh = Vector256.ConditionalSelect(
                            condition: Vector256.LessThanOrEqual(sourceVectorHigh, filterVector),
                            left: sourceVectorLow,
                            right: replaceVector);
                        Vector256.Narrow(sourceVectorLow, sourceVectorHigh).Store(destination);
                        source = (char*)sourceLimit;
                        destination = (byte*)destinationLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<ushort>* sourceLimit = ((Vector128<ushort>*)source) + 2;
                if (sourceLimit < sourceEnd)
                {
                    Vector128<byte>* destinationLimit = ((Vector128<byte>*)destination) + 1;
                    Vector128<ushort> filterVector = Vector128.Create<ushort>(Latin1EncodingLimit);
                    Vector128<ushort> replaceVector = Vector128.Create<ushort>(ReplaceCharacter);
                    do
                    {
                        Vector128<ushort> sourceVectorLow = Vector128.Load((ushort*)source);
                        Vector128<ushort> sourceVectorHigh = Vector128.Load((ushort*)(((Vector128<ushort>*)source) + 1));
                        sourceVectorLow = Vector128.ConditionalSelect(
                            condition: Vector128.LessThanOrEqual(sourceVectorLow, filterVector),
                            left: sourceVectorLow,
                            right: replaceVector);
                        sourceVectorHigh = Vector128.ConditionalSelect(
                            condition: Vector128.LessThanOrEqual(sourceVectorHigh, filterVector),
                            left: sourceVectorLow,
                            right: replaceVector);
                        Vector128.Narrow(sourceVectorLow, sourceVectorHigh).Store(destination);
                        source = (char*)sourceLimit;
                        destination = (byte*)destinationLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector64())
            {
                Vector64<ushort>* sourceLimit = ((Vector64<ushort>*)source) + 2;
                if (sourceLimit < sourceEnd)
                {
                    Vector64<byte>* destinationLimit = ((Vector64<byte>*)destination) + 1;
                    Vector64<ushort> filterVector = Vector64.Create<ushort>(Latin1EncodingLimit);
                    Vector64<ushort> replaceVector = Vector64.Create<ushort>(ReplaceCharacter);
                    do
                    {
                        Vector64<ushort> sourceVectorLow = Vector64.Load((ushort*)source);
                        Vector64<ushort> sourceVectorHigh = Vector64.Load((ushort*)(((Vector64<ushort>*)source) + 1));
                        sourceVectorLow = Vector64.ConditionalSelect(
                            condition: Vector64.LessThanOrEqual(sourceVectorLow, filterVector),
                            left: sourceVectorLow,
                            right: replaceVector);
                        sourceVectorHigh = Vector64.ConditionalSelect(
                            condition: Vector64.LessThanOrEqual(sourceVectorHigh, filterVector),
                            left: sourceVectorLow,
                            right: replaceVector);
                        Vector64.Narrow(sourceVectorLow, sourceVectorHigh).Store(destination);
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
                if (c > Latin1EncodingLimit)
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
            if (Limits.UseVector512())
            {
                Vector512<ushort>* sourceLimit = ((Vector512<ushort>*)source) + 2;
                if (sourceLimit < sourceEnd)
                {
                    Vector512<byte>* destinationLimit = ((Vector512<byte>*)destination) + 1;
                    do
                    {
                        Vector512<ushort> sourceVectorLow = Vector512.Load((ushort*)source);
                        Vector512<ushort> sourceVectorHigh = Vector512.Load((ushort*)(((Vector512<ushort>*)source) + 1));
                        Vector512.Narrow(sourceVectorLow, sourceVectorHigh).Store(destination);
                        source = (char*)sourceLimit;
                        destination = (byte*)destinationLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<ushort>* sourceLimit = ((Vector256<ushort>*)source) + 2;
                if (sourceLimit < sourceEnd)
                {
                    Vector256<byte>* destinationLimit = ((Vector256<byte>*)destination) + 1;
                    do
                    {
                        Vector256<ushort> sourceVectorLow = Vector256.Load((ushort*)source);
                        Vector256<ushort> sourceVectorHigh = Vector256.Load((ushort*)(((Vector256<ushort>*)source) + 1));
                        Vector256.Narrow(sourceVectorLow, sourceVectorHigh).Store(destination);
                        source = (char*)sourceLimit;
                        destination = (byte*)destinationLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<ushort>* sourceLimit = ((Vector128<ushort>*)source) + 2;
                if (sourceLimit < sourceEnd)
                {
                    Vector128<byte>* destinationLimit = ((Vector128<byte>*)destination) + 1;
                    do
                    {
                        Vector128<ushort> sourceVectorLow = Vector128.Load((ushort*)source);
                        Vector128<ushort> sourceVectorHigh = Vector128.Load((ushort*)(((Vector128<ushort>*)source) + 1));
                        Vector128.Narrow(sourceVectorLow, sourceVectorHigh).Store(destination);
                        source = (char*)sourceLimit;
                        destination = (byte*)destinationLimit++;
                    } while ((sourceLimit += 2) < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector64())
            {
                Vector64<ushort>* sourceLimit = ((Vector64<ushort>*)source) + 2;
                if (sourceLimit < sourceEnd)
                {
                    Vector64<byte>* destinationLimit = ((Vector64<byte>*)destination) + 1;
                    do
                    {
                        Vector64<ushort> sourceVectorLow = Vector64.Load((ushort*)source);
                        Vector64<ushort> sourceVectorHigh = Vector64.Load((ushort*)(((Vector64<ushort>*)source) + 1));
                        Vector64.Narrow(sourceVectorLow, sourceVectorHigh).Store(destination);
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
            if (Limits.UseVector512())
            {
                Vector512<byte>* sourceLimit = ((Vector512<byte>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector512<ushort>* destinationLimit = ((Vector512<ushort>*)destination) + 2;
                    do
                    {
                        Vector512<byte> sourceVector = Vector512.Load(source);
                        (Vector512<ushort> destVectorLow, Vector512<ushort> destVectorHigh) = Vector512.Widen(sourceVector);
                        destVectorLow.Store((ushort*)destination);
                        destVectorHigh.Store((ushort*)(((Vector512<ushort>*)destination) + 1));
                        source = (byte*)sourceLimit;
                        destination = (char*)destinationLimit;
                        destinationLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<byte>* sourceLimit = ((Vector256<byte>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector256<ushort>* destinationLimit = ((Vector256<ushort>*)destination) + 2;
                    do
                    {
                        Vector256<byte> sourceVector = Vector256.Load(source);
                        (Vector256<ushort> destVectorLow, Vector256<ushort> destVectorHigh) = Vector256.Widen(sourceVector);
                        destVectorLow.Store((ushort*)destination);
                        destVectorHigh.Store((ushort*)(((Vector256<ushort>*)destination) + 1));
                        source = (byte*)sourceLimit;
                        destination = (char*)destinationLimit;
                        destinationLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<byte>* sourceLimit = ((Vector128<byte>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector128<ushort>* destinationLimit = ((Vector128<ushort>*)destination) + 2;
                    do
                    {
                        Vector128<byte> sourceVector = Vector128.Load(source);
                        (Vector128<ushort> destVectorLow, Vector128<ushort> destVectorHigh) = Vector128.Widen(sourceVector);
                        destVectorLow.Store((ushort*)destination);
                        destVectorHigh.Store((ushort*)(((Vector128<ushort>*)destination) + 1));
                        source = (byte*)sourceLimit;
                        destination = (char*)destinationLimit;
                        destinationLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector64())
            {
                Vector64<byte>* sourceLimit = ((Vector64<byte>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector64<ushort>* destinationLimit = ((Vector64<ushort>*)destination) + 2;
                    do
                    {
                        Vector64<byte> sourceVector = Vector64.Load(source);
                        (Vector64<ushort> destVectorLow, Vector64<ushort> destVectorHigh) = Vector64.Widen(sourceVector);
                        destVectorLow.Store((ushort*)destination);
                        destVectorHigh.Store((ushort*)(((Vector64<ushort>*)destination) + 1));
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