using System;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class StringBase
    {

        public bool Contains(char value)
        {
            int length = Length;
            if (length <= 0)
                return false;
            return ContainsCore(value, 0, unchecked((nuint)length));
        }

        public bool Contains(char value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return false;
            return unchecked(ContainsCore(value, (nuint)startIndex, (nuint)length));
        }

        public bool Contains(string value)
        {
            int length = Length;
            if (length <= 0)
                return false;
            int valueLength = value.Length;
            if (valueLength <= 0)
                return true;
            if (valueLength > length)
                return false;
            return unchecked(ContainsCore(value, (nuint)valueLength, 0, (nuint)length));
        }

        public bool Contains(string value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return false;
            length = value.Length;
            if (length <= 0)
                return true;
            if (length > count)
                return false;
            return unchecked(ContainsCore(value, (nuint)length, 0, (nuint)count));
        }

        public bool Contains(StringBase value)
        {
            int length = Length;
            if (length <= 0)
                return false;
            int valueLength = value.Length;
            if (valueLength <= 0)
                return true;
            if (valueLength > length)
                return false;
            return unchecked(ContainsCore(value, (nuint)valueLength, 0, (nuint)length));
        }

        public bool Contains(StringBase value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return false;
            length = value.Length;
            if (length <= 0)
                return true;
            if (length > count)
                return false;
            return unchecked(ContainsCore(value, (nuint)length, 0, (nuint)count));
        }

        public int IndexOf(char value)
        {
            int length = Length;
            if (length <= 0)
                return -1;
            return IndexOfCore(value, 0, unchecked((nuint)length));
        }

        public int IndexOf(char value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return -1;
            return unchecked(IndexOfCore(value, (nuint)startIndex, (nuint)count));
        }

        public int IndexOf(string value)
        {
            int length = Length;
            if (length <= 0)
                return -1;
            int valueLength = value.Length;
            if (valueLength <= 0)
                return 0;
            if (valueLength > length)
                return -1;
            return unchecked(IndexOfCore(value, (nuint)valueLength, 0, (nuint)length));
        }

        public int IndexOf(string value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return -1;
            length = value.Length;
            if (length <= 0)
                return 0;
            if (length > count)
                return -1;
            return unchecked(IndexOfCore(value, (nuint)length, 0, (nuint)count));
        }

        public int IndexOf(StringBase value)
        {
            int length = Length;
            if (length <= 0)
                return -1;
            int valueLength = value.Length;
            if (valueLength <= 0)
                return 0;
            if (valueLength > length)
                return -1;
            return unchecked(IndexOfCore(value, (nuint)valueLength, 0, (nuint)length));
        }

        public int IndexOf(StringBase value, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return -1;
            length = value.Length;
            if (length <= 0)
                return 0;
            if (length > count)
                return -1;
            return unchecked(IndexOfCore(value, (nuint)length, 0, (nuint)count));
        }

        protected virtual bool ContainsCore(char value, nuint startIndex, nuint count)
            => ContainsCore_Fallback(value, startIndex, count);

        protected virtual unsafe bool ContainsCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (valueLength == 1)
                return ContainsCore(value[0], startIndex, count);

            fixed (char* ptr = value)
                return ContainsCore_Fallback(ptr, valueLength, startIndex, count);
        }

        protected virtual bool ContainsCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (valueLength == 1)
                return ContainsCore(value.GetCharAt(0), startIndex, count);

            return ContainsCore_Fallback(value, valueLength, startIndex, count);
        }

        protected virtual int IndexOfCore(char value, nuint startIndex, nuint count)
            => IndexOfCore_Fallback(value, startIndex, count);

        protected virtual unsafe int IndexOfCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (valueLength == 1)
                return IndexOfCore(value[0], startIndex, count);

            fixed (char* ptr = value)
                return IndexOfCore_Fallback(ptr, valueLength, startIndex, count);
        }

        protected virtual int IndexOfCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (valueLength == 1)
                return IndexOfCore(value.GetCharAt(0), startIndex, count);

            return IndexOfCore_Fallback(value, valueLength, startIndex, count);
        }

        private unsafe bool ContainsCore_Fallback(char value, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                fixed (char* temp = buffer)
                {
                    CopyToCore(temp, startIndex, count);
                    return SequenceHelper.Contains(temp, count, value);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool ContainsCore_Fallback(char* value, nuint valueLength, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                fixed (char* temp = buffer)
                {
                    CopyToCore(temp, startIndex, count);
                    return InternalSequenceHelper.Contains(temp, count, value, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool ContainsCore_Fallback(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                fixed (char* temp = buffer)
                {
                    value.CopyToCore(temp, 0, valueLength);
                    return ContainsCore_Fallback(temp, valueLength, startIndex, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe int IndexOfCore_Fallback(char value, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                fixed (char* temp = buffer)
                {
                    CopyToCore(temp, startIndex, count);
                    char* result = SequenceHelper.PointerIndexOf(temp, count, value);
                    return result < temp ? -1 : unchecked((int)(result - temp)) + unchecked((int)MathHelper.MakeSigned(startIndex));
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe int IndexOfCore_Fallback(char* value, nuint valueLength, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                fixed (char* temp = buffer)
                {
                    CopyToCore(temp, startIndex, count);
                    char* result = InternalSequenceHelper.PointerIndexOf(temp, count, value, valueLength);
                    return result < temp ? -1 : unchecked((int)(result - temp)) + unchecked((int)MathHelper.MakeSigned(startIndex));
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe int IndexOfCore_Fallback(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(count);
            try
            {
                fixed (char* temp = buffer)
                {
                    value.CopyToCore(temp, 0, valueLength);
                    return IndexOfCore_Fallback(temp, valueLength, startIndex, count);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
