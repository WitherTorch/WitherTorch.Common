#if NET472_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class CLRStringHelper
    {
        private static readonly nuint StringAdjustment = MeasureStringAdjustment();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial ref readonly char GetPinnableReference(string source)
        {
            PinnableString pinnable = UnsafeHelper.As<PinnableString>(source);
            return ref UnsafeHelper.AddByteOffset(ref pinnable.Data, StringAdjustment);
        }

        private unsafe static nuint MeasureStringAdjustment()
        {
            string text = "a";
            fixed (char* source = text)
                return UnsafeHelper.ByteOffsetUnsigned(ref UnsafeHelper.As<PinnableString>(text).Data, ref UnsafeHelper.AsRef(source));
        }

        [StructLayout(LayoutKind.Sequential)]
        private sealed class PinnableString
        {
            public char Data;
        }
    }
}
#endif