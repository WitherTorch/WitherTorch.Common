using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class Latin1String : StringBase, IWrapper<byte[]>
    {
        private const int MaxLatin1StringLength = Limits.MaxArrayLength - 1;

        private readonly byte[] _value;
        private readonly int _length;

        public override StringType StringType => StringType.Latin1;

        public override int Length => _length;

        public Latin1String(byte[] value)
        {
            _value = value;
            _length = value.Length - 1;
        }

        public static unsafe bool TryCreate(char* source, [NotNullWhen(true)] out Latin1String? result)
        {
            nuint length = 0;
            do
            {
                char c = source[length];
                if (c == '\0')
                    break;
                if (c > InternalStringHelper.Latin1StringLimit || ++length >= MaxLatin1StringLength)
                    goto Failed;
            } while (true);

            byte[] buffer = new byte[length + 1];
            fixed (byte* dest = buffer)
                InternalStringHelper.NarrowAndCopyTo(source, length, dest);
            result = new Latin1String(buffer);
            return true;

        Failed:
            result = null;
            return false;
        }

        public static unsafe bool TryCreate(char* source, nuint length, [NotNullWhen(true)] out Latin1String? result)
        {
            if (SequenceHelper.ContainsGreaterThan(source, length, InternalStringHelper.Latin1StringLimit) || length > MaxLatin1StringLength)
                goto Failed;

            byte[] buffer = new byte[length + 1];
            fixed (byte* dest = buffer)
                InternalStringHelper.NarrowAndCopyTo(source, length, dest);
            result = new Latin1String(buffer);
            return true;

        Failed:
            result = null;
            return false;
        }

        protected internal override unsafe char GetCharAt(nuint index)
        {
            fixed (byte* ptr = _value)
                return unchecked((char)ptr[index]);
        }

        protected override bool IsFullyWhitespaced()
        {
            byte[] value = _value;
            for (int i = 0, length = _length; i < length; i++)
            {
                if (!IsWhiteSpace(value[i]))
                    return false;
            }
            return true;
        }

        [Inline(InlineBehavior.Remove)]
        private static bool IsWhiteSpace(byte value) => value switch
        {
            (byte)'\t' or (byte)'\n' or (byte)'\v' or (byte)'\f' or (byte)'\r' or (byte)' ' or
            (byte)'\u00a0' or (byte)'\u0085' => true,
            _ => false,
        };

        protected internal override unsafe void CopyToCore(char* destination, nuint startIndex, nuint count)
        {
            fixed (byte* ptr = _value)
                InternalStringHelper.WidenAndCopyTo(ptr + startIndex, count, destination);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal byte[] GetInternalRepresentation() => _value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe string ToCLRStringCore()
        {
            byte[] value = _value;
            int length = _length;
            if (length <= 0)
                return string.Empty;
            string result = StringHelper.AllocateRawString(length);
            fixed (byte* source = value)
            fixed (char* dest = result)
                InternalStringHelper.WidenAndCopyTo(source, unchecked((nuint)length), dest);
            return result;
        }

        public override unsafe string ToCLRString() => ToCLRStringCore();

        public override string ToString() => ToCLRStringCore();

        byte[] IWrapper<byte[]>.Unwrap() => _value;
    }
}
