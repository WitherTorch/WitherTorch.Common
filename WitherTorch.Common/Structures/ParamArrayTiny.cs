using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WitherTorch.Common.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct ParamArrayTiny<T>
    {
        private readonly T[]? _array;

        private readonly int _length;
        private readonly T? _item1, _item2, _item3;

        public ParamArrayTiny(T item)
        {
            _length = 1;
            _array = default;
            _item1 = item;
            _item2 = default;
            _item3 = default;
        }

        public ParamArrayTiny(T item1, T item2)
        {
            _length = 2;
            _array = default;
            _item1 = item1;
            _item2 = item2;
            _item3 = default;
        }

        public ParamArrayTiny(T item1, T item2, T item3)
        {
            _length = 3;
            _array = default;
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
        }

        public ParamArrayTiny(params T[] items)
        {
            int length = items.Length;
            _length = length;
            switch (length)
            {
                case 0:
                    _array = default;
                    _item1 = default;
                    _item2 = default;
                    _item3 = default;
                    break;
                case 1:
                    _array = default;
                    _item1 = items[0];
                    _item2 = default;
                    _item3 = default;
                    break;
                case 2:
                    _array = default;
                    _item1 = items[0];
                    _item2 = items[1];
                    _item3 = default;
                    break;
                case 3:
                    _array = default;
                    _item1 = items[0];
                    _item2 = items[1];
                    _item3 = items[2];
                    break;
                default:
                    _array = items;
                    _item1 = default;
                    _item2 = default;
                    _item3 = default;
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
                    0 => _item1!,
                    1 => _item2!,
                    2 => _item3!,
                    _ => throw new ArgumentOutOfRangeException(nameof(index)),
                };
            }
        }
    }
}
