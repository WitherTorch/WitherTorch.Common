#if NET8_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf8EncodingHelper
    {
        private static unsafe partial bool HasSurrogateCharacters(char* source, nuint length)
        {
            char* sourceEnd = source + length;

            if (Limits.UseVector512())
            {
                Vector512<ushort>* sourceLimit = ((Vector512<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector512<ushort> zeroVector = Vector512<ushort>.Zero;
                    Vector512<ushort> surrogateStartVector = Vector512.Create<ushort>(Utf16LeadSurrogateStart);
                    Vector512<ushort> surrogateEndVector = Vector512.Create<ushort>(Utf16LeadSurrogateEnd);
                    do
                    {
                        Vector512<ushort> sourceVector = Vector512.Load((ushort*)source);
                        Vector512<ushort> resultVector = Vector512.GreaterThanOrEqual(sourceVector, surrogateStartVector) &
                            Vector512.LessThanOrEqual(sourceVector, surrogateEndVector);
                        if (resultVector != zeroVector)
                            return true;
                        source = (char*)sourceLimit;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<ushort>* sourceLimit = ((Vector256<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector256<ushort> zeroVector = Vector256<ushort>.Zero;
                    Vector256<ushort> surrogateStartVector = Vector256.Create<ushort>(Utf16LeadSurrogateStart);
                    Vector256<ushort> surrogateEndVector = Vector256.Create<ushort>(Utf16LeadSurrogateEnd);
                    do
                    {
                        Vector256<ushort> sourceVector = Vector256.Load((ushort*)source);
                        Vector256<ushort> resultVector = Vector256.GreaterThanOrEqual(sourceVector, surrogateStartVector) &
                            Vector256.LessThanOrEqual(sourceVector, surrogateEndVector);
                        if (resultVector != zeroVector)
                            return true;
                        source = (char*)sourceLimit;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<ushort>* sourceLimit = ((Vector128<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector128<ushort> zeroVector = Vector128<ushort>.Zero;
                    Vector128<ushort> surrogateStartVector = Vector128.Create<ushort>(Utf16LeadSurrogateStart);
                    Vector128<ushort> surrogateEndVector = Vector128.Create<ushort>(Utf16LeadSurrogateEnd);
                    do
                    {
                        Vector128<ushort> sourceVector = Vector128.Load((ushort*)source);
                        Vector128<ushort> resultVector = Vector128.GreaterThanOrEqual(sourceVector, surrogateStartVector) &
                            Vector128.LessThanOrEqual(sourceVector, surrogateEndVector);
                        if (resultVector != zeroVector)
                            return true;
                        source = (char*)sourceLimit;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }

            for (; source < sourceEnd; source++)
            {
                if (IsLeadSurrogate(*source))
                    return true;
            }

        Result:
            return false;
        }

        private static unsafe partial uint* TryWriteUtf16ToUtf32BufferCore_HasSurrogate(char* source, uint* destination, nuint length)
        {
            char* sourceEnd = source + length;
            if (Limits.UseVector512())
            {
                Vector512<ushort>* sourceLimit = ((Vector512<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector512<ushort> zeroVector = Vector512<ushort>.Zero;
                    Vector512<ushort> leadSurrogateStartVector = Vector512.Create<ushort>(Utf16LeadSurrogateStart);
                    Vector512<ushort> leadSurrogateEndVector = Vector512.Create<ushort>(Utf16LeadSurrogateEnd);
                    Vector512<uint>* destinationLimit = ((Vector512<uint>*)destination) + 2;
                    do
                    {
                        Vector512<ushort> sourceVector = Vector512.Load((ushort*)source);
                        Vector512<ushort> selectionVector = Vector512.LessThan(sourceVector, leadSurrogateStartVector) &
                            Vector512.GreaterThan(sourceVector, leadSurrogateEndVector);
                        (Vector512<uint> destVectorLow, Vector512<uint> destVectorHigh) = Vector512.Widen(sourceVector & selectionVector);
                        destVectorLow.Store(destination);
                        destVectorHigh.Store(destination + Vector512<uint>.Count);
                        if (selectionVector != zeroVector)
                        {
                            for (int i = 0; i < Vector512<ushort>.Count - 1; i++)
                            {
                                if (selectionVector[i] != 0)
                                    continue;

                                int trailIndex = i + 1;
                                char leadSurrogate = unchecked((char)sourceVector[i]);
                                char trailSurrogate = unchecked((char)sourceVector[trailIndex]);
                                if (IsTrailSurrogate(trailSurrogate))
                                {
                                    destination[i] = ComposeSurrogatePair(leadSurrogate, trailSurrogate);
                                    destination[trailSurrogate] = 0;
                                    i = trailSurrogate;
                                    continue;
                                }
                                destination[i] = leadSurrogate;
                            }

                            if (selectionVector[Vector512<ushort>.Count - 1] == 0)
                            {
                                sourceLimit = (Vector512<ushort>*)(((char*)sourceLimit) - 1);
                                destinationLimit = (Vector512<uint>*)(((uint*)sourceLimit) - 1);
                            }
                        }
                        source = (char*)sourceLimit;
                        destination = (uint*)destinationLimit;
                        destinationLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<ushort>* sourceLimit = ((Vector256<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector256<ushort> zeroVector = Vector256<ushort>.Zero;
                    Vector256<ushort> leadSurrogateStartVector = Vector256.Create<ushort>(Utf16LeadSurrogateStart);
                    Vector256<ushort> leadSurrogateEndVector = Vector256.Create<ushort>(Utf16LeadSurrogateEnd);
                    Vector256<uint>* destinationLimit = ((Vector256<uint>*)destination) + 2;
                    do
                    {
                        Vector256<ushort> sourceVector = Vector256.Load((ushort*)source);
                        Vector256<ushort> selectionVector = Vector256.LessThan(sourceVector, leadSurrogateStartVector) &
                            Vector256.GreaterThan(sourceVector, leadSurrogateEndVector);
                        (Vector256<uint> destVectorLow, Vector256<uint> destVectorHigh) = Vector256.Widen(sourceVector & selectionVector);
                        destVectorLow.Store(destination);
                        destVectorHigh.Store(destination + Vector256<uint>.Count);
                        if (selectionVector != zeroVector)
                        {
                            for (int i = 0; i < Vector256<ushort>.Count - 1; i++)
                            {
                                if (selectionVector[i] != 0)
                                    continue;

                                int trailIndex = i + 1;
                                char leadSurrogate = unchecked((char)sourceVector[i]);
                                char trailSurrogate = unchecked((char)sourceVector[trailIndex]);
                                if (IsTrailSurrogate(trailSurrogate))
                                {
                                    destination[i] = ComposeSurrogatePair(leadSurrogate, trailSurrogate);
                                    destination[trailSurrogate] = 0;
                                    i = trailSurrogate;
                                    continue;
                                }
                                destination[i] = leadSurrogate;
                            }

                            if (selectionVector[Vector256<ushort>.Count - 1] == 0)
                            {
                                sourceLimit = (Vector256<ushort>*)(((char*)sourceLimit) - 1);
                                destinationLimit = (Vector256<uint>*)(((uint*)sourceLimit) - 1);
                            }
                        }
                        source = (char*)sourceLimit;
                        destination = (uint*)destinationLimit;
                        destinationLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<ushort>* sourceLimit = ((Vector128<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector128<ushort> zeroVector = Vector128<ushort>.Zero;
                    Vector128<ushort> leadSurrogateStartVector = Vector128.Create<ushort>(Utf16LeadSurrogateStart);
                    Vector128<ushort> leadSurrogateEndVector = Vector128.Create<ushort>(Utf16LeadSurrogateEnd);
                    Vector128<uint>* destinationLimit = ((Vector128<uint>*)destination) + 2;
                    do
                    {
                        Vector128<ushort> sourceVector = Vector128.Load((ushort*)source);
                        Vector128<ushort> selectionVector = Vector128.LessThan(sourceVector, leadSurrogateStartVector) &
                            Vector128.GreaterThan(sourceVector, leadSurrogateEndVector);
                        (Vector128<uint> destVectorLow, Vector128<uint> destVectorHigh) = Vector128.Widen(sourceVector & selectionVector);
                        destVectorLow.Store(destination);
                        destVectorHigh.Store(destination + Vector128<uint>.Count);
                        if (selectionVector != zeroVector)
                        {
                            for (int i = 0; i < Vector128<ushort>.Count - 1; i++)
                            {
                                if (selectionVector[i] != 0)
                                    continue;

                                int trailIndex = i + 1;
                                char leadSurrogate = unchecked((char)sourceVector[i]);
                                char trailSurrogate = unchecked((char)sourceVector[trailIndex]);
                                if (IsTrailSurrogate(trailSurrogate))
                                {
                                    destination[i] = ComposeSurrogatePair(leadSurrogate, trailSurrogate);
                                    destination[trailSurrogate] = 0;
                                    i = trailSurrogate;
                                    continue;
                                }
                                destination[i] = leadSurrogate;
                            }

                            if (selectionVector[Vector128<ushort>.Count - 1] == 0)
                            {
                                sourceLimit = (Vector128<ushort>*)(((char*)sourceLimit) - 1);
                                destinationLimit = (Vector128<uint>*)(((uint*)sourceLimit) - 1);
                            }
                        }
                        source = (char*)sourceLimit;
                        destination = (uint*)destinationLimit;
                        destinationLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }

            while ((source = TryReadUtf16Character(source, sourceEnd, out uint unicodeValue)) != null)
                *destination++ = unicodeValue;

            Result:
            return destination;
        }

        private static unsafe partial uint* TryWriteUtf16ToUtf32BufferCore(char* source, uint* destination, nuint length)
        {
            char* sourceEnd = source + length;
            if (Limits.UseVector512())
            {
                Vector512<ushort>* sourceLimit = ((Vector512<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector512<uint>* destinationLimit = ((Vector512<uint>*)destination) + 2;
                    do
                    {
                        Vector512<ushort> sourceVector = Vector512.Load((ushort*)source);
                        (Vector512<uint> destVectorLow, Vector512<uint> destVectorHigh) = Vector512.Widen(sourceVector);
                        destVectorLow.Store(destination);
                        destVectorHigh.Store(destination + Vector512<uint>.Count);
                        source = (char*)sourceLimit;
                        destination = (uint*)destinationLimit;
                        destinationLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<ushort>* sourceLimit = ((Vector256<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector256<uint>* destinationLimit = ((Vector256<uint>*)destination) + 2;
                    do
                    {
                        Vector256<ushort> sourceVector = Vector256.Load((ushort*)source);
                        (Vector256<uint> destVectorLow, Vector256<uint> destVectorHigh) = Vector256.Widen(sourceVector);
                        destVectorLow.Store(destination);
                        destVectorHigh.Store(destination + Vector256<uint>.Count);
                        source = (char*)sourceLimit;
                        destination = (uint*)destinationLimit;
                        destinationLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<ushort>* sourceLimit = ((Vector128<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector128<uint>* destinationLimit = ((Vector128<uint>*)destination) + 2;
                    do
                    {
                        Vector128<ushort> sourceVector = Vector128.Load((ushort*)source);
                        (Vector128<uint> destVectorLow, Vector128<uint> destVectorHigh) = Vector128.Widen(sourceVector);
                        destVectorLow.Store(destination);
                        destVectorHigh.Store(destination + Vector128<uint>.Count);
                        source = (char*)sourceLimit;
                        destination = (uint*)destinationLimit;
                        destinationLimit += 2;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            for (; source < sourceEnd; source++, destination++)
                *destination = *source;

            Result:
            return destination;
        }

        private static unsafe partial uint* TryWriteUtf8ToUtf32BufferCore(byte* source, uint* destination, nuint length)
        {
            byte* sourceEnd = source + length;
            if (Limits.UseVector512())
            {
                Vector512<byte>* sourceLimit = ((Vector512<byte>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector512<byte> zeroVector = Vector512<byte>.Zero;
                    Vector512<byte> fullVector = Vector512.Create<byte>(byte.MaxValue);

                    Vector512<byte> section1LimitVector = Vector512.Create<byte>(Latin1EncodingHelper.AsciiEncodingLimit_InByte);

                    Vector512<byte> section2StartVector = Vector512.Create<byte>(Utf8Section2Head);
                    Vector512<byte> section2EndVector = Vector512.Create<byte>(Utf8Section2Head | Utf8Section2Mask);
                    Vector512<byte> section2SelectionMaskVector = CreateTailZeroVector(fullVector, 1);

                    Vector512<byte> section3StartVector = Vector512.Create<byte>(Utf8Section3Head);
                    Vector512<byte> section3EndVector = Vector512.Create<byte>(Utf8Section3Head | Utf8Section3Mask);
                    Vector512<byte> section3SelectionMaskVector = CreateTailZeroVector(fullVector, 2);

                    Vector512<byte> section4StartVector = Vector512.Create<byte>(Utf8Section4Head);
                    Vector512<byte> section4EndVector = Vector512.Create<byte>(Utf8Section4Head | Utf8Section4Mask);
                    Vector512<byte> section4SelectionMaskVector = CreateTailZeroVector(fullVector, 3);

                    Vector512<uint>* destinationLimit = ((Vector512<uint>*)destination) + 4;
                    Vector512<uint>* widenedVectors = stackalloc Vector512<uint>[4];
                    do
                    {
                        Vector512<byte> sourceVector = Vector512.Load(source);
                        Vector512<byte> selectionVector = Vector512.LessThan(sourceVector, section1LimitVector);

                        // Section 1: U+0001 ~ U+007F (1 Byte)
                        WidenVectorTwice(
                            sourceVector: sourceVector & selectionVector,
                            destinationVectorBuffer: (Vector512<uint>*)destination
                            );

                        // Section 2: U+0080 ~ U+07FF (2 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector512.GreaterThanOrEqual(sourceVector, section2StartVector) &
                            Vector512.LessThanOrEqual(sourceVector, section2EndVector) & section2SelectionMaskVector;
                        if (selectionVector != zeroVector)
                            DoSection2Decode(in sourceVector, ref selectionVector, destination);

                        // Section 3: U+0800 ~ U+FFFF (3 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector512.GreaterThanOrEqual(sourceVector, section3StartVector) &
                            Vector512.LessThanOrEqual(sourceVector, section3EndVector) & section3SelectionMaskVector;
                        if (selectionVector != zeroVector)
                            DoSection3Decode(in sourceVector, ref selectionVector, destination);

                        // Section 4: U+010000 ~ U+10FFFF (4 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector512.GreaterThanOrEqual(sourceVector, section4StartVector) &
                            Vector512.LessThanOrEqual(sourceVector, section4EndVector) & section2SelectionMaskVector; ;
                        if (selectionVector != zeroVector)
                            DoSection4Decode(in sourceVector, ref selectionVector, destination);

                        // Section "Error"
                        sourceVector &= ~selectionVector;
                        if (sourceVector != zeroVector)
                        {
                            nuint offset = GetOffsetForVectorTail(sourceVector);
                            WriteErrorCodePointToDestinationVector(sourceVector, destination);
                            destinationLimit = (Vector512<uint>*)(((uint*)destinationLimit) - offset);
                            sourceLimit = (Vector512<byte>*)(((byte*)sourceLimit) - offset);
                        }

                    LoopEnd:
                        destination = (uint*)destinationLimit;
                        destinationLimit += 4;
                        source = (byte*)sourceLimit;
                    }
                    while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector256())
            {
                Vector256<byte>* sourceLimit = ((Vector256<byte>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector256<byte> zeroVector = Vector256<byte>.Zero;
                    Vector256<byte> fullVector = Vector256.Create<byte>(byte.MaxValue);

                    Vector256<byte> section1LimitVector = Vector256.Create<byte>(Latin1EncodingHelper.AsciiEncodingLimit_InByte);

                    Vector256<byte> section2StartVector = Vector256.Create<byte>(Utf8Section2Head);
                    Vector256<byte> section2EndVector = Vector256.Create<byte>(Utf8Section2Head | Utf8Section2Mask);
                    Vector256<byte> section2SelectionMaskVector = CreateTailZeroVector(fullVector, 1);

                    Vector256<byte> section3StartVector = Vector256.Create<byte>(Utf8Section3Head);
                    Vector256<byte> section3EndVector = Vector256.Create<byte>(Utf8Section3Head | Utf8Section3Mask);
                    Vector256<byte> section3SelectionMaskVector = CreateTailZeroVector(fullVector, 2);

                    Vector256<byte> section4StartVector = Vector256.Create<byte>(Utf8Section4Head);
                    Vector256<byte> section4EndVector = Vector256.Create<byte>(Utf8Section4Head | Utf8Section4Mask);
                    Vector256<byte> section4SelectionMaskVector = CreateTailZeroVector(fullVector, 3);

                    Vector256<uint>* destinationLimit = ((Vector256<uint>*)destination) + 4;
                    Vector256<uint>* widenedVectors = stackalloc Vector256<uint>[4];
                    do
                    {
                        Vector256<byte> sourceVector = Vector256.Load(source);
                        Vector256<byte> selectionVector = Vector256.LessThan(sourceVector, section1LimitVector);

                        // Section 1: U+0001 ~ U+007F (1 Byte)
                        WidenVectorTwice(
                            sourceVector: sourceVector & selectionVector,
                            destinationVectorBuffer: (Vector256<uint>*)destination
                            );

                        // Section 2: U+0080 ~ U+07FF (2 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector256.GreaterThanOrEqual(sourceVector, section2StartVector) &
                            Vector256.LessThanOrEqual(sourceVector, section2EndVector) & section2SelectionMaskVector;
                        if (selectionVector != zeroVector)
                            DoSection2Decode(in sourceVector, ref selectionVector, destination);

                        // Section 3: U+0800 ~ U+FFFF (3 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector256.GreaterThanOrEqual(sourceVector, section3StartVector) &
                            Vector256.LessThanOrEqual(sourceVector, section3EndVector) & section3SelectionMaskVector;
                        if (selectionVector != zeroVector)
                            DoSection3Decode(in sourceVector, ref selectionVector, destination);

                        // Section 4: U+010000 ~ U+10FFFF (4 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector256.GreaterThanOrEqual(sourceVector, section4StartVector) &
                            Vector256.LessThanOrEqual(sourceVector, section4EndVector) & section2SelectionMaskVector; ;
                        if (selectionVector != zeroVector)
                            DoSection4Decode(in sourceVector, ref selectionVector, destination);

                        // Section "Error"
                        sourceVector &= ~selectionVector;
                        if (sourceVector != zeroVector)
                        {
                            nuint offset = GetOffsetForVectorTail(sourceVector);
                            WriteErrorCodePointToDestinationVector(sourceVector, destination);
                            destinationLimit = (Vector256<uint>*)(((uint*)destinationLimit) - offset);
                            sourceLimit = (Vector256<byte>*)(((byte*)sourceLimit) - offset);
                        }

                    LoopEnd:
                        destination = (uint*)destinationLimit;
                        destinationLimit += 4;
                        source = (byte*)sourceLimit;
                    }
                    while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }
            if (Limits.UseVector128())
            {
                Vector128<byte>* sourceLimit = ((Vector128<byte>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector128<byte> zeroVector = Vector128<byte>.Zero;
                    Vector128<byte> fullVector = Vector128.Create<byte>(byte.MaxValue);

                    Vector128<byte> section1LimitVector = Vector128.Create<byte>(Latin1EncodingHelper.AsciiEncodingLimit_InByte);

                    Vector128<byte> section2StartVector = Vector128.Create<byte>(Utf8Section2Head);
                    Vector128<byte> section2EndVector = Vector128.Create<byte>(Utf8Section2Head | Utf8Section2Mask);
                    Vector128<byte> section2SelectionMaskVector = CreateTailZeroVector(fullVector, 1);

                    Vector128<byte> section3StartVector = Vector128.Create<byte>(Utf8Section3Head);
                    Vector128<byte> section3EndVector = Vector128.Create<byte>(Utf8Section3Head | Utf8Section3Mask);
                    Vector128<byte> section3SelectionMaskVector = CreateTailZeroVector(fullVector, 2);

                    Vector128<byte> section4StartVector = Vector128.Create<byte>(Utf8Section4Head);
                    Vector128<byte> section4EndVector = Vector128.Create<byte>(Utf8Section4Head | Utf8Section4Mask);
                    Vector128<byte> section4SelectionMaskVector = CreateTailZeroVector(fullVector, 3);

                    Vector128<uint>* destinationLimit = ((Vector128<uint>*)destination) + 4;
                    Vector128<uint>* widenedVectors = stackalloc Vector128<uint>[4];
                    do
                    {
                        Vector128<byte> sourceVector = Vector128.Load(source);
                        Vector128<byte> selectionVector = Vector128.LessThan(sourceVector, section1LimitVector);

                        // Section 1: U+0001 ~ U+007F (1 Byte)
                        WidenVectorTwice(
                            sourceVector: sourceVector & selectionVector,
                            destinationVectorBuffer: (Vector128<uint>*)destination
                            );

                        // Section 2: U+0080 ~ U+07FF (2 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector128.GreaterThanOrEqual(sourceVector, section2StartVector) &
                            Vector128.LessThanOrEqual(sourceVector, section2EndVector) & section2SelectionMaskVector;
                        if (selectionVector != zeroVector)
                            DoSection2Decode(in sourceVector, ref selectionVector, destination);

                        // Section 3: U+0800 ~ U+FFFF (3 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector128.GreaterThanOrEqual(sourceVector, section3StartVector) &
                            Vector128.LessThanOrEqual(sourceVector, section3EndVector) & section3SelectionMaskVector;
                        if (selectionVector != zeroVector)
                            DoSection3Decode(in sourceVector, ref selectionVector, destination);

                        // Section 4: U+010000 ~ U+10FFFF (4 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector128.GreaterThanOrEqual(sourceVector, section4StartVector) &
                            Vector128.LessThanOrEqual(sourceVector, section4EndVector) & section2SelectionMaskVector; ;
                        if (selectionVector != zeroVector)
                            DoSection4Decode(in sourceVector, ref selectionVector, destination);

                        // Section "Error"
                        sourceVector &= ~selectionVector;
                        if (sourceVector != zeroVector)
                        {
                            nuint offset = GetOffsetForVectorTail(sourceVector);
                            WriteErrorCodePointToDestinationVector(sourceVector, destination);
                            destinationLimit = (Vector128<uint>*)(((uint*)destinationLimit) - offset);
                            sourceLimit = (Vector128<byte>*)(((byte*)sourceLimit) - offset);
                        }

                    LoopEnd:
                        destination = (uint*)destinationLimit;
                        destinationLimit += 4;
                        source = (byte*)sourceLimit;
                    }
                    while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        goto Result;
                }
            }

            while ((source = TryReadUtf8Character(source, sourceEnd, out uint unicodeValue)) != null)
                *destination++ = unicodeValue;

            Result:
            return destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void DoSection2Decode(in Vector512<byte> sourceVector, ref Vector512<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector512<byte>.Count - 1; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                int trailIndex = i + 1;
                byte trailingByte = sourceVector[trailIndex];
                if (IsTrailByte(trailingByte))
                {
                    destination[i] = unchecked(((uint)sourceVector[i] & Utf8Section2Mask) << 6 | DecodeTrailByte(trailingByte));
                    destination[trailIndex] = 0;
                    SetVectorSlotsToFull(ref selectionVector, trailIndex);
                    i = trailIndex;
                    continue;
                }
                destination[i] = Utf8DecodeErrorCodePoint;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void DoSection2Decode(in Vector256<byte> sourceVector, ref Vector256<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector256<byte>.Count - 1; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                int trailIndex = i + 1;
                byte trailingByte = sourceVector[trailIndex];
                if (IsTrailByte(trailingByte))
                {
                    destination[i] = unchecked(((uint)sourceVector[i] & Utf8Section2Mask) << 6 | DecodeTrailByte(trailingByte));
                    destination[trailIndex] = 0;
                    SetVectorSlotsToFull(ref selectionVector, trailIndex);
                    i = trailIndex;
                    continue;
                }
                destination[i] = Utf8DecodeErrorCodePoint;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void DoSection2Decode(in Vector128<byte> sourceVector, ref Vector128<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector128<byte>.Count - 1; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                int trailIndex = i + 1;
                byte trailingByte = sourceVector[trailIndex];
                if (IsTrailByte(trailingByte))
                {
                    destination[i] = unchecked(((uint)sourceVector[i] & Utf8Section2Mask) << 6 | DecodeTrailByte(trailingByte));
                    destination[trailIndex] = 0;
                    SetVectorSlotsToFull(ref selectionVector, trailIndex);
                    i = trailIndex;
                    continue;
                }
                destination[i] = Utf8DecodeErrorCodePoint;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void DoSection3Decode(in Vector512<byte> sourceVector, ref Vector512<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector512<byte>.Count - 2; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                int trailIndex = i + 1, trailIndex2 = i + 2;
                byte trailingByte = sourceVector[trailIndex];
                byte trailingByte2 = sourceVector[trailIndex2];
                if (IsTrailByte(trailingByte) && IsTrailByte(trailingByte2))
                {
                    destination[i] = unchecked(((uint)sourceVector[i] & Utf8Section3Mask) << 12 | DecodeTrailByte(trailingByte) << 6 | DecodeTrailByte(trailingByte2));
                    destination[trailIndex] = 0;
                    destination[trailIndex2] = 0;
                    SetVectorSlotsToFull(ref selectionVector, trailIndex, trailIndex2);
                    i = trailIndex2;
                    continue;
                }
                destination[i] = Utf8DecodeErrorCodePoint;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void DoSection3Decode(in Vector256<byte> sourceVector, ref Vector256<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector256<byte>.Count - 2; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                int trailIndex = i + 1, trailIndex2 = i + 2;
                byte trailingByte = sourceVector[trailIndex];
                byte trailingByte2 = sourceVector[trailIndex2];
                if (IsTrailByte(trailingByte) && IsTrailByte(trailingByte2))
                {
                    destination[i] = unchecked(((uint)sourceVector[i] & Utf8Section3Mask) << 12 | DecodeTrailByte(trailingByte) << 6 | DecodeTrailByte(trailingByte2));
                    destination[trailIndex] = 0;
                    destination[trailIndex2] = 0;
                    SetVectorSlotsToFull(ref selectionVector, trailIndex, trailIndex2);
                    i = trailIndex2;
                    continue;
                }
                destination[i] = Utf8DecodeErrorCodePoint;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void DoSection3Decode(in Vector128<byte> sourceVector, ref Vector128<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector128<byte>.Count - 2; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                int trailIndex = i + 1, trailIndex2 = i + 2;
                byte trailingByte = sourceVector[trailIndex];
                byte trailingByte2 = sourceVector[trailIndex2];
                if (IsTrailByte(trailingByte) && IsTrailByte(trailingByte2))
                {
                    destination[i] = unchecked(((uint)sourceVector[i] & Utf8Section3Mask) << 12 | DecodeTrailByte(trailingByte) << 6 | DecodeTrailByte(trailingByte2));
                    destination[trailIndex] = 0;
                    destination[trailIndex2] = 0;
                    SetVectorSlotsToFull(ref selectionVector, trailIndex, trailIndex2);
                    i = trailIndex2;
                    continue;
                }
                destination[i] = Utf8DecodeErrorCodePoint;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void DoSection4Decode(in Vector512<byte> sourceVector, ref Vector512<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector512<byte>.Count - 3; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                int trailIndex = i + 1, trailIndex2 = i + 3, trailIndex3 = i + 3;
                byte trailingByte = sourceVector[trailIndex];
                byte trailingByte2 = sourceVector[trailIndex2];
                byte trailingByte3 = sourceVector[trailIndex3];
                if (IsTrailByte(trailingByte) && IsTrailByte(trailingByte2) && IsTrailByte(trailingByte3))
                {
                    uint unicodeValue = unchecked(((uint)sourceVector[i] & Utf8Section4Mask) << 18 | DecodeTrailByte(trailingByte) << 12 |
                        DecodeTrailByte(trailingByte2) << 6 | DecodeTrailByte(trailingByte3));
                    destination[i] = unicodeValue > Utf8EncodingLimit ? Utf8DecodeErrorCodePoint : unicodeValue;
                    destination[trailIndex] = 0;
                    destination[trailIndex2] = 0;
                    destination[trailIndex3] = 0;
                    SetVectorSlotsToFull(ref selectionVector, trailIndex, trailIndex2, trailIndex3);
                    i = trailIndex3;
                    continue;
                }
                destination[i] = Utf8DecodeErrorCodePoint;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void DoSection4Decode(in Vector256<byte> sourceVector, ref Vector256<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector256<byte>.Count - 3; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                int trailIndex = i + 1, trailIndex2 = i + 3, trailIndex3 = i + 3;
                byte trailingByte = sourceVector[trailIndex];
                byte trailingByte2 = sourceVector[trailIndex2];
                byte trailingByte3 = sourceVector[trailIndex3];
                if (IsTrailByte(trailingByte) && IsTrailByte(trailingByte2) && IsTrailByte(trailingByte3))
                {
                    uint unicodeValue = unchecked(((uint)sourceVector[i] & Utf8Section4Mask) << 18 | DecodeTrailByte(trailingByte) << 12 |
                        DecodeTrailByte(trailingByte2) << 6 | DecodeTrailByte(trailingByte3));
                    destination[i] = unicodeValue > Utf8EncodingLimit ? Utf8DecodeErrorCodePoint : unicodeValue;
                    destination[trailIndex] = 0;
                    destination[trailIndex2] = 0;
                    destination[trailIndex3] = 0;
                    SetVectorSlotsToFull(ref selectionVector, trailIndex, trailIndex2, trailIndex3);
                    i = trailIndex3;
                    continue;
                }
                destination[i] = Utf8DecodeErrorCodePoint;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void DoSection4Decode(in Vector128<byte> sourceVector, ref Vector128<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector128<byte>.Count - 3; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                int trailIndex = i + 1, trailIndex2 = i + 3, trailIndex3 = i + 3;
                byte trailingByte = sourceVector[trailIndex];
                byte trailingByte2 = sourceVector[trailIndex2];
                byte trailingByte3 = sourceVector[trailIndex3];
                if (IsTrailByte(trailingByte) && IsTrailByte(trailingByte2) && IsTrailByte(trailingByte3))
                {
                    uint unicodeValue = unchecked(((uint)sourceVector[i] & Utf8Section4Mask) << 18 | DecodeTrailByte(trailingByte) << 12 |
                        DecodeTrailByte(trailingByte2) << 6 | DecodeTrailByte(trailingByte3));
                    destination[i] = unicodeValue > Utf8EncodingLimit ? Utf8DecodeErrorCodePoint : unicodeValue;
                    destination[trailIndex] = 0;
                    destination[trailIndex2] = 0;
                    destination[trailIndex3] = 0;
                    SetVectorSlotsToFull(ref selectionVector, trailIndex, trailIndex2, trailIndex3);
                    i = trailIndex3;
                    continue;
                }
                destination[i] = Utf8DecodeErrorCodePoint;
            }
        }

        private static unsafe partial void EncodeUtf32ToUtf8(uint* source, nuint length)
        {
            uint* sourceEnd = source + length;
            if (Limits.UseVector128())
            {
                Vector128<uint>* sourceLimit = ((Vector128<uint>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector128<uint> zeroVector = Vector128<uint>.Zero;
                    Vector128<uint> section1LimitVector = Vector128.Create<uint>(Utf8Section1Limit);
                    Vector128<uint> section2LimitVector = Vector128.Create<uint>(Utf8Section2Limit);
                    Vector128<uint> section3LimitVector = Vector128.Create<uint>(Utf8Section3Limit);
                    do
                    {
                        Vector128<uint> sourceVector = Vector128.Load(source);

                        // Section 1 (U+0000 ~ U+007F)
                        Vector128<uint> selectionVector = Vector128.GreaterThan(sourceVector, section1LimitVector);
                        if (selectionVector == zeroVector)
                            goto LoopEnd;

                        // Section 2 (U+0080 ~ U+07FF)
                        Vector128<uint> newSelectionVector = Vector128.GreaterThan(sourceVector, section2LimitVector);
                        if (selectionVector != newSelectionVector)
                        {
                            sourceVector = DoSection2Encode(sourceVector, selectionVector & ~newSelectionVector);
                            if (newSelectionVector == zeroVector)
                                goto LoopEnd_Dirty;
                            selectionVector = newSelectionVector;
                        }

                        // Section 3 (U+0800 ~ U+FFFF)
                        newSelectionVector = Vector128.GreaterThan(sourceVector, section3LimitVector);
                        if (selectionVector != newSelectionVector)
                        {
                            sourceVector = DoSection3Encode(sourceVector, selectionVector & ~newSelectionVector);
                            if (newSelectionVector == zeroVector)
                                goto LoopEnd_Dirty;
                            selectionVector = newSelectionVector;
                        }

                        // Section 4 (U+010000 ~ U+1FFFFF)
                        sourceVector = DoSection4Encode(sourceVector, selectionVector);

                    LoopEnd_Dirty:
                        sourceVector.Store(source);
                    LoopEnd:
                        source = (uint*)sourceLimit;
                    } while (++sourceLimit < sourceEnd);
                    if (source >= sourceEnd)
                        return;
                }
            }

            for (; source < sourceEnd; source++)
                EncodeUtf32ToUtf8ForSingleCodePoint(source);
        }

        private static unsafe Vector512<uint> DoSection2Encode(in Vector512<uint> sourceVector, in Vector512<uint> selectionVector)
        {
            Vector512<uint> headByteVector = Vector512.Create<uint>(Utf8Section2Head);
            Vector512<uint> trailHeadVector = Vector512.Create<uint>(Utf8TrailHeader);
            Vector512<uint> trailMaskVector = Vector512.Create<uint>(Utf8TrailHeader);
            Vector512<uint> resultVector = headByteVector | (sourceVector >> 6) | (trailHeadVector | (sourceVector & trailMaskVector)) << 8;
            return Vector512.ConditionalSelect(selectionVector, resultVector, sourceVector);
        }

        private static unsafe Vector256<uint> DoSection2Encode(in Vector256<uint> sourceVector, in Vector256<uint> selectionVector)
        {
            Vector256<uint> headByteVector = Vector256.Create<uint>(Utf8Section2Head);
            Vector256<uint> trailHeadVector = Vector256.Create<uint>(Utf8TrailHeader);
            Vector256<uint> trailMaskVector = Vector256.Create<uint>(Utf8TrailHeader);
            Vector256<uint> resultVector = headByteVector | (sourceVector >> 6) | (trailHeadVector | (sourceVector & trailMaskVector)) << 8;
            return Vector256.ConditionalSelect(selectionVector, resultVector, sourceVector);
        }

        private static unsafe Vector128<uint> DoSection2Encode(in Vector128<uint> sourceVector, in Vector128<uint> selectionVector)
        {
            Vector128<uint> headByteVector = Vector128.Create<uint>(Utf8Section2Head);
            Vector128<uint> trailHeadVector = Vector128.Create<uint>(Utf8TrailHeader);
            Vector128<uint> trailMaskVector = Vector128.Create<uint>(Utf8TrailHeader);
            Vector128<uint> resultVector = headByteVector | (sourceVector >> 6) | (trailHeadVector | (sourceVector & trailMaskVector)) << 8;
            return Vector128.ConditionalSelect(selectionVector, resultVector, sourceVector);
        }

        private static unsafe Vector512<uint> DoSection3Encode(in Vector512<uint> sourceVector, in Vector512<uint> selectionVector)
        {
            Vector512<uint> headByteVector = Vector512.Create<uint>(Utf8Section3Head);
            Vector512<uint> trailHeadVector = Vector512.Create<uint>(Utf8TrailHeader);
            Vector512<uint> trailMaskVector = Vector512.Create<uint>(Utf8TrailHeader);
            Vector512<uint> resultVector = headByteVector | (sourceVector >> 12) | (trailHeadVector | ((sourceVector >> 6) & trailMaskVector)) << 8 |
                    (trailHeadVector | (sourceVector & trailMaskVector)) << 16;
            return Vector512.ConditionalSelect(selectionVector, resultVector, sourceVector);
        }

        private static unsafe Vector256<uint> DoSection3Encode(in Vector256<uint> sourceVector, in Vector256<uint> selectionVector)
        {
            Vector256<uint> headByteVector = Vector256.Create<uint>(Utf8Section3Head);
            Vector256<uint> trailHeadVector = Vector256.Create<uint>(Utf8TrailHeader);
            Vector256<uint> trailMaskVector = Vector256.Create<uint>(Utf8TrailHeader);
            Vector256<uint> resultVector = headByteVector | (sourceVector >> 12) | (trailHeadVector | ((sourceVector >> 6) & trailMaskVector)) << 8 |
                    (trailHeadVector | (sourceVector & trailMaskVector)) << 16;
            return Vector256.ConditionalSelect(selectionVector, resultVector, sourceVector);
        }

        private static unsafe Vector128<uint> DoSection3Encode(in Vector128<uint> sourceVector, in Vector128<uint> selectionVector)
        {
            Vector128<uint> headByteVector = Vector128.Create<uint>(Utf8Section3Head);
            Vector128<uint> trailHeadVector = Vector128.Create<uint>(Utf8TrailHeader);
            Vector128<uint> trailMaskVector = Vector128.Create<uint>(Utf8TrailHeader);
            Vector128<uint> resultVector = headByteVector | (sourceVector >> 12) | (trailHeadVector | ((sourceVector >> 6) & trailMaskVector)) << 8 |
                    (trailHeadVector | (sourceVector & trailMaskVector)) << 16;
            return Vector128.ConditionalSelect(selectionVector, resultVector, sourceVector);
        }

        private static unsafe Vector512<uint> DoSection4Encode(in Vector512<uint> sourceVector, in Vector512<uint> selectionVector)
        {
            Vector512<uint> headByteVector = Vector512.Create<uint>(Utf8Section4Head);
            Vector512<uint> trailHeadVector = Vector512.Create<uint>(Utf8TrailHeader);
            Vector512<uint> trailMaskVector = Vector512.Create<uint>(Utf8TrailHeader);
            Vector512<uint> resultVector = headByteVector | (sourceVector >> 18) | (trailHeadVector | ((sourceVector >> 12) & trailMaskVector)) << 8 |
                (trailHeadVector | ((sourceVector >> 6) & trailMaskVector)) << 16 | (trailHeadVector | (sourceVector & trailMaskVector)) << 24;
            return Vector512.ConditionalSelect(selectionVector, resultVector, sourceVector);
        }

        private static unsafe Vector256<uint> DoSection4Encode(in Vector256<uint> sourceVector, in Vector256<uint> selectionVector)
        {
            Vector256<uint> headByteVector = Vector256.Create<uint>(Utf8Section4Head);
            Vector256<uint> trailHeadVector = Vector256.Create<uint>(Utf8TrailHeader);
            Vector256<uint> trailMaskVector = Vector256.Create<uint>(Utf8TrailHeader);
            Vector256<uint> resultVector = headByteVector | (sourceVector >> 18) | (trailHeadVector | ((sourceVector >> 12) & trailMaskVector)) << 8 |
                (trailHeadVector | ((sourceVector >> 6) & trailMaskVector)) << 16 | (trailHeadVector | (sourceVector & trailMaskVector)) << 24;
            return Vector256.ConditionalSelect(selectionVector, resultVector, sourceVector);
        }

        private static unsafe Vector128<uint> DoSection4Encode(in Vector128<uint> sourceVector, in Vector128<uint> selectionVector)
        {
            Vector128<uint> headByteVector = Vector128.Create<uint>(Utf8Section4Head);
            Vector128<uint> trailHeadVector = Vector128.Create<uint>(Utf8TrailHeader);
            Vector128<uint> trailMaskVector = Vector128.Create<uint>(Utf8TrailHeader);
            Vector128<uint> resultVector = headByteVector | (sourceVector >> 18) | (trailHeadVector | ((sourceVector >> 12) & trailMaskVector)) << 8 |
                (trailHeadVector | ((sourceVector >> 6) & trailMaskVector)) << 16 | (trailHeadVector | (sourceVector & trailMaskVector)) << 24;
            return Vector128.ConditionalSelect(selectionVector, resultVector, sourceVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint GetOffsetForVectorTail(in Vector512<byte> vector)
        {
            byte tailByte = vector[Vector512<byte>.Count - 1];
            if (IsSection2HeadByte(tailByte) || IsSection3HeadByte(tailByte) || IsSection4HeadByte(tailByte))
                return 1;
            tailByte = vector[Vector512<byte>.Count - 2];
            if (IsSection3HeadByte(tailByte) || IsSection4HeadByte(tailByte))
                return 2;
            tailByte = vector[Vector512<byte>.Count - 3];
            if (IsSection4HeadByte(tailByte))
                return 3;
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint GetOffsetForVectorTail(in Vector256<byte> vector)
        {
            byte tailByte = vector[Vector256<byte>.Count - 1];
            if (IsSection2HeadByte(tailByte) || IsSection3HeadByte(tailByte) || IsSection4HeadByte(tailByte))
                return 1;
            tailByte = vector[Vector256<byte>.Count - 2];
            if (IsSection3HeadByte(tailByte) || IsSection4HeadByte(tailByte))
                return 2;
            tailByte = vector[Vector256<byte>.Count - 3];
            if (IsSection4HeadByte(tailByte))
                return 3;
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint GetOffsetForVectorTail(in Vector128<byte> vector)
        {
            byte tailByte = vector[Vector128<byte>.Count - 1];
            if (IsSection2HeadByte(tailByte) || IsSection3HeadByte(tailByte) || IsSection4HeadByte(tailByte))
                return 1;
            tailByte = vector[Vector128<byte>.Count - 2];
            if (IsSection3HeadByte(tailByte) || IsSection4HeadByte(tailByte))
                return 2;
            tailByte = vector[Vector128<byte>.Count - 3];
            if (IsSection4HeadByte(tailByte))
                return 3;
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WidenVectorTwice(in Vector512<byte> sourceVector, Vector512<uint>* destinationVectorBuffer)
        {
            (Vector512<ushort> sourceVectorLow, Vector512<ushort> sourceVectorHigh) = Vector512.Widen(sourceVector);
            (destinationVectorBuffer[0], destinationVectorBuffer[1]) = Vector512.Widen(sourceVectorLow);
            (destinationVectorBuffer[2], destinationVectorBuffer[3]) = Vector512.Widen(sourceVectorHigh);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WidenVectorTwice(in Vector256<byte> sourceVector, Vector256<uint>* destinationVectorBuffer)
        {
            (Vector256<ushort> sourceVectorLow, Vector256<ushort> sourceVectorHigh) = Vector256.Widen(sourceVector);
            (destinationVectorBuffer[0], destinationVectorBuffer[1]) = Vector256.Widen(sourceVectorLow);
            (destinationVectorBuffer[2], destinationVectorBuffer[3]) = Vector256.Widen(sourceVectorHigh);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WidenVectorTwice(in Vector128<byte> sourceVector, Vector128<uint>* destinationVectorBuffer)
        {
            (Vector128<ushort> sourceVectorLow, Vector128<ushort> sourceVectorHigh) = Vector128.Widen(sourceVector);
            (destinationVectorBuffer[0], destinationVectorBuffer[1]) = Vector128.Widen(sourceVectorLow);
            (destinationVectorBuffer[2], destinationVectorBuffer[3]) = Vector128.Widen(sourceVectorHigh);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WriteErrorCodePointToDestinationVector(in Vector512<byte> sourceVector, uint* destination)
        {
            Vector512<uint> zeroVector = Vector512<uint>.Zero;
            Vector512<uint> errorCodePointVector = Vector512.Create<uint>(Utf8DecodeErrorCodePoint);

            (Vector512<ushort> sourceVectorLow, Vector512<ushort> sourceVectorHigh) = Vector512.Widen(sourceVector);
            (Vector512<uint> sourceVectorLowLow, Vector512<uint> sourceVectorLowHigh) = Vector512.Widen(sourceVectorLow);
            (Vector512<uint> sourceVectorHighLow, Vector512<uint> sourceVectorHighHigh) = Vector512.Widen(sourceVectorHigh);
            sourceVectorLowLow = Vector512.GreaterThan(sourceVectorLowLow, zeroVector) & errorCodePointVector;
            sourceVectorLowHigh = Vector512.GreaterThan(sourceVectorLowHigh, zeroVector) & errorCodePointVector;
            sourceVectorHighLow = Vector512.GreaterThan(sourceVectorHighLow, zeroVector) & errorCodePointVector;
            sourceVectorHighHigh = Vector512.GreaterThan(sourceVectorHighHigh, zeroVector) & errorCodePointVector;
            sourceVectorLowLow.Store(destination);
            sourceVectorLowHigh.Store(destination + Vector512<uint>.Count);
            sourceVectorHighLow.Store(destination + Vector512<uint>.Count * 2);
            sourceVectorHighHigh.Store(destination + Vector512<uint>.Count * 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WriteErrorCodePointToDestinationVector(in Vector256<byte> sourceVector, uint* destination)
        {
            Vector256<uint> zeroVector = Vector256<uint>.Zero;
            Vector256<uint> errorCodePointVector = Vector256.Create<uint>(Utf8DecodeErrorCodePoint);

            (Vector256<ushort> sourceVectorLow, Vector256<ushort> sourceVectorHigh) = Vector256.Widen(sourceVector);
            (Vector256<uint> sourceVectorLowLow, Vector256<uint> sourceVectorLowHigh) = Vector256.Widen(sourceVectorLow);
            (Vector256<uint> sourceVectorHighLow, Vector256<uint> sourceVectorHighHigh) = Vector256.Widen(sourceVectorHigh);
            sourceVectorLowLow = Vector256.GreaterThan(sourceVectorLowLow, zeroVector) & errorCodePointVector;
            sourceVectorLowHigh = Vector256.GreaterThan(sourceVectorLowHigh, zeroVector) & errorCodePointVector;
            sourceVectorHighLow = Vector256.GreaterThan(sourceVectorHighLow, zeroVector) & errorCodePointVector;
            sourceVectorHighHigh = Vector256.GreaterThan(sourceVectorHighHigh, zeroVector) & errorCodePointVector;
            sourceVectorLowLow.Store(destination);
            sourceVectorLowHigh.Store(destination + Vector256<uint>.Count);
            sourceVectorHighLow.Store(destination + Vector256<uint>.Count * 2);
            sourceVectorHighHigh.Store(destination + Vector256<uint>.Count * 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WriteErrorCodePointToDestinationVector(in Vector128<byte> sourceVector, uint* destination)
        {
            Vector128<uint> zeroVector = Vector128<uint>.Zero;
            Vector128<uint> errorCodePointVector = Vector128.Create<uint>(Utf8DecodeErrorCodePoint);

            (Vector128<ushort> sourceVectorLow, Vector128<ushort> sourceVectorHigh) = Vector128.Widen(sourceVector);
            (Vector128<uint> sourceVectorLowLow, Vector128<uint> sourceVectorLowHigh) = Vector128.Widen(sourceVectorLow);
            (Vector128<uint> sourceVectorHighLow, Vector128<uint> sourceVectorHighHigh) = Vector128.Widen(sourceVectorHigh);
            sourceVectorLowLow = Vector128.GreaterThan(sourceVectorLowLow, zeroVector) & errorCodePointVector;
            sourceVectorLowHigh = Vector128.GreaterThan(sourceVectorLowHigh, zeroVector) & errorCodePointVector;
            sourceVectorHighLow = Vector128.GreaterThan(sourceVectorHighLow, zeroVector) & errorCodePointVector;
            sourceVectorHighHigh = Vector128.GreaterThan(sourceVectorHighHigh, zeroVector) & errorCodePointVector;
            sourceVectorLowLow.Store(destination);
            sourceVectorLowHigh.Store(destination + Vector128<uint>.Count);
            sourceVectorHighLow.Store(destination + Vector128<uint>.Count * 2);
            sourceVectorHighHigh.Store(destination + Vector128<uint>.Count * 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Vector512<byte> CreateTailZeroVector(in Vector512<byte> original, int tailLength)
        {
            Vector512<byte> result = original;
            byte* ptr = (byte*)&result;
            for (int i = 0; i < tailLength; i++)
                ptr[Vector512<byte>.Count - 2 - i] = 0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Vector256<byte> CreateTailZeroVector(in Vector256<byte> original, int tailLength)
        {
            Vector256<byte> result = original;
            byte* ptr = (byte*)&result;
            for (int i = 0; i < tailLength; i++)
                ptr[Vector256<byte>.Count - 2 - i] = 0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Vector128<byte> CreateTailZeroVector(in Vector128<byte> original, int tailLength)
        {
            Vector128<byte> result = original;
            byte* ptr = (byte*)&result;
            for (int i = 0; i < tailLength; i++)
                ptr[Vector128<byte>.Count - 2 - i] = 0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void SetVectorSlotsToFull(ref Vector512<byte> target, params ReadOnlySpan<int> slots)
        {
            byte* ptr = (byte*)UnsafeHelper.AsPointerRef(ref target);
            foreach (int slot in slots)
                ptr[slot] = byte.MaxValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void SetVectorSlotsToFull(ref Vector256<byte> target, params ReadOnlySpan<int> slots)
        {
            byte* ptr = (byte*)UnsafeHelper.AsPointerRef(ref target);
            foreach (int slot in slots)
                ptr[slot] = byte.MaxValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void SetVectorSlotsToFull(ref Vector128<byte> target, params ReadOnlySpan<int> slots)
        {
            byte* ptr = (byte*)UnsafeHelper.AsPointerRef(ref target);
            foreach (int slot in slots)
                ptr[slot] = byte.MaxValue;
        }
    }
}
#endif