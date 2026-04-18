using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WitherTorch.Common.Structures
{
    [StructLayout(LayoutKind.Auto)] // 不能通過 unmanaged 程式碼傳出
    public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable, ICloneable, IConvertible
    {
        private static readonly object _boxedValue = default(Unit);
        public static readonly Unit Default = default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Unit _) => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj) => obj is Unit;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CompareTo(Unit _) => 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CompareTo(object? obj) => 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        object ICloneable.Clone() => this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => string.Empty;

        TypeCode IConvertible.GetTypeCode() => TypeCode.Empty;

        sbyte IConvertible.ToSByte(IFormatProvider? provider) => default;

        bool IConvertible.ToBoolean(IFormatProvider? provider) => default;

        byte IConvertible.ToByte(IFormatProvider? provider) => default;

        char IConvertible.ToChar(IFormatProvider? provider) => default;

        short IConvertible.ToInt16(IFormatProvider? provider) => default;

        ushort IConvertible.ToUInt16(IFormatProvider? provider) => default;

        int IConvertible.ToInt32(IFormatProvider? provider) => default;

        uint IConvertible.ToUInt32(IFormatProvider? provider) => default;

        long IConvertible.ToInt64(IFormatProvider? provider) => default;

        ulong IConvertible.ToUInt64(IFormatProvider? provider) => default;

        float IConvertible.ToSingle(IFormatProvider? provider) => default;

        string IConvertible.ToString(IFormatProvider? provider) => string.Empty;

        double IConvertible.ToDouble(IFormatProvider? provider) => default;

        decimal IConvertible.ToDecimal(IFormatProvider? provider) => default;

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) => default;

        object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
        {
            if (conversionType == typeof(Unit))
                return _boxedValue;
            if (conversionType == typeof(string))
                return string.Empty;
            if (conversionType == typeof(bool))
                return default(bool);
            if (conversionType == typeof(byte))
                return default(byte);
            if (conversionType == typeof(char))
                return default(char);
            if (conversionType == typeof(DateTime))
                return default(DateTime);
            if (conversionType == typeof(decimal))
                return default(decimal);
            if (conversionType == typeof(double))
                return default(double);
            if (conversionType == typeof(short))
                return default(short);
            if (conversionType == typeof(int))
                return default(int);
            if (conversionType == typeof(long))
                return default(long);
            if (conversionType == typeof(sbyte))
                return default(sbyte);
            if (conversionType == typeof(float))
                return default(float);
            if (conversionType == typeof(ushort))
                return default(ushort);
            if (conversionType == typeof(uint))
                return default(uint);
            if (conversionType == typeof(ulong))
                return default(ulong);
            throw new InvalidCastException($"Cannot convert {nameof(Unit)} to {conversionType}.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(bool _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(sbyte _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(byte _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(short _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(ushort _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(int _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(uint _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(long _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(ulong _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(nint _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Unit(nuint _) => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Unit _1, Unit _2) => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Unit _1, Unit _2) => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Unit _1, Unit _2) => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Unit _1, Unit _2) => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Unit _1, Unit _2) => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Unit _1, Unit _2) => true;
    }
}
