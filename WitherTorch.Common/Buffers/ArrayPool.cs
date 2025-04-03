using InlineMethod;

using System.Runtime.CompilerServices;

using WitherTorch.Common.Collections;

namespace WitherTorch.Common.Buffers
{
    public abstract partial class ArrayPool<T> : IPool<T[]>
    {
        protected const int MinimumArraySize = 16;

        private static readonly bool _isPrimitiveType = typeof(T).IsPrimitive;

        public static ArrayPool<T> Shared => SharedArrayPool<T>.Instance;

        [Inline(InlineBehavior.Keep, export: true)]
        public T[] Rent() => Rent(MinimumArraySize);

        public abstract T[] Rent(int capacity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]  
        public void Return(T[] obj) => Return(obj, !_isPrimitiveType);

        public abstract void Return(T[] obj, bool clearArray);

        [Inline(InlineBehavior.Keep, export: true)]
        public FixedArrayList<T> RentList() => new FixedArrayList<T>(Rent(), initialCount: 0);

        [Inline(InlineBehavior.Keep, export: true)]
        public FixedArrayList<T> RentList(int capacity) => new FixedArrayList<T>(Rent(capacity), initialCount: 0);

        [Inline(InlineBehavior.Keep, export: true)]
        public void ReturnList(FixedArrayList<T> list) => Return(list.AsArray());

        [Inline(InlineBehavior.Keep, export: true)]
        public void ReturnList(FixedArrayList<T> list, bool clearArray) => Return(list.AsArray(), clearArray);
    }
}