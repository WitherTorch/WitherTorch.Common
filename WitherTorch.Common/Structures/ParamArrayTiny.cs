using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable CS8500

namespace WitherTorch.Common.Structures
{

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public readonly ref struct ParamArrayTiny<T>
    {
        public static readonly int ArrayOffset;

        private readonly int _length;
        private readonly T[]? _array;
        private readonly TinyArray _tinyArray;

        unsafe static ParamArrayTiny()
        {
            ParamArrayTiny<T> a = new ParamArrayTiny<T>();
            ArrayOffset = unchecked((int)((byte*)&a._tinyArray - (byte*)&a));
        }

        public ParamArrayTiny(T item)
        {
            _length = 1;
            _tinyArray = new TinyArray(item);
        }

        public ParamArrayTiny(T item1, T item2)
        {
            _length = 2;
            _tinyArray = new TinyArray(item1, item2);
        }

        public ParamArrayTiny(T item1, T item2, T item3)
        {
            _length = 3;
            _tinyArray = new TinyArray(item1, item2, item3);
        }

        public ParamArrayTiny(params T[] items)
        {
            int length = items.Length;
            _length = length;
            switch (length)
            {
                case 0:
                    break;
                case 1:
                    _tinyArray = new TinyArray(items[0]);
                    break;
                case 2:
                    _tinyArray = new TinyArray(items[0], items[1]);
                    break;
                case 3:
                    _tinyArray = new TinyArray(items[0], items[1], items[2]);
                    break;
                default:
                    _array = items;
                    break;
            }
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _length;
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int length = _length;
                if (index < 0 || index >= length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (length > 3)
                    return _array![index];
                return index switch
                {
                    0 => _tinyArray._item1!,
                    1 => _tinyArray._item2!,
                    2 => _tinyArray._item3!,
                    _ => throw new ArgumentOutOfRangeException(nameof(index)),
                };
            }
        }

        private readonly struct TinyArray
        {
            public readonly T? _item1, _item2, _item3;

            public TinyArray(T item1)
            {
                _item1 = item1;
                _item2 = default;
                _item3 = default;
            }

            public TinyArray(T item1, T item2)
            {
                _item1 = item1;
                _item2 = item2;
                _item3 = default;
            }

            public TinyArray(T item1, T item2, T item3)
            {
                _item1 = item1;
                _item2 = item2;
                _item3 = item3;
            }
        }

    }
}
