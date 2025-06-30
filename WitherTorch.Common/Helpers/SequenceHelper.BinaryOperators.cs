using System;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    unsafe partial class SequenceHelper
    {
        #region Or
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Or<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                OrCore(ptr, ptr + array.Length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Or<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                OrCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Or<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                OrCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Or<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => OrCore(ptr, ptrEnd, value);
        #endregion

        #region And
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void And<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                AndCore(ptr, ptr + array.Length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void And<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                AndCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void And<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                AndCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void And<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => AndCore(ptr, ptrEnd, value);
        #endregion

        #region Xor
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Xor<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                XorCore(ptr, ptr + array.Length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Xor<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                XorCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Xor<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                XorCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Xor<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => XorCore(ptr, ptrEnd, value);
        #endregion

        #region Add
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                AddCore(ptr, ptr + array.Length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                AddCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                AddCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => AddCore(ptr, ptrEnd, value);
        #endregion

        #region Substract
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                SubstractCore(ptr, ptr + array.Length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                SubstractCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                SubstractCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => SubstractCore(ptr, ptrEnd, value);
        #endregion

        #region Multiply
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                MultiplyCore(ptr, ptr + array.Length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                MultiplyCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                MultiplyCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => MultiplyCore(ptr, ptrEnd, value);
        #endregion

        #region Divide
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide<T>(T[] array, T value) where T : unmanaged
        {
            fixed (T* ptr = array)
                DivideCore(ptr, ptr + array.Length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide<T>(T[] array, T value, int startIndex) where T : unmanaged
        {
            int length = array.Length;
            if (startIndex >= length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            fixed (T* ptr = array)
                DivideCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide<T>(T[] array, T value, int startIndex, int count) where T : unmanaged
        {
            int length = startIndex + count;
            if (length > array.Length)
                throw new ArgumentOutOfRangeException(startIndex >= array.Length ? nameof(startIndex) : nameof(count));
            fixed (T* ptr = array)
                DivideCore(ptr + startIndex, ptr + length, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
            => DivideCore(ptr, ptrEnd, value);
        #endregion

        #region Core Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OrCore<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
            {
                Core<byte>.Or((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(char))
            {
                Core<ushort>.Or((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Or((nint*)ptr, (nint*)ptrEnd, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Or((nuint*)ptr, (nuint*)ptrEnd, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            if (UnsafeHelper.IsPrimitiveType<T>())
            {
                Core<T>.Or(ptr, ptrEnd, value);
                return;
            }
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        Core<byte>.Or((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        Core<sbyte>.Or((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        Core<short>.Or((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        Core<ushort>.Or((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        Core<int>.Or((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        Core<uint>.Or((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        Core<long>.Or((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        Core<ulong>.Or((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        Core<float>.Or((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        Core<double>.Or((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        Core<T>.Or(ptr, ptrEnd, value);
                        return;
                }
            }
            Core<T>.Or(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AndCore<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
            {
                Core<byte>.And((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(char))
            {
                Core<ushort>.And((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.And((nint*)ptr, (nint*)ptrEnd, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.And((nuint*)ptr, (nuint*)ptrEnd, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            if (UnsafeHelper.IsPrimitiveType<T>())
            {
                Core<T>.And(ptr, ptrEnd, value);
                return;
            }
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        Core<byte>.And((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        Core<sbyte>.And((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        Core<short>.And((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        Core<ushort>.And((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        Core<int>.And((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        Core<uint>.And((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        Core<long>.And((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        Core<ulong>.And((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        Core<float>.And((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        Core<double>.And((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        Core<T>.And(ptr, ptrEnd, value);
                        return;
                }
            }
            Core<T>.And(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void XorCore<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
            {
                Core<byte>.Xor((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(char))
            {
                Core<ushort>.Xor((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Xor((nint*)ptr, (nint*)ptrEnd, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Xor((nuint*)ptr, (nuint*)ptrEnd, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            if (UnsafeHelper.IsPrimitiveType<T>())
            {
                Core<T>.Xor(ptr, ptrEnd, value);
                return;
            }
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        Core<byte>.Xor((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        Core<sbyte>.Xor((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        Core<short>.Xor((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        Core<ushort>.Xor((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        Core<int>.Xor((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        Core<uint>.Xor((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        Core<long>.Xor((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        Core<ulong>.Xor((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        Core<float>.Xor((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        Core<double>.Xor((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        Core<T>.Xor(ptr, ptrEnd, value);
                        return;
                }
            }
            Core<T>.Xor(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AddCore<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
            {
                Core<byte>.Add((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(char))
            {
                Core<ushort>.Add((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Add((nint*)ptr, (nint*)ptrEnd, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Add((nuint*)ptr, (nuint*)ptrEnd, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            if (UnsafeHelper.IsPrimitiveType<T>())
            {
                Core<T>.Add(ptr, ptrEnd, value);
                return;
            }
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        Core<byte>.Add((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        Core<sbyte>.Add((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        Core<short>.Add((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        Core<ushort>.Add((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        Core<int>.Add((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        Core<uint>.Add((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        Core<long>.Add((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        Core<ulong>.Add((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        Core<float>.Add((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        Core<double>.Add((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        Core<T>.Add(ptr, ptrEnd, value);
                        return;
                }
            }
            Core<T>.Add(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SubstractCore<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
            {
                Core<byte>.Substract((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(char))
            {
                Core<ushort>.Substract((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Substract((nint*)ptr, (nint*)ptrEnd, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Substract((nuint*)ptr, (nuint*)ptrEnd, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            if (UnsafeHelper.IsPrimitiveType<T>())
            {
                Core<T>.Substract(ptr, ptrEnd, value);
                return;
            }
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        Core<byte>.Substract((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        Core<sbyte>.Substract((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        Core<short>.Substract((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        Core<ushort>.Substract((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        Core<int>.Substract((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        Core<uint>.Substract((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        Core<long>.Substract((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        Core<ulong>.Substract((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        Core<float>.Substract((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        Core<double>.Substract((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        Core<T>.Substract(ptr, ptrEnd, value);
                        return;
                }
            }
            Core<T>.Substract(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MultiplyCore<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
            {
                Core<byte>.Multiply((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(char))
            {
                Core<ushort>.Multiply((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Multiply((nint*)ptr, (nint*)ptrEnd, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Multiply((nuint*)ptr, (nuint*)ptrEnd, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            if (UnsafeHelper.IsPrimitiveType<T>())
            {
                Core<T>.Multiply(ptr, ptrEnd, value);
                return;
            }
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        Core<byte>.Multiply((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        Core<sbyte>.Multiply((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        Core<short>.Multiply((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        Core<ushort>.Multiply((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        Core<int>.Multiply((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        Core<uint>.Multiply((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        Core<long>.Multiply((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        Core<ulong>.Multiply((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        Core<float>.Multiply((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        Core<double>.Multiply((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        Core<T>.Multiply(ptr, ptrEnd, value);
                        return;
                }
            }
            Core<T>.Multiply(ptr, ptrEnd, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DivideCore<T>(T* ptr, T* ptrEnd, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
            {
                Core<byte>.Divide((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                return;
            }
            if (typeof(T) == typeof(char))
            {
                Core<ushort>.Divide((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Divide((nint*)ptr, (nint*)ptrEnd, UnsafeHelper.As<T, nint>(value));
                return;
            }
            if (typeof(T) == typeof(nint))
            {
                Core.Divide((nuint*)ptr, (nuint*)ptrEnd, UnsafeHelper.As<T, nuint>(value));
                return;
            }
            if (UnsafeHelper.IsPrimitiveType<T>())
            {
                Core<T>.Divide(ptr, ptrEnd, value);
                return;
            }
            Type type = typeof(T);
            if (type.IsEnum)
            {
                switch (Type.GetTypeCode(type.GetEnumUnderlyingType()))
                {
                    case TypeCode.Boolean or TypeCode.Byte:
                        Core<byte>.Divide((byte*)ptr, (byte*)ptrEnd, UnsafeHelper.As<T, byte>(value));
                        return;
                    case TypeCode.SByte:
                        Core<sbyte>.Divide((sbyte*)ptr, (sbyte*)ptrEnd, UnsafeHelper.As<T, sbyte>(value));
                        return;
                    case TypeCode.Int16:
                        Core<short>.Divide((short*)ptr, (short*)ptrEnd, UnsafeHelper.As<T, short>(value));
                        return;
                    case TypeCode.Char or TypeCode.UInt16:
                        Core<ushort>.Divide((ushort*)ptr, (ushort*)ptrEnd, UnsafeHelper.As<T, ushort>(value));
                        return;
                    case TypeCode.Int32:
                        Core<int>.Divide((int*)ptr, (int*)ptrEnd, UnsafeHelper.As<T, int>(value));
                        return;
                    case TypeCode.UInt32:
                        Core<uint>.Divide((uint*)ptr, (uint*)ptrEnd, UnsafeHelper.As<T, uint>(value));
                        return;
                    case TypeCode.Int64:
                        Core<long>.Divide((long*)ptr, (long*)ptrEnd, UnsafeHelper.As<T, long>(value));
                        return;
                    case TypeCode.UInt64:
                        Core<ulong>.Divide((ulong*)ptr, (ulong*)ptrEnd, UnsafeHelper.As<T, ulong>(value));
                        return;
                    case TypeCode.Single:
                        Core<float>.Divide((float*)ptr, (float*)ptrEnd, UnsafeHelper.As<T, float>(value));
                        return;
                    case TypeCode.Double:
                        Core<double>.Divide((double*)ptr, (double*)ptrEnd, UnsafeHelper.As<T, double>(value));
                        return;
                    default:
                        Core<T>.Divide(ptr, ptrEnd, value);
                        return;
                }
            }
            Core<T>.Divide(ptr, ptrEnd, value);
        }
        #endregion
    }
}
