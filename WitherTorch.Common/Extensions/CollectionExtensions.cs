using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Collections;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Extensions
{
    public static partial class CollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this ICollection<T> collection)
            => collection as IReadOnlyCollection<T> ?? new ReadOnlyCollectionAdapter<T>(collection);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<T> AsReadOnlyList<T>(this IList<T> collection)
            => collection as IReadOnlyList<T> ?? new ReadOnlyListAdapter<T>(collection);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasNonNullItem<T>(this IEnumerable<T> enumerable) where T : class
        {
            return enumerable switch
            {
                T[] array => ArrayHelper.HasNonNullItem(array),
                UnwrappableList<T> list => ArrayHelper.HasNonNullItem(list.Unwrap(), 0, list.Count),
                _ => enumerable.AsParallel().Where(val => val is not null).Any()
            };
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool HasAnyItem<T>(this IList<T> obj) => obj.Count > 0;

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool HasAnyItem<T>(this Stack<T> obj) => obj.Count > 0;

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool HasAnyItem<T>(this Queue<T> obj) => obj.Count > 0;

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
            => SequenceHelper.Contains(array, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Contains<T>(this T[] array, T value, int startIndex, int length)
            => SequenceHelper.Contains(array, value, startIndex, length);

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool TryPop<T>(this Stack<T> obj, [MaybeNullWhen(false)] out T? value)
        {
            if (obj.Count > 0)
            {
                value = obj.Pop();
                return true;
            }
            value = default;
            return false;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool TryDequeue<T>(this Queue<T> obj, [MaybeNullWhen(false)] out T? value)
        {
            if (obj.Count > 0)
            {
                value = obj.Dequeue();
                return true;
            }
            value = default;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRange<T, TEnumerable>(this ICollection<T> _this, TEnumerable items) where TEnumerable : IEnumerable<T>
        {
            switch (_this)
            {
                case List<T> list:
                    list.AddRange(items);
                    return;
                case IAddRangeCollectionGenerics<T> list:
                    list.AddRange(items);
                    return;
                case IAddRangeCollection<T> list:
                    list.AddRange(items);
                    return;
                default:
                    break;
            }

            T[] array;
            int length;

            if (typeof(TEnumerable) == typeof(T[]) || items is T[])
                goto IsArray;
            if (typeof(TEnumerable) == typeof(UnwrappableList<T>) || items is UnwrappableList<T>)
                goto IsUnwrappableList;
            if (typeof(TEnumerable) == typeof(PooledList<T>) || items is PooledList<T>)
                goto IsPooledList;

            goto Fallback;

        IsArray:
            array = UnsafeHelper.As<TEnumerable, T[]>(items);
            length = array.Length;
            goto IsArray_Core;

        IsUnwrappableList:
            UnwrappableList<T> unwrappableList = UnsafeHelper.As<TEnumerable, UnwrappableList<T>>(items);
            array = unwrappableList.Unwrap();
            length = unwrappableList.Count;
            goto IsArray_Core;

        IsPooledList:
            PooledList<T> pooledList = UnsafeHelper.As<TEnumerable, PooledList<T>>(items);
            array = pooledList.GetBuffer();
            length = pooledList.Count;
            goto IsArray_Core;

        IsArray_Core:
            if (length <= 0)
                return;
            ref T itemRef = ref array[0];
            for (nuint i = 0, limit = (nuint)length; i < limit; i++)
                _this.Add(UnsafeHelper.AddByteOffset(ref itemRef, i * UnsafeHelper.SizeOf<T>()));
            return;

        Fallback:
            foreach (T item in items)
                _this.Add(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveAll<T, TEnumerable>(this ICollection<T> _this, TEnumerable items) where TEnumerable : IEnumerable<T>
        {
            if (_this.IsReadOnly)
                throw new InvalidOperationException("The collection is empty!");

            T[] array;
            int length;

            if (typeof(TEnumerable) == typeof(T[]) || items is T[])
                goto IsArray;
            if (typeof(TEnumerable) == typeof(UnwrappableList<T>) || items is UnwrappableList<T>)
                goto IsUnwrappableList;
            if (typeof(TEnumerable) == typeof(PooledList<T>) || items is PooledList<T>)
                goto IsPooledList;

            goto Fallback;

        IsArray:
            array = UnsafeHelper.As<TEnumerable, T[]>(items);
            length = array.Length;
            goto IsArray_Core;

        IsUnwrappableList:
            UnwrappableList<T> unwrappableList = UnsafeHelper.As<TEnumerable, UnwrappableList<T>>(items);
            array = unwrappableList.Unwrap();
            length = unwrappableList.Count;
            goto IsArray_Core;

        IsPooledList:
            PooledList<T> pooledList = UnsafeHelper.As<TEnumerable, PooledList<T>>(items);
            array = pooledList.GetBuffer();
            length = pooledList.Count;
            goto IsArray_Core;

        IsArray_Core:
            if (length <= 0)
                return;
            ref T itemRef = ref array[0];
            for (nuint i = 0, limit = (nuint)length; i < limit; i++)
                _this.Remove(UnsafeHelper.AddByteOffset(ref itemRef, i * UnsafeHelper.SizeOf<T>()));
            return;

        Fallback:
            foreach (T item in items)
                _this.Remove(item);
        }
    }
}
