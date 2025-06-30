using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Structures;

namespace WitherTorch.Common.Text
{
    public unsafe ref struct StringBuilderTiny
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
        [SecuritySafeCritical]
        public void Append(string value)
        {
            int count = value.Length;
            if (count <= 0)
                return;
            fixed (char* ptr = value)
            {
                AppendCore(ptr, count);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
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
            {
                AppendCore(ptr + startIndex, count);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public void Append(char* ptr, int count)
        {
            if (count <= 0)
                return;
            AppendCore(ptr, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Inline(InlineBehavior.Remove)]
        [SecuritySafeCritical]
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
                UnsafeHelper.CopyBlock(iterator, ptr, unchecked((uint)(sizeof(char) * count)));
                _iterator = endIterator + 1;
                return;
            }
            GetOrRentAlternateStringBuilder(count).Append(ptr, count);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        public void AppendFormat<T>(string format, T arg0) => AppendFormat(format, new ParamArrayTiny<T>(arg0));

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        public void AppendFormat<T>(string format, T arg0, T arg1) => AppendFormat(format, new ParamArrayTiny<T>(arg0, arg1));

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        public void AppendFormat<T>(string format, T arg0, T arg1, T arg2) => AppendFormat(format, new ParamArrayTiny<T>(arg0, arg1, arg2));

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        public void AppendFormat<T>(string format, params T[] args) => AppendFormat(format, new ParamArrayTiny<T>(args));

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        public void AppendFormat(string format, object arg0) => AppendFormat(format, new ParamArrayTiny<object>(arg0));

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        public void AppendFormat(string format, object arg0, object arg1) => AppendFormat(format, new ParamArrayTiny<object>(arg0, arg1));

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        public void AppendFormat(string format, object arg0, object arg1, object arg2) => AppendFormat(format, new ParamArrayTiny<object>(arg0, arg1, arg2));

        [Inline(InlineBehavior.Keep, export: true)]
        [SecuritySafeCritical]
        public void AppendFormat(string format, params object[] args) => AppendFormat(format, new ParamArrayTiny<object>(args));

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
        [SecuritySafeCritical]
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
        [SecuritySafeCritical]
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
        [SecuritySafeCritical]
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
                    UnsafeHelper.CopyBlock(copyStartPtr, ptr, unchecked((uint)(sizeof(char) * valueLength)));
                }
                _iterator = endIterator + 1;
                return;
            }
            GetOrRentAlternateStringBuilder(valueLength).Insert(index, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
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
        [SecuritySafeCritical]
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
        [SecuritySafeCritical]
        public override readonly string ToString()
        {
            DelayedCollectingStringBuilder? builder = _builder;
            if (builder is not null)
                return builder.GetObject().ToString();
            char* start = _start;
            char* iterator = _iterator;
            if (start == iterator)
                return string.Empty;
            return new string(start, 0, unchecked((int)(iterator - start)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
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

        #region Inline Methods
        [Inline(InlineBehavior.Keep, export: true)]
        public void Append(char* ptr, int startIndex, int count)
            => Append(ptr + startIndex, count);

        [Inline(InlineBehavior.Keep, export: true)]
        public void Append(char* ptr, char* ptrEnd)
            => Append(ptr, unchecked((int)(ptrEnd - ptr)));

        [Inline(InlineBehavior.Keep, export: true)]
        public void Append(object value)
            => Append(value?.ToString() ?? "null");

        [Inline(InlineBehavior.Keep, export: true)]
        public void Append<T>(T value)
            => Append(value?.ToString() ?? "null");

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine()
            => Append('\n');

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine(char value)
        {
            Append(value);
            AppendLine();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine(char value, int repeatCount)
        {
            Append(value, repeatCount);
            AppendLine();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine(string value)
        {
            Append(value);
            AppendLine();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine(string value, int startIndex, int count)
        {
            Append(value, startIndex, count);
            AppendLine();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine(char* ptr, int count)
        {
            Append(ptr, count);
            AppendLine();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine(char* ptr, int startIndex, int count)
        {
            Append(ptr, startIndex, count);
            AppendLine();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine(char* ptr, char* ptrEnd)
        {
            Append(ptr, ptrEnd);
            AppendLine();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine(object value)
        {
            Append(value);
            AppendLine();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine<T>(T value)
        {
            Append(value);
            AppendLine();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public void Insert(int index, object value)
            => Insert(index, value?.ToString() ?? "null");

        [Inline(InlineBehavior.Keep, export: true)]
        public void Insert<T>(int index, T value)
            => Insert(index, value?.ToString() ?? "null");
        #endregion

        #region Private Methods
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
        #endregion
    }
}
