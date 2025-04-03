using InlineMethod;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Extensions
{
    public static class CollectionExtensions
    {
        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool HasAnyItem<T>(this IList<T> obj)
        {
            return obj.Count > 0;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool HasAnyItem<T>(this Stack<T> obj)
        {
            return obj.Count > 0;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool HasAnyItem<T>(this Queue<T> obj)
        {
            return obj.Count > 0;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T? FirstOrDefault<T>(this T[] array) => FirstOrDefault(array, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T? FirstOrDefault<T>(this T[] array, T? defaultValue) => array.Length > 0 ? array[0] : defaultValue;

        [Inline(InlineBehavior.Keep, export: true)]
        public static T? LastOrDefault<T>(this T[] array) => LastOrDefault(array, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T? LastOrDefault<T>(this T[] array, T? defaultValue)
        {
            int length = array.Length;
            return length > 0 ? array[length - 1] : defaultValue;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T? FirstOrDefault<T>(this IList<T> list) => FirstOrDefault(list, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T? FirstOrDefault<T>(this IList<T> list, T? defaultValue) => list.Count > 0 ? list[0] : defaultValue;

        [Inline(InlineBehavior.Keep, export: true)]
        public static T? LastOrDefault<T>(this IList<T> list) => LastOrDefault(list, default);

        [Inline(InlineBehavior.Keep, export: true)]
        public static T? LastOrDefault<T>(this IList<T> list, T? defaultValue)
        {
            int count = list.Count;
            return count > 0 ? list[count - 1] : defaultValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToArray<T>(this ICollection<T> obj)
        {
            int count = obj.Count;
            if (count <= 0)
                return Array.Empty<T>();
            T[] array = new T[count];
            if (count > 0)
                obj.CopyTo(array, 0);
            return array;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe bool Contains<T>(this T[] array, T value) where T : unmanaged
            => Contains(array, value, 0, array.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Contains<T>(this T[] array, T value, int startIndex, int length) where T : unmanaged
        {
            if (length <= 0)
                return false;
            fixed (T* ptr = array)
                return ContainsCore(ptr + startIndex, value, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Contains<T>(T* ptr, T value, int length) where T : unmanaged
        {
            if (length <= 0)
                return false;
            return ContainsCore(ptr, value, length);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe bool ContainsCore<T>(T* ptr, T value, int length) where T : unmanaged
        {
            switch (sizeof(T))
            {
                case sizeof(byte):
                    return SequenceHelper.Contains((byte*)ptr, (byte*)ptr + length, *(byte*)&value);
                case sizeof(ushort):
                    return SequenceHelper.Contains((ushort*)ptr, (ushort*)ptr + length, *(ushort*)&value);
                case sizeof(uint):
                    return SequenceHelper.Contains((uint*)ptr, (uint*)ptr + length, *(uint*)&value);
                case sizeof(ulong):
                    return SequenceHelper.Contains((ulong*)ptr, (ulong*)ptr + length, *(ulong*)&value);
                default:
                    break;
            }
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < length; i++)
            {
                if (comparer.Equals(ptr[i], value))
                    return true;
            }
            return false;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool TryPop<T>(this Stack<T> obj, out T? value)
        {
            UnsafeHelper.SkipInit(out value);
            if (obj.Count > 0)
            {
                value = obj.Pop();
                return true;
            }
            return false;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool TryDequeue<T>(this Queue<T> obj, out T value)
        {
            UnsafeHelper.SkipInit(out value);
            if (obj.Count > 0)
            {
                value = obj.Dequeue();
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrueForAny<T>(this IList<T> obj, Func<T, bool> condiction)
        {
            int count = obj.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (condiction(obj[i]))
                        return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrueForAny<T>(this T[] obj, Func<T, bool> condiction)
        {
            int length = obj.Length;
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    if (condiction(obj[i]))
                        return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
