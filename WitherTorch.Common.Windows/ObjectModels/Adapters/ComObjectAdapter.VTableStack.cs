using System;

namespace WitherTorch.Common.Windows.ObjectModels.Adapters
{
    partial class ComObjectAdapter
    {
        protected unsafe ref struct VTableStack
        {
            private readonly void** _ptr;
            private readonly int _length;

            private int _index;

            public VTableStack(void** ptr, int length)
            {
                _ptr = ptr;
                _length = length;
                _index = 0;
            }

            public readonly void* this[int index]
            {
                get => index >= 0 && index < _length ? _ptr[index] : throw new IndexOutOfRangeException();
                set
                {
                    if (index < 0 || index >= _length)
                        throw new IndexOutOfRangeException();
                    _ptr[index] = value;
                }
            }

            public void Push(void* value)
            {
                int index = _index;
                if (index >= _length)
                    throw new InvalidOperationException("VTable stack overflow");
                _ptr[index++] = value;
                _index = index;
            }
        }
    }
}
