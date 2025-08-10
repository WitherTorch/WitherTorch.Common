using System.Linq;
using System.Runtime.CompilerServices;

using LocalsInit;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Extensions;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf8String
    {
        protected override unsafe bool ContainsCore(char value, nuint startIndex, nuint count)
        {
            if (startIndex > 0 || count < unchecked((nuint)_length) || value > AsciiEncodingHelper.AsciiEncodingLimit) // Slow route
            {
                return this.SkipAndTake(startIndex, count)
                    .WhereEqualsTo(value)
                    .Any();
            }

            byte[] source = _value;
            fixed (byte* ptr = source)
                return SequenceHelper.Contains(ptr, MathHelper.MakeUnsigned(source.Length - 1), unchecked((byte)value));
        }

        protected override unsafe int IndexOfCore(char value, nuint startIndex, nuint count)
            => this.WithIndex()
            .SkipAndTake(startIndex, count)
            .WhereEqualsTo(value)
            .Select(static item => item.Index)
            .DefaultIfEmpty(-1)
            .First();
    }
}
