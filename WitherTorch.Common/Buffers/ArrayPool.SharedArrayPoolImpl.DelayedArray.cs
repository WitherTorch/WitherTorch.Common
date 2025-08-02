using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

#if NET472_OR_GREATER
#endif

namespace WitherTorch.Common.Buffers
{
    partial class ArrayPool<T>
    {
        partial class SharedArrayPoolImpl
        {
            private sealed class DelayedArray : DelayedCollectingObject
            {
                private readonly uint _capacity;
                private T[]? _array;

                public T[] Array => NullSafetyHelper.ThrowIfNull(_array);

                public DelayedArray(uint capacity)
                {
                    _capacity = capacity;
                }

                protected override void DestroyObject()
                {
                    _array = null;
                }

                protected override void GenerateObject()
                {
                    _array = new T[_capacity];
                }
            }
        }
    }
}
