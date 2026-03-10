using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public readonly struct SysBool32 : IEquatable<SysBool32>, IComparable<SysBool32>
    {
        public static readonly SysBool32 True = new SysBool32(Booleans.TrueInt);
        public static readonly SysBool32 False = new SysBool32(Booleans.FalseInt);

        private readonly int _value;

        public readonly bool Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value != 0;
        }

        public readonly bool IsSuccessed => _value >= 0;

        public readonly bool IsFailed => _value < 0;

        private SysBool32(int value) => _value = value;

        public SysBool32(bool value) : this(MathHelper.BooleanToInt32(value)) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(SysBool32 boolean) => boolean.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SysBool32(bool boolean) => new SysBool32(boolean);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? obj) => obj is SysBool32 other && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(SysBool32 other) => (_value == 0) != (other._value == 0);

        public readonly int CompareTo(SysBool32 other) => _value.CompareTo(other._value);

        public override int GetHashCode() => _value != 0 ? 1 : 0;

        public override readonly string ToString() => _value != 0 ? bool.TrueString : bool.FalseString;

        public static bool operator ==(SysBool32 a, SysBool32 b) => a.Equals(b);

        public static bool operator !=(SysBool32 a, SysBool32 b) => !a.Equals(b);
    }
}
