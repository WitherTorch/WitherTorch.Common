using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Structures;
using WitherTorch.Common.Threading;

#pragma warning disable CS8500

namespace WitherTorch.Common.Text
{
    public unsafe ref partial struct StringBuilderTiny : IDisposable
    {
        private char* _start, _iterator, _end;
        private LazyTinyRef<StringBuilder> _builderLazy;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringBuilderTiny()
        {
            _builderLazy = new LazyTinyRef<StringBuilder>(StringBuilderPool.Shared.Rent);
            _start = null;
            _iterator = null;
            _end = null;
        }

        public readonly int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                StringBuilder? builder = _builderLazy.GetValueDirectly();
                if (builder is null)
                    return unchecked((int)(_iterator - _start));
                return builder.Length;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetStartPointer(char* ptr, int length)
        {
            _start = ptr;
            _iterator = ptr;
            _end = ptr + length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity(int capacity)
        {
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
            {
                builder.EnsureCapacity(capacity);
                return;
            }
            if (_end - _start >= capacity)
                return;
            _builderLazy.Value.EnsureCapacity(capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public void Append(char value)
        {
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
            {
                builder.Append(value);
                return;
            }
            char* iterator = _iterator;
            char* end = _end;
            if (iterator < end)
            {
                *iterator = value;
                _iterator = iterator + 1;
                return;
            }
            GetAlternateStringBuilder(1).Append(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public void Append(char value, int repeatCount)
        {
            if (repeatCount < 1)
                return;
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
            {
                builder.Append(value, repeatCount);
                return;
            }
            char* iterator = _iterator;
            char* endIterator = iterator + repeatCount - 1;
            char* end = _end;
            if (endIterator < end)
            {
                for (; iterator <= endIterator; iterator++)
                {
                    *iterator = value;
                }
                _iterator = iterator;
                return;
            }
            GetAlternateStringBuilder(repeatCount).Append(value, repeatCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string value)
        {
            int count = value.Length;
            if (count <= 0)
                return;
            fixed (char* ptr = value)
                AppendCore(ptr, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(StringBase value)
        {
            int count = value.Length;
            if (count <= 0)
                return;
            AppendCore(value, 0, unchecked((nuint)count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(StringSlice value)
        {
            int count = value.Length;
            if (count <= 0)
                return;
            int startIndex = value.StartIndex;
            if (startIndex < 0)
                return;
            AppendCore(value.Original, (nuint)startIndex, (nuint)count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            int valueLength = value.Length;
            if (valueLength <= 0)
                return;
            if (startIndex + count > valueLength)
                throw new ArgumentOutOfRangeException(startIndex >= valueLength ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = value)
                AppendCore(ptr + startIndex, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(StringBase value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            int valueLength = value.Length;
            if (valueLength <= 0)
                return;
            if (startIndex + count > valueLength)
                throw new ArgumentOutOfRangeException(startIndex >= valueLength ? nameof(startIndex) : nameof(count));
            AppendCore(value, unchecked((nuint)startIndex), unchecked((nuint)count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char* ptr, int count)
        {
            if (count <= 0)
                return;
            AppendCore(ptr, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat<T>(string format, T arg0)
        {
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                AppendFormatCore(format, &arg0, 1);
                return;
            }
            AppendFormatCore(format, new ParamArrayTiny<T>(arg0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat<T>(string format, T arg0, T arg1)
        {
            ParamArrayTiny<T> array = new(arg0, arg1);
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                AppendFormatCore(format, (T*)(((byte*)&array) + ParamArrayTiny<T>.ArrayOffset), 2);
                return;
            }
            AppendFormatCore(format, array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat<T>(string format, T arg0, T arg1, T arg2)
        {
            ParamArrayTiny<T> array = new(arg0, arg1, arg2);
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                AppendFormatCore(format, (T*)(((byte*)&array) + ParamArrayTiny<T>.ArrayOffset), 3);
                return;
            }
            AppendFormatCore(format, array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat<T>(string format, params T[] args)
        {
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                fixed (T* ptr = args)
                    AppendFormatCore(format, ptr, args.Length);
                return;
            }
            AppendFormatCore<T, T[]>(format, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatWithArgs<T, TEnumerable>(string format, TEnumerable args) where TEnumerable : IEnumerable<T>
        {
            if (typeof(TEnumerable) == typeof(T[]))
                goto Array;
            if (typeof(TEnumerable) == typeof(IList<T>))
                goto List;
            if (typeof(TEnumerable) == typeof(IReadOnlyList<T>))
                goto ReadOnlyList;

            if (args is T[])
                goto Array;
            if (args is IList<T>)
                goto List;
            if (args is IReadOnlyList<T>)
                goto ReadOnlyList;

            goto Fallback;

        Array:
            T[] argArray = UnsafeHelper.As<TEnumerable, T[]>(args);
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                fixed (T* ptr = argArray)
                    AppendFormatCore(format, ptr, argArray.Length);
            }
            else
                AppendFormatCore<T, T[]>(format, argArray);
            return;

        List:
            AppendFormatCore<T, IList<T>>(format, UnsafeHelper.As<TEnumerable, IList<T>>(args));
            return;

        ReadOnlyList:
            AppendFormatCore<T, IReadOnlyList<T>>(format, UnsafeHelper.As<TEnumerable, IReadOnlyList<T>>(args));
            return;

        Fallback:
            using PooledList<T> list = new PooledList<T>(capacity: 0);
            list.AddRange(args);
            AppendFormatCore<T, IList<T>>(format, UnsafeHelper.As<IList<T>>(list));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat(string format, object arg0)
            => AppendFormatCore(format, new ParamArrayTiny<object>(arg0));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat(string format, object arg0, object arg1)
            => AppendFormatCore(format, new ParamArrayTiny<object>(arg0, arg1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat(string format, object arg0, object arg1, object arg2)
            => AppendFormatCore(format, new ParamArrayTiny<object>(arg0, arg1, arg2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat(string format, params object[] args)
            => AppendFormatCore(format, new ParamArrayTiny<object>(args));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat<T>(StringBase format, T arg0)
        {
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                AppendFormatCore(format, &arg0, 1);
                return;
            }
            AppendFormatCore(format, new ParamArrayTiny<T>(arg0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat<T>(StringBase format, T arg0, T arg1)
        {
            ParamArrayTiny<T> array = new(arg0, arg1);
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                AppendFormatCore(format, (T*)(((byte*)&array) + ParamArrayTiny<T>.ArrayOffset), 2);
                return;
            }
            AppendFormatCore(format, array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat<T>(StringBase format, T arg0, T arg1, T arg2)
        {
            ParamArrayTiny<T> array = new(arg0, arg1, arg2);
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                AppendFormatCore(format, (T*)(((byte*)&array) + ParamArrayTiny<T>.ArrayOffset), 3);
                return;
            }
            AppendFormatCore(format, array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat<T>(StringBase format, params T[] args)
        {
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                fixed (T* ptr = args)
                    AppendFormatCore(format, ptr, args.Length);
                return;
            }
            AppendFormatCore<T, T[]>(format, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatWithArgs<T, TEnumerable>(StringBase format, TEnumerable args) where TEnumerable : IEnumerable<T>
        {
            if (typeof(TEnumerable) == typeof(T[]))
                goto Array;
            if (typeof(TEnumerable) == typeof(IList<T>))
                goto List;
            if (typeof(TEnumerable) == typeof(IReadOnlyList<T>))
                goto ReadOnlyList;

            if (args is T[])
                goto Array;
            if (args is IList<T>)
                goto List;
            if (args is IReadOnlyList<T>)
                goto ReadOnlyList;

            goto Fallback;

        Array:
            T[] argArray = UnsafeHelper.As<TEnumerable, T[]>(args);
            if (UnsafeHelper.IsUnmanagedType<T>())
            {
                fixed (T* ptr = argArray)
                    AppendFormatCore(format, ptr, argArray.Length);
            }
            else
                AppendFormatCore<T, T[]>(format, argArray);
            return;

        List:
            AppendFormatCore<T, IList<T>>(format, UnsafeHelper.As<TEnumerable, IList<T>>(args));
            return;

        ReadOnlyList:
            AppendFormatCore<T, IReadOnlyList<T>>(format, UnsafeHelper.As<TEnumerable, IReadOnlyList<T>>(args));
            return;

        Fallback:
            using PooledList<T> list = new PooledList<T>(capacity: 0);
            list.AddRange(args);
            AppendFormatCore<T, IList<T>>(format, UnsafeHelper.As<IList<T>>(list));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat(StringBase format, object arg0)
            => AppendFormatCore(format, new ParamArrayTiny<object>(arg0));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat(StringBase format, object arg0, object arg1)
            => AppendFormatCore(format, new ParamArrayTiny<object>(arg0, arg1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat(StringBase format, object arg0, object arg1, object arg2)
            => AppendFormatCore(format, new ParamArrayTiny<object>(arg0, arg1, arg2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat(StringBase format, params object[] args)
            => AppendFormatCore(format, new ParamArrayTiny<object>(args));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringBuilderTiny Clear()
        {
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is null)
            {
                _iterator = _start;
                return this;
            }
            builder.Clear();
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, char value)
        {
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
            {
                builder.Insert(index, value);
                return;
            }
            char* iterator = _iterator;
            char* endIterator = iterator;
            char* end = _end;
            if (endIterator < end)
            {
                char* copyStartPtr = _start + index;
                for (char* sourcePtr = iterator - 1, destPtr = endIterator; sourcePtr >= copyStartPtr; sourcePtr--, destPtr--)
                {
                    *destPtr = *sourcePtr;
                }
                *copyStartPtr = value;
                _iterator = endIterator + 1;
                return;
            }
            GetAlternateStringBuilder(1).Insert(index, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, string value)
        {
            int valueLength = value.Length;
            if (valueLength <= 0)
                return;
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
            {
                builder.Insert(index, value);
                return;
            }
            char* iterator = _iterator;
            char* endIterator = iterator + valueLength - 1;
            char* end = _end;
            if (endIterator < end)
            {
                char* copyStartPtr = _start + index;
                for (char* sourcePtr = iterator - 1, destPtr = endIterator; sourcePtr >= copyStartPtr; sourcePtr--, destPtr--)
                {
                    *destPtr = *sourcePtr;
                }
                fixed (char* ptr = value)
                {
                    UnsafeHelper.CopyBlockUnaligned(copyStartPtr, ptr, unchecked((uint)(sizeof(char) * valueLength)));
                }
                _iterator = endIterator + 1;
                return;
            }
            GetAlternateStringBuilder(valueLength).Insert(index, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, StringBase value)
        {
            int valueLength = value.Length;
            if (valueLength <= 0)
                return;
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
            {
                builder.Insert(index, value.ToString());
                return;
            }
            char* iterator = _iterator;
            char* endIterator = iterator + valueLength - 1;
            char* end = _end;
            if (endIterator < end)
            {
                char* copyStartPtr = _start + index;
                for (char* sourcePtr = iterator - 1, destPtr = endIterator; sourcePtr >= copyStartPtr; sourcePtr--, destPtr--)
                    *destPtr = *sourcePtr;
                value.CopyToCore(copyStartPtr, 0u, unchecked((nuint)valueLength));
                _iterator = endIterator + 1;
                return;
            }
            GetAlternateStringBuilder(valueLength).Insert(index, value.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length <= 0)
                return;
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
            {
                builder.Remove(startIndex, length);
                return;
            }
            char* start = _start + startIndex;
            char* iterator = _iterator;
            if (iterator - length <= start)
            {
                _iterator = start;
                return;
            }
            for (char* sourcePtr = start + length, destPtr = start; sourcePtr < iterator; sourcePtr++, destPtr++)
            {
                *destPtr = *sourcePtr;
            }
            _iterator = iterator - length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveLast()
        {
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
            {
                builder.Remove(builder.Length - 1, 1);
                return;
            }
            char* start = _start;
            char* iterator = _iterator;
            if (iterator > start)
                _iterator = iterator - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
        {
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
                return builder.ToString();
            char* start = _start;
            char* iterator = _iterator;
            if (start >= iterator)
                return string.Empty;
            return new string(start, 0, unchecked((int)(iterator - start)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly StringBase ToStringBase()
        {
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
                return StringBase.Create(builder.ToString());
            char* start = _start;
            char* iterator = _iterator;
            if (start >= iterator)
                return StringBase.Empty;
            return StringBase.Create(start, 0, unchecked((nuint)(iterator - start)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(int startIndex, int length)
        {
            if (length <= 0)
                return string.Empty;
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
                return builder.ToString(startIndex, length);
            char* start = _start + startIndex;
            char* end = start + length;
            char* iterator = _iterator;
            if (start == iterator)
                return string.Empty;
            if (end > iterator)
                end = iterator;
            return new string(start, 0, unchecked((int)(end - start)));
        }

        public readonly void Dispose() => ReturnStringBuilder();
    }
}
