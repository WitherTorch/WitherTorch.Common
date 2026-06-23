using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0130
namespace System;

public static partial class ArgumentOutOfRangeExceptionExtensions
{
    extension(ArgumentOutOfRangeException)
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void Throw(string? paramName) => throw new ArgumentOutOfRangeException(paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static T Throw<T>(string? paramName) => throw new ArgumentOutOfRangeException(paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void Throw(string? paramName, object? actualValue, string? message) => throw new ArgumentOutOfRangeException(paramName, actualValue, message);

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static T Throw<T>(string? paramName, object? actualValue, string? message) => throw new ArgumentOutOfRangeException(paramName, actualValue, message);
    }
}
