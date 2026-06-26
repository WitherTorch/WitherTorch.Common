#if !NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using RiceTea.Core.Helpers;

#pragma warning disable IDE0130
namespace System;

partial class ArgumentExceptionExtensions
{
    extension(ArgumentException)
    {
        /// <summary>Throws an exception if <paramref name="argument"/> is null or empty.</summary>
        /// <param name="argument">The string argument to validate as non-null and non-empty.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="argument"/> is empty.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (StringHelper.IsNullOrEmpty(argument))
                ThrowNullOrEmptyException(argument, paramName);
        }

        /// <summary>Throws an exception if <paramref name="argument"/> is null, empty, or consists only of white-space characters.</summary>
        /// <param name="argument">The string argument to validate.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="argument"/> is empty or consists only of white-space characters.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (StringHelper.IsNullOrWhiteSpace(argument))
                ThrowNullOrWhiteSpaceException(argument, paramName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        private static void ThrowNullOrEmptyException(string? argument, string? paramName)
        {
            const string Message = "The value cannot be an empty string.";

            ArgumentNullException.ThrowIfNull(argument, paramName);
            throw new ArgumentException(Message, paramName);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        private static void ThrowNullOrWhiteSpaceException(string? argument, string? paramName)
        {
            const string Message = "The value cannot be an empty string or composed entirely of whitespace.";

            ArgumentNullException.ThrowIfNull(argument, paramName);
            throw new ArgumentException(Message, paramName);
        }
    }
}
#endif