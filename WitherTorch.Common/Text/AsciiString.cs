using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class AsciiString : AsciiLikeString, IPinnableReference<byte>,
        IHolder<ArraySegment<byte>>
    {
        public override StringType StringType => StringType.Ascii;

        private AsciiString(byte[] value) : base(value) { }

        protected override byte GetCharacterLimit() => AsciiEncodingHelper.AsciiEncodingLimit_InByte;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsciiString Allocate(nuint length, out byte[] buffer)
        {
            if (length > MaxStringLength)
                throw new OutOfMemoryException();

            return new AsciiString(buffer = new byte[length + 1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe AsciiString Create(byte* source)
        {
            nuint length = 0;
            do
            {
                byte c = source[length];
                if (c == 0)
                    break;
                if (++length > MaxStringLength)
                    throw new OutOfMemoryException();
            } while (true);

            byte[] buffer = new byte[length]; // Tail zero is included
            fixed (byte* ptr = buffer)
                UnsafeHelper.CopyBlockUnaligned(ptr, source, length * sizeof(byte));
            return new AsciiString(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe AsciiString Create(byte* source, nuint length)
        {
            if (length >= MaxStringLength)
                throw new OutOfMemoryException();

            byte[] buffer = new byte[length + 1];
            fixed (byte* ptr = buffer)
                UnsafeHelper.CopyBlockUnaligned(ptr, source, length * sizeof(byte));
            return new AsciiString(buffer);
        }

        public static unsafe bool TryCreate(char* source, nuint length, StringCreateOptions options, [NotNullWhen(true)] out AsciiString? result)
        {
            if (length > MaxStringLength)
                goto Failed;

            byte[] buffer;
            if ((options & StringCreateOptions._Force_Flag) == StringCreateOptions._Force_Flag)
            {
                buffer = CreateAsciiStringCore_OutOfAsciiRange(source, length);
            }
            else if (SequenceHelper.ContainsGreaterThan(source, length, AsciiEncodingHelper.AsciiEncodingLimit))
                goto Failed;
            else
            {
                buffer = CreateAsciiStringCore(source, length);
            }

            result = new AsciiString(buffer);
            return true;

        Failed:
            result = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe byte[] CreateAsciiStringCore_OutOfAsciiRange(char* source, nuint length)
        {
            byte[] buffer = new byte[length + 1];
            fixed (byte* destination = buffer)
                AsciiEncodingHelper.ReadFromUtf16BufferCore_OutOfAsciiRange(source, destination, length);
            return buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe byte[] CreateAsciiStringCore(char* source, nuint length)
        {
            byte[] buffer = new byte[length + 1];
            fixed (byte* destination = buffer)
                AsciiEncodingHelper.ReadFromUtf16BufferCore(source, destination, length);
            return buffer;
        }
    }
}
