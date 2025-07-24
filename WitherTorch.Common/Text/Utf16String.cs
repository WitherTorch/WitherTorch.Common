using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class Utf16String : StringBase, IWrapper<string>
    {
        private readonly string _value;

        public override StringType StringType => StringType.Utf16;

        public override int Length => _value.Length;

        public Utf16String(string value)
        {
            _value = value;
        }

        protected internal override unsafe char GetCharAt(nuint index)
        {
            fixed (char* ptr = _value)
                return ptr[index];
        }

        protected override bool IsFullyWhitespaced()
        {
            foreach (char character in _value)
            {
                if (!char.IsWhiteSpace(character))
                    return false;
            }
            return true;
        }

        protected override unsafe void CopyToCore(char* destination, nuint startIndex, nuint count)
        {
            fixed (char* ptr = _value)
                UnsafeHelper.CopyBlockUnaligned(destination, ptr, unchecked((uint)count * sizeof(char)));
        }

        public override IEnumerator<char> GetEnumerator() => _value.GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal string GetInternalRepresentation() => _value;

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToCLRString() => _value;

        public override string ToString() => _value;

        string IWrapper<string>.Unwrap() => _value;
    }
}
