using System;

namespace WitherTorch.Common.Buffers
{
    partial class ArrayPool<T>
    {
        private sealed class EmptyArrayPoolImpl : ArrayPool<T>
        {
            public static readonly EmptyArrayPoolImpl Instance = new EmptyArrayPoolImpl();

            private EmptyArrayPoolImpl() { }

            public override T[] Rent(nuint capacity) => new T[capacity];

            public override void Return(T[] obj, bool clearArray)
            {
                if (clearArray)
                    Array.Clear(obj, 0, obj.Length);
            }
        }
    }
}