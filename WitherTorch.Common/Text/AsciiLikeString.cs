using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal abstract partial class AsciiLikeString : StringBase, IPinnableReference<byte>,
        IHolder<ArraySegment<byte>>
    {
        protected readonly byte[] _value;
        protected readonly int _length;

        public override int Length => _length;

        protected AsciiLikeString(byte[] value)
        {
            _value = value;
            _length = value.Length - 1;
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
                Latin1EncodingHelper.WriteToUtf16BufferCore(ptr + startIndex, destination, count);
        }

        protected abstract byte GetCharacterLimit();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal byte[] GetInternalRepresentation() => _value;

        ArraySegment<byte> IHolder<ArraySegment<byte>>.Value => new ArraySegment<byte>(_value, 0, _length);

        ref readonly byte IPinnableReference<byte>.GetPinnableReference() => ref _value[0];

        nuint IPinnableReference<byte>.GetPinnedLength() => MathHelper.MakeUnsigned(_length);
    }
}
