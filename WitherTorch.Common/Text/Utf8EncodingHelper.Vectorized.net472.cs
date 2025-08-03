#if NET472_OR_GREATER
using System;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

using System.Numerics;

namespace WitherTorch.Common.Text
{
    partial class Utf8EncodingHelper
    {
        private static unsafe partial bool HasSurrogateCharacters(char* source, nuint length)
        {
            char* sourceEnd = source + length;

            if (Limits.UseVector())
            {
                Vector<ushort>* sourceLimit = ((Vector<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector<ushort> zeroVector = Vector<ushort>.Zero;
                    Vector<ushort> leadSurrogateStartVector = new Vector<ushort>(Utf16LeadSurrogateStart);
                    Vector<ushort> leadSurrogateEndVector = new Vector<ushort>(Utf16LeadSurrogateEnd);
                    do
                    {
                        Vector<ushort> sourceVector = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source);
                        Vector<ushort> resultVector = Vector.GreaterThanOrEqual(sourceVector, leadSurrogateStartVector) &
                            Vector.LessThanOrEqual(sourceVector, leadSurrogateEndVector);
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
            if (Limits.UseVector())
            {
                Vector<ushort>* sourceLimit = ((Vector<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector<ushort> zeroVector = Vector<ushort>.Zero;
                    Vector<ushort> leadSurrogateStartVector = new Vector<ushort>(Utf16LeadSurrogateStart);
                    Vector<ushort> leadSurrogateEndVector = new Vector<ushort>(Utf16LeadSurrogateEnd);
                    Vector<uint>* destinationLimit = ((Vector<uint>*)destination) + 2;
                    do
                    {
                        Vector<ushort> sourceVector = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source);
                        Vector<ushort> selectionVector = Vector.LessThan(sourceVector, leadSurrogateStartVector) &
                            Vector.GreaterThan(sourceVector, leadSurrogateEndVector);
                        Vector.Widen(sourceVector & selectionVector, out Vector<uint> destVectorLow, out Vector<uint> destVectorHigh);
                        UnsafeHelper.WriteUnaligned(destination, destVectorLow);
                        UnsafeHelper.WriteUnaligned(((Vector<uint>*)destination) + 1, destVectorHigh);
                        if (selectionVector != zeroVector)
                        {
                            for (int i = 0; i < Vector<ushort>.Count - 1; i++)
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

                            if (selectionVector[Vector<ushort>.Count - 1] == 0)
                            {
                                sourceLimit = (Vector<ushort>*)(((char*)sourceLimit) - 1);
                                destinationLimit = (Vector<uint>*)(((uint*)sourceLimit) - 1);
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
            if (Limits.UseVector())
            {
                Vector<ushort>* sourceLimit = ((Vector<ushort>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector<uint>* destinationLimit = ((Vector<uint>*)destination) + 2;
                    do
                    {
                        Vector<ushort> sourceVector = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source);
                        Vector.Widen(sourceVector, out Vector<uint> destVectorLow, out Vector<uint> destVectorHigh);
                        UnsafeHelper.WriteUnaligned(destination, destVectorLow);
                        UnsafeHelper.WriteUnaligned(((Vector<uint>*)destination) + 1, destVectorHigh);
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
            if (Limits.UseVector())
            {
                Vector<byte>* sourceLimit = ((Vector<byte>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector<byte> zeroVector = Vector<byte>.Zero;
                    Vector<byte> fullVector = new Vector<byte>(byte.MaxValue);

                    Vector<byte> section1LimitVector = new Vector<byte>(Latin1EncodingHelper.AsciiEncodingLimit_InByte);

                    Vector<byte> section2StartVector = new Vector<byte>(Utf8Section2Head);
                    Vector<byte> section2EndVector = new Vector<byte>(Utf8Section2Head | Utf8Section2Mask);
                    Vector<byte> section2SelectionMaskVector = CreateTailZeroVector(fullVector, 1);

                    Vector<byte> section3StartVector = new Vector<byte>(Utf8Section3Head);
                    Vector<byte> section3EndVector = new Vector<byte>(Utf8Section3Head | Utf8Section3Mask);
                    Vector<byte> section3SelectionMaskVector = CreateTailZeroVector(fullVector, 2);

                    Vector<byte> section4StartVector = new Vector<byte>(Utf8Section4Head);
                    Vector<byte> section4EndVector = new Vector<byte>(Utf8Section4Head | Utf8Section4Mask);
                    Vector<byte> section4SelectionMaskVector = CreateTailZeroVector(fullVector, 3);

                    Vector<uint>* destinationLimit = ((Vector<uint>*)destination) + 4;
                    Vector<uint>* widenedVectors = stackalloc Vector<uint>[4];
                    do
                    {
                        Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(source);
                        Vector<byte> selectionVector = Vector.LessThan(sourceVector, section1LimitVector);

                        // Section 1: U+0001 ~ U+007F (1 Byte)
                        WidenVectorTwice(
                            sourceVector: sourceVector & selectionVector,
                            destinationVectorBuffer: (Vector<uint>*)destination
                            );

                        // Section 2: U+0080 ~ U+07FF (2 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector.GreaterThanOrEqual(sourceVector, section2StartVector) &
                            Vector.LessThanOrEqual(sourceVector, section2EndVector) & section2SelectionMaskVector;
                        if (selectionVector != zeroVector)
                            DoSection2Decode(in sourceVector, ref selectionVector, destination);

                        // Section 3: U+0800 ~ U+FFFF (3 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector.GreaterThanOrEqual(sourceVector, section3StartVector) &
                            Vector.LessThanOrEqual(sourceVector, section3EndVector) & section3SelectionMaskVector;
                        if (selectionVector != zeroVector)
                            DoSection3Decode(in sourceVector, ref selectionVector, destination);

                        // Section 4: U+010000 ~ U+10FFFF (4 Bytes)
                        sourceVector &= ~selectionVector;
                        if (sourceVector == zeroVector)
                            goto LoopEnd;
                        selectionVector = Vector.GreaterThanOrEqual(sourceVector, section4StartVector) &
                            Vector.LessThanOrEqual(sourceVector, section4EndVector) & section2SelectionMaskVector; ;
                        if (selectionVector != zeroVector)
                            DoSection4Decode(in sourceVector, ref selectionVector, destination);

                        // Section "Error"
                        sourceVector &= ~selectionVector;
                        if (sourceVector != zeroVector)
                        {
                            nuint offset = GetOffsetForVectorTail(sourceVector);
                            WriteErrorCodePointToDestinationVector(sourceVector, destination);
                            destinationLimit = (Vector<uint>*)(((uint*)destinationLimit) - offset);
                            sourceLimit = (Vector<byte>*)(((byte*)sourceLimit) - offset);
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
        private static unsafe void DoSection2Decode(in Vector<byte> sourceVector, ref Vector<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector<byte>.Count - 1; i++)
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
        private static unsafe void DoSection3Decode(in Vector<byte> sourceVector, ref Vector<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector<byte>.Count - 2; i++)
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
        private static unsafe void DoSection4Decode(in Vector<byte> sourceVector, ref Vector<byte> selectionVector, uint* destination)
        {
            for (int i = 0; i < Vector<byte>.Count - 3; i++)
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
            if (Limits.UseVector())
            {
                Vector<uint>* sourceLimit = ((Vector<uint>*)source) + 1;
                if (sourceLimit < sourceEnd)
                {
                    Vector<uint> zeroVector = Vector<uint>.Zero;
                    Vector<uint> section1LimitVector = new Vector<uint>(Utf8Section1Limit);
                    Vector<uint> section2LimitVector = new Vector<uint>(Utf8Section2Limit);
                    Vector<uint> section3LimitVector = new Vector<uint>(Utf8Section3Limit);
                    do
                    {
                        Vector<uint> sourceVector = UnsafeHelper.ReadUnaligned<Vector<uint>>(source);

                        // Section 1 (U+0000 ~ U+007F)
                        Vector<uint> selectionVector = Vector.GreaterThan(sourceVector, section1LimitVector);
                        if (selectionVector == zeroVector)
                            goto LoopEnd;

                        // Section 2 (U+0080 ~ U+07FF)
                        Vector<uint> newSelectionVector = Vector.GreaterThan(sourceVector, section2LimitVector);
                        if (selectionVector != newSelectionVector)
                        {
                            DoSection2Encode(source, selectionVector & ~newSelectionVector);
                            if (newSelectionVector == zeroVector)
                                goto LoopEnd;
                            selectionVector = newSelectionVector;
                        }

                        // Section 3 (U+0800 ~ U+FFFF)
                        newSelectionVector = Vector.GreaterThan(sourceVector, section3LimitVector);
                        if (selectionVector != newSelectionVector)
                        {
                            DoSection3Encode(source, selectionVector & ~newSelectionVector);
                            if (newSelectionVector == zeroVector)
                                goto LoopEnd;
                            selectionVector = newSelectionVector;
                        }

                        // Section 4 (U+010000 ~ U+1FFFFF)
                        DoSection4Encode(source, selectionVector);

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

        private static unsafe void DoSection2Encode(uint* source, in Vector<uint> selectionVector)
        {
            for (int i = 0; i < Vector<uint>.Count; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                source[i] = EncodeUtf32ToUtf8ForSingleCodePoint_Section2(source[i]);
            }
        }

        private static unsafe void DoSection3Encode(uint* source, in Vector<uint> selectionVector)
        {
            for (int i = 0; i < Vector<uint>.Count; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                source[i] = EncodeUtf32ToUtf8ForSingleCodePoint_Section3(source[i]);
            }
        }

        private static unsafe void DoSection4Encode(uint* source, in Vector<uint> selectionVector)
        {
            for (int i = 0; i < Vector<uint>.Count; i++)
            {
                if (selectionVector[i] == 0)
                    continue;
                source[i] = EncodeUtf32ToUtf8ForSingleCodePoint_Section4(source[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static nuint GetOffsetForVectorTail(in Vector<byte> vector)
        {
            byte tailByte = vector[Vector<byte>.Count - 1];
            if (IsSection2HeadByte(tailByte) || IsSection3HeadByte(tailByte) || IsSection4HeadByte(tailByte))
                return 1;
            tailByte = vector[Vector<byte>.Count - 2];
            if (IsSection3HeadByte(tailByte) || IsSection4HeadByte(tailByte))
                return 2;
            tailByte = vector[Vector<byte>.Count - 3];
            if (IsSection4HeadByte(tailByte))
                return 3;
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WidenVectorTwice(in Vector<byte> sourceVector, Vector<uint>* destinationVectorBuffer)
        {
            Vector.Widen(sourceVector, out Vector<ushort> sourceVectorLow, out Vector<ushort> sourceVectorHigh);
            Vector.Widen(sourceVectorLow, out destinationVectorBuffer[0], out destinationVectorBuffer[1]);
            Vector.Widen(sourceVectorHigh, out destinationVectorBuffer[2], out destinationVectorBuffer[3]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void WriteErrorCodePointToDestinationVector(in Vector<byte> sourceVector, uint* destination)
        {
            Vector<uint> zeroVector = Vector<uint>.Zero;
            Vector<uint> errorCodePointVector = new Vector<uint>(Utf8DecodeErrorCodePoint);

            Vector.Widen(sourceVector, out Vector<ushort> sourceVectorLow, out Vector<ushort> sourceVectorHigh);
            Vector.Widen(sourceVectorLow, out Vector<uint> sourceVectorLowLow, out Vector<uint> sourceVectorLowHigh);
            Vector.Widen(sourceVectorHigh, out Vector<uint> sourceVectorHighLow, out Vector<uint> sourceVectorHighHigh);
            sourceVectorLowLow = Vector.GreaterThan(sourceVectorLowLow, zeroVector) & errorCodePointVector;
            sourceVectorLowHigh = Vector.GreaterThan(sourceVectorLowHigh, zeroVector) & errorCodePointVector;
            sourceVectorHighLow = Vector.GreaterThan(sourceVectorHighLow, zeroVector) & errorCodePointVector;
            sourceVectorHighHigh = Vector.GreaterThan(sourceVectorHighHigh, zeroVector) & errorCodePointVector;
            UnsafeHelper.WriteUnaligned(destination, sourceVectorLowLow);
            UnsafeHelper.WriteUnaligned(((Vector<uint>*)destination) + 1, sourceVectorLowHigh);
            UnsafeHelper.WriteUnaligned(((Vector<uint>*)destination) + 2, sourceVectorHighLow);
            UnsafeHelper.WriteUnaligned(((Vector<uint>*)destination) + 3, sourceVectorHighHigh);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Vector<byte> CreateTailZeroVector(in Vector<byte> original, int tailLength)
        {
            Vector<byte> result = original;
            byte* ptr = ((byte*)&result);
            for (int i = 0; i < tailLength; i++)
                ptr[Vector<byte>.Count - 2 - i] = 0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void SetVectorSlotsToFull(ref Vector<byte> target, params ReadOnlySpan<int> slots)
        {
            byte* ptr = (byte*)UnsafeHelper.AsPointerRef(ref target);
            foreach (int slot in slots)
                ptr[slot] = byte.MaxValue;
        }
    }
}
#endif