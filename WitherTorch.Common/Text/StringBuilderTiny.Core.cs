using System.Runtime.CompilerServices;
using System.Text;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Text
{
    unsafe partial struct StringBuilderTiny
    {
        [Inline(InlineBehavior.Remove)]
        private void AppendCore(char* ptr, [InlineParameter] int count)
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
            {
                builder.GetObject().Append(ptr, count);
                return;
            }
            char* iterator = _iterator;
            char* endIterator = iterator + count - 1;
            char* end = _end;
            if (endIterator < end)
            {
                UnsafeHelper.CopyBlockUnaligned(iterator, ptr, unchecked((uint)(sizeof(char) * count)));
                _iterator = endIterator + 1;
                return;
            }
            GetOrRentAlternateStringBuilder(count).Append(ptr, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AppendCore(StringBase str, nuint startIndex, nuint count)
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
            {
                AppendCore(builder.GetObject(), str, startIndex, count);
                return;
            }
            char* iterator = _iterator;
            char* endIterator = iterator + count - 1;
            char* end = _end;
            if (endIterator < end)
            {
                str.CopyToCore(iterator, startIndex, count);
                _iterator = endIterator + 1;
                return;
            }
            AppendCore(GetOrRentAlternateStringBuilder(unchecked((int)count)), str, startIndex, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AppendCore(StringBuilder builder, StringBase str, nuint startIndex, nuint count)
        {
            if (str.StringType == StringType.Utf16)
                builder.Append(str.ToString(), (int)startIndex, (int)count);
            else
                builder.Append(str.SubstringCore(startIndex, count).ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private StringBuilder GetOrRentAlternateStringBuilder(int extraLength)
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is null)
                return RentAlternateStringBuilder(extraLength);
            return builder.GetObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private StringBuilder RentAlternateStringBuilder(int extraLength)
        {
            char* start = _start;
            char* iterator = _iterator;
            int size = unchecked((int)(iterator - start));
            DelayedCollectingStringBuilder builder = StringBuilderPool.Shared.Rent();
            _builder = builder;
            StringBuilder result = builder.GetObject();
            result.EnsureCapacity(size + extraLength);
            result.Append(start, size);
            return result;
        }

        [Inline(InlineBehavior.Remove)]
        private void ReturnStringBuilder()
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is null)
                return;
            _builder = null;
            StringBuilderPool.Shared.Return(builder);
        }
    }
}
