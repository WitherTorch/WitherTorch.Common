using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0130
namespace System;

public static partial class NotSupportedExceptionExtensions
{
    extension(NotSupportedException)
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void Throw() => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static T Throw<T>() => throw new NotSupportedException();
    }
}
