using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class Utf8String : StringBase, IPinnableReference<byte>
    {
        private const int MaxUtf8StringBufferSize = Limits.MaxArrayLength - 1;
        private const int Utf16CompressionLengthLimit = Limits.MaxArrayLength / 2 - 1;
        private const byte AsciiCharacterLimit = 0x007F;

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
        public static unsafe StringBase Create(byte* source)
        {
            bool isAsciiOnly = true;
            nuint length = 0;
            do
            {
                byte c = source[length];
                if (c == 0)
                    break;
                if (isAsciiOnly && c > AsciiCharacterLimit)
                    isAsciiOnly = false;
                if (++length > MaxUtf8StringBufferSize)
                    throw new OutOfMemoryException();
            } while (true);

            if (isAsciiOnly)
                return Latin1String.Create(source, length);

            byte[] buffer = new byte[length]; // Tail zero is included
            fixed (byte* ptr = buffer)
                UnsafeHelper.CopyBlockUnaligned(ptr, source, unchecked((uint)length * sizeof(byte)));

            int resultLength = 0;
            using CharEnumerator enumerator = new CharEnumerator(buffer);
            while (enumerator.MoveNext())
                resultLength++;
            return new Utf8String(buffer, resultLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringBase Create(byte* source, nuint length)
        {
            if (length > MaxUtf8StringBufferSize)
                throw new OutOfMemoryException();

            if (!SequenceHelper.ContainsGreaterThan(source, length, AsciiCharacterLimit))
            {
                Latin1String result = Latin1String.Allocate(length, out byte[] buffer);
                fixed (byte* ptr = buffer)
                    UnsafeHelper.CopyBlockUnaligned(ptr, source, unchecked((uint)length * sizeof(byte)));
                return result;
            }
            else
            {
                byte[] buffer = new byte[length + 1];
                fixed (byte* ptr = buffer)
                    UnsafeHelper.CopyBlockUnaligned(ptr, source, unchecked((uint)length * sizeof(byte)));

                int resultLength = 0;
                using CharEnumerator enumerator = new CharEnumerator(buffer);
                while (enumerator.MoveNext())
                    resultLength++;
                return new Utf8String(buffer, resultLength);
            }
        }

        public static unsafe bool TryCreate(char* source, nuint length, StringCreateOptions options, [NotNullWhen(true)] out StringBase? result)
        {
            if (length > MaxUtf8StringBufferSize)
                goto Failed;

            if (!SequenceHelper.ContainsGreaterThan(source, length, (char)AsciiCharacterLimit))
            {
                result = Latin1String.Allocate(length, out byte[] buffer);
                fixed (byte* ptr = buffer)
                    Latin1StringHelper.NarrowAndCopyTo(source, length, ptr);
                return true;
            }

            bool tryEncodeAsPossible = (options & StringCreateOptions._ForceUseUtf8_Flag) == StringCreateOptions._ForceUseUtf8_Flag;
            return TryCreateCore(source, length, tryEncodeAsPossible, out result);

        Failed:
            result = null;
            return false;
        }

        private static unsafe bool TryCreateCore(char* source, nuint length, bool tryEncodeAsPossible, [NotNullWhen(true)] out StringBase? result)
        {
            nuint bufferLength;

            if (tryEncodeAsPossible)
            {
                bufferLength = MathHelper.Min(Utf8EncodingHelper.GetWorstCaseForEncodeLength(length), MaxUtf8StringBufferSize);
            }
            else
            {
                if (length > Utf16CompressionLengthLimit)
                    goto Failed;

                bufferLength = length * sizeof(char);
            }

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
                        UnsafeHelper.CopyBlockUnaligned(dest, ptr, unchecked((uint)resultLength * sizeof(byte)));
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

        protected internal override char GetCharAt(nuint index)
        {
            using CharEnumerator enumerator = new CharEnumerator(_value);
            for (nuint i = 0; i <= index; i++)
                enumerator.MoveNext();
            return enumerator.Current;
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

        protected override unsafe string ToStringCore()
        {
            byte[] source = _value;
            int length = _length;
            string result = StringHelper.AllocateRawString(length);
            fixed (byte* ptrSource = source)
            fixed (char* ptrResult = result)
                Utf8EncodingHelper.TryWriteToUtf16BufferCore(ptrSource, ptrSource + source.Length - 1, ptrResult, ptrResult + length);
            return result;
        }

        ref readonly byte IPinnableReference<byte>.GetPinnableReference() => ref _value[0];

        nuint IPinnableReference<byte>.GetPinnedLength() => MathHelper.MakeUnsigned(_value.Length);
    }
}
