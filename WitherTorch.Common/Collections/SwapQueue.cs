using System.Collections.Generic;

using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Collections
{
    public sealed class SwapQueue<T>
    {
        private Queue<T> _frontQueue;
        private Queue<T> _backQueue;

        private ulong _version;

        public SwapQueue()
        {
            _frontQueue = new Queue<T>();
            _backQueue = new Queue<T>();
        }

        public SwapQueue(int capacity)
        {
            _frontQueue = new Queue<T>(capacity);
            _backQueue = new Queue<T>(capacity);
        }

        public SwapQueue(IEnumerable<T> collection)
        {
            _frontQueue = new Queue<T>(collection);
            _backQueue = new Queue<T>();
        }

        public Queue<T> Swap()
        {
            Queue<T> backQueue, frontQueue;
            OptimisticLock.Enter(ref _version, out ulong version);
            do
            {
                backQueue = _backQueue;
                frontQueue = _frontQueue;
            }
            while (!OptimisticLock.TryLeave(ref _version, ref version));

            _frontQueue = backQueue;
            _backQueue = frontQueue;
            _version++;
            return _frontQueue;
        }

        public Queue<T> Value
        {
            get
            {
                Queue<T> queue;
                OptimisticLock.Enter(ref _version, out ulong version);
                do
                {
                    queue = _frontQueue;
                }
                while (!OptimisticLock.TryLeave(ref _version, ref version));
                return queue;
            }
        }
    }
}
