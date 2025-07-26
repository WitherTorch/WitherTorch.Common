using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class Utf8String : StringBase
    {
        private const int MaxUtf8StringBufferSize = Limits.MaxArrayLength - 1;
        private const int Utf16CompressionLengthLimit = Limits.MaxArrayLength / 2 - 1;
        private const byte AsciiCharacterLimit = 0x007F;

        private readonly byte[] _value;
        private readonly int _length;
        private readonly bool _isAsciiOnly;

        public override StringType StringType => StringType.Utf8;
        public override int Length => _length;
        public bool IsAsciiOnly => _isAsciiOnly;

        private Utf8String(byte[] value, int length, bool isAsciiOnly)
        {
            _value = value;
            _length = length;
            _isAsciiOnly = isAsciiOnly;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Utf8String Create(byte* source)
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

            byte[] buffer = new byte[length]; // Tail zero is included
            fixed (byte* ptr = buffer)
                UnsafeHelper.CopyBlockUnaligned(ptr, source, unchecked((uint)length * sizeof(byte)));

            if (isAsciiOnly)
                return new Utf8String(buffer, unchecked((int)length), isAsciiOnly: true);

            int resultLength = 0;
            fixed (byte* ptr = buffer)
            {
                byte* iterator = ptr, ptrEnd = ptr + length + 1;
                while ((iterator = Utf8StringHelper.TryReadUtf8Character(iterator, ptrEnd, out uint unicodeValue)) != null)
                    resultLength += Utf8StringHelper.ToUtf16Characters(unicodeValue, out _, out _) ? 2 : 1;
            }
            return new Utf8String(buffer, resultLength, isAsciiOnly: false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Utf8String Create(byte* source, nuint length)
        {
            if (length > MaxUtf8StringBufferSize)
                throw new OutOfMemoryException();

            byte[] buffer = new byte[length + 1];
            fixed (byte* ptr = buffer)
            {
                UnsafeHelper.CopyBlockUnaligned(ptr, source, unchecked((uint)length * sizeof(byte)));
                if (!SequenceHelper.ContainsGreaterThan(ptr, length, AsciiCharacterLimit))
                    return new Utf8String(buffer, (int)length, isAsciiOnly: true);

                byte* iterator = ptr, ptrEnd = ptr + length + 1;
                int resultLength = 0;
                while ((iterator = Utf8StringHelper.TryReadUtf8Character(iterator, ptrEnd, out uint unicodeValue)) != null)
                    resultLength += Utf8StringHelper.ToUtf16Characters(unicodeValue, out _, out _) ? 2 : 1;

                return new Utf8String(buffer, resultLength, isAsciiOnly: false);
            }
        }

        public static unsafe bool TryCreate(char* source, nuint length, [NotNullWhen(true)] out Utf8String? result)
        {
            if (length > MaxUtf8StringBufferSize)
                goto Failed;

            if (!SequenceHelper.ContainsGreaterThan(source, length, unchecked((char)AsciiCharacterLimit)))
            {
                byte[] buffer = new byte[length + 1];
                fixed (byte* dest = buffer)
                    Latin1StringHelper.NarrowAndCopyTo(source, length, dest);
                result = new Utf8String(buffer, unchecked((int)length), isAsciiOnly: true);
                return true;
            }

            return TryCreateCore(source, length, out result);

        Failed:
            result = null;
            return false;
        }

        private static unsafe bool TryCreateCore(char* source, nuint length, [NotNullWhen(true)] out Utf8String? result)
        {
            if (length > Utf16CompressionLengthLimit)
                goto Failed;

            nuint bufferLength = length * sizeof(char);
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(bufferLength);
            try
            {
                fixed (byte* ptr = buffer)
                {
                    char* sourceEnd = source + length;
                    byte* iterator = ptr, ptrEnd = ptr + bufferLength;
                    while ((source = Utf8StringHelper.TryReadUtf16Character(source, sourceEnd, out uint unicodeValue)) != null)
                    {
                        if ((iterator = Utf8StringHelper.TryWriteUtf8Character(iterator, ptrEnd, unicodeValue)) == null)
                            goto Failed;
                    }
                    nuint resultLength = unchecked((nuint)(iterator - ptr));
                    byte[] resultBuffer = new byte[resultLength + 1];
                    fixed (byte* dest = resultBuffer)
                        UnsafeHelper.CopyBlockUnaligned(dest, ptr, unchecked((uint)resultLength * sizeof(byte)));
                    result = new Utf8String(resultBuffer, unchecked((int)length), isAsciiOnly: false);
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
            if (_isAsciiOnly)
                return unchecked((char)_value[index]); // O(1) 時間複雜度

            // O(N) 時間複雜度
            using CharEnumerator enumerator = new CharEnumerator(_value);
            for (nuint i = 0; i <= index; i++)
                enumerator.MoveNext();
            return enumerator.Current;
        }

        public override IEnumerator<char> GetEnumerator() => new CharEnumerator(_value);

        protected internal override unsafe void CopyToCore(char* destination, nuint startIndex, nuint count)
        {
            if (_isAsciiOnly)
            {
                CopyToCoreFast(destination, startIndex, count);
                return;
            }

            CopyToCoreSlow(destination, startIndex, count);
        }

        public byte[] GetInternalRepresentation() => _value;

        protected override unsafe string ToStringCore()
        {
            byte[] source = _value;
            int length = _length;
            string result = StringHelper.AllocateRawString(length);
            if (_isAsciiOnly)
                ToStringCoreFast(source, result, unchecked((nuint)length));
            else
                ToStringCoreSlow(source, result, unchecked((nuint)length));
            return result;
        }

        private unsafe void CopyToCoreFast(char* destination, nuint startIndex, nuint count)
        {
            fixed (byte* ptr = _value)
                Latin1StringHelper.WidenAndCopyTo(ptr + startIndex, count, destination);
        }

        private unsafe void CopyToCoreSlow(char* destination, nuint startIndex, nuint count)
        {
            byte[] source = _value;
            fixed (byte* ptrSource = source)
            {
                byte* sourceIterator = ptrSource, sourceEnd = ptrSource + source.Length;
                nuint offset = SkipCharacters(ref sourceIterator, sourceEnd, destination, startIndex);
                Utf8StringHelper.WriteToUtf16Buffer(sourceIterator, sourceEnd, destination + offset, destination + count);
            }
        }

        private static unsafe void ToStringCoreFast(byte[] source, string result, nuint length)
        {
            fixed (byte* ptrSource = source)
            fixed (char* ptrResult = result)
                Latin1StringHelper.WidenAndCopyTo(ptrSource, length, ptrResult);
        }

        private static unsafe void ToStringCoreSlow(byte[] source, string result, nuint length)
        {
            fixed (byte* ptrSource = source)
            fixed (char* ptrResult = result)
                Utf8StringHelper.WriteToUtf16Buffer(ptrSource, ptrSource + source.Length, ptrResult, ptrResult + length);
        }
    }
}
