namespace WitherTorch.Common.Buffers
{
    partial class ArrayPool<T>
    {
        private sealed class EmptyImpl : ArrayPool<T>
        {
            public static readonly EmptyImpl Instance = new EmptyImpl();

            private EmptyImpl() { }

            protected override T[] RentCore(nuint capacity) => new T[capacity];

            protected override void ReturnCore(T[] array) { }
        }
    }
}