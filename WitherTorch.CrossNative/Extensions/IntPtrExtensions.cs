using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using InlineMethod;

namespace WitherTorch.CrossNative.Extensions
{
    public static class IntPtrExtensions
    {
        [StructLayout(LayoutKind.Explicit, Size = 4)]
        public struct Words
        {
            [FieldOffset(0)]
            public ushort highWord;

            [FieldOffset(2)]
            public ushort lowWord;

            [FieldOffset(0)]
            public short highWordSigned;

            [FieldOffset(2)]
            public short lowWordSigned;

            [FieldOffset(0)]
            public uint rawValue;

            public static implicit operator IntPtr(Words words) => new IntPtr(words.rawValue);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Words GetWords(this IntPtr _this)
        {
            return new Words() { rawValue = unchecked(((UIntPtr)_this.ToPointer()).ToUInt32()) };
        }

        public static nint One => 1;
        public static nint Zero => default;
    }
}
