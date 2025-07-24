using InlineMethod;

using WitherTorch.Common.Structures;

namespace WitherTorch.Common.Text
{
    unsafe partial struct StringBuilderTiny
    {
        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat<T>(string format, T arg0) => AppendFormat(format, new ParamArrayTiny<T>(arg0));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat<T>(string format, T arg0, T arg1) => AppendFormat(format, new ParamArrayTiny<T>(arg0, arg1));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat<T>(string format, T arg0, T arg1, T arg2) => AppendFormat(format, new ParamArrayTiny<T>(arg0, arg1, arg2));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat<T>(string format, params T[] args) => AppendFormat(format, new ParamArrayTiny<T>(args));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat(string format, object arg0) => AppendFormat(format, new ParamArrayTiny<object>(arg0));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat(string format, object arg0, object arg1) => AppendFormat(format, new ParamArrayTiny<object>(arg0, arg1));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat(string format, object arg0, object arg1, object arg2) => AppendFormat(format, new ParamArrayTiny<object>(arg0, arg1, arg2));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat(string format, params object[] args) => AppendFormat(format, new ParamArrayTiny<object>(args));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat<T>(StringBase format, T arg0) => AppendFormat(format, new ParamArrayTiny<T>(arg0));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat<T>(StringBase format, T arg0, T arg1) => AppendFormat(format, new ParamArrayTiny<T>(arg0, arg1));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat<T>(StringBase format, T arg0, T arg1, T arg2) => AppendFormat(format, new ParamArrayTiny<T>(arg0, arg1, arg2));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat<T>(StringBase format, params T[] args) => AppendFormat(format, new ParamArrayTiny<T>(args));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat(StringBase format, object arg0) => AppendFormat(format, new ParamArrayTiny<object>(arg0));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat(StringBase format, object arg0, object arg1) => AppendFormat(format, new ParamArrayTiny<object>(arg0, arg1));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat(StringBase format, object arg0, object arg1, object arg2) => AppendFormat(format, new ParamArrayTiny<object>(arg0, arg1, arg2));

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendFormat(StringBase format, params object[] args) => AppendFormat(format, new ParamArrayTiny<object>(args));

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
        public void AppendLine(StringBase value)
        {
            Append(value);
            AppendLine();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public void AppendLine(StringBase value, int startIndex, int count)
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
    }
}
