using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0130
namespace System;

public static partial class ObjectDisposedExceptionExtensions
{
    extension(ObjectDisposedException)
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static void Throw(string? objectName) => throw new ObjectDisposedException(objectName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DoesNotReturn]
        public static T Throw<T>(string? objectName) => throw new ObjectDisposedException(objectName);
    }
}
