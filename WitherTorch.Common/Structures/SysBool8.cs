using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct SysBool8 : IEquatable<SysBool8>, IComparable<SysBool8>
    {
        public static readonly SysBool8 True = new SysBool8(Booleans.TrueInt);
        public static readonly SysBool8 False = new SysBool8(Booleans.FalseInt);

        private readonly byte _value;

        public readonly bool Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value != 0;
        }

        public readonly bool IsSuccessed => _value >= 0;

        public readonly bool IsFailed => _value < 0;

        private SysBool8(byte value) => _value = value;

        public SysBool8(bool value) : this(MathHelper.BooleanToUInt8(value)) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(SysBool8 boolean) => boolean.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SysBool8(bool boolean) => new SysBool8(boolean);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? obj) => obj is SysBool8 other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(SysBool8 other) => (_value == 0) != (other._value == 0);

        public readonly int CompareTo(SysBool8 other) => _value.CompareTo(other._value);

        public override int GetHashCode() => _value != 0 ? 1 : 0;

        public override readonly string ToString() => _value != 0 ? bool.TrueString : bool.FalseString;

        public static bool operator ==(SysBool8 a, SysBool8 b) => a.Equals(b);

        public static bool operator !=(SysBool8 a, SysBool8 b) => !a.Equals(b);
    }
}
