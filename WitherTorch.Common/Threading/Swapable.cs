using System.Collections.Generic;

namespace WitherTorch.Common.Threading
{
    public static partial class Swapable
    {
        public static Swapable<T> Create<T>(bool optimistic = false) where T : class, new()
            => Create(new T(), new T(), optimistic);

        public static Swapable<T> Create<T>(T front, T back, bool optimistic = false) where T : class
            => optimistic ? new OptimisticSwapable<T>(front, back) : new PessimisticSwapable<T>(front, back);

        public static Swapable<List<T>> CreateList<T>(bool optimistic = false)
            => Create(new List<T>(), new List<T>(), optimistic);

        public static Swapable<List<T>> CreateList<T>(int capacity, bool optimistic = false)
        {
            List<T> front = new List<T>(capacity);
            List<T> back = new List<T>();
            return Create(front, back, optimistic);
        }

        public static Swapable<List<T>> CreateList<T>(IEnumerable<T> collection, bool optimistic = false)
        {
            List<T> front = new List<T>(collection);
            List<T> back = new List<T>();
            return Create(front, back, optimistic);
        }

        public static Swapable<Queue<T>> CreateQueue<T>(bool optimistic = false)
            => Create(new Queue<T>(), new Queue<T>(), optimistic);

        public static Swapable<Queue<T>> CreateQueue<T>(int capacity, bool optimistic = false)
        {
            Queue<T> front = new Queue<T>(capacity);
            Queue<T> back = new Queue<T>();
            return Create(front, back, optimistic);
        }

        public static Swapable<Queue<T>> CreateQueue<T>(IEnumerable<T> collection, bool optimistic = false)
        {
            Queue<T> front = new Queue<T>(collection);
            Queue<T> back = new Queue<T>();
            return Create(front, back, optimistic);
        }

        public static Swapable<Stack<T>> CreateStack<T>(bool optimistic = false)
            => Create(new Stack<T>(), new Stack<T>(), optimistic);

        public static Swapable<Stack<T>> CreateStack<T>(int capacity, bool optimistic = false)
        {
            Stack<T> front = new Stack<T>(capacity);
            Stack<T> back = new Stack<T>();
            return Create(front, back, optimistic);
        }

        public static Swapable<Stack<T>> CreateStack<T>(IEnumerable<T> collection, bool optimistic = false)
        {
            Stack<T> front = new Stack<T>(collection);
            Stack<T> back = new Stack<T>();
            return Create(front, back, optimistic);
        }
    }

    public abstract class Swapable<T> where T : class
    {
        private T _front, _back;

        protected Swapable(T front, T back)
        {
            _front = front;
            _back = back;
        }

        public T Value => GetValueCore(ref _front);

        public T Swap() => SwapCore(ref _front, ref _back);

        protected abstract T GetValueCore(ref T valueRef);

        protected abstract T SwapCore(ref T front, ref T back);
    }
}
