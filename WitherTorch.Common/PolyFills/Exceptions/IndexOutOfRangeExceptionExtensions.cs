using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0130
namespace System;

public static partial class IndexOutOfRangeExceptionExtensions
{
    extension(IndexOutOfRangeException)
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void Throw() => throw new IndexOutOfRangeException();

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static T Throw<T>() => throw new IndexOutOfRangeException();
    }
}
