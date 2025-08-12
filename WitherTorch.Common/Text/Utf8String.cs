using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class Utf8String : StringBase, IPinnableReference<byte>,
        IHolder<ArraySegment<byte>>
    {
        private static readonly nuint MaxUtf8StringBufferSize = unchecked((nuint)Limits.MaxArrayLength - 1);
        private static readonly nuint Utf16CompressionLengthLimit = unchecked((nuint)Limits.MaxArrayLength / 2 - 1);

        private readonly byte[] _value;
        private readonly int _length;

        public override StringType StringType => StringType.Utf8;
        public override int Length => _length;

        private Utf8String(byte[] value, int length)
        {
            _value = value;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase Create(byte* source, nuint length)
        {
            if (length > MaxUtf8StringBufferSize)
                throw new OutOfMemoryException();

            if (!SequenceHelper.ContainsGreaterThan(source, length, AsciiEncodingHelper.AsciiEncodingLimit_InByte))
            {
                AsciiString result = AsciiString.Allocate(length, out byte[] buffer);
                fixed (byte* ptr = buffer)
                    UnsafeHelper.CopyBlockUnaligned(ptr, source, length * sizeof(byte));
                return result;
            }
            else
            {
                byte[] buffer = new byte[length + 1];
                fixed (byte* ptr = buffer)
                    UnsafeHelper.CopyBlockUnaligned(ptr, source, length * sizeof(byte));

                int resultLength = 0;
                using CharEnumerator enumerator = new CharEnumerator(buffer);
                while (enumerator.MoveNext())
                    resultLength++;
                return new Utf8String(buffer, resultLength);
            }
        }

        public static unsafe bool TryCreate(char* source, nuint length, StringCreateOptions options, [NotNullWhen(true)] out StringBase? result)
        {
            if ((options & StringCreateOptions.UseAsciiCompression) != StringCreateOptions.UseAsciiCompression &&
                !SequenceHelper.ContainsGreaterThan(source, length, AsciiEncodingHelper.AsciiEncodingLimit))
            {
                result = AsciiString.Allocate(length, out byte[] buffer);
                fixed (byte* ptr = buffer)
                    AsciiEncodingHelper.ReadFromUtf16BufferCore(source, ptr, length);
                return true;
            }

            bool tryEncodeAsPossible = (options & StringCreateOptions._Force_Flag) == StringCreateOptions._Force_Flag;
            return TryCreateCore(source, length, tryEncodeAsPossible, out result);
        }

        private static unsafe bool TryCreateCore(char* source, nuint length, bool tryEncodeAsPossible, [NotNullWhen(true)] out StringBase? result)
        {
            nuint bufferLength;

            if (tryEncodeAsPossible)
                bufferLength = Utf8EncodingHelper.GetWorstCaseForEncodeLength(length);
            else
                bufferLength = length * sizeof(char);
            bufferLength = MathHelper.Min(bufferLength, MaxUtf8StringBufferSize);

            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(bufferLength);
            try
            {
                fixed (byte* ptr = buffer)
                {
                    byte* bufferEnd = Utf8EncodingHelper.TryReadFromUtf16BufferCore(source, source + length, ptr, ptr + bufferLength);
                    if (bufferEnd == null)
                        goto Failed;
                    nuint resultLength = unchecked((nuint)(bufferEnd - ptr));
                    byte[] resultBuffer = new byte[resultLength + 1];
                    fixed (byte* dest = resultBuffer)
                        UnsafeHelper.CopyBlockUnaligned(dest, ptr, resultLength * sizeof(byte));
                    result = new Utf8String(resultBuffer, unchecked((int)length));
                    return true;
                }
            }
            finally
            {
                pool.Return(buffer);
            }

        Failed:
            result = null;
            return false;
        }

        public override IEnumerator<char> GetEnumerator() => new CharEnumerator(_value);

        protected internal override unsafe void CopyToCore(char* destination, nuint startIndex, nuint count)
        {
            byte[] source = _value;
            fixed (byte* ptrSource = source)
            {
                byte* sourceIterator = ptrSource, sourceEnd = ptrSource + source.Length - 1;
                nuint offset = SkipCharacters(ref sourceIterator, sourceEnd, destination, startIndex);
                Utf8EncodingHelper.TryWriteToUtf16BufferCore(sourceIterator, sourceEnd, destination + offset, destination + count);
            }
        }

        public byte[] GetInternalRepresentation() => _value;

        ref readonly byte IPinnableReference<byte>.GetPinnableReference()
        {
            byte[] value = _value;
            int length = value.Length;
            if (length <= 0)
                return ref ((IPinnableReference<byte>)Empty).GetPinnableReference();
            return ref value[0];
        }

        nuint IPinnableReference<byte>.GetPinnedLength() => MathHelper.MakeUnsigned(_value.Length - 1);

        ArraySegment<byte> IHolder<ArraySegment<byte>>.Value => new ArraySegment<byte>(_value, 0, _value.Length - 1);
    }
}
