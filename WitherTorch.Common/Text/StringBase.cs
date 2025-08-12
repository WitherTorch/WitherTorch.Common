using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Extensions;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    public abstract partial class StringBase : IEnumerable<char>, ICloneable,
        IComparable<string>, IComparable<StringBase>,
        IEquatable<string>, IEquatable<StringBase>
    {
        public static readonly StringBase Empty = EmptyString.Instance;

        public abstract StringType StringType { get; }

        public abstract int Length { get; }

        public char this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index < 0 || index >= Length)
                    throw new IndexOutOfRangeException();
                return GetCharAt(unchecked((nuint)index));
            }
        }

        protected internal virtual char GetCharAt(nuint index) => this.Skip(index).First();

        protected virtual bool IsFullyWhitespaced() => this.All(Utf16StringHelper.IsWhiteSpaceCharacter);

        public unsafe void CopyTo(char[] destination)
        {
            int length = MathHelper.Min(Length, destination.Length);
            if (length <= 0)
                return;
            fixed (char* ptr = destination)
                CopyToCore(ptr, 0, unchecked((nuint)length));
        }

        public unsafe void CopyTo(char[] destination, int sourceStartIndex, int destStartIndex, int count)
        {
            int length = Length;
            if (sourceStartIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(sourceStartIndex));
            if (destStartIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(destStartIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (sourceStartIndex + count > length)
                throw new ArgumentOutOfRangeException(sourceStartIndex >= length ? nameof(sourceStartIndex) : nameof(count));
            if (destStartIndex + count > destination.Length)
                throw new ArgumentOutOfRangeException(destStartIndex >= destination.Length ? nameof(destStartIndex) : nameof(count));
            if (count == 0)
                return;
            fixed (char* ptr = destination)
                CopyToCore(ptr + destStartIndex, unchecked((nuint)sourceStartIndex), unchecked((nuint)count));
        }

        public unsafe void CopyTo(char* destination)
        {
            int length = Length;
            if (length <= 0)
                return;
            CopyToCore(destination, 0, unchecked((nuint)length));
        }

        public unsafe void CopyTo(char* destination, int startIndex, int count)
        {
            int length = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex + count > length)
                throw new ArgumentOutOfRangeException(startIndex >= length ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return;
            CopyToCore(destination, unchecked((nuint)startIndex), unchecked((nuint)count));
        }

        public unsafe void CopyTo(char* destination, nuint startIndex, nuint count)
        {
            int length = Length;
            if (length <= 0)
                return;
            nuint castedLength = unchecked((nuint)length);
            if (startIndex + count > castedLength)
                throw new ArgumentOutOfRangeException(startIndex >= castedLength ? nameof(startIndex) : nameof(count));
            if (count == 0)
                return;
            CopyToCore(destination, startIndex, count);
        }

        protected internal virtual unsafe void CopyToCore(char* destination, nuint startIndex, nuint count)
        {
            using IEnumerator<char> enumerator = this.Skip(startIndex).GetEnumerator();
            for (nuint i = 0; i < count; i++)
            {
                if (!enumerator.MoveNext())
                    return;
                *destination = enumerator.Current;
            }
        }

        public abstract IEnumerator<char> GetEnumerator();

        public StringBase Clone() => this;

        public override int GetHashCode() => ToString().GetHashCode();

        public unsafe virtual char[] ToCharArray()
        {
            int length = Length;
            if (length <= 0)
                return Array.Empty<char>();
            char[] result = new char[length];
            fixed (char* ptr = result)
                CopyToCore(ptr, 0, unchecked((nuint)length));
            return result;
        }

        public override unsafe string ToString()
        {
            int length = Length;
            if (length <= 0)
                return string.Empty;
            string result = StringHelper.AllocateRawString(length);
            fixed (char* ptr = result)
                CopyToCore(ptr, 0, unchecked((nuint)length));
            return result;
        }

        #region Interface Implementations
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        object ICloneable.Clone() => this;
        #endregion
    }
}
