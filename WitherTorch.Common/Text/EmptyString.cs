using System;
using System.Collections.Generic;
using System.Linq;

using InlineMethod;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class EmptyString : StringBase, IPinnableReference<char>, IPinnableReference<byte>, 
        IHolder<byte[]>, IHolder<char[]>,
        IHolder<ArraySegment<byte>>, IHolder<ArraySegment<char>>
    {
        private static readonly EmptyString _instance = new EmptyString();

        public static EmptyString Instance => _instance;

        private readonly char _zeroChar = '\0';

        public override StringType StringType => StringType.Utf16;
        public override int Length => 0;

        private EmptyString() { }

        protected internal override char GetCharAt(nuint index) => throw new IndexOutOfRangeException();

        protected override bool IsFullyWhitespaced() => true;

        protected internal override unsafe void CopyToCore(char* destination, nuint startIndex, nuint count) { }

        protected override bool PartiallyEqualsCore(string other, nuint startIndex, nuint count) => false;

        protected override bool PartiallyEqualsCore(StringBase other, nuint startIndex, nuint count) => false;

        protected override int IndexOfCore(char value, nuint startIndex, nuint count) => -1;

        protected override int IndexOfCore(string value, nuint valueLength, nuint startIndex, nuint count) => -1;

        protected override int IndexOfCore(StringBase value, nuint valueLength, nuint startIndex, nuint count) => -1;

        protected override bool ContainsCore(char value, nuint startIndex, nuint count) => false;

        protected override bool ContainsCore(string value, nuint valueLength, nuint startIndex, nuint count) => false;

        protected override bool ContainsCore(StringBase value, nuint valueLength, nuint startIndex, nuint count) => false;

        protected internal override StringBase SubstringCore(nuint startIndex, nuint count) => this;

        protected override StringBase RemoveCore(nuint startIndex, nuint count) => this;

        protected override nuint GetSplitCount(char separator, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            rangeBuffer = null;
            return 1;
        }

        protected override nuint GetSplitCount(string separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            rangeBuffer = null;
            return 1;
        }

        protected override nuint GetSplitCount(StringBase separator, nuint separatorLength, ArrayPool<SplitRange> pool, out SplitRange[]? rangeBuffer)
        {
            rangeBuffer = null;
            return 1;
        }

        public override IEnumerator<char> GetEnumerator() => Enumerable.Empty<char>().GetEnumerator();

        public override int GetHashCode() => string.Empty.GetHashCode();

        protected override int CompareToCore(string other, nuint length) => 0;

        protected override int CompareToCore(StringBase other, nuint length) => 0;

        protected override bool EqualsCore(string other, nuint length) => true;

        protected override bool EqualsCore(StringBase other, nuint length) => true;

        public override char[] ToCharArray() => Array.Empty<char>();

        public override string ToString() => string.Empty;

        ref readonly char IPinnableReference<char>.GetPinnableReference() => ref _zeroChar;

        nuint IPinnableReference<char>.GetPinnedLength() => 0;

        ref readonly byte IPinnableReference<byte>.GetPinnableReference() => ref UnsafeHelper.As<char, byte>(ref UnsafeHelper.AsRefIn(in _zeroChar));

        nuint IPinnableReference<byte>.GetPinnedLength() => 0;

        byte[] IHolder<byte[]>.Value => Array.Empty<byte>();

        char[] IHolder<char[]>.Value => Array.Empty<char>();

        ArraySegment<byte> IHolder<ArraySegment<byte>>.Value => GetEmptySegment<byte>();

        ArraySegment<char> IHolder<ArraySegment<char>>.Value => GetEmptySegment<char>();

        [Inline(InlineBehavior.Remove)]
        private static ArraySegment<T> GetEmptySegment<T>()
        {
#if NET8_0_OR_GREATER
            return ArraySegment<T>.Empty;
#else
            return new ArraySegment<T>(Array.Empty<T>(), 0, 0);
#endif
        }
    }
}
