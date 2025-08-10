using InlineMethod;

namespace WitherTorch.Common.Text
{
    partial class InternalSequenceHelper
    {
        [Inline(InlineBehavior.Remove)]
        private static partial bool CheckTypeCanBeVectorized<T>() where T : unmanaged;

        public static unsafe int CompareTo<T>(T* ptrA, T* ptrB, nuint length) where T : unmanaged
        {
            if (CheckTypeCanBeVectorized<T>())
                return VectorizedCompareTo(ptrA, ptrB, length);
            if (typeof(T) == typeof(char))
                return VectorizedCompareTo((ushort*)ptrA, (ushort*)ptrB, length);
            return LegacyCompareTo(ptrA, ptrA + length, ptrB);
        }

        private static unsafe partial int VectorizedCompareTo<T>(T* ptrA, T* ptrB, nuint length) where T : unmanaged;
    }
}
