﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class Utf16String : StringBase, IWrapper<string>
    {
        private readonly string _value;

        public override StringType StringType => StringType.Utf16;
        public override int Length => _value.Length;

        private Utf16String(string value)
        {
            _value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Utf16String Allocate(nuint length, out string buffer)
        {
            if (length > int.MaxValue)
                throw new OutOfMemoryException();

            return new Utf16String(buffer = StringHelper.AllocateRawString(unchecked((int)length)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe new Utf16String Create(string source) => new Utf16String(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe new Utf16String Create(char* source) => new Utf16String(new string(source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Utf16String Create(char* source, nuint length)
        {
            if (length > int.MaxValue)
                throw new OutOfMemoryException();

            return new Utf16String(new string(source, 0, unchecked((int)length)));
        }

        protected internal override unsafe char GetCharAt(nuint index)
        {
            fixed (char* ptr = _value)
                return ptr[index];
        }

        protected override bool IsFullyWhitespaced()
        {
            foreach (char character in _value)
            {
                if (!char.IsWhiteSpace(character))
                    return false;
            }
            return true;
        }

        protected internal override unsafe void CopyToCore(char* destination, nuint startIndex, nuint count)
        {
            fixed (char* ptr = _value)
                UnsafeHelper.CopyBlockUnaligned(destination, ptr, unchecked((uint)count * sizeof(char)));
        }

        public override IEnumerator<char> GetEnumerator() => _value.GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal string GetInternalRepresentation() => _value;

        public override int GetHashCode() => _value.GetHashCode();

        protected override string ToStringCore() => _value;

        public override string ToString() => _value;

        string IWrapper<string>.Unwrap() => _value;
    }
}
