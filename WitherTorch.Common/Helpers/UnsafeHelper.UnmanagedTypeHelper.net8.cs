#if NET8_0_OR_GREATER
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    partial class UnsafeHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsUnmanageTypeSlow(Type type)
        {
            if (!type.IsValueType)
                return false;
            return (bool)typeof(RuntimeHelpers)
                .GetMethod(nameof(RuntimeHelpers.IsReferenceOrContainsReferences), BindingFlags.Public | BindingFlags.Static)!
                .MakeGenericMethod(type)
                .Invoke(null, null)!;
        }
    }
}
#endif