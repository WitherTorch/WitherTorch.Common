using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Extensions;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    internal sealed partial class Latin1String : AsciiLikeString, IPinnableReference<byte>
    {
        public override StringType StringType => StringType.Latin1;

        internal Latin1String(byte[] value) : base(value) { }

        protected override byte GetCharacterLimit() => Latin1EncodingHelper.Latin1EncodingLimit_InByte;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Latin1String Allocate(nuint length, out byte[] buffer)
        {
            if (length > MaxStringLength)
                throw new OutOfMemoryException();

            return new Latin1String(buffer = new byte[length + 1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringWrapper Create(byte* source, nuint length)
        {
            if (length >= MaxStringLength)
                throw new OutOfMemoryException();

            StringWrapper result;
            byte[] buffer;
            if (!SequenceHelper.ContainsGreaterThan(source, length, AsciiEncodingHelper.AsciiEncodingLimit_InByte))
                result = AsciiString.Allocate(length, out buffer);
            else
                result = Allocate(length, out buffer);
            fixed (byte* destination = buffer)
                UnsafeHelper.CopyBlockUnaligned(destination, source, length * sizeof(byte));
            return result;
        }

        public static unsafe bool TryCreate(char* source, nuint length, StringCreateOptions options, [NotNullWhen(true)] out StringWrapper? result)
        {
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

        public override StringWrapper ToStringWrapper(StringCreateOptions options)
        {
            if (!options.HasFlagFast(StringCreateOptions._Force_Flag))
                return this;

            if (options.HasFlagFast(StringCreateOptions.UseAsciiCompression))
                return ToStringWrapper_Ascii();
            if (options.HasFlagFast(StringCreateOptions.UseUtf8Compression))
                return ToStringWrapper_Utf8();
            if (options.HasFlagFast(StringCreateOptions.UseUtf16Compression))
                return CreateUtf16String(ToString());

            return this;
        }

        public override StringWrapper ToStringWrapper(StringType type)
            => type switch
            {
                StringType.Empty => Empty,
                StringType.Ascii => ToStringWrapper_Ascii(),
                StringType.Latin1 => this,
                StringType.Utf8 => ToStringWrapper_Utf8(),
                StringType.Utf16 => CreateUtf16String(ToString()),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

        private unsafe AsciiString ToStringWrapper_Ascii()
        {
            nuint length = MathHelper.MakeUnsigned(_length);
            AsciiString result = AsciiString.Allocate(length, out byte[] buffer);
            fixed (byte* source = _value, destination = buffer)
            {
                UnsafeHelper.CopyBlockUnaligned(destination, source, length * sizeof(byte));
                SequenceHelper.ReplaceGreaterThan(destination, length, AsciiEncodingHelper.AsciiEncodingLimit_InByte, (byte)'?');
            }
            return result;
        }

        private unsafe StringWrapper ToStringWrapper_Utf8()
        {
            int signedLength = _length;
            nuint length = MathHelper.MakeUnsigned(signedLength);
            fixed (byte* source = _value)
            {
                nuint count = SequenceHelper.CountOfGreaterThan(source, length, AsciiEncodingHelper.AsciiEncodingLimit_InByte);
                if (count == 0)
                {
                    AsciiString result = AsciiString.Allocate(length, out byte[] buffer);
                    fixed (byte* destination = buffer)
                        UnsafeHelper.CopyBlockUnaligned(destination, source, length * sizeof(byte));
                    return result;
                }
                else
                {
                    nuint bufferLength = length + count;
                    Utf8String result = Utf8String.Allocate(bufferLength, signedLength, out byte[] buffer);
                    fixed (byte* destination = buffer)
                    {
                        byte* iterator = destination, destinationEnd = destination + bufferLength;
                        for (nuint i = 0; i < length; i++)
                        {
                            byte c = source[i];
                            iterator = Utf8EncodingHelper.TryWriteUtf8Character(iterator, destinationEnd, c);
                        }
                    }
                    return result;
                }
            }
        }
    }
}
