using System.Collections.Generic;
using System.Text;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class OtherEncodingString : StringWrapper, IPinnableReference<byte>, IReadOnlyViewProvider<byte>
    {
        private readonly Encoding _encoding;
        private readonly byte[] _value;
        private readonly int _codePage, _length;

        public OtherEncodingString(Encoding encoding, byte[] value)
        {
            _encoding = encoding;
            _codePage = encoding.CodePage;
            _value = value;
            _length = encoding.GetCharCount(value, 0, value.Length - 1);
        }

        public override StringType StringType => StringType.Other;

        public override int Length => _length;

        public override IEnumerator<char> GetEnumerator() => new CharEnumerator(_encoding, _value);

        public override bool IsSpecificEncoding(Encoding encoding)
            => ReferenceEquals(encoding, _encoding) || encoding.CodePage == _codePage;

        ref readonly byte IPinnableReference<byte>.GetPinnableReference() => ref _value[0];

        nuint IPinnableReference<byte>.GetPinnedLength() => MathHelper.MakeUnsigned(_value.Length - 1);

        ReadOnlyView<byte> IReadOnlyViewProvider<byte>.CreateView()
        {
            byte[] value = _value;
            return new ReadOnlyView<byte>(value).Slice(0, value.Length - 1);
        }
    }
}
