using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using InlineMethod;

namespace WitherTorch.Common.Extensions
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

            public static implicit operator nint(Words words) => new nint(words.rawValue);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Words GetWords(this nint _this)
        {
            return new Words() { rawValue = unchecked(((nuint)_this.ToPointer()).ToUInt32()) };
        }

        public static nint One => 1;
        public static nint Zero => default;
    }
}
