namespace WitherTorch.Common.Threading
{
    partial class Swapable
    {
        private sealed class OptimisticSwapable<T> : Swapable<T> where T : class
        {
            private nuint _version;

            public OptimisticSwapable(T front, T back) : base(front, back) => _version = 0;

            protected override T GetValueCore(ref T valueRef)
            {
                T result;

                ref nuint versionRef = ref _version;
                OptimisticLock.Enter(ref versionRef, out nuint currentVersion);
                do
                {
                    result = valueRef;
                } while (!OptimisticLock.TryLeave(ref versionRef, ref currentVersion));

                return result;
            }

            protected override T SwapCore(ref T front, ref T back)
            {
                ref nuint versionRef = ref _version;

                T frontObj, backObj;
                OptimisticLock.Enter(ref versionRef, out nuint currentVersion);
                do
                {
                    frontObj = front;
                    backObj = back;
                } while (!OptimisticLock.TryLeave(ref versionRef, ref currentVersion));

                front = backObj;
                back = frontObj;
                versionRef++;

                return frontObj;
            }
        }
    }
}
