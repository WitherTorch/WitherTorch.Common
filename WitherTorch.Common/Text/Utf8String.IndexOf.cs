using System.Runtime.CompilerServices;

using LocalsInit;

using WitherTorch.Common.Buffers;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    partial class Utf8String
    {
        [LocalsInit(false)]
        protected override unsafe bool ContainsCore(char value, nuint startIndex, nuint count)
        {
            if (startIndex > 0 || count < unchecked((nuint)_length)) // Very slow route
                return IndexOfCore(value, startIndex, count) >= 0;

            if (value <= Latin1EncodingHelper.AsciiEncodingLimit) // Very fast route
                return ContainsCoreFast(unchecked((byte)value));

            // Fast route
            byte* buffer = stackalloc byte[3];
            byte* bufferEnd = Utf8EncodingHelper.TryWriteUtf8Character(buffer, buffer + 3, value);
            return ContainsCoreFast(buffer, unchecked((nuint)(bufferEnd - buffer)));
        }

        protected override unsafe bool ContainsCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (valueLength == 1)
                return ContainsCore(value[0], startIndex, count);

            if (startIndex > 0 || count < unchecked((nuint)_length)) // Very slow route
                return IndexOfCore(value, valueLength, startIndex, count) >= 0;

            fixed (char* ptr = value)
            {
                if (SequenceHelper.ContainsGreaterThan(value, Latin1EncodingHelper.AsciiEncodingLimit)) // Fast route
                    return ContainsCoreFast(ptr, valueLength);

                return ContainsCoreVeryFast(ptr, valueLength); // Very fast route
            }
        }

        protected override unsafe bool ContainsCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (valueLength == 1)
                return ContainsCore(value.GetCharAt(0), startIndex, count);

            if (startIndex > 0 || count < unchecked((nuint)_length)) // Very slow route
                return IndexOfCore(value, valueLength, startIndex, count) >= 0;

            return value switch
            {
                Utf8String utf8 => ContainsCoreFast(utf8, valueLength),
                Utf16String utf16 => ContainsCoreFast(utf16, valueLength),
                Latin1String latin1 => ContainsCoreFast(latin1, valueLength),
                _ => ContainsCoreFast(value, valueLength),
            };
        }

        protected override unsafe int IndexOfCore(char value, nuint startIndex, nuint count)
        {
            using CharEnumerator enumerator = new CharEnumerator(_value);
            for (nuint i = 0; i < startIndex; i++)
            {
                if (!enumerator.MoveNext())
                    goto Failed;
            }
            for (nuint i = 0; i < count; i++)
            {
                if (!enumerator.MoveNext())
                    goto Failed;
                if (enumerator.Current == value)
                    return unchecked((int)(startIndex + i));
            }

        Failed:
            return -1;
        }

        protected override unsafe int IndexOfCore(string value, nuint valueLength, nuint startIndex, nuint count)
        {
            char valueHead = value[0];

            if (valueLength == 1)
                return IndexOfCore(valueHead, startIndex, count);

            fixed (char* ptr = value)
            {
                using CharEnumerator enumerator = new CharEnumerator(_value);
                for (nuint i = 0; i < startIndex; i++)
                {
                    if (!enumerator.MoveNext())
                        goto Failed;
                }

                count = count - valueLength + 1;
                for (nuint i = 0; i < count; i++)
                {
                    if (!enumerator.MoveNext())
                        goto Failed;
                    if (enumerator.Current == valueHead)
                    {
                        bool flag = true;
                        using CharEnumerator loopEnumerator = new CharEnumerator(enumerator);
                        for (nuint j = 1; j < valueLength; j++)
                        {
                            if (!loopEnumerator.MoveNext())
                                goto Failed;
                            if (loopEnumerator.Current != ptr[j])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                            return unchecked((int)(startIndex + i));
                    }
                }

            Failed:
                return -1;
            }
        }

        protected override int IndexOfCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
        {
            if (valueLength == 1)
                return IndexOfCore(value.GetCharAt(0), startIndex, count);

            return value switch
            {
                Utf8String utf8 => IndexOfCoreUtf8(utf8, valueLength, startIndex, count),
                Utf16String utf16 => IndexOfCore(utf16.GetInternalRepresentation(), valueLength, startIndex, count),
                Latin1String latin1 => IndexOfCoreLatin1(latin1, valueLength, startIndex, count),
                _ => base.IndexOfCore(value, valueLength, startIndex, count)
            };
        }

        private unsafe int IndexOfCoreUtf8(Utf8String value, nuint valueLength, nuint startIndex, nuint count)
        {
            using CharEnumerator enumeratorSource = new CharEnumerator(_value);
            for (nuint i = 0; i < startIndex; i++)
            {
                if (!enumeratorSource.MoveNext())
                    return -1;
            }

            count = count - valueLength + 1;
            using CharEnumerator enumeratorValue = new CharEnumerator(value._value);
            if (!enumeratorValue.MoveNext())
                return (int)startIndex; // 空字串永遠在最開頭
            char valueHead = enumeratorValue.Current;
            for (nuint i = 0; i < count; i++)
            {
                if (!enumeratorSource.MoveNext())
                    return -1;

                if (enumeratorSource.Current == valueHead)
                {
                    bool flag = true;
                    using CharEnumerator loopEnumeratorSource = new CharEnumerator(enumeratorSource);
                    using CharEnumerator loopEnumeratorValue = new CharEnumerator(enumeratorValue);
                    for (nuint j = 1; j < valueLength; j++)
                    {
                        if (!loopEnumeratorSource.MoveNext())
                            return -1;
                        if (!loopEnumeratorValue.MoveNext())
                            break;
                        if (loopEnumeratorSource.Current != loopEnumeratorValue.Current)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                        return unchecked((int)(startIndex + i));
                }
            }

            return -1;
        }

        private unsafe int IndexOfCoreLatin1(Latin1String value, nuint valueLength, nuint startIndex, nuint count)
        {
            using CharEnumerator enumerator = new CharEnumerator(_value);
            for (nuint i = 0; i < startIndex; i++)
            {
                if (!enumerator.MoveNext())
                    return -1;
            }

            count = count - valueLength + 1;
            fixed (byte* ptrValue = value.GetInternalRepresentation())
            {
                byte valueHead = ptrValue[0];
                for (nuint i = 0; i < count; i++)
                {
                    if (!enumerator.MoveNext())
                        return -1;

                    if (enumerator.Current == valueHead)
                    {
                        bool flag = true;
                        using CharEnumerator loopEnumerator = new CharEnumerator(enumerator);
                        for (nuint j = 1; j < valueLength; j++)
                        {
                            if (!loopEnumerator.MoveNext())
                                return -1;
                            if (loopEnumerator.Current != ptrValue[j])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                            return unchecked((int)(startIndex + i));
                    }
                }
            }

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool ContainsCoreFast(Utf8String str, nuint valueLength)
        {
            byte[] anotherSource = str._value;
            fixed (byte* ptr = anotherSource)
                return ContainsCoreFast(ptr, MathHelper.MakeUnsigned(anotherSource.Length - 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool ContainsCoreFast(Utf16String str, nuint valueLength)
        {
            fixed (char* ptr = str.GetInternalRepresentation())
                return ContainsCoreFast(ptr, valueLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool ContainsCoreFast(Latin1String str, nuint valueLength)
        {
            fixed (byte* ptr = str.GetInternalRepresentation())
            {
                if (SequenceHelper.ContainsExclude(ptr, valueLength, Latin1EncodingHelper.AsciiEncodingLimit_InByte))
                    return ContainsCoreFast((StringBase)str, valueLength);

                return ContainsCoreFast(ptr, valueLength);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool ContainsCoreFast(StringBase str, nuint valueLength)
        {
            ArrayPool<char> pool = ArrayPool<char>.Shared;
            char[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (char* temp = buffer)
                {
                    str.CopyToCore(temp, 0, valueLength);
                    return ContainsCoreFast(temp, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool ContainsCoreFast(byte value)
        {
            byte[] source = _value;
            fixed (byte* ptr = source)
                return SequenceHelper.Contains(ptr, MathHelper.MakeUnsigned(source.Length - 1), value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool ContainsCoreFast(byte* value, nuint valueLength)
        {
            DebugHelper.ThrowIf(valueLength == 0);
            byte[] source = _value;
            fixed (byte* ptr = source)
                return InternalSequenceHelper.PointerIndexOf(ptr, MathHelper.MakeUnsigned(source.Length - 1), value, valueLength) != null;
        }

        private unsafe bool ContainsCoreVeryFast(char* value, nuint valueLength)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            byte[] buffer = pool.Rent(valueLength);
            try
            {
                fixed (byte* temp = buffer)
                {
                    Latin1EncodingHelper.ReadFromUtf16BufferCore(value, temp, valueLength);
                    return ContainsCoreFast(temp, valueLength);
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }

        private unsafe bool ContainsCoreFast(char* value, nuint valueLength)
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Shared;
            nuint bufferLength = Utf8EncodingHelper.GetWorstCaseForEncodeLength(valueLength);
            byte[] buffer = pool.Rent(bufferLength);
            try
            {
                fixed (byte* temp = buffer)
                {
                    byte* tempEnd = Utf8EncodingHelper.TryReadFromUtf16BufferCore(value, value + valueLength, temp, temp + bufferLength);
                    return ContainsCoreFast(temp, unchecked((nuint)(tempEnd - temp)));
                }
            }
            finally
            {
                pool.Return(buffer);
            }
        }
    }
}
