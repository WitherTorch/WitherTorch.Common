using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal static partial class InternalSequenceHelper
    {
        private static unsafe int LegacyCompareTo<T>(T* ptr, T* ptrEnd, T* ptrB) where T : unmanaged
        {
            for (; ptr < ptrEnd; ptr++, ptrB++)
            {
                int comparison = CompareTo(*ptr, *ptrB);
                if (comparison != 0)
                    return MathHelper.Sign(comparison);
            }
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CompareTo<T>(T a, T b) where T : unmanaged
        {
            if (typeof(T) == typeof(byte))
                return UnsafeHelper.As<T, byte>(a).CompareTo(UnsafeHelper.As<T, byte>(b));
            if (typeof(T) == typeof(sbyte))
                return UnsafeHelper.As<T, sbyte>(a).CompareTo(UnsafeHelper.As<T, sbyte>(b));
            if (typeof(T) == typeof(char))
                return UnsafeHelper.As<T, char>(a).CompareTo(UnsafeHelper.As<T, char>(b));
            if (typeof(T) == typeof(short))
                return UnsafeHelper.As<T, short>(a).CompareTo(UnsafeHelper.As<T, short>(b));
            if (typeof(T) == typeof(ushort))
                return UnsafeHelper.As<T, ushort>(a).CompareTo(UnsafeHelper.As<T, ushort>(b));
            if (typeof(T) == typeof(int))
                return UnsafeHelper.As<T, int>(a).CompareTo(UnsafeHelper.As<T, int>(b));
            if (typeof(T) == typeof(uint))
                return UnsafeHelper.As<T, uint>(a).CompareTo(UnsafeHelper.As<T, uint>(b));
            if (typeof(T) == typeof(long))
                return UnsafeHelper.As<T, long>(a).CompareTo(UnsafeHelper.As<T, long>(b));
            if (typeof(T) == typeof(ulong))
                return UnsafeHelper.As<T, long>(a).CompareTo(UnsafeHelper.As<T, ulong>(b));
#if NET8_0_OR_GREATER
            if (typeof(T) == typeof(nint))
                return UnsafeHelper.As<T, nint>(a).CompareTo(UnsafeHelper.As<T, nint>(b));
            if (typeof(T) == typeof(nuint))
                return UnsafeHelper.As<T, nuint>(a).CompareTo(UnsafeHelper.As<T, nuint>(b));
#endif
            if (typeof(T) == typeof(float))
                return UnsafeHelper.As<T, long>(a).CompareTo(UnsafeHelper.As<T, float>(b));
            if (typeof(T) == typeof(double))
                return UnsafeHelper.As<T, long>(a).CompareTo(UnsafeHelper.As<T, double>(b));
            return Comparer<T>.Default.Compare(a, b);
        }

        public static unsafe T* PointerIndexOf<T>(T* ptr, T* ptrEnd, T* value, nuint valueLength) where T : unmanaged
        {
            if (ptrEnd < ptr)
                return null;

            DebugHelper.ThrowIf(valueLength == 0, "valueLength should not be zero!");
            if (valueLength == 1)
                return SequenceHelper.PointerIndexOf(ptr, ptrEnd, *value);

            return PointerIndexOfCore(ptr, unchecked((nuint)(ptrEnd - ptr)), value, valueLength);
        }

        public static unsafe bool Contains<T>(T* ptr, nuint count, T* value, nuint valueLength) where T : unmanaged
        {
            DebugHelper.ThrowIf(valueLength == 0, "valueLength should not be zero!");
            if (valueLength == 1)
                return SequenceHelper.Contains(ptr, count, *value);

            return PointerIndexOfCore(ptr, count, value, valueLength) != null;
        }

        public static unsafe T* PointerIndexOf<T>(T* ptr, nuint count, T* value, nuint valueLength) where T : unmanaged
        {
            DebugHelper.ThrowIf(valueLength == 0, "valueLength should not be zero!");
            if (valueLength == 1)
                return SequenceHelper.PointerIndexOf(ptr, count, *value);

            return PointerIndexOfCore(ptr, count, value, valueLength);
        }

        private static unsafe T* PointerIndexOfCore<T>(T* ptr, nuint count, T* value, nuint valueLength) where T : unmanaged
        {
            T valueHead = *value;
            value++;
            valueLength--;

            T* ptrEnd = ptr + count - valueLength;
            while ((ptr = SequenceHelper.PointerIndexOf(ptr, ptrEnd, valueHead)) != null)
            {
                if (SequenceHelper.Equals(ptr + 1, value, valueLength))
                    return ptr;
                ptr++;
            }
            return null;
        }

        [Inline(InlineBehavior.Remove)]
        public static unsafe int ConvertToIndex32<T>(T* ptrResult, T* ptrBase) where T : unmanaged
            => ptrResult < ptrBase ? -1 : unchecked((int)(ptrResult - ptrBase));
    }
}
