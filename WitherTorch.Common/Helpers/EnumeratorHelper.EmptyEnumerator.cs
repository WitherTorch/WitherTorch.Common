using System;
using System.Collections;
using System.Collections.Generic;

namespace WitherTorch.Common.Helpers
{
    partial class EnumeratorHelper
    {
        private sealed class EmptyEnumerator<T> : IEnumerator<T>
        {
            public EmptyEnumerator() { }

            public T Current => throw new InvalidOperationException();

            object IEnumerator.Current => throw new InvalidOperationException();

            public void Dispose() { }

            public bool MoveNext() => false;

            public void Reset() { }
        }
    }
}
