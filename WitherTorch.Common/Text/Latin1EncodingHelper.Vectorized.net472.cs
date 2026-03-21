#if NET472_OR_GREATER
using System.Numerics;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    unsafe partial class Latin1EncodingHelper
    {
        private static partial byte* VectorizedReadFromUtf16BufferCore_OutOfLatin1Range(char* source, byte* destination, nuint length)
        {
            const byte ReplaceCharacter = 0x003F;

            byte* result = destination + length;

            Vector<ushort> filterVector = new Vector<ushort>(Latin1EncodingLimit);
            Vector<byte> replaceVector = new Vector<byte>(ReplaceCharacter);

            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % (UnsafeHelper.SizeOf<Vector<ushort>>() * 2);
            nuint headRemainder2 = (nuint)destination % UnsafeHelper.SizeOf<Vector<byte>>();
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector<ushort> sourceVector = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source);
                Vector<ushort> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source + Vector<ushort>.Count);
                Vector<byte> maskVector = Vector.Narrow(Vector.GreaterThan(sourceVector, filterVector), Vector.GreaterThan(sourceVector2, filterVector));
                UnsafeHelper.WriteUnaligned(destination, Vector.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector.Narrow(sourceVector, sourceVector2)));
                if (length > (nuint)Vector<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder == headRemainder2 * 2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = ((UnsafeHelper.SizeOf<Vector<ushort>>() * 2) - headRemainder) / UnsafeHelper.SizeOf<char>();
                    source += headRemainder;
                    destination += headRemainder;
                    length -= headRemainder;
                    if (canFullyAligned)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    source += Vector<ushort>.Count * 2;
                    destination += Vector<byte>.Count;
                    length -= (nuint)Vector<ushort>.Count * 2;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector<ushort> sourceVector = UnsafeHelper.Read<Vector<ushort>>(source);
                Vector<ushort> sourceVector2 = UnsafeHelper.Read<Vector<ushort>>(source + Vector<ushort>.Count);
                Vector<byte> maskVector = Vector.Narrow(Vector.GreaterThan(sourceVector, filterVector), Vector.GreaterThan(sourceVector2, filterVector));
                UnsafeHelper.WriteUnaligned(destination, Vector.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector.Narrow(sourceVector, sourceVector2)));
                source += (nuint)Vector<ushort>.Count * 2;
                destination += (nuint)Vector<byte>.Count;
                length -= (nuint)Vector<ushort>.Count * 2;
            } while (length >= (nuint)Vector<ushort>.Count * 2);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector<ushort> sourceVector = UnsafeHelper.Read<Vector<ushort>>(source);
                Vector<ushort> sourceVector2 = UnsafeHelper.Read<Vector<ushort>>(source + Vector<ushort>.Count);
                Vector<byte> maskVector = Vector.Narrow(Vector.GreaterThan(sourceVector, filterVector), Vector.GreaterThan(sourceVector2, filterVector));
                UnsafeHelper.Write(destination, Vector.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector.Narrow(sourceVector, sourceVector2)));
                source += (nuint)Vector<ushort>.Count * 2;
                destination += (nuint)Vector<byte>.Count;
                length -= (nuint)Vector<ushort>.Count * 2;
            } while (length >= (nuint)Vector<ushort>.Count * 2);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector<ushort>.Count * 2;
                destination = destination + length - (nuint)Vector<byte>.Count;
                Vector<ushort> sourceVector = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source);
                Vector<ushort> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source + Vector<ushort>.Count);
                Vector<byte> maskVector = Vector.Narrow(Vector.GreaterThan(sourceVector, filterVector), Vector.GreaterThan(sourceVector2, filterVector));
                UnsafeHelper.WriteUnaligned(destination, Vector.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector.Narrow(sourceVector, sourceVector2)));
            }

            return result;
        }

        private static partial byte* VectorizedReadFromUtf16BufferCore(char* source, byte* destination, nuint length)
        {
            byte* result = destination + length;

            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % (UnsafeHelper.SizeOf<Vector<ushort>>() * 2);
            nuint headRemainder2 = (nuint)destination % UnsafeHelper.SizeOf<Vector<byte>>();
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector<ushort> sourceVector = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source);
                Vector<ushort> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source + Vector<ushort>.Count);
                UnsafeHelper.WriteUnaligned(destination, Vector.Narrow(sourceVector, sourceVector2));
                if (length > (nuint)Vector<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder == headRemainder2 * 2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = ((UnsafeHelper.SizeOf<Vector<ushort>>() * 2) - headRemainder) / UnsafeHelper.SizeOf<char>();
                    source += headRemainder;
                    destination += headRemainder;
                    length -= headRemainder;
                    if (canFullyAligned)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    source += Vector<ushort>.Count * 2;
                    destination += Vector<byte>.Count;
                    length -= (nuint)Vector<ushort>.Count * 2;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector<ushort> sourceVector = UnsafeHelper.Read<Vector<ushort>>(source);
                Vector<ushort> sourceVector2 = UnsafeHelper.Read<Vector<ushort>>(source + Vector<ushort>.Count);
                UnsafeHelper.WriteUnaligned(destination, Vector.Narrow(sourceVector, sourceVector2));
                source += (nuint)Vector<ushort>.Count * 2;
                destination += (nuint)Vector<byte>.Count;
                length -= (nuint)Vector<ushort>.Count * 2;
            } while (length >= (nuint)Vector<ushort>.Count * 2);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector<ushort> sourceVector = UnsafeHelper.Read<Vector<ushort>>(source);
                Vector<ushort> sourceVector2 = UnsafeHelper.Read<Vector<ushort>>(source + Vector<ushort>.Count);
                UnsafeHelper.Write(destination, Vector.Narrow(sourceVector, sourceVector2));
                source += (nuint)Vector<ushort>.Count * 2;
                destination += (nuint)Vector<byte>.Count;
                length -= (nuint)Vector<ushort>.Count * 2;
            } while (length >= (nuint)Vector<ushort>.Count * 2);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector<ushort>.Count * 2;
                destination = destination + length - (nuint)Vector<byte>.Count;
                Vector<ushort> sourceVector = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source);
                Vector<ushort> sourceVector2 = UnsafeHelper.ReadUnaligned<Vector<ushort>>(source + Vector<ushort>.Count);
                UnsafeHelper.WriteUnaligned(destination, Vector.Narrow(sourceVector, sourceVector2));
            }

            return result;
        }

        private static partial char* VectorizedWriteToUtf16BufferCore(byte* source, char* destination, nuint length)
        {
            char* result = destination + length;

            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % UnsafeHelper.SizeOf<Vector<byte>>();
            nuint headRemainder2 = (nuint)destination % (UnsafeHelper.SizeOf<Vector<ushort>>() * 2);
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(source);
                Vector.Widen(sourceVector, out Vector<ushort> destinationVectorLow, out Vector<ushort> destinationVectorHigh);
                UnsafeHelper.WriteUnaligned(destination, destinationVectorLow);
                UnsafeHelper.WriteUnaligned(destination + Vector<ushort>.Count, destinationVectorHigh);
                if (length > (nuint)Vector<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder * 2 == headRemainder2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = (UnsafeHelper.SizeOf<Vector<byte>>() - headRemainder) / UnsafeHelper.SizeOf<byte>();
                    source += headRemainder;
                    destination += headRemainder;
                    length -= headRemainder;
                    if (canFullyAligned)
                        goto VectorizedLoop_FullAligned;
                    else
                        goto VectorizedLoop_PartialAligned;
                }
                else
                {
                    source += Vector<byte>.Count;
                    destination += Vector<ushort>.Count * 2;
                    length -= (nuint)Vector<byte>.Count;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector<byte> sourceVector = UnsafeHelper.Read<Vector<byte>>(source);
                Vector.Widen(sourceVector, out Vector<ushort> destinationVectorLow, out Vector<ushort> destinationVectorHigh);
                UnsafeHelper.WriteUnaligned(destination, destinationVectorLow);
                UnsafeHelper.WriteUnaligned(destination + Vector<ushort>.Count, destinationVectorHigh);
                source += Vector<byte>.Count;
                destination += Vector<ushort>.Count * 2;
                length -= (nuint)Vector<byte>.Count;
            } while (length >= (nuint)Vector<byte>.Count);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector<byte> sourceVector = UnsafeHelper.Read<Vector<byte>>(source);
                Vector.Widen(sourceVector, out Vector<ushort> destinationVectorLow, out Vector<ushort> destinationVectorHigh);
                UnsafeHelper.Write(destination, destinationVectorLow);
                UnsafeHelper.Write(destination + Vector<ushort>.Count, destinationVectorHigh);
                source += Vector<byte>.Count;
                destination += Vector<ushort>.Count * 2;
                length -= (nuint)Vector<byte>.Count;
            } while (length >= (nuint)Vector<byte>.Count);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector<byte>.Count;
                destination = destination + length - (nuint)Vector<ushort>.Count * 2;
                Vector<byte> sourceVector = UnsafeHelper.ReadUnaligned<Vector<byte>>(source);
                Vector.Widen(sourceVector, out Vector<ushort> destinationVectorLow, out Vector<ushort> destinationVectorHigh);
                UnsafeHelper.WriteUnaligned(destination, destinationVectorLow);
                UnsafeHelper.WriteUnaligned(destination + Vector<ushort>.Count, destinationVectorHigh);
            }

            return result;
        }
    }
}
#endif