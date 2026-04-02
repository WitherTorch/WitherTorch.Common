namespace WitherTorch.Common.Native
{
    unsafe partial class NativeMemoryPool
    {
        private sealed class EmptyImpl : NativeMemoryPool
        {
            public static readonly EmptyImpl Instance = new EmptyImpl();

            protected override void* RentCore(ref nuint capacity) => NativeMethods.AllocMemory(capacity);
            protected override void ReturnCore(void* ptr, nuint length) => NativeMethods.FreeMemory(ptr);
        }
    }
}