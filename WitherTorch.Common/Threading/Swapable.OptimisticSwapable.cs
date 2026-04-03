namespace WitherTorch.Common.Threading
{
    partial class Swapable
    {
        private sealed class OptimisticSwapable<T> : Swapable<T> where T : class
        {
            private nuint _version;

            public OptimisticSwapable(T front, T back) : base(front, back) => _version = 0;

            protected override T GetValueCore(ref readonly T valueRef)
            {
                ref readonly nuint versionRef = ref _version;
                T result = OptimisticLock.EnterWithObject(in valueRef, in versionRef, out nuint currentVersion);
                while (!OptimisticLock.TryLeaveWithObject(in valueRef, in versionRef, ref result, ref currentVersion)) ;
                return result;
            }

            protected override T SwapCore(ref T front, ref T back)
            {
                ref nuint versionRef = ref _version;

                T frontObj, backObj;
                nuint currentVersion = OptimisticLock.Enter(ref versionRef);
                do
                {
                    frontObj = front;
                    backObj = back;
                } while (!OptimisticLock.TryLeave(ref versionRef, ref currentVersion));

                front = backObj;
                back = frontObj;
                OptimisticLock.Increase(ref versionRef);

                return frontObj;
            }
        }
    }
}
