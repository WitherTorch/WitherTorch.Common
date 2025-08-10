using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    unsafe partial class Utf16String
    {
        protected override bool ContainsCore(char value, nuint startIndex, nuint count)
        {
            fixed (char* source = _value)
                return SequenceHelper.Contains(source + startIndex, count, value);
        }

        protected override bool ContainsCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = value)
                return ContainsCore(ptr, valueLength, startIndex, count);
        }

        protected override bool ContainsCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => value switch
            {
                Utf16String utf16 => ContainsCore(utf16, valueLength, startIndex, count),
                _ => ContainsCore_Other(value, valueLength, startIndex, count)
            };

        protected override int IndexOfCore(char value, nuint startIndex, nuint count)
        {
            fixed (char* source = _value)
                return InternalSequenceHelper.ConvertToIndex32(
                    SequenceHelper.PointerIndexOf(source + startIndex, count, value), source);
        }

        protected override int IndexOfCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = value)
                return IndexOfCore(ptr, valueLength, startIndex, count);
        }

        protected override int IndexOfCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => value switch
            {
                Utf16String utf16 => IndexOfCore(utf16, valueLength, startIndex, count),
                _ => IndexOfCore_Other(value, valueLength, startIndex, count)
            };

        private bool ContainsCore(Utf16String value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = value._value)
                return ContainsCore(ptr, valueLength, startIndex, count);
        }

        private bool ContainsCore(char* value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* source = _value)
                return InternalSequenceHelper.Contains(source + startIndex, count, value, valueLength);
        }

        private bool ContainsCore_Other(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (char* temp = buffer)
                {
                    value.CopyToCore(temp, 0u, valueLength);
                    return ContainsCore(temp, valueLength, startIndex, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private int IndexOfCore(Utf16String value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = value._value)
                return IndexOfCore(ptr, valueLength, startIndex, count);
        }

        private int IndexOfCore(char* value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* source = _value)
                return InternalSequenceHelper.ConvertToIndex32(
                    InternalSequenceHelper.PointerIndexOf(source + startIndex, count, value, valueLength), source);
        }

        private int IndexOfCore_Other(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (char* temp = buffer)
                {
                    value.CopyToCore(temp, 0u, valueLength);
                    return IndexOfCore(temp, valueLength, startIndex, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
