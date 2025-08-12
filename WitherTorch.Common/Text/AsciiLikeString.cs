using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal abstract partial class AsciiLikeString : StringBase, IPinnableReference<byte>,
        IHolder<ArraySegment<byte>>
    {
        protected static readonly nuint MaxStringLength = unchecked((nuint)Limits.MaxArrayLength - 1);

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

        public override IEnumerator<char> GetEnumerator() => new CharEnumerator(this);

        protected override unsafe bool IsFullyWhitespaced()
        {
            nuint length = MathHelper.MakeUnsigned(_length);
            fixed (byte* source = _value) // 消除邊界檢查
            {
                for (nuint i = 0; i < length; i++)
                {
                    if (!IsWhiteSpace(source[i]))
                        return false;
                }
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
