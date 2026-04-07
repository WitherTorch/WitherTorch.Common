using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class OtherEncodingString
    {
        private sealed class CharEnumerator : IEnumerator<char>
        {
            private readonly Decoder _decoder;
            private readonly byte[] _value;

            private nuint _index;
            private char _current;
            private bool _isInitialized;

            public CharEnumerator(Encoding encoding, byte[] value)
            {
                _decoder = encoding.GetDecoder();
                _value = value;
            }

            public char Current => _isInitialized ? _current : throw new InvalidOperationException();

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (!_isInitialized)
                    _index = 0;
                return TryGetNextCharacter(_decoder, _value, ref _index, out _current);
            }

            public void Reset() => _isInitialized = false;

            public void Dispose() { }

            private static unsafe bool TryGetNextCharacter(Decoder decoder, byte[] source, ref nuint index, out char result)
            {
                char val;
                fixed (byte* ptr = source)
                {
                    byte* iterator = ptr + index, ptrEnd = ptr + source.Length - 1;
                    while (!MathHelper.ToBooleanUnsafe(decoder.GetChars(iterator, 1, &val, 1, flush: false)))
                    {
                        if (++iterator >= ptrEnd)
                        {
                            result = default;
                            return false;
                        }
                    }
                    index = (nuint)(iterator - ptr);
                }
                result = val;
                return true;
            }
        }
    }
}
