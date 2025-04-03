using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace WitherTorch.Common.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct BitVector64 : IComparable<BitVector64>, IEquatable<BitVector64>
    {
        private ulong _data;

        public BitVector64(ulong data) => _data = data;

        public bool this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                if (index < 0 || index > 63)
                    throw new IndexOutOfRangeException();
                ulong mask = 1UL << index;
                return (_data & mask) == mask;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index < 0 || index > 63)
                    throw new IndexOutOfRangeException();
                ulong mask = 1UL << index;
                if (value)
                    _data |= mask;
                else
                    _data &= ~mask;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset() => _data = ulong.MinValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set() => _data = ulong.MaxValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong Exchange(ulong value)
        {
            ulong result = _data;
            _data = value;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ulong(BitVector64 bitVector) => bitVector._data;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BitVector64(ulong value) => new BitVector64 { _data = value };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BitVector64 a, BitVector64 b) => a._data == b._data;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BitVector64 a, BitVector64 b) => a._data != b._data;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitVector64 operator ~(BitVector64 a) => new BitVector64(~a._data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitVector64 operator &(BitVector64 a, BitVector64 b) => new BitVector64(a._data & b._data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitVector64 operator |(BitVector64 a, BitVector64 b) => new BitVector64(a._data | b._data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitVector64 operator ^(BitVector64 a, BitVector64 b) => new BitVector64(a._data ^ b._data);

        public readonly override bool Equals(object? obj) => obj is BitVector64 other && Equals(other);

        public readonly bool Equals(BitVector64 vector) => _data == vector._data;

        public readonly int CompareTo(BitVector64 other) => _data.CompareTo(other._data);

        public readonly override int GetHashCode() => _data.GetHashCode();

        public readonly override string ToString() => $"0x{_data:X}";
    }
}
