using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0130
namespace System;

public static partial class ArgumentNullExceptionExtensions
{
    extension(ArgumentNullException)
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void Throw(string? paramName) => throw new ArgumentNullException(paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static T Throw<T>(string? paramName) => throw new ArgumentNullException(paramName);
    }
}
