using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        /// <inheritdoc cref="string.Equals(string?, string?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(string? a, string? b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (a is null || b is null)
                return false;
            int length = a.Length;
            if (length != b.Length)
                return false;
            return EqualsCore(a, b, length);
        }

        /// <inheritdoc cref="string.Equals(string?, string?, StringComparison)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(string? a, string? b, StringComparison comparisonType)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (a is null || b is null)
                return false;
            int length = a.Length;
            if (length != b.Length)
                return false;
            return comparisonType switch
            {
                StringComparison.CurrentCulture => CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.None) == 0,
                StringComparison.CurrentCultureIgnoreCase => CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.IgnoreCase) == 0,
                StringComparison.InvariantCulture => CultureInfo.InvariantCulture.CompareInfo.Compare(a, b, CompareOptions.None) == 0,
                StringComparison.InvariantCultureIgnoreCase => CultureInfo.InvariantCulture.CompareInfo.Compare(a, b, CompareOptions.IgnoreCase) == 0,
                StringComparison.OrdinalIgnoreCase => EqualsIgnoreCaseCore(a, b, length),
                StringComparison.Ordinal => EqualsCore(a, b, length),
                _ => throw new ArgumentOutOfRangeException(nameof(comparisonType))
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(T[]? a, T[]? b) where T : unmanaged
        {
            if (ReferenceEquals(a, b))
                return true;
            if (a is null || b is null)
                return false;
            int length = a.Length;
            if (length != b.Length)
                return false;
            fixed (T* ptr = a, ptr2 = b)
                return EqualsCore(ptr, ptr + length, ptr2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(T* ptr, T* ptrEnd, T* ptr2) where T : unmanaged
        {
            if (ptr == ptr2)
                return true;
            return EqualsCore(ptr, ptrEnd, ptr2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualsCore(string str1, string str2, int length)
        {
            fixed (char* ptr = str1, ptr2 = str2)
                return Core<ushort>.Equals((ushort*)ptr, (ushort*)ptr + length, (ushort*)ptr2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualsIgnoreCaseCore(string str1, string str2, int length)
        {
            fixed (char* ptr = str1, ptr2 = str2)
                return Core<ushort>.RangedAddAndEquals((ushort*)ptr, (ushort*)ptr + length, (ushort*)ptr2, 'A', 'Z', 'a' - 'A');
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualsCore<T>(T* ptr, T* ptrEnd, T* ptr2) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return Core<byte>.Equals((byte*)ptr, (byte*)ptrEnd, (byte*)ptr2);
            if (typeof(T) == typeof(char))
                return Core<ushort>.Equals((ushort*)ptr, (ushort*)ptrEnd, (ushort*)ptr2);
            if (typeof(T) == typeof(nint))
                return Core.Equals((IntPtr*)ptr, (IntPtr*)ptrEnd, (IntPtr*)ptr2);
            if (typeof(T) == typeof(nuint))
                return Core.Equals((UIntPtr*)ptr, (UIntPtr*)ptrEnd, (UIntPtr*)ptr2);
            if (UnsafeHelper.IsPrimitiveType<T>())
                return Core<T>.Equals(ptr, ptrEnd, ptr2);
            Type type = typeof(T);
            if (type.IsEnum)
            {
                return Type.GetTypeCode(type.GetEnumUnderlyingType()) switch
                {
                    TypeCode.Boolean or TypeCode.Byte => Core<byte>.Equals((byte*)ptr, (byte*)ptrEnd, (byte*)ptr2),
                    TypeCode.SByte => Core<sbyte>.Equals((sbyte*)ptr, (sbyte*)ptrEnd, (sbyte*)ptr2),
                    TypeCode.Int16 => Core<short>.Equals((short*)ptr, (short*)ptrEnd, (short*)ptr2),
                    TypeCode.Char or TypeCode.UInt16 => Core<ushort>.Equals((ushort*)ptr, (ushort*)ptrEnd, (ushort*)ptr2),
                    TypeCode.Int32 => Core<int>.Equals((int*)ptr, (int*)ptrEnd, (int*)ptr2),
                    TypeCode.UInt32 => Core<uint>.Equals((uint*)ptr, (uint*)ptrEnd, (uint*)ptr2),
                    TypeCode.Int64 => Core<long>.Equals((long*)ptr, (long*)ptrEnd, (long*)ptr2),
                    TypeCode.UInt64 => Core<ulong>.Equals((ulong*)ptr, (ulong*)ptrEnd, (ulong*)ptr2),
                    TypeCode.Single => Core<float>.Equals((float*)ptr, (float*)ptrEnd, (float*)ptr2),
                    TypeCode.Double => Core<double>.Equals((double*)ptr, (double*)ptrEnd, (double*)ptr2),
                    _ => Core<T>.Equals(ptr, ptrEnd, ptr2)
                };
            }
            return Core<T>.Equals(ptr, ptrEnd, ptr2);
        }
    }
}
