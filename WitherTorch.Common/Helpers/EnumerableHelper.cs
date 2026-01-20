using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Helpers
{
    public static class EnumerableHelper
    {
        public static IEnumerable<T> Wrap<T>(IEnumerator<T> enumerator)
            => new EnumeratorWrapper<T>(enumerator);

        private sealed class EnumeratorWrapper<T> : IEnumerable<T>
        {
            private readonly IEnumerator<T> _enumerator;

            public EnumeratorWrapper(IEnumerator<T> enumerator) => _enumerator = enumerator;

            public IEnumerator<T> GetEnumerator() => _enumerator;

            IEnumerator IEnumerable.GetEnumerator() => _enumerator;
        }
    }
}
