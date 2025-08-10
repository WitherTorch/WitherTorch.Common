using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class Latin1String : AsciiLikeString, IPinnableReference<byte>,
        IHolder<ArraySegment<byte>>
    {
        public override StringType StringType => StringType.Latin1;

        private Latin1String(byte[] value) : base(value) { }

        protected override byte GetCharacterLimit() => Latin1EncodingHelper.Latin1EncodingLimit_InByte;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Latin1String Allocate(nuint length, out byte[] buffer)
        {
            if (length > MaxStringLength)
                throw new OutOfMemoryException();

            return new Latin1String(buffer = new byte[length + 1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Latin1String Create(byte* source)
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
            return new Latin1String(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Latin1String Create(byte* source, nuint length)
        {
            if (length >= MaxStringLength)
                throw new OutOfMemoryException();

            byte[] buffer = new byte[length + 1];
            fixed (byte* ptr = buffer)
                UnsafeHelper.CopyBlockUnaligned(ptr, source, length * sizeof(byte));
            return new Latin1String(buffer);
        }

        public static unsafe bool TryCreate(char* source, nuint length, StringCreateOptions options, [NotNullWhen(true)] out StringBase? result)
        {
            if (length > MaxStringLength)
                goto Failed;

            if ((options & StringCreateOptions.UseAsciiCompression) != StringCreateOptions.UseAsciiCompression && 
                !SequenceHelper.ContainsGreaterThan(source, length, AsciiEncodingHelper.AsciiEncodingLimit))
            {
                result = AsciiString.Allocate(length, out byte[] buffer);
                fixed (byte* desination = buffer)
                    AsciiEncodingHelper.ReadFromUtf16BufferCore(source, desination, length);
                return true;
            }
            if ((options & StringCreateOptions._Force_Flag) == StringCreateOptions._Force_Flag)
            {
                byte[] buffer = CreateLatin1StringCore_OutOfLatin1Range(source, length);
                result = new Latin1String(buffer);
                return true;
            }
            if (!SequenceHelper.ContainsGreaterThan(source, length, Latin1EncodingHelper.Latin1EncodingLimit))
            {
                byte[] buffer = CreateLatin1StringCore(source, length);
                result = new Latin1String(buffer);
                return true;
            }

        Failed:
            result = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe byte[] CreateLatin1StringCore_OutOfLatin1Range(char* source, nuint length)
        {
            byte[] buffer = new byte[length + 1];
            fixed (byte* destination = buffer)
                Latin1EncodingHelper.ReadFromUtf16BufferCore_OutOfLatin1Range(source, destination, length);
            return buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe byte[] CreateLatin1StringCore(char* source, nuint length)
        {
            byte[] buffer = new byte[length + 1];
            fixed (byte* destination = buffer)
                Latin1EncodingHelper.ReadFromUtf16BufferCore(source, destination, length);
            return buffer;
        }
    }
}
