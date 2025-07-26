using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    unsafe partial class Latin1String
    {
        protected override int IndexOfCore(char value, nuint startIndex, nuint count)
        {
            if (value > Latin1StringHelper.Latin1StringLimit)
                return -1;

            fixed (byte* ptr = _value)
            {
                byte* result = SequenceHelper.PointerIndexOf(ptr + startIndex, count, unchecked((byte)value));
                if (result == null)
                    return -1;
                return unchecked((int)(result - ptr));
            }
        }

        protected override int IndexOfCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (byte* ptr = _value)
            fixed (char* ptr2 = value)
            {
                byte* result = Latin1StringHelper.PointerIndexOf(ptr + startIndex, count, ptr2, valueLength);
                if (result == null)
                    return -1;
                return unchecked((int)(result - ptr));
            }
        }

        protected override int IndexOfCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => value switch
            {
                Latin1String latin1 => IndexOfCore(latin1, valueLength, startIndex, count),
                Utf16String utf16 => IndexOfCore(utf16.GetInternalRepresentation(), valueLength, startIndex, count),
                _ => base.IndexOfCore(value, valueLength, startIndex, count),
            };

        protected override bool ContainsCore(char value, nuint startIndex, nuint count)
        {
            if (value > Latin1StringHelper.Latin1StringLimit)
                return false;

            fixed (byte* ptr = _value)
                return SequenceHelper.Contains(ptr + startIndex, count, unchecked((byte)value));
        }

        protected override bool ContainsCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (byte* ptr = _value)
            fixed (char* ptr2 = value)
            {
                byte* result = Latin1StringHelper.PointerIndexOf(ptr + startIndex, count, ptr2, valueLength);
                return result != null;
            }
        }

        protected override bool ContainsCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => value switch
            {
                Latin1String latin1 => ContainsCore(latin1, valueLength, startIndex, count),
                Utf16String utf16 => ContainsCore(utf16.GetInternalRepresentation(), valueLength, startIndex, count),
                _ => base.ContainsCore(value, valueLength, startIndex, count),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int IndexOfCore(Latin1String value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (byte* ptr = _value, ptr2 = value._value)
            {
                byte* result = InternalSequenceHelper.PointerIndexOf(ptr + startIndex, count, ptr2, valueLength);
                if (result == null)
                    return -1;
                return unchecked((int)(result - ptr));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ContainsCore(Latin1String value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (byte* ptr = _value, ptr2 = value._value)
                return InternalSequenceHelper.PointerIndexOf(ptr + startIndex, count, ptr2, valueLength) != null;
        }
    }
}
