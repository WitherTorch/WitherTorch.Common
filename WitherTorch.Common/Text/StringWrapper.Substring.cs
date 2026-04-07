using System;
using System.Collections.Generic;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class StringWrapper
    {
        public StringWrapper Substring(int startIndex)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex == 0)
                return this;

            int length = Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length == 0)
                return this;

            return unchecked(SubstringCore((nuint)startIndex, (nuint)(length - startIndex)));
        }

        public StringWrapper Substring(int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            int length = Length;
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (startIndex == 0 && count == length)
                return this;
            if (count == 0)
                return Empty;

            return unchecked(SubstringCore((nuint)startIndex, (nuint)count));
        }

        protected internal virtual unsafe StringWrapper SubstringCore(nuint startIndex, nuint count)
        {
            string result = StringHelper.AllocateRawString((int)MathHelper.MakeSigned(count));
            fixed (char* ptr = result)
                CopyToCore(ptr, startIndex, count);
            return result;
        }

        public StringWrapper Remove(int startIndex)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex == 0)
                return Empty;

            int length = Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            return RemoveCore((nuint)startIndex, (nuint)(length - startIndex));
        }

        public StringWrapper Remove(int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            int length = Length;
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (startIndex == 0 && count == length)
                return Empty;
            if (count == 0)
                return this;

            return RemoveCore((nuint)startIndex, (nuint)count);
        }

        protected virtual unsafe StringWrapper RemoveCore(nuint startIndex, nuint count)
        {
            string result = StringHelper.AllocateRawString((int)MathHelper.MakeSigned(count));
            using IEnumerator<char> enumerator = GetEnumerator();
            fixed (char* ptr = result)
            {
                char* iterator = ptr;
                for (nuint i = 0; i < startIndex && enumerator.MoveNext(); i++)
                    *iterator++ = enumerator.Current;
                for (nuint i = 0; i < count && enumerator.MoveNext(); i++) ;
                while (enumerator.MoveNext())
                    *iterator++ = enumerator.Current;
            }
            return CreateUtf16String(result);
        }
    }
}
