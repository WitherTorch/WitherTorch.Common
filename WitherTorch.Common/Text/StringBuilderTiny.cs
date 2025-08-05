using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Structures;

namespace WitherTorch.Common.Text
{
    public unsafe ref partial struct StringBuilderTiny
    {
        private char* _start, _iterator, _end;
        private DelayedCollectingStringBuilder? _builder;

        public readonly int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                DelayedCollectingStringBuilder? builder = _builder;
                if (builder is null)
                    return unchecked((int)(_iterator - _start));
                return builder.GetObject().Length;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetStartPointer(char* ptr, int length)
        {
            _start = ptr;
            _iterator = ptr;
            _end = ptr + length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetStartPointer(in Span<char> span)
        {
            char* ptr = UnsafeHelper.AsPointerRef(ref span.GetPinnableReference());
            int length = span.Length;
            SetStartPointer(ptr, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetStartPointer(in ReadOnlySpan<char> span)
        {
            char* ptr = UnsafeHelper.AsPointerIn(in span.GetPinnableReference());
            int length = span.Length;
            SetStartPointer(ptr, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public void Append(char value)
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
            {
                builder.GetObject().Append(value);
                return;
            }
            char* iterator = _iterator;
            char* end = _end;
            if (iterator < end)
            {
                *iterator = value;
                _iterator = iterator + 1;
                return;
            }
            GetOrRentAlternateStringBuilder(1).Append(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public void Append(char value, int repeatCount)
        {
            if (repeatCount < 1)
                return;
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
            {
                builder.GetObject().Append(value, repeatCount);
                return;
            }
            char* iterator = _iterator;
            char* endIterator = iterator + repeatCount - 1;
            char* end = _end;
            if (endIterator < end)
            {
                for (; iterator <= endIterator; iterator++)
                {
                    *iterator = value;
                }
                _iterator = iterator;
                return;
            }
            GetOrRentAlternateStringBuilder(repeatCount).Append(value, repeatCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string value)
        {
            int count = value.Length;
            if (count <= 0)
                return;
            fixed (char* ptr = value)
                AppendCore(ptr, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(StringBase value)
        {
            int count = value.Length;
            if (count <= 0)
                return;
            AppendCore(value, 0, unchecked((nuint)count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(StringSlice value)
        {
            nuint count = value.Length;
            if (count <= 0)
                return;
            AppendCore(value.Original, value.StartIndex, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            int valueLength = value.Length;
            if (valueLength <= 0)
                return;
            if (startIndex + count > valueLength)
                throw new ArgumentOutOfRangeException(startIndex >= valueLength ? nameof(startIndex) : nameof(count));
            fixed (char* ptr = value)
                AppendCore(ptr + startIndex, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(StringBase value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            int valueLength = value.Length;
            if (valueLength <= 0)
                return;
            if (startIndex + count > valueLength)
                throw new ArgumentOutOfRangeException(startIndex >= valueLength ? nameof(startIndex) : nameof(count));
            AppendCore(value, unchecked((nuint)startIndex), unchecked((nuint)count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char* ptr, int count)
        {
            if (count <= 0)
                return;
            AppendCore(ptr, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat<T>(string format, in ParamArrayTiny<T> args)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormat<T>(StringBase format, in ParamArrayTiny<T> args)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringBuilderTiny Clear()
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is null)
            {
                _iterator = _start;
                return this;
            }
            builder.GetObject().Clear();
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, char value)
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
            {
                builder.GetObject().Insert(index, value);
                return;
            }
            char* iterator = _iterator;
            char* endIterator = iterator;
            char* end = _end;
            if (endIterator < end)
            {
                char* copyStartPtr = _start + index;
                for (char* sourcePtr = iterator - 1, destPtr = endIterator; sourcePtr >= copyStartPtr; sourcePtr--, destPtr--)
                {
                    *destPtr = *sourcePtr;
                }
                *copyStartPtr = value;
                _iterator = endIterator + 1;
                return;
            }
            GetOrRentAlternateStringBuilder(1).Insert(index, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, string value)
        {
            int valueLength = value.Length;
            if (valueLength <= 0)
                return;
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
            {
                builder.GetObject().Insert(index, value);
                return;
            }
            char* iterator = _iterator;
            char* endIterator = iterator + valueLength - 1;
            char* end = _end;
            if (endIterator < end)
            {
                char* copyStartPtr = _start + index;
                for (char* sourcePtr = iterator - 1, destPtr = endIterator; sourcePtr >= copyStartPtr; sourcePtr--, destPtr--)
                {
                    *destPtr = *sourcePtr;
                }
                fixed (char* ptr = value)
                {
                    UnsafeHelper.CopyBlockUnaligned(copyStartPtr, ptr, unchecked((uint)(sizeof(char) * valueLength)));
                }
                _iterator = endIterator + 1;
                return;
            }
            GetOrRentAlternateStringBuilder(valueLength).Insert(index, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, StringBase value)
        {
            int valueLength = value.Length;
            if (valueLength <= 0)
                return;
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
            {
                builder.GetObject().Insert(index, value.ToString());
                return;
            }
            char* iterator = _iterator;
            char* endIterator = iterator + valueLength - 1;
            char* end = _end;
            if (endIterator < end)
            {
                char* copyStartPtr = _start + index;
                for (char* sourcePtr = iterator - 1, destPtr = endIterator; sourcePtr >= copyStartPtr; sourcePtr--, destPtr--)
                    *destPtr = *sourcePtr;
                value.CopyToCore(copyStartPtr, 0u, unchecked((nuint)valueLength));
                _iterator = endIterator + 1;
                return;
            }
            GetOrRentAlternateStringBuilder(valueLength).Insert(index, value.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length <= 0)
                return;
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
            {
                builder.GetObject().Remove(startIndex, length);
                return;
            }
            char* start = _start + startIndex;
            char* iterator = _iterator;
            if (iterator - length <= start)
            {
                _iterator = start;
                return;
            }
            for (char* sourcePtr = start + length, destPtr = start; sourcePtr < iterator; sourcePtr++, destPtr++)
            {
                *destPtr = *sourcePtr;
            }
            _iterator -= length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveLast()
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
            {
                StringBuilder inlineBuilder = builder.GetObject();
                inlineBuilder.Remove(inlineBuilder.Length - 1, 1);
                return;
            }
            char* start = _start;
            char* iterator = _iterator;
            if (iterator > start)
                _iterator = iterator - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString()
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
                return builder.GetObject().ToString();
            char* start = _start;
            char* iterator = _iterator;
            if (start >= iterator)
                return string.Empty;
            return new string(start, 0, unchecked((int)(iterator - start)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly StringBase ToStringBase()
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
                return StringBase.Create(builder.GetObject().ToString());
            char* start = _start;
            char* iterator = _iterator;
            if (start >= iterator)
                return StringBase.Empty;
            return StringBase.Create(start, 0, unchecked((nuint)(iterator - start)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(int startIndex, int length)
        {
            if (length <= 0)
                return string.Empty;
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
                return builder.GetObject().ToString(startIndex, length);
            char* start = _start + startIndex;
            char* end = start + length;
            char* iterator = _iterator;
            if (start == iterator)
                return string.Empty;
            if (end > iterator)
                end = iterator;
            return new string(start, 0, unchecked((int)(end - start)));
        }

        public void Dispose() => ReturnStringBuilder();
    }
}
