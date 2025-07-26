namespace WitherTorch.Common.Text
{
    partial class Utf8String
    {
        protected override bool ContainsCore(char value, nuint startIndex, nuint count)
            => IndexOfCore(value, startIndex, count) >= 0;

        protected override bool ContainsCore(string value, nuint valueLength, nuint startIndex, nuint count)
            => IndexOfCore(value, valueLength, startIndex, count) >= 0;

        protected override bool ContainsCore(StringBase value, nuint valueLength, nuint startIndex, nuint count)
            => IndexOfCore(value, valueLength, startIndex, count) >= 0;

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
    }
}
