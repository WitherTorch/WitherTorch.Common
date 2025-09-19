namespace WitherTorch.Common.Threading
{
    partial class Swapable
    {
        private sealed class PessimisticSwapable<T> : Swapable<T> where T : class
        {
            private readonly object _syncLock;

            public PessimisticSwapable(T front, T back) : base(front, back) => _syncLock = new object();

            protected override T GetValueCore(ref T valueRef)
            {
                lock (_syncLock)
                    return valueRef;
            }

            protected override T SwapCore(ref T front, ref T back)
            {
                T result;
                lock (_syncLock)
                {
                    result = front;
                    front = back;
                    back = result;
                }
                return result;
            }
        }
    }
}
