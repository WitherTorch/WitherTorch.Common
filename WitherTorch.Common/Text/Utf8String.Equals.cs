using System.Linq;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Extensions;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf8String
    {
        protected override unsafe bool PartiallyEqualsCore(string other, nuint startIndex, nuint count)
        {
            fixed (char* ptr = other)
                return PartiallyEqualsCore(ptr, startIndex, count);
        }

        protected override bool PartiallyEqualsCore(StringBase other, nuint startIndex, nuint count)
            => other switch
            {
                Utf8String utf8 => PartiallyEqualsCore(utf8, startIndex, count),
                Utf16String utf16 => PartiallyEqualsCore(utf16, startIndex, count),
                Latin1String latin1 => PartiallyEqualsCore(latin1, startIndex, count),
                AsciiString ascii => PartiallyEqualsCore(ascii, startIndex, count),
                _ => base.PartiallyEqualsCore(other, startIndex, count)
            };

        protected override unsafe int CompareToCore(string other, nuint length)
        {
            fixed (char* ptr = other)
                return CompareToCore(ptr, length);
        }

        protected override int CompareToCore(StringBase other, nuint length)
            => other switch
            {
                Utf8String utf8 => CompareToCore(utf8),
                Utf16String utf16 => CompareToCore(utf16, length),
                Latin1String latin1 => CompareToCore(latin1, length),
                AsciiString ascii => CompareToCore(ascii, length),
                _ => base.CompareToCore(other, length)
            };

        protected override unsafe bool EqualsCore(string other, nuint length)
        {
            fixed (char* ptr = other)
                return EqualsCore(ptr, length);
        }

        protected override bool EqualsCore(StringBase other, nuint length)
            => other switch
            {
                Utf8String utf8 => EqualsCore(utf8, length),
                Utf16String utf16 => EqualsCore(utf16, length),
                Latin1String latin1 => EqualsCore(latin1, length),
                AsciiString ascii => EqualsCore(ascii, length),
                _ => base.EqualsCore(other, length)
            };

        private unsafe bool PartiallyEqualsCore(Utf16String other, nuint startIndex, nuint count)
        {
            fixed (char* ptr = other.GetInternalRepresentation())
                return PartiallyEqualsCore(ptr, startIndex, count);
        }

        private unsafe bool PartiallyEqualsCore(Utf8String other, nuint startIndex, nuint count)
            => this.SkipAndTake(startIndex, count).SequenceEqual(other);

        private unsafe bool PartiallyEqualsCore(Latin1String other, nuint startIndex, nuint count)
        {
            fixed (byte* ptr = other.GetInternalRepresentation())
            {
                if (SequenceHelper.ContainsGreaterThan(ptr, count, AsciiEncodingHelper.AsciiEncodingLimit_InByte))
                    return PartiallyEqualsCoreSlow(ptr, startIndex, count);
                return PartiallyEqualsCoreFast(ptr, startIndex, count);
            }
        }

        private unsafe bool PartiallyEqualsCore(AsciiString other, nuint startIndex, nuint count)
        {
            fixed (byte* ptr = other.GetInternalRepresentation())
                return PartiallyEqualsCoreFast(ptr, startIndex, count);
        }

        private unsafe bool PartiallyEqualsCore(char* other, nuint startIndex, nuint count)
        {
            if (SequenceHelper.ContainsGreaterThan(other, count, AsciiEncodingHelper.AsciiEncodingLimit))
                return PartiallyEqualsCoreSlow(other, startIndex, count);
            return PartiallyEqualsCoreFast(other, startIndex, count);
        }

        private unsafe bool PartiallyEqualsCoreFast(char* other, nuint startIndex, nuint count)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(count);
            try
            {
                fixed (byte* temp = buffer)
                {
                    AsciiEncodingHelper.ReadFromUtf16BufferCore(other, temp, count);
                    return PartiallyEqualsCoreFast(temp, startIndex, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool PartiallyEqualsCoreFast(byte* other, nuint startIndex, nuint count)
        {
            fixed (byte* source = _value)
                return SequenceHelper.Equals(source + startIndex, other, count);
        }

        private unsafe bool PartiallyEqualsCoreSlow(char* other, nuint startIndex, nuint count)
            => this.SkipAndTake(startIndex, count).SequenceEqual(other, count);

        private unsafe bool PartiallyEqualsCoreSlow(byte* other, nuint startIndex, nuint count)
            => this.SkipAndTake(startIndex, count)
            .WithNativeIndex()
            .Select(item => item.Value == other[item.Index])
            .NativeCount() == count;

        private unsafe int CompareToCore(Utf16String other, nuint length)
        {
            fixed (char* ptr = other.GetInternalRepresentation())
                return CompareToCore(ptr, length);
        }

        private int CompareToCore(Utf8String other) => this.SequenceCompare(other);

        private unsafe int CompareToCore(Latin1String other, nuint length)
        {
            fixed (byte* ptr = other.GetInternalRepresentation())
            {
                if (SequenceHelper.ContainsGreaterThan(ptr, length, AsciiEncodingHelper.AsciiEncodingLimit_InByte))
                    return CompareToCoreSlow(ptr, length);
                return CompareToCoreFast(ptr, length);
            }
        }

        private unsafe int CompareToCore(AsciiString other, nuint length)
        {
            fixed (byte* ptr = other.GetInternalRepresentation())
                return CompareToCoreFast(ptr, length);
        }

        private unsafe int CompareToCore(char* other, nuint length)
        {
            if (SequenceHelper.ContainsGreaterThan(other, length, AsciiEncodingHelper.AsciiEncodingLimit))
                return CompareToCoreSlow(other, length);
            return CompareToCoreFast(other, length);
        }

        private unsafe int CompareToCoreFast(char* other, nuint length)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(length);
            try
            {
                fixed (byte* temp = buffer)
                {
                    AsciiEncodingHelper.ReadFromUtf16BufferCore(other, temp, length);
                    return CompareToCoreFast(temp, length);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe int CompareToCoreFast(byte* other, nuint length)
        {
            fixed (byte* source = _value)
                return InternalSequenceHelper.CompareTo(source, other, length);
        }

        private unsafe int CompareToCoreSlow(char* other, nuint length)
            => this.SequenceCompare(other, length);

        private unsafe int CompareToCoreSlow(byte* other, nuint length)
        {
            int result = this.WithNativeIndex()
               .Select(item => item.Value.CompareTo(other[item.Index]))
               .FirstOrDefault(static val => val != 0);
            return MathHelper.Sign(result);
        }

        private unsafe bool EqualsCore(Utf16String other, nuint length)
        {
            fixed (char* ptr = other.GetInternalRepresentation())
                return EqualsCore(ptr, length);
        }

        private unsafe bool EqualsCore(Utf8String other, nuint length)
        {
            fixed (byte* ptr = other.GetInternalRepresentation())
                return EqualsCoreFast(ptr, length);
        }

        private unsafe bool EqualsCore(Latin1String other, nuint length)
        {
            fixed (byte* ptr = other.GetInternalRepresentation())
            {
                if (SequenceHelper.ContainsGreaterThan(ptr, length, AsciiEncodingHelper.AsciiEncodingLimit_InByte))
                    return EqualsCoreSlow(ptr, length);
                return EqualsCoreFast(ptr, length);
            }
        }

        private unsafe bool EqualsCore(AsciiString other, nuint length)
        {
            fixed (byte* ptr = other.GetInternalRepresentation())
                return EqualsCoreFast(ptr, length);
        }

        private unsafe bool EqualsCore(char* other, nuint length)
        {
            if (SequenceHelper.ContainsGreaterThan(other, length, AsciiEncodingHelper.AsciiEncodingLimit))
                return EqualsCoreSlow(other, length);
            return EqualsCoreFast(other, length);
        }

        private unsafe bool EqualsCoreFast(char* other, nuint length)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(length);
            try
            {
                fixed (byte* temp = buffer)
                {
                    AsciiEncodingHelper.ReadFromUtf16BufferCore(other, temp, length);
                    return EqualsCoreFast(temp, length);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool EqualsCoreFast(byte* other, nuint length)
        {
            fixed (byte* source = _value)
                return SequenceHelper.Equals(source, other, length);
        }

        private unsafe bool EqualsCoreSlow(char* other, nuint length)
            => this.SequenceEqual(other, length);

        private unsafe bool EqualsCoreSlow(byte* other, nuint length)
            => this.WithNativeIndex().All(item => item.Value == other[item.Index]);
    }
}
