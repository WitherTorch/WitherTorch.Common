using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf16String
    {
        protected override unsafe bool PartiallyEqualsCore(string other, nuint startIndex, nuint count)
        {
            fixed (char* ptr = other)
                return PartiallyEqualsCore(ptr, startIndex, count);
        }

        protected override bool PartiallyEqualsCore(StringBase other, nuint startIndex, nuint count)
            => other switch
            {
                Utf16String utf16 => PartiallyEqualsCore(utf16, startIndex, count),
                _ => PartiallyEqualsCore_Other(other, startIndex, count),
            };

        protected override unsafe int CompareToCore(string other, nuint length)
        {
            fixed (char* ptr = other)
                return CompareToCore(ptr, length);
        }

        protected override int CompareToCore(StringBase other, nuint length)
            => other switch
            {
                Utf16String utf16 => CompareToCore(utf16, length),
                _ => CompareToCore_Other(other, length),
            };

        protected override unsafe bool EqualsCore(string other, nuint length)
        {
            fixed (char* ptr = other)
                return EqualsCore(ptr, length);
        }

        protected override bool EqualsCore(StringBase other, nuint length)
            => other switch
            {
                Utf16String utf16 => EqualsCore(utf16, length),
                _ => EqualsCore_Other(other, length),
            };

        private unsafe bool PartiallyEqualsCore(Utf16String other, nuint startIndex, nuint count)
        {
            fixed (char* ptr = other._value)
                return PartiallyEqualsCore(ptr, startIndex, count);
        }

        private unsafe bool PartiallyEqualsCore(char* other, nuint startIndex, nuint count)
        {
            fixed (char* source = _value)
                return SequenceHelper.Equals(source + startIndex, other, count);
        }

        private unsafe bool PartiallyEqualsCore_Other(StringBase other, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                fixed (char* temp = buffer)
                {
                    other.CopyToCore(temp, 0, count);
                    return PartiallyEqualsCore(temp, startIndex, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe int CompareToCore(Utf16String other, nuint length)
        {
            fixed (char* ptr = other._value)
                return CompareToCore(ptr, length);
        }

        private unsafe int CompareToCore(char* other, nuint length)
        {
            fixed (char* source = _value)
                return InternalSequenceHelper.CompareTo(source, other, length);
        }

        private unsafe int CompareToCore_Other(StringBase other, nuint length)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(length);
            try
            {
                fixed (char* temp = buffer)
                {
                    other.CopyToCore(temp, 0, length);
                    return CompareToCore(temp, length);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool EqualsCore(Utf16String other, nuint length)
        {
            fixed (char* ptr = other._value)
                return EqualsCore(ptr, length);
        }

        private unsafe bool EqualsCore(char* other, nuint length)
        {
            fixed (char* source = _value)
                return SequenceHelper.Equals(source, other, length);
        }

        private unsafe bool EqualsCore_Other(StringBase other, nuint length)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(length);
            try
            {
                fixed (char* temp = buffer)
                {
                    other.CopyToCore(temp, 0, length);
                    return EqualsCore(temp, length);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
