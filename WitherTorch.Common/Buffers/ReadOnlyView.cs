using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Buffers
{
    public static class ReadOnlyView
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyView<T> FromArray<T>(T[] source) where T : unmanaged => new ReadOnlyView<T>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyView<char> FromString(string source) => new ReadOnlyView<char>(source);
    }

    [StructLayout(LayoutKind.Auto)]
    public readonly struct ReadOnlyView<T> : IEnumerable<T>, IReadOnlyList<T>, IPinnableReference<T> where T : unmanaged
    {
        public static readonly ReadOnlyView<T> Empty = default;

        internal readonly object? _source;
        internal readonly int _startIndex, _count;

        internal ReadOnlyView(T[] source)
        {
            _source = source;
            _startIndex = 0;
            _count = source.Length;
        }

        internal ReadOnlyView(string source)
        {
            if (typeof(T) != typeof(char))
                throw new InvalidOperationException();
            _source = source;
            _startIndex = 0;
            _count = source.Length;
        }

        private ReadOnlyView(object? source, int startIndex, int count)
        {
            _source = source;
            _startIndex = startIndex;
            _count = count;
        }

        public readonly int StartIndex => _startIndex;
        public readonly int Count => _count;
        public readonly bool IsEmpty => _source is null || _count <= 0;
        public readonly T this[int index]
        {
            get
            {
                object? source = _source;
                if (source is not null)
                {
                    if (source is T[] array)
                        return array[_startIndex + index];
                    if (typeof(T) == typeof(char) && source is string str)
                        return UnsafeHelper.As<char, T>(str[_startIndex + index]);
                }
                throw new IndexOutOfRangeException();
            }
        }

        public readonly ReadOnlyView<T> Slice(int startIndex)
        {
            if (startIndex < 0 || startIndex >= _count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex == 0)
                return this;
            return new ReadOnlyView<T>(_source, _startIndex + startIndex, _count - startIndex);
        }

        public readonly ReadOnlyView<T> Slice(int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if ((startIndex + count) >= _count)
                throw new ArgumentOutOfRangeException(startIndex >= _count ? nameof(startIndex) : nameof(count));
            if (startIndex == 0 && count == _count)
                return this;
            if (count == 0)
                return Empty;
            return new ReadOnlyView<T>(_source, _startIndex + startIndex, count);
        }

        public readonly IEnumerator<T> GetEnumerator()
        {
            object? source = _source;
            if (source is not null)
            {
                if (source is T[] array)
                    return EnumeratorHelper.CreateArrayEnumerator(array);
                if (typeof(T) == typeof(char) && source is string str)
                    return UnsafeHelper.As<IEnumerator<char>, IEnumerator<T>>(str.GetEnumerator());
            }
            return EnumeratorHelper.CreateEmptyEnumerator<T>();
        }

        public readonly ref readonly T GetPinnableReference()
        {
            object? source = _source;
            if (source is T[] array)
                return ref array[0];
            if (typeof(T) == typeof(char) && source is string str)
                return ref UnsafeHelper.As<char, T>(ref UnsafeHelper.AsRefIn(in CLRStringHelper.GetPinnableReference(str)));
            return ref UnsafeHelper.NullRef<T>();
        }

        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        readonly nuint IPinnableReference<T>.GetPinnedLength() => MathHelper.MakeUnsigned(Count);

        public override string? ToString()
        {
            if (typeof(T) == typeof(char))
            {
                object? source = _source;
                if (source is char[] array)
                    return new string(array, _startIndex, _count);
                else if (source is string str)
                    return str.Substring(_startIndex, _count);
                else
                    return string.Empty;
            }
            return base.ToString();
        }
    }
}
