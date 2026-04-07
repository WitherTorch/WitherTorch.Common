using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Text;

namespace WitherTorch.Common.Extensions
{
    public static class ToStringWrapperExtensions
    {
        /// <inheritdoc cref="bool.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this bool _this) => _this ? BooleanStringWrapperStore.TrueString : BooleanStringWrapperStore.FalseString;

        /// <inheritdoc cref="byte.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this byte _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="sbyte.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this sbyte _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="short.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this short _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="ushort.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this ushort _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="int.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this int _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="uint.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this uint _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="long.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this long _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="ulong.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this ulong _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="nint.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this nint _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="nuint.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this nuint _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="float.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this float _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="double.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this double _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="decimal.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringWrapper? ToStringWrapper(this decimal _this) => StringWrapper.CreateUtf16String(_this.ToString());

        /// <inheritdoc cref="char.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe StringWrapper? ToStringWrapper(this char _this)
        {
            if (_this > Latin1EncodingHelper.Latin1EncodingLimit)
                return StringWrapper.CreateUtf16String(&_this, 0u, 1u);
            if (_this > AsciiEncodingHelper.AsciiEncodingLimit)
                return StringWrapper.CreateLatin1String((byte*)&_this, 0u, 1u);
            return StringWrapper.CreateAsciiString((byte*)&_this, 0u, 1u);
        }

        /// <inheritdoc cref="string.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper(this string? _this) => _this is null ? null : StringWrapper.CreateUtf16String(_this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper(this string? _this, StringCreateOptions options) => _this is null ? null : StringWrapper.Create(_this, options);

        /// <inheritdoc cref="StringBuilder.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper(this StringBuilder? _this) => _this is null ? null : StringWrapper.CreateUtf16String(_this.ToString());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper(this StringBuilder? _this, StringCreateOptions options) => _this is null ? null : StringWrapper.Create(_this.ToString(), options);

        /// <inheritdoc cref="object.ToString()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(int.MinValue)]
        public static StringWrapper? ToStringWrapper<T>(this T? _this)
            => _this switch
            {
                string str => StringWrapper.CreateUtf16String(str),
                StringWrapper str => str,
                IStringWrapperConvertible convertible => convertible.ToStringWrapper(),
                null => null,
                _ => _this?.ToString()?.ToStringWrapper()
            };

        /// <inheritdoc cref="IConvertible.ToString(IFormatProvider?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(int.MinValue)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper<TConvertible>(this TConvertible? _this, IFormatProvider? provider) where TConvertible : IConvertible
            => _this is null ? null : StringWrapper.CreateUtf16String(_this.ToString(provider));

        /// <inheritdoc cref="IConvertible.ToString(IFormatProvider?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(int.MinValue + 1)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper<TFormattable>(this TFormattable? _this, string? format) where TFormattable : IFormattable
            => _this is null ? null : StringWrapper.CreateUtf16String(_this.ToString(format, null));

        /// <inheritdoc cref="IConvertible.ToString(IFormatProvider?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(int.MinValue)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper<TFormattable>(this TFormattable? _this, StringWrapper? format) where TFormattable : IFormattable
            => _this is null ? null : StringWrapper.CreateUtf16String(_this.ToString(format?.ToString(), null));

        /// <inheritdoc cref="IConvertible.ToString(IFormatProvider?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(int.MinValue + 1)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper<TFormattable>(this TFormattable? _this, string? format, IFormatProvider? provider) where TFormattable : IFormattable
            => _this is null ? null : StringWrapper.CreateUtf16String(_this.ToString(format, provider));

        /// <inheritdoc cref="IConvertible.ToString(IFormatProvider?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [OverloadResolutionPriority(int.MinValue)]
        [return: NotNullIfNotNull(nameof(_this))]
        public static StringWrapper? ToStringWrapper<TFormattable>(this TFormattable? _this, StringWrapper? format, IFormatProvider? provider) where TFormattable : IFormattable
            => _this is null ? null : StringWrapper.CreateUtf16String(_this.ToString(format?.ToString(), provider));

        private static class BooleanStringWrapperStore
        {
            public static readonly StringWrapper TrueString;
            public static readonly StringWrapper FalseString;

            static BooleanStringWrapperStore()
            {
                DebugHelper.ThrowIf(bool.TrueString != "True");
                DebugHelper.ThrowIf(bool.FalseString != "False");
#if NET472_OR_GREATER
                unsafe
                {
                    ulong dummy;
                    byte* buffer = (byte*)&dummy;
                    buffer[0] = (byte)'T';
                    buffer[1] = (byte)'r';
                    buffer[2] = (byte)'u';
                    buffer[3] = (byte)'e';
                    TrueString = StringWrapper.CreateAsciiString(buffer, 0u, 4u);
                    buffer[0] = (byte)'F';
                    buffer[1] = (byte)'a';
                    buffer[2] = (byte)'l';
                    buffer[3] = (byte)'s';
                    buffer[4] = (byte)'e';
                    FalseString = StringWrapper.CreateAsciiString(buffer, 0u, 5u);
                }
#else
                TrueString = "True"u8.ToStringWrapperAsAscii();
                FalseString = "False"u8.ToStringWrapperAsAscii();
#endif
            }
        }
    }
}
