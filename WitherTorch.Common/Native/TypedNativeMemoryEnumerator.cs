using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WitherTorch.Common.Native
{
    [StructLayout(LayoutKind.Auto)]
    public unsafe struct TypedNativeMemoryEnumerator<T> : IEnumerator<T> where T : unmanaged
    {
        private readonly T* _nativePointer;
        private readonly nuint _length;

        private nuint _index;
        private bool _isStarted;

        public TypedNativeMemoryEnumerator(T* nativePointer, nuint length)
        {
            _nativePointer = nativePointer;
            _length = length;
            _isStarted = false;
        }

        public readonly T Current
        {
            get
            {
                nuint index = _index;
                if (!_isStarted || index >= _length)
                    throw new InvalidOperationException();
                return _nativePointer[index];
            }
        }

        readonly object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (!_isStarted)
            {
                _index = 0;
                return _length > 0;
            }
            nuint nextIndex = _index + 1;
            if (nextIndex >= _length)
                return false;
            _index = nextIndex;
            return true;
        }

        public void Reset() => _isStarted = false;

        public void Dispose() { }
    }
}
