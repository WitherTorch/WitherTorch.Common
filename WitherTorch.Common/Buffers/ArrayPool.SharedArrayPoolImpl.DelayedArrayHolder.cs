#if NET472_OR_GREATER
using WitherTorch.Common.Helpers;
using WitherTorch.Common.Native;

namespace WitherTorch.Common.Buffers
{
    partial class ArrayPool<T>
    {
        partial class SharedArrayPoolImpl
        {
            private sealed class DelayedArrayHolder : DelayedCollectingObject
            {
                private readonly uint _capacity;
                private T[]? _array;

                public T[] Array => NullSafetyHelper.ThrowIfNull(_array);

                public DelayedArrayHolder(uint capacity)
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
#endif