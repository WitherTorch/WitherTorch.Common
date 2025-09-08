using System;
using System.Runtime.CompilerServices;
using System.Text;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Structures;

#pragma warning disable CS8500

namespace WitherTorch.Common.Text
{
    unsafe partial struct StringBuilderTiny
    {
        [Inline(InlineBehavior.Remove)]
        private void AppendCore(char* ptr, [InlineParameter] int count)
        {
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
            {
                builder.Append(ptr, count);
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
            GetAlternateStringBuilder(count).Append(ptr, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AppendCore(StringBase str, nuint startIndex, nuint count)
        {
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is not null)
            {
                AppendCore(builder, str, startIndex, count);
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
            AppendCore(GetAlternateStringBuilder(unchecked((int)count)), str, startIndex, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AppendCore(StringBuilder builder, StringBase str, nuint startIndex, nuint count)
        {
            if (str.StringType == StringType.Utf16)
                builder.Append(str.ToString(), (int)startIndex, (int)count);
            else
                builder.Append(str.SubstringCore(startIndex, count).ToString());
        }

        private void AppendFormatCore<T>(string format, in ParamArrayTiny<T> args)
        {
            int length = format.Length;
            int lastPos = 0, indexOfLeft, indexOfRight;
            do
            {
                indexOfLeft = StringHelper.IndexOf(format, '{', lastPos);
                if (indexOfLeft < 0)
                    break;
                indexOfRight = StringHelper.IndexOf(format, '}', indexOfLeft);
                if (indexOfRight < 0)
                    break;
                Append(format, lastPos, indexOfLeft - lastPos);
                int count = indexOfRight - indexOfLeft - 1;
                int delimiterIndex = StringHelper.IndexOf(format, ':', indexOfLeft, count);
                if (delimiterIndex < 0)
                    Append(args[ParseHelper.ParseToInt32(format, indexOfLeft + 1, count)]?.ToString() ?? "null");
                else
                {
                    T arg = args[ParseHelper.ParseToInt32(format, indexOfLeft + 1, delimiterIndex - indexOfLeft - 1)];
                    if (arg is IFormattable formattable)
                    {
                        string subFormat = format.Substring(delimiterIndex + 1, indexOfRight - delimiterIndex - 1);
                        Append(formattable.ToString(subFormat, null));
                    }
                    else
                    {
                        Append(arg?.ToString() ?? "null");
                    }
                }
                lastPos = indexOfRight + 1;
            } while (lastPos < length);
            int lastCount = length - lastPos;
            if (lastCount > 0)
                Append(format, lastPos, lastCount);
        }

        // 允許 Span<T> 參與高性能 AppendFormat 操作，並優化 unmanaged 類型在 AppendFormat 時的性能消耗
        internal void AppendFormatCore<T>(string format, T* args, int argc)
        {
            int length = format.Length;
            int lastPos = 0, indexOfLeft, indexOfRight;
            do
            {
                indexOfLeft = StringHelper.IndexOf(format, '{', lastPos);
                if (indexOfLeft < 0)
                    break;
                indexOfRight = StringHelper.IndexOf(format, '}', indexOfLeft);
                if (indexOfRight < 0)
                    break;
                Append(format, lastPos, indexOfLeft - lastPos);
                int count = indexOfRight - indexOfLeft - 1;
                int delimiterIndex = StringHelper.IndexOf(format, ':', indexOfLeft, count);
                if (delimiterIndex < 0)
                {
                    int argIndex = ParseHelper.ParseToInt32(format, indexOfLeft + 1, count);
                    if (argIndex < 0 || argIndex >= argc)
                        throw new ArgumentOutOfRangeException(nameof(args));
                    Append(args[argIndex].ToString() ?? "null");
                }
                else
                {
                    int argIndex = ParseHelper.ParseToInt32(format, indexOfLeft + 1, delimiterIndex - indexOfLeft - 1);
                    if (argIndex < 0 || argIndex >= argc)
                        throw new ArgumentOutOfRangeException(nameof(args));
                    T arg = args[argIndex];
                    if (arg is IFormattable formattable)
                    {
                        string subFormat = format.Substring(delimiterIndex + 1, indexOfRight - delimiterIndex - 1);
                        Append(formattable.ToString(subFormat, null));
                    }
                    else
                    {
                        Append(arg.ToString() ?? "null");
                    }
                }
                lastPos = indexOfRight + 1;
            } while (lastPos < length);
            int lastCount = length - lastPos;
            if (lastCount > 0)
                Append(format, lastPos, lastCount);
        }

        private void AppendFormatCore<T>(StringBase format, in ParamArrayTiny<T> args)
        {
            int length = format.Length;
            int lastPos = 0, indexOfLeft, indexOfRight;
            do
            {
                indexOfLeft = format.IndexOf('{', lastPos, length - lastPos);
                if (indexOfLeft < 0)
                    break;
                indexOfRight = format.IndexOf('}', indexOfLeft, length - indexOfLeft);
                if (indexOfRight < 0)
                    break;
                Append(format, lastPos, indexOfLeft - lastPos);
                int count = indexOfRight - indexOfLeft - 1;
                int delimiterIndex = format.IndexOf(':', indexOfLeft, count);
                if (delimiterIndex < 0)
                    Append(args[ParseHelper.ParseToInt32(format, indexOfLeft + 1, count)]?.ToString() ?? "null");
                else
                {
                    T arg = args[ParseHelper.ParseToInt32(format, indexOfLeft + 1, delimiterIndex - indexOfLeft - 1)];
                    if (arg is IFormattable formattable)
                    {
                        string subFormat = format.Slice(delimiterIndex + 1, indexOfRight - delimiterIndex - 1).ToString();
                        Append(formattable.ToString(subFormat, null));
                    }
                    else
                    {
                        Append(arg?.ToString() ?? "null");
                    }
                }
                lastPos = indexOfRight + 1;
            } while (lastPos < length);
            int lastCount = length - lastPos;
            if (lastCount > 0)
                Append(format, lastPos, lastCount);
        }

        // 允許 Span<T> 參與高性能 AppendFormat 操作，並優化 unmanaged 類型在 AppendFormat 時的性能消耗
        internal void AppendFormatCore<T>(StringBase format, T* args, int argc)
        {
            int length = format.Length;
            int lastPos = 0, indexOfLeft, indexOfRight;
            do
            {
                indexOfLeft = format.IndexOf('{', lastPos, length - lastPos);
                if (indexOfLeft < 0)
                    break;
                indexOfRight = format.IndexOf('}', indexOfLeft, length - indexOfLeft);
                if (indexOfRight < 0)
                    break;
                Append(format, lastPos, indexOfLeft - lastPos);
                int count = indexOfRight - indexOfLeft - 1;
                int delimiterIndex = format.IndexOf(':', indexOfLeft, count);
                if (delimiterIndex < 0)
                {
                    int argIndex = ParseHelper.ParseToInt32(format, indexOfLeft + 1, count);
                    if (argIndex < 0 || argIndex >= argc)
                        throw new ArgumentOutOfRangeException(nameof(args));
                    Append(args[argIndex].ToString() ?? "null");
                }
                else
                {
                    int argIndex = ParseHelper.ParseToInt32(format, indexOfLeft + 1, delimiterIndex - indexOfLeft - 1);
                    if (argIndex < 0 || argIndex >= argc)
                        throw new ArgumentOutOfRangeException(nameof(args));
                    T arg = args[argIndex];
                    if (arg is IFormattable formattable)
                    {
                        string subFormat = format.Slice(delimiterIndex + 1, indexOfRight - delimiterIndex - 1).ToString();
                        Append(formattable.ToString(subFormat, null));
                    }
                    else
                    {
                        Append(arg.ToString() ?? "null");
                    }
                }
                lastPos = indexOfRight + 1;
            } while (lastPos < length);
            int lastCount = length - lastPos;
            if (lastCount > 0)
                Append(format, lastPos, lastCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private StringBuilder GetAlternateStringBuilder(int extraLength)
        {
            char* start = _start;
            char* iterator = _iterator;
            int size = unchecked((int)(iterator - start));

            StringBuilder builder = _builderLazy.Value;
            builder.EnsureCapacity(size + extraLength);
            builder.Append(start, size);
            return builder;
        }

        [Inline(InlineBehavior.Remove)]
        private readonly void ReturnStringBuilder()
        {
            StringBuilder? builder = _builderLazy.GetValueDirectly();
            if (builder is null)
                return;
            StringBuilderPool.Shared.Return(builder);
        }
    }
}
