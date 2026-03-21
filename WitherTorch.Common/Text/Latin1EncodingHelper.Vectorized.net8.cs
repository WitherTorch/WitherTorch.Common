#if NET8_0_OR_GREATER
using System.Numerics;
using System.Runtime.Intrinsics;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    unsafe partial class Latin1EncodingHelper
    {
        private static partial byte* VectorizedReadFromUtf16BufferCore_OutOfLatin1Range(char* source, byte* destination, nuint length)
        {
            byte* result = destination + length;
            if (Limits.UseVector512() && length >= (nuint)Vector512<byte>.Count)
                VectorizedReadFromUtf16BufferCore_OutOfLatin1Range_512(ref source, ref destination, ref length);
            else if (Limits.UseVector256() && length >= (nuint)Vector256<byte>.Count)
                VectorizedReadFromUtf16BufferCore_OutOfLatin1Range_256(ref source, ref destination, ref length);
            else if (Limits.UseVector128() && length >= (nuint)Vector128<byte>.Count)
                VectorizedReadFromUtf16BufferCore_OutOfLatin1Range_128(ref source, ref destination, ref length);
            else
                VectorizedReadFromUtf16BufferCore_OutOfLatin1Range_64(ref source, ref destination, ref length);
            return result;
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedReadFromUtf16BufferCore_OutOfLatin1Range_512(ref char* source, ref byte* destination, ref nuint length)
        {
            const byte ReplaceCharacter = 0x003F;

            Vector512<ushort> filterVector = Vector512.Create<ushort>(Latin1EncodingLimit);
            Vector512<byte> replaceVector = Vector512.Create(ReplaceCharacter);

            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % (UnsafeHelper.SizeOf<Vector512<ushort>>() * 2);
            nuint headRemainder2 = (nuint)destination % UnsafeHelper.SizeOf<Vector512<byte>>();
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector512<ushort> sourceVector = Vector512.Load((ushort*)source);
                Vector512<ushort> sourceVector2 = Vector512.Load((ushort*)source + Vector512<ushort>.Count);
                Vector512<byte> maskVector = Vector512.Narrow(Vector512.GreaterThan(sourceVector, filterVector), Vector512.GreaterThan(sourceVector2, filterVector));
                Vector512.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector512.Narrow(sourceVector, sourceVector2)).Store(destination);
                if (length > (nuint)Vector512<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder == headRemainder2 * 2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = ((UnsafeHelper.SizeOf<Vector512<ushort>>() * 2) - headRemainder) / UnsafeHelper.SizeOf<char>();
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
                    source += Vector512<ushort>.Count * 2;
                    destination += Vector512<byte>.Count;
                    length -= (nuint)Vector512<ushort>.Count * 2;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector512<ushort> sourceVector = Vector512.LoadAligned((ushort*)source);
                Vector512<ushort> sourceVector2 = Vector512.LoadAligned((ushort*)source + Vector512<ushort>.Count);
                Vector512<byte> maskVector = Vector512.Narrow(Vector512.GreaterThan(sourceVector, filterVector), Vector512.GreaterThan(sourceVector2, filterVector));
                Vector512.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector512.Narrow(sourceVector, sourceVector2)).Store(destination);
                source += (nuint)Vector512<ushort>.Count * 2;
                destination += (nuint)Vector512<byte>.Count;
                length -= (nuint)Vector512<ushort>.Count * 2;
            } while (length >= (nuint)Vector512<ushort>.Count * 2);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector512<ushort> sourceVector = Vector512.LoadAligned((ushort*)source);
                Vector512<ushort> sourceVector2 = Vector512.LoadAligned((ushort*)source + Vector512<ushort>.Count);
                Vector512<byte> maskVector = Vector512.Narrow(Vector512.GreaterThan(sourceVector, filterVector), Vector512.GreaterThan(sourceVector2, filterVector));
                Vector512.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector512.Narrow(sourceVector, sourceVector2)).StoreAligned(destination);
                source += (nuint)Vector512<ushort>.Count * 2;
                destination += (nuint)Vector512<byte>.Count;
                length -= (nuint)Vector512<ushort>.Count * 2;
            } while (length >= (nuint)Vector512<ushort>.Count * 2);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector512<ushort>.Count * 2;
                destination = destination + length - (nuint)Vector512<byte>.Count;
                Vector512<ushort> sourceVector = Vector512.Load((ushort*)source);
                Vector512<ushort> sourceVector2 = Vector512.Load((ushort*)source + Vector512<ushort>.Count);
                Vector512<byte> maskVector = Vector512.Narrow(Vector512.GreaterThan(sourceVector, filterVector), Vector512.GreaterThan(sourceVector2, filterVector));
                Vector512.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector512.Narrow(sourceVector, sourceVector2)).Store(destination);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedReadFromUtf16BufferCore_OutOfLatin1Range_256(ref char* source, ref byte* destination, ref nuint length)
        {
            const byte ReplaceCharacter = 0x003F;

            Vector256<ushort> filterVector = Vector256.Create<ushort>(Latin1EncodingLimit);
            Vector256<byte> replaceVector = Vector256.Create(ReplaceCharacter);

            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % (UnsafeHelper.SizeOf<Vector256<ushort>>() * 2);
            nuint headRemainder2 = (nuint)destination % UnsafeHelper.SizeOf<Vector256<byte>>();
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector256<ushort> sourceVector = Vector256.Load((ushort*)source);
                Vector256<ushort> sourceVector2 = Vector256.Load((ushort*)source + Vector256<ushort>.Count);
                Vector256<byte> maskVector = Vector256.Narrow(Vector256.GreaterThan(sourceVector, filterVector), Vector256.GreaterThan(sourceVector2, filterVector));
                Vector256.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector256.Narrow(sourceVector, sourceVector2)).Store(destination);
                if (length > (nuint)Vector256<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder == headRemainder2 * 2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = ((UnsafeHelper.SizeOf<Vector256<ushort>>() * 2) - headRemainder) / UnsafeHelper.SizeOf<char>();
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
                    source += Vector256<ushort>.Count * 2;
                    destination += Vector256<byte>.Count;
                    length -= (nuint)Vector256<ushort>.Count * 2;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector256<ushort> sourceVector = Vector256.LoadAligned((ushort*)source);
                Vector256<ushort> sourceVector2 = Vector256.LoadAligned((ushort*)source + Vector256<ushort>.Count);
                Vector256<byte> maskVector = Vector256.Narrow(Vector256.GreaterThan(sourceVector, filterVector), Vector256.GreaterThan(sourceVector2, filterVector));
                Vector256.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector256.Narrow(sourceVector, sourceVector2)).Store(destination);
                source += (nuint)Vector256<ushort>.Count * 2;
                destination += (nuint)Vector256<byte>.Count;
                length -= (nuint)Vector256<ushort>.Count * 2;
            } while (length >= (nuint)Vector256<ushort>.Count * 2);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector256<ushort> sourceVector = Vector256.LoadAligned((ushort*)source);
                Vector256<ushort> sourceVector2 = Vector256.LoadAligned((ushort*)source + Vector256<ushort>.Count);
                Vector256<byte> maskVector = Vector256.Narrow(Vector256.GreaterThan(sourceVector, filterVector), Vector256.GreaterThan(sourceVector2, filterVector));
                Vector256.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector256.Narrow(sourceVector, sourceVector2)).StoreAligned(destination);
                source += (nuint)Vector256<ushort>.Count * 2;
                destination += (nuint)Vector256<byte>.Count;
                length -= (nuint)Vector256<ushort>.Count * 2;
            } while (length >= (nuint)Vector256<ushort>.Count * 2);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector256<ushort>.Count * 2;
                destination = destination + length - (nuint)Vector256<byte>.Count;
                Vector256<ushort> sourceVector = Vector256.Load((ushort*)source);
                Vector256<ushort> sourceVector2 = Vector256.Load((ushort*)source + Vector256<ushort>.Count);
                Vector256<byte> maskVector = Vector256.Narrow(Vector256.GreaterThan(sourceVector, filterVector), Vector256.GreaterThan(sourceVector2, filterVector));
                Vector256.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector256.Narrow(sourceVector, sourceVector2)).Store(destination);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedReadFromUtf16BufferCore_OutOfLatin1Range_128(ref char* source, ref byte* destination, ref nuint length)
        {
            const byte ReplaceCharacter = 0x003F;

            Vector128<ushort> filterVector = Vector128.Create<ushort>(Latin1EncodingLimit);
            Vector128<byte> replaceVector = Vector128.Create(ReplaceCharacter);

            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % (UnsafeHelper.SizeOf<Vector128<ushort>>() * 2);
            nuint headRemainder2 = (nuint)destination % UnsafeHelper.SizeOf<Vector128<byte>>();
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector128<ushort> sourceVector = Vector128.Load((ushort*)source);
                Vector128<ushort> sourceVector2 = Vector128.Load((ushort*)source + Vector128<ushort>.Count);
                Vector128<byte> maskVector = Vector128.Narrow(Vector128.GreaterThan(sourceVector, filterVector), Vector128.GreaterThan(sourceVector2, filterVector));
                Vector128.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector128.Narrow(sourceVector, sourceVector2)).Store(destination);
                if (length > (nuint)Vector128<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder == headRemainder2 * 2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = ((UnsafeHelper.SizeOf<Vector128<ushort>>() * 2) - headRemainder) / UnsafeHelper.SizeOf<char>();
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
                    source += Vector128<ushort>.Count * 2;
                    destination += Vector128<byte>.Count;
                    length -= (nuint)Vector128<ushort>.Count * 2;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector128<ushort> sourceVector = Vector128.LoadAligned((ushort*)source);
                Vector128<ushort> sourceVector2 = Vector128.LoadAligned((ushort*)source + Vector128<ushort>.Count);
                Vector128<byte> maskVector = Vector128.Narrow(Vector128.GreaterThan(sourceVector, filterVector), Vector128.GreaterThan(sourceVector2, filterVector));
                Vector128.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector128.Narrow(sourceVector, sourceVector2)).Store(destination);
                source += (nuint)Vector128<ushort>.Count * 2;
                destination += (nuint)Vector128<byte>.Count;
                length -= (nuint)Vector128<ushort>.Count * 2;
            } while (length >= (nuint)Vector128<ushort>.Count * 2);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector128<ushort> sourceVector = Vector128.LoadAligned((ushort*)source);
                Vector128<ushort> sourceVector2 = Vector128.LoadAligned((ushort*)source + Vector128<ushort>.Count);
                Vector128<byte> maskVector = Vector128.Narrow(Vector128.GreaterThan(sourceVector, filterVector), Vector128.GreaterThan(sourceVector2, filterVector));
                Vector128.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector128.Narrow(sourceVector, sourceVector2)).StoreAligned(destination);
                source += (nuint)Vector128<ushort>.Count * 2;
                destination += (nuint)Vector128<byte>.Count;
                length -= (nuint)Vector128<ushort>.Count * 2;
            } while (length >= (nuint)Vector128<ushort>.Count * 2);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector128<ushort>.Count * 2;
                destination = destination + length - (nuint)Vector128<byte>.Count;
                Vector128<ushort> sourceVector = Vector128.Load((ushort*)source);
                Vector128<ushort> sourceVector2 = Vector128.Load((ushort*)source + Vector128<ushort>.Count);
                Vector128<byte> maskVector = Vector128.Narrow(Vector128.GreaterThan(sourceVector, filterVector), Vector128.GreaterThan(sourceVector2, filterVector));
                Vector128.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector128.Narrow(sourceVector, sourceVector2)).Store(destination);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedReadFromUtf16BufferCore_OutOfLatin1Range_64(ref char* source, ref byte* destination, ref nuint length)
        {
            const byte ReplaceCharacter = 0x003F;

            Vector64<ushort> filterVector = Vector64.Create<ushort>(Latin1EncodingLimit);
            Vector64<byte> replaceVector = Vector64.Create(ReplaceCharacter);

            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % (UnsafeHelper.SizeOf<Vector64<ushort>>() * 2);
            nuint headRemainder2 = (nuint)destination % UnsafeHelper.SizeOf<Vector64<byte>>();
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector64<ushort> sourceVector = Vector64.Load((ushort*)source);
                Vector64<ushort> sourceVector2 = Vector64.Load((ushort*)source + Vector64<ushort>.Count);
                Vector64<byte> maskVector = Vector64.Narrow(Vector64.GreaterThan(sourceVector, filterVector), Vector64.GreaterThan(sourceVector2, filterVector));
                Vector64.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector64.Narrow(sourceVector, sourceVector2)).Store(destination);
                if (length > (nuint)Vector64<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder == headRemainder2 * 2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = ((UnsafeHelper.SizeOf<Vector64<ushort>>() * 2) - headRemainder) / UnsafeHelper.SizeOf<char>();
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
                    source += Vector64<ushort>.Count * 2;
                    destination += Vector64<byte>.Count;
                    length -= (nuint)Vector64<ushort>.Count * 2;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector64<ushort> sourceVector = Vector64.LoadAligned((ushort*)source);
                Vector64<ushort> sourceVector2 = Vector64.LoadAligned((ushort*)source + Vector64<ushort>.Count);
                Vector64<byte> maskVector = Vector64.Narrow(Vector64.GreaterThan(sourceVector, filterVector), Vector64.GreaterThan(sourceVector2, filterVector));
                Vector64.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector64.Narrow(sourceVector, sourceVector2)).Store(destination);
                source += (nuint)Vector64<ushort>.Count * 2;
                destination += (nuint)Vector64<byte>.Count;
                length -= (nuint)Vector64<ushort>.Count * 2;
            } while (length >= (nuint)Vector64<ushort>.Count * 2);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector64<ushort> sourceVector = Vector64.LoadAligned((ushort*)source);
                Vector64<ushort> sourceVector2 = Vector64.LoadAligned((ushort*)source + Vector64<ushort>.Count);
                Vector64<byte> maskVector = Vector64.Narrow(Vector64.GreaterThan(sourceVector, filterVector), Vector64.GreaterThan(sourceVector2, filterVector));
                Vector64.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector64.Narrow(sourceVector, sourceVector2)).StoreAligned(destination);
                source += (nuint)Vector64<ushort>.Count * 2;
                destination += (nuint)Vector64<byte>.Count;
                length -= (nuint)Vector64<ushort>.Count * 2;
            } while (length >= (nuint)Vector64<ushort>.Count * 2);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector64<ushort>.Count * 2;
                destination = destination + length - (nuint)Vector64<byte>.Count;
                Vector64<ushort> sourceVector = Vector64.Load((ushort*)source);
                Vector64<ushort> sourceVector2 = Vector64.Load((ushort*)source + Vector64<ushort>.Count);
                Vector64<byte> maskVector = Vector64.Narrow(Vector64.GreaterThan(sourceVector, filterVector), Vector64.GreaterThan(sourceVector2, filterVector));
                Vector64.ConditionalSelect(
                    condition: maskVector,
                    left: replaceVector,
                    right: Vector64.Narrow(sourceVector, sourceVector2)).Store(destination);
            }
        }

        private static partial byte* VectorizedReadFromUtf16BufferCore(char* source, byte* destination, nuint length)
        {
            byte* result = destination + length;
            if (Limits.UseVector512() && length >= (nuint)Vector512<byte>.Count)
                VectorizedReadFromUtf16BufferCore_512(ref source, ref destination, ref length);
            else if (Limits.UseVector256() && length >= (nuint)Vector256<byte>.Count)
                VectorizedReadFromUtf16BufferCore_256(ref source, ref destination, ref length);
            else if (Limits.UseVector128() && length >= (nuint)Vector128<byte>.Count)
                VectorizedReadFromUtf16BufferCore_128(ref source, ref destination, ref length);
            else
                VectorizedReadFromUtf16BufferCore_64(ref source, ref destination, ref length);
            return result;
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedReadFromUtf16BufferCore_512(ref char* source, ref byte* destination, ref nuint length)
        {
            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % (UnsafeHelper.SizeOf<Vector512<ushort>>() * 2);
            nuint headRemainder2 = (nuint)destination % UnsafeHelper.SizeOf<Vector512<byte>>();
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector512<ushort> sourceVector = Vector512.Load((ushort*)source);
                Vector512<ushort> sourceVector2 = Vector512.Load((ushort*)source + Vector512<ushort>.Count);
                Vector512.Narrow(sourceVector, sourceVector2).Store(destination);
                if (length > (nuint)Vector512<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder == headRemainder2 * 2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = ((UnsafeHelper.SizeOf<Vector512<ushort>>() * 2) - headRemainder) / UnsafeHelper.SizeOf<char>();
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
                    source += Vector512<ushort>.Count * 2;
                    destination += Vector512<byte>.Count;
                    length -= (nuint)Vector512<ushort>.Count * 2;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector512<ushort> sourceVector = Vector512.LoadAligned((ushort*)source);
                Vector512<ushort> sourceVector2 = Vector512.LoadAligned((ushort*)source + Vector512<ushort>.Count);
                Vector512.Narrow(sourceVector, sourceVector2).Store(destination);
                source += (nuint)Vector512<ushort>.Count * 2;
                destination += (nuint)Vector512<byte>.Count;
                length -= (nuint)Vector512<ushort>.Count * 2;
            } while (length >= (nuint)Vector512<ushort>.Count * 2);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector512<ushort> sourceVector = Vector512.LoadAligned((ushort*)source);
                Vector512<ushort> sourceVector2 = Vector512.LoadAligned((ushort*)source + Vector512<ushort>.Count);
                Vector512.Narrow(sourceVector, sourceVector2).StoreAligned(destination);
                source += (nuint)Vector512<ushort>.Count * 2;
                destination += (nuint)Vector512<byte>.Count;
                length -= (nuint)Vector512<ushort>.Count * 2;
            } while (length >= (nuint)Vector512<ushort>.Count * 2);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector512<ushort>.Count * 2;
                destination = destination + length - (nuint)Vector512<byte>.Count;
                Vector512<ushort> sourceVector = Vector512.Load((ushort*)source);
                Vector512<ushort> sourceVector2 = Vector512.Load((ushort*)source + Vector512<ushort>.Count);
                Vector512.Narrow(sourceVector, sourceVector2).Store(destination);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedReadFromUtf16BufferCore_256(ref char* source, ref byte* destination, ref nuint length)
        {
            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % (UnsafeHelper.SizeOf<Vector256<ushort>>() * 2);
            nuint headRemainder2 = (nuint)destination % UnsafeHelper.SizeOf<Vector256<byte>>();
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector256<ushort> sourceVector = Vector256.Load((ushort*)source);
                Vector256<ushort> sourceVector2 = Vector256.Load((ushort*)source + Vector256<ushort>.Count);
                Vector256.Narrow(sourceVector, sourceVector2).Store(destination);
                if (length > (nuint)Vector256<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder == headRemainder2 * 2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = ((UnsafeHelper.SizeOf<Vector256<ushort>>() * 2) - headRemainder) / UnsafeHelper.SizeOf<char>();
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
                    source += Vector256<ushort>.Count * 2;
                    destination += Vector256<byte>.Count;
                    length -= (nuint)Vector256<ushort>.Count * 2;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector256<ushort> sourceVector = Vector256.LoadAligned((ushort*)source);
                Vector256<ushort> sourceVector2 = Vector256.LoadAligned((ushort*)source + Vector256<ushort>.Count);
                Vector256.Narrow(sourceVector, sourceVector2).Store(destination);
                source += (nuint)Vector256<ushort>.Count * 2;
                destination += (nuint)Vector256<byte>.Count;
                length -= (nuint)Vector256<ushort>.Count * 2;
            } while (length >= (nuint)Vector256<ushort>.Count * 2);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector256<ushort> sourceVector = Vector256.LoadAligned((ushort*)source);
                Vector256<ushort> sourceVector2 = Vector256.LoadAligned((ushort*)source + Vector256<ushort>.Count);
                Vector256.Narrow(sourceVector, sourceVector2).StoreAligned(destination);
                source += (nuint)Vector256<ushort>.Count * 2;
                destination += (nuint)Vector256<byte>.Count;
                length -= (nuint)Vector256<ushort>.Count * 2;
            } while (length >= (nuint)Vector256<ushort>.Count * 2);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector256<ushort>.Count * 2;
                destination = destination + length - (nuint)Vector256<byte>.Count;
                Vector256<ushort> sourceVector = Vector256.Load((ushort*)source);
                Vector256<ushort> sourceVector2 = Vector256.Load((ushort*)source + Vector256<ushort>.Count);
                Vector256.Narrow(sourceVector, sourceVector2).Store(destination);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedReadFromUtf16BufferCore_128(ref char* source, ref byte* destination, ref nuint length)
        {
            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % (UnsafeHelper.SizeOf<Vector128<ushort>>() * 2);
            nuint headRemainder2 = (nuint)destination % UnsafeHelper.SizeOf<Vector128<byte>>();
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector128<ushort> sourceVector = Vector128.Load((ushort*)source);
                Vector128<ushort> sourceVector2 = Vector128.Load((ushort*)source + Vector128<ushort>.Count);
                Vector128.Narrow(sourceVector, sourceVector2).Store(destination);
                if (length > (nuint)Vector128<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder == headRemainder2 * 2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = ((UnsafeHelper.SizeOf<Vector128<ushort>>() * 2) - headRemainder) / UnsafeHelper.SizeOf<char>();
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
                    source += Vector128<ushort>.Count * 2;
                    destination += Vector128<byte>.Count;
                    length -= (nuint)Vector128<ushort>.Count * 2;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector128<ushort> sourceVector = Vector128.LoadAligned((ushort*)source);
                Vector128<ushort> sourceVector2 = Vector128.LoadAligned((ushort*)source + Vector128<ushort>.Count);
                Vector128.Narrow(sourceVector, sourceVector2).Store(destination);
                source += (nuint)Vector128<ushort>.Count * 2;
                destination += (nuint)Vector128<byte>.Count;
                length -= (nuint)Vector128<ushort>.Count * 2;
            } while (length >= (nuint)Vector128<ushort>.Count * 2);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector128<ushort> sourceVector = Vector128.LoadAligned((ushort*)source);
                Vector128<ushort> sourceVector2 = Vector128.LoadAligned((ushort*)source + Vector128<ushort>.Count);
                Vector128.Narrow(sourceVector, sourceVector2).StoreAligned(destination);
                source += (nuint)Vector128<ushort>.Count * 2;
                destination += (nuint)Vector128<byte>.Count;
                length -= (nuint)Vector128<ushort>.Count * 2;
            } while (length >= (nuint)Vector128<ushort>.Count * 2);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector128<ushort>.Count * 2;
                destination = destination + length - (nuint)Vector128<byte>.Count;
                Vector128<ushort> sourceVector = Vector128.Load((ushort*)source);
                Vector128<ushort> sourceVector2 = Vector128.Load((ushort*)source + Vector128<ushort>.Count);
                Vector128.Narrow(sourceVector, sourceVector2).Store(destination);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedReadFromUtf16BufferCore_64(ref char* source, ref byte* destination, ref nuint length)
        {
            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % (UnsafeHelper.SizeOf<Vector64<ushort>>() * 2);
            nuint headRemainder2 = (nuint)destination % UnsafeHelper.SizeOf<Vector64<byte>>();
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector64<ushort> sourceVector = Vector64.Load((ushort*)source);
                Vector64<ushort> sourceVector2 = Vector64.Load((ushort*)source + Vector64<ushort>.Count);
                Vector64.Narrow(sourceVector, sourceVector2).Store(destination);
                if (length > (nuint)Vector64<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder == headRemainder2 * 2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = ((UnsafeHelper.SizeOf<Vector64<ushort>>() * 2) - headRemainder) / UnsafeHelper.SizeOf<char>();
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
                    source += Vector64<ushort>.Count * 2;
                    destination += Vector64<byte>.Count;
                    length -= (nuint)Vector64<ushort>.Count * 2;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector64<ushort> sourceVector = Vector64.LoadAligned((ushort*)source);
                Vector64<ushort> sourceVector2 = Vector64.LoadAligned((ushort*)source + Vector64<ushort>.Count);
                Vector64.Narrow(sourceVector, sourceVector2).Store(destination);
                source += (nuint)Vector64<ushort>.Count * 2;
                destination += (nuint)Vector64<byte>.Count;
                length -= (nuint)Vector64<ushort>.Count * 2;
            } while (length >= (nuint)Vector64<ushort>.Count * 2);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector64<ushort> sourceVector = Vector64.LoadAligned((ushort*)source);
                Vector64<ushort> sourceVector2 = Vector64.LoadAligned((ushort*)source + Vector64<ushort>.Count);
                Vector64.Narrow(sourceVector, sourceVector2).StoreAligned(destination);
                source += (nuint)Vector64<ushort>.Count * 2;
                destination += (nuint)Vector64<byte>.Count;
                length -= (nuint)Vector64<ushort>.Count * 2;
            } while (length >= (nuint)Vector64<ushort>.Count * 2);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector64<ushort>.Count * 2;
                destination = destination + length - (nuint)Vector64<byte>.Count;
                Vector64<ushort> sourceVector = Vector64.Load((ushort*)source);
                Vector64<ushort> sourceVector2 = Vector64.Load((ushort*)source + Vector64<ushort>.Count);
                Vector64.Narrow(sourceVector, sourceVector2).Store(destination);
            }
        }

        private static partial char* VectorizedWriteToUtf16BufferCore(byte* source, char* destination, nuint length)
        {
            char* result = destination + length;
            if (Limits.UseVector512() && length >= (nuint)Vector512<byte>.Count)
                VectorizedWriteToUtf16BufferCore_512(ref source, ref destination, ref length);
            else if (Limits.UseVector256() && length >= (nuint)Vector256<byte>.Count)
                VectorizedWriteToUtf16BufferCore_256(ref source, ref destination, ref length);
            else if (Limits.UseVector128() && length >= (nuint)Vector128<byte>.Count)
                VectorizedWriteToUtf16BufferCore_128(ref source, ref destination, ref length);
            else
                VectorizedWriteToUtf16BufferCore_64(ref source, ref destination, ref length);
            return result;

        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedWriteToUtf16BufferCore_512(ref byte* source, ref char* destination, ref nuint length)
        {
            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % UnsafeHelper.SizeOf<Vector512<byte>>();
            nuint headRemainder2 = (nuint)destination % (UnsafeHelper.SizeOf<Vector512<ushort>>() * 2);
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector512<byte> sourceVector = Vector512.Load(source);
                (Vector512<ushort> destinationVectorLow, Vector512<ushort> destinationVectorHigh) = Vector512.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector512<ushort>.Count);
                if (length > (nuint)Vector512<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder * 2 == headRemainder2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = (UnsafeHelper.SizeOf<Vector512<byte>>() - headRemainder) / UnsafeHelper.SizeOf<byte>();
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
                    source += Vector512<byte>.Count;
                    destination += Vector512<ushort>.Count * 2;
                    length -= (nuint)Vector512<byte>.Count;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector512<byte> sourceVector = Vector512.LoadAligned(source);
                (Vector512<ushort> destinationVectorLow, Vector512<ushort> destinationVectorHigh) = Vector512.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector512<ushort>.Count);
                source += Vector512<byte>.Count;
                destination += Vector512<ushort>.Count * 2;
                length -= (nuint)Vector512<byte>.Count;
            } while (length >= (nuint)Vector512<byte>.Count);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector512<byte> sourceVector = Vector512.LoadAligned(source);
                (Vector512<ushort> destinationVectorLow, Vector512<ushort> destinationVectorHigh) = Vector512.Widen(sourceVector);
                destinationVectorLow.StoreAligned((ushort*)destination);
                destinationVectorHigh.StoreAligned((ushort*)destination + Vector512<ushort>.Count);
                source += Vector512<byte>.Count;
                destination += Vector512<ushort>.Count * 2;
                length -= (nuint)Vector512<byte>.Count;
            } while (length >= (nuint)Vector512<byte>.Count);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector512<byte>.Count;
                destination = destination + length - (nuint)Vector512<ushort>.Count * 2;
                Vector512<byte> sourceVector = Vector512.Load(source);
                (Vector512<ushort> destinationVectorLow, Vector512<ushort> destinationVectorHigh) = Vector512.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector512<ushort>.Count);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedWriteToUtf16BufferCore_256(ref byte* source, ref char* destination, ref nuint length)
        {
            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % UnsafeHelper.SizeOf<Vector256<byte>>();
            nuint headRemainder2 = (nuint)destination % (UnsafeHelper.SizeOf<Vector256<ushort>>() * 2);
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector256<byte> sourceVector = Vector256.Load(source);
                (Vector256<ushort> destinationVectorLow, Vector256<ushort> destinationVectorHigh) = Vector256.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector256<ushort>.Count);
                if (length > (nuint)Vector256<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder * 2 == headRemainder2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = (UnsafeHelper.SizeOf<Vector256<byte>>() - headRemainder) / UnsafeHelper.SizeOf<byte>();
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
                    source += Vector256<byte>.Count;
                    destination += Vector256<ushort>.Count * 2;
                    length -= (nuint)Vector256<byte>.Count;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector256<byte> sourceVector = Vector256.LoadAligned(source);
                (Vector256<ushort> destinationVectorLow, Vector256<ushort> destinationVectorHigh) = Vector256.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector256<ushort>.Count);
                source += Vector256<byte>.Count;
                destination += Vector256<ushort>.Count * 2;
                length -= (nuint)Vector256<byte>.Count;
            } while (length >= (nuint)Vector256<byte>.Count);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector256<byte> sourceVector = Vector256.LoadAligned(source);
                (Vector256<ushort> destinationVectorLow, Vector256<ushort> destinationVectorHigh) = Vector256.Widen(sourceVector);
                destinationVectorLow.StoreAligned((ushort*)destination);
                destinationVectorHigh.StoreAligned((ushort*)destination + Vector256<ushort>.Count);
                source += Vector256<byte>.Count;
                destination += Vector256<ushort>.Count * 2;
                length -= (nuint)Vector256<byte>.Count;
            } while (length >= (nuint)Vector256<byte>.Count);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector256<byte>.Count;
                destination = destination + length - (nuint)Vector256<ushort>.Count * 2;
                Vector256<byte> sourceVector = Vector256.Load(source);
                (Vector256<ushort> destinationVectorLow, Vector256<ushort> destinationVectorHigh) = Vector256.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector256<ushort>.Count);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedWriteToUtf16BufferCore_128(ref byte* source, ref char* destination, ref nuint length)
        {
            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % UnsafeHelper.SizeOf<Vector128<byte>>();
            nuint headRemainder2 = (nuint)destination % (UnsafeHelper.SizeOf<Vector128<ushort>>() * 2);
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector128<byte> sourceVector = Vector128.Load(source);
                (Vector128<ushort> destinationVectorLow, Vector128<ushort> destinationVectorHigh) = Vector128.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector128<ushort>.Count);
                if (length > (nuint)Vector128<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder * 2 == headRemainder2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = (UnsafeHelper.SizeOf<Vector128<byte>>() - headRemainder) / UnsafeHelper.SizeOf<byte>();
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
                    source += Vector128<byte>.Count;
                    destination += Vector128<ushort>.Count * 2;
                    length -= (nuint)Vector128<byte>.Count;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector128<byte> sourceVector = Vector128.LoadAligned(source);
                (Vector128<ushort> destinationVectorLow, Vector128<ushort> destinationVectorHigh) = Vector128.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector128<ushort>.Count);
                source += Vector128<byte>.Count;
                destination += Vector128<ushort>.Count * 2;
                length -= (nuint)Vector128<byte>.Count;
            } while (length >= (nuint)Vector128<byte>.Count);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector128<byte> sourceVector = Vector128.LoadAligned(source);
                (Vector128<ushort> destinationVectorLow, Vector128<ushort> destinationVectorHigh) = Vector128.Widen(sourceVector);
                destinationVectorLow.StoreAligned((ushort*)destination);
                destinationVectorHigh.StoreAligned((ushort*)destination + Vector128<ushort>.Count);
                source += Vector128<byte>.Count;
                destination += Vector128<ushort>.Count * 2;
                length -= (nuint)Vector128<byte>.Count;
            } while (length >= (nuint)Vector128<byte>.Count);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector128<byte>.Count;
                destination = destination + length - (nuint)Vector128<ushort>.Count * 2;
                Vector128<byte> sourceVector = Vector128.Load(source);
                (Vector128<ushort> destinationVectorLow, Vector128<ushort> destinationVectorHigh) = Vector128.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector128<ushort>.Count);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static void VectorizedWriteToUtf16BufferCore_64(ref byte* source, ref char* destination, ref nuint length)
        {
            // 取得非對齊的位元組偏移
            nuint headRemainder = (nuint)source % UnsafeHelper.SizeOf<Vector64<byte>>();
            nuint headRemainder2 = (nuint)destination % (UnsafeHelper.SizeOf<Vector64<ushort>>() * 2);
            if (headRemainder == 0 && headRemainder2 == 0)
                goto VectorizedLoop_FullAligned;
            else
            {
                Vector64<byte> sourceVector = Vector64.Load(source);
                (Vector64<ushort> destinationVectorLow, Vector64<ushort> destinationVectorHigh) = Vector64.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector64<ushort>.Count);
                if (length > (nuint)Vector64<ushort>.Count * 4)
                {
                    bool canFullyAligned = headRemainder * 2 == headRemainder2;
                    // 此處之後，headRemainder 變成殘留元素數量，而不是非對齊的位元組偏移!
                    headRemainder = (UnsafeHelper.SizeOf<Vector64<byte>>() - headRemainder) / UnsafeHelper.SizeOf<byte>();
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
                    source += Vector64<byte>.Count;
                    destination += Vector64<ushort>.Count * 2;
                    length -= (nuint)Vector64<byte>.Count;
                    goto TailProcess;
                }
            }

        VectorizedLoop_PartialAligned:
            do
            {
                Vector64<byte> sourceVector = Vector64.LoadAligned(source);
                (Vector64<ushort> destinationVectorLow, Vector64<ushort> destinationVectorHigh) = Vector64.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector64<ushort>.Count);
                source += Vector64<byte>.Count;
                destination += Vector64<ushort>.Count * 2;
                length -= (nuint)Vector64<byte>.Count;
            } while (length >= (nuint)Vector64<byte>.Count);
            goto TailProcess;

        VectorizedLoop_FullAligned:
            do
            {
                Vector64<byte> sourceVector = Vector64.LoadAligned(source);
                (Vector64<ushort> destinationVectorLow, Vector64<ushort> destinationVectorHigh) = Vector64.Widen(sourceVector);
                destinationVectorLow.StoreAligned((ushort*)destination);
                destinationVectorHigh.StoreAligned((ushort*)destination + Vector64<ushort>.Count);
                source += Vector64<byte>.Count;
                destination += Vector64<ushort>.Count * 2;
                length -= (nuint)Vector64<byte>.Count;
            } while (length >= (nuint)Vector64<byte>.Count);
            goto TailProcess;

        TailProcess:
            if (length > 0)
            {
                source = source + length - (nuint)Vector64<byte>.Count;
                destination = destination + length - (nuint)Vector64<ushort>.Count * 2;
                Vector64<byte> sourceVector = Vector64.Load(source);
                (Vector64<ushort> destinationVectorLow, Vector64<ushort> destinationVectorHigh) = Vector64.Widen(sourceVector);
                destinationVectorLow.Store((ushort*)destination);
                destinationVectorHigh.Store((ushort*)destination + Vector64<ushort>.Count);
            }
        }
    }
}
#endif