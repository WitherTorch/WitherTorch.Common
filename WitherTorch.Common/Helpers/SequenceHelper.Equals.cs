using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

using InlineMethod;

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
            return EqualsCore(a, b);
        }

        /// <inheritdoc cref="string.Equals(string?, string?, StringComparison)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(string? a, string? b, StringComparison comparisonType)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (a is null || b is null)
                return false;
            return comparisonType switch
            {
                StringComparison.CurrentCulture => CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.None) == 0,
                StringComparison.CurrentCultureIgnoreCase => CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.IgnoreCase) == 0,
                StringComparison.InvariantCulture => CultureInfo.InvariantCulture.CompareInfo.Compare(a, b, CompareOptions.None) == 0,
                StringComparison.InvariantCultureIgnoreCase => CultureInfo.InvariantCulture.CompareInfo.Compare(a, b, CompareOptions.IgnoreCase) == 0,
                StringComparison.OrdinalIgnoreCase => EqualsIgnoreCaseCore(a, b),
                StringComparison.Ordinal => EqualsCore(a, b),
                _ => throw new ArgumentOutOfRangeException(nameof(comparisonType))
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals<T>(T[]? a, T[]? b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (a is null || b is null)
                return false;
            int length = a.Length;
            if (length != b.Length)
                return false;
            if (UnsafeHelper.IsPrimitiveType<T>())
            {
#pragma warning disable CS8500
                fixed (T* ptr = a, ptr2 = b)
                    return EqualsCore(ptr, ptr2, MathHelper.MakeUnsigned(length));
#pragma warning restore CS8500
            }
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < length; i++)
            {
                if (!comparer.Equals(a[i], b[i]))
                    return false;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(void* ptr, void* ptrEnd, void* ptr2)
        {
            if (ptr == ptr2 || ptrEnd <= ptr)
                return true;
            return EqualsCore((byte*)ptr, (byte*)ptrEnd, (byte*)ptr2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualsCore(string str1, string str2)
        {
            int length = str1.Length;
            if (length != str2.Length)
                return false;
            fixed (char* ptr = str1, ptr2 = str2)
                return EqualsCore(ptr, ptr2, MathHelper.MakeUnsigned(length));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualsIgnoreCaseCore(string str1, string str2)
        {
            int length = str1.Length;
            if (length != str2.Length)
                return false;
            if (ContainsGreaterThan(str1, '\u007F'))
                return ContainsGreaterThan(str2, '\u007F') && string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
            if (ContainsGreaterThan(str2, '\u007F'))
                return false;
            fixed (char* ptr = str1, ptr2 = str2)
                return FastCore<ushort>.RangedAddAndEquals((ushort*)ptr, (ushort*)ptr2, MathHelper.MakeUnsigned(length), 'A', 'Z', 'a' - 'A');
        }

        [Inline(InlineBehavior.Remove)]
        private static bool EqualsCore(byte* ptr, byte* ptrEnd, byte* ptr2) => FastCore.Equals(ptr, ptr2, unchecked((nuint)(ptrEnd - ptr)));

        [Inline(InlineBehavior.Remove)]
        private static bool EqualsCore(void* ptr, void* ptr2, nuint length) => FastCore.Equals((byte*)ptr, (byte*)ptr2, length);
    }
}
