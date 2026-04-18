using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WitherTorch.Common.Structures
{
    [StructLayout(LayoutKind.Auto)] // 不能通過 unmanaged 程式碼傳出
    public readonly struct Unit
    {
        public static readonly Unit Value = default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Unit _) => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj) => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => string.Empty;

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
