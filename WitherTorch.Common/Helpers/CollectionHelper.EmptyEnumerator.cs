using System;
using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Helpers
{
    partial class CollectionHelper
    {
        private sealed class EmptyEnumeratorImpl<T> : IEnumerator<T>
        {
            public static readonly EmptyEnumeratorImpl<T> Instance = new EmptyEnumeratorImpl<T>();

            public T Current => throw new InvalidOperationException();

            object IEnumerator.Current => throw new InvalidOperationException();

            public void Dispose() { }

            public bool MoveNext() => false;

            public void Reset() { }
        }

    }
}
