namespace WitherTorch.Common.Buffers
{
    partial class ArrayPool<T>
    {
        private sealed class EmptyArrayPoolImpl : ArrayPool<T>
        {
            public static readonly EmptyArrayPoolImpl Instance = new EmptyArrayPoolImpl();

            private EmptyArrayPoolImpl() { }

            protected override T[] RentCore(nuint capacity) => new T[capacity];

            protected override void ReturnCore(T[] array) { }
        }
    }
}