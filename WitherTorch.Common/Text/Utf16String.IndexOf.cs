using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    unsafe partial class Utf16String
    {
        protected override int IndexOfCore(char value, nuint startIndex, nuint count)
        {
            fixed (char* ptr = _value)
            {
                char* result = SequenceHelper.PointerIndexOf(ptr + startIndex, count, value);
                if (result == null)
                    return -1;
                return unchecked((int)(result - ptr));
            }
        }

        protected override int IndexOfCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = _value, ptr2 = value)
            {
                char* result = InternalSequenceHelper.PointerIndexOf(ptr + startIndex, count, ptr2, valueLength);
                if (result == null)
                    return -1;
                return unchecked((int)(result - ptr));
            }
        }

        protected override int IndexOfCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => value switch
            {
                Latin1String latin1 => IndexOfCore(latin1, valueLength, startIndex, count),
                Utf16String utf16 => IndexOfCore(utf16._value, valueLength, startIndex, count),
                _ => base.IndexOfCore(value, valueLength, startIndex, count),
            };

        protected override bool ContainsCore(char value, nuint startIndex, nuint count)
        {
            fixed (char* ptr = _value)
                return SequenceHelper.Contains(ptr + startIndex, count, value);
        }

        protected override bool ContainsCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = _value, ptr2 = value)
            {
                char* result = InternalSequenceHelper.PointerIndexOf(ptr + startIndex, count, ptr2, valueLength);
                return result != null;
            }
        }

        protected override bool ContainsCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => value switch
            {
                Latin1String latin1 => ContainsCore(latin1, valueLength, startIndex, count),
                Utf16String utf16 => ContainsCore(utf16._value, valueLength, startIndex, count),
                _ => base.ContainsCore(value, valueLength, startIndex, count),
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int IndexOfCore(Latin1String value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = _value)
            fixed (byte* ptr2 = value.GetInternalRepresentation())
            {
                char* result = Latin1StringHelper.PointerIndexOf(ptr + startIndex, count, ptr2, valueLength);
                if (result == null)
                    return -1;
                return unchecked((int)(result - ptr));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ContainsCore(Latin1String value, nuint valueLength, nuint startIndex, nuint count)
        {
            fixed (char* ptr = _value)
            fixed (byte* ptr2 = value.GetInternalRepresentation())
                return Latin1StringHelper.PointerIndexOf(ptr + startIndex, count, ptr2, valueLength) != null;
        }
    }
}
