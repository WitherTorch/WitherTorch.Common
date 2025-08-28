using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Windows.Structures
{
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public readonly struct SysBool : IEquatable<SysBool>, IComparable<SysBool>
    {
        public static readonly SysBool True = new SysBool(Booleans.TrueInt);
        public static readonly SysBool False = new SysBool(Booleans.FalseInt);

        private readonly int _value;

        public readonly bool Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value != 0;
        }

        public readonly bool IsSuccessed => _value >= 0;

        public readonly bool IsFailed => _value < 0;

        private SysBool(int value) => _value = value;

        public SysBool(bool value) : this(MathHelper.BooleanToInt32(value)) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator bool(SysBool boolean) => boolean.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SysBool(bool boolean) => new SysBool(boolean);

        public override bool Equals(object? obj) => obj is SysBool other && Equals(other);

        public bool Equals(SysBool other) => _value == other._value;

        public int CompareTo(SysBool other) => _value.CompareTo(other._value);

        public override int GetHashCode() => _value.GetHashCode();

        public override readonly string ToString() => _value == 0x1 ? bool.TrueString : bool.FalseString;

        public static bool operator ==(SysBool a, SysBool b) => a.Equals(b);

        public static bool operator !=(SysBool a, SysBool b) => !a.Equals(b);
    }
}
