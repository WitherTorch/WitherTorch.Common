﻿using System;
using System.Collections.Generic;

using WitherTorch.Common.Buffers;

namespace WitherTorch.Common.Text
{
    internal sealed class EmptyString : StringBase, IWrapper<string>, IWrapper<byte[]>
    {
        private static readonly EmptyString _instance = new EmptyString();

        public static EmptyString Instance => _instance;

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

        protected override nuint GetSplitCount(char separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
        {
            sliceBuffer = null;
            return 1;
        }

        protected override nuint GetSplitCount(string separator, ArrayPool<StringSlice> pool, out StringSlice[]? sliceBuffer)
        {
            sliceBuffer = null;
            return 1;
        }

        public override IEnumerator<char> GetEnumerator() => string.Empty.GetEnumerator();

        public override int GetHashCode() => string.Empty.GetHashCode();

        public override int CompareToCore(string other) => other.Length == 0 ? 0 : -1;

        public override int CompareToCore(StringBase other) => other.Length == 0 ? 0 : -1;

        public override bool EqualsCore(string other) => other.Length == 0;

        public override bool EqualsCore(StringBase other) => other.Length == 0;

        protected override string ToStringCore() => string.Empty;

        public override string ToString() => string.Empty;

        byte[] IWrapper<byte[]>.Unwrap() => Array.Empty<byte>();

        string IWrapper<string>.Unwrap() => string.Empty;
    }
}
