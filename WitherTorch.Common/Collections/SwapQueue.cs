using System.Collections.Generic;
using System.Threading;

using WitherTorch.Common.Helpers;
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
            OptimisticLock.Enter(ref _version, out ulong version);
            Queue<T> queue;
            do
            {
                queue = _frontQueue;
                (_frontQueue, _backQueue) = (_backQueue, queue);
            }
            while (!OptimisticLock.TryLeave(ref _version, ref version));
            return queue;
        }

        public Queue<T> Value => InterlockedHelper.Read(ref _frontQueue);
    }
}
