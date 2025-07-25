﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class Latin1String : StringBase, IWrapper<byte[]>
    {
        private const int MaxLatin1StringLength = Limits.MaxArrayLength - 1;

        private readonly byte[] _value;
        private readonly int _length;

        public override StringType StringType => StringType.Latin1;
        public override int Length => _length;

        private Latin1String(byte[] value)
        {
            _value = value;
            _length = value.Length - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Latin1String Allocate(nuint length, out byte[] buffer)
        {
            if (length > MaxLatin1StringLength)
                throw new OutOfMemoryException();

            return new Latin1String(buffer = new byte[length + 1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Latin1String Create(byte* source)
        {
            nuint length = 0;
            do
            {
                byte c = source[length];
                if (c == 0)
                    break;
                if (++length > MaxLatin1StringLength)
                    throw new OutOfMemoryException();
            } while (true);

            byte[] buffer = new byte[length]; // Tail zero is included
            fixed (byte* ptr = buffer)
                UnsafeHelper.CopyBlockUnaligned(ptr, source, unchecked((uint)length * sizeof(byte)));
            return new Latin1String(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Latin1String Create(byte* source, nuint length)
        {
            if (length >= MaxLatin1StringLength)
                throw new OutOfMemoryException();

            byte[] buffer = new byte[length + 1];
            fixed (byte* ptr = buffer)
                UnsafeHelper.CopyBlockUnaligned(ptr, source, unchecked((uint)length * sizeof(byte)));
            return new Latin1String(buffer);
        }

        public static unsafe bool TryCreate(char* source, nuint length, [NotNullWhen(true)] out Latin1String? result)
        {
            if (SequenceHelper.ContainsGreaterThan(source, length, Latin1StringHelper.Latin1StringLimit) || length > MaxLatin1StringLength)
                goto Failed;

            byte[] buffer = new byte[length + 1];
            fixed (byte* dest = buffer)
                Latin1StringHelper.NarrowAndCopyTo(source, length, dest);
            result = new Latin1String(buffer);
            return true;

        Failed:
            result = null;
            return false;
        }

        protected internal override unsafe char GetCharAt(nuint index)
        {
            fixed (byte* ptr = _value)
                return unchecked((char)ptr[index]);
        }

        protected override bool IsFullyWhitespaced()
        {
            byte[] value = _value;
            for (int i = 0, length = _length; i < length; i++)
            {
                if (!IsWhiteSpace(value[i]))
                    return false;
            }
            return true;
        }

        [Inline(InlineBehavior.Remove)]
        private static bool IsWhiteSpace(byte value) => value switch
        {
            (byte)'\t' or (byte)'\n' or (byte)'\v' or (byte)'\f' or (byte)'\r' or (byte)' ' or
            (byte)'\u00a0' or (byte)'\u0085' => true,
            _ => false,
        };

        protected internal override unsafe void CopyToCore(char* destination, nuint startIndex, nuint count)
        {
            fixed (byte* ptr = _value)
                Latin1StringHelper.WidenAndCopyTo(ptr + startIndex, count, destination);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal byte[] GetInternalRepresentation() => _value;

        protected override unsafe string ToStringCore()
        {
            int length = _length;
            string result = StringHelper.AllocateRawString(length);
            fixed (byte* source = _value)
            fixed (char* dest = result)
                Latin1StringHelper.WidenAndCopyTo(source, unchecked((nuint)length), dest);
            return result;
        }

        byte[] IWrapper<byte[]>.Unwrap() => _value;
    }
}
