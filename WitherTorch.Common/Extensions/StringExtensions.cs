using System;
using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Structures;

namespace WitherTorch.Common.Extensions;

public static partial class StringExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char FirstOrDefault(this string obj, char defaultValue = '\0')
        => obj.Length <= 0 ? defaultValue : UnsafeHelper.GetStringDataReference(obj);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char LastOrDefault(this string obj, char defaultValue = '\0')
    {
        int length = obj.Length;
        if (length <= 0)
            return defaultValue;
        return UnsafeHelper.AddTypedOffsetAsReadOnly(in UnsafeHelper.GetStringDataReference(obj), length - 1);
    }

#if !NET8_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny(this string obj, params char[] values) => StringHelper.ContainsAny(obj, values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny(this string obj, string values) => StringHelper.ContainsAny(obj, values);
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains(this string[] array, string value, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, 0, array.Length, comparison) < 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains(this string[] array, string value, int startIndex, int length, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, startIndex, length, comparison) < 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf(this string[] array, string value, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, 0, array.Length, comparison);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf(this string[] array, string value, int startIndex, int length, StringComparison comparison = StringComparison.Ordinal)
    {
        for (int i = startIndex; i < length; i++)
        {
            if (SequenceHelper.Equals(array[i], value, comparison))
                return i;
        }
        return -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EndsWith(this string str, char c)
    {
        int count = str.Length;
        if (count > 0)
            return str[count - 1] == c;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EndsWithAny(this string str, params char[] chars)
    {
        int count = str.Length;
        if (count > 0)
        {
            char c = str[count - 1];
            for (int i = 0, length = chars.Length; i < length; i++)
            {
                if (c == chars[i])
                    return true;
            }
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool StartsWith(this string _this, char c)
        => _this.Length > 0 && c == _this.AsUnsafeRef().FirstElement;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool StartsWithAny(this string _this, params char[] chars)
        => _this.Length > 0 && SequenceHelper.Contains(chars, _this.AsUnsafeRef().FirstElement);

    public static string[] ToUpperAscii(this string[] _this)
    {
        for (int i = 0, length = _this.Length; i < length; i++)
            _this[i] = ToUpperAscii(_this[i]);
        return _this;
    }

    public static string[] ToLowerAscii(this string[] _this)
    {
        for (int i = 0, length = _this.Length; i < length; i++)
            _this[i] = ToLowerAscii(_this[i]);
        return _this;
    }

    public static unsafe string ToUpperAscii(this string _this)
    {
        int length = _this.Length;
        if (length <= 0)
            return string.Empty;
        fixed (char* ptr = _this)
            return ToUpperAsciiCore(ptr, ptr + length) ?? _this;
    }

    public static unsafe string ToLowerAscii(this string _this)
    {
        int length = _this.Length;
        if (length <= 0)
            return string.Empty;
        fixed (char* ptr = _this)
            return ToLowerAsciiCore(ptr, ptr + length) ?? _this;
    }

    private static unsafe string? ToUpperAsciiCore(char* ptr, char* ptrEnd)
        => ToLowerOrUpperAsciiCore(ptr, ptrEnd, isUpper: true);

    private static unsafe string? ToLowerAsciiCore(char* ptr, char* ptrEnd)
        => ToLowerOrUpperAsciiCore(ptr, ptrEnd, isUpper: false);

    public static string[] WithPrefix(this string[] _this, string prefix)
    {
        for (int i = 0, length = _this.Length; i < length; i++)
            _this[i] = _this[i].WithPrefix(prefix);
        return _this;
    }

    [Inline(InlineBehavior.Keep, export: true)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string WithPrefix(this string _this, string prefix) => string.Concat(prefix, _this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnsafeStringRef AsUnsafeRef(this string _this) => new UnsafeStringRef(_this);
}
