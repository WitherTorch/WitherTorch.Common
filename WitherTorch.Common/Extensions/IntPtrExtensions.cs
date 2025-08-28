using System.Runtime.InteropServices;

using InlineMethod;

namespace WitherTorch.Common.Extensions
{
    public static class IntPtrExtensions
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe Words GetWords(this nint This) => new Words() { RawValueSigned = (int)This };

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe Words GetWords(this nuint This) => new Words() { RawValue = (uint)This };
    }

    [StructLayout(LayoutKind.Explicit, Size = sizeof(uint))]
    public struct Words
    {
        [FieldOffset(0)]
        public uint RawValue;

        [FieldOffset(0)]
        public int RawValueSigned;

        [FieldOffset(0)]
        public ushort LowWord;

        [FieldOffset(0)]
        public short LowWordSigned;

        [FieldOffset(sizeof(ushort))]
        public ushort HighWord;

        [FieldOffset(sizeof(ushort))]
        public short HighWordSigned;

        public static implicit operator nint(Words words) => words.RawValueSigned;

        public static implicit operator nuint(Words words) => words.RawValue;

        public readonly void Deconstruct(out ushort lowWord, out ushort highWord)
        {
            lowWord = LowWord;
            highWord = HighWord;
        }

        public readonly void DeconstructSigned(out short lowWord, out short highWord)
        {
            lowWord = LowWordSigned;
            highWord = HighWordSigned;
        }
    }

}
