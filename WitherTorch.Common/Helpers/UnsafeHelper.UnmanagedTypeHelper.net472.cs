#if NET472_OR_GREATER
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using InlineIL;

namespace WitherTorch.Common.Helpers
{
    partial class UnsafeHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsUnmanageTypeSlow<T>()
        {
            if (!typeof(T).IsValueType)
                return false;
            IL.Emit.Ldtoken(typeof(UnmanagedTypeHelper<T>));
            IL.Emit.Call(new MethodRef(typeof(RuntimeHelpers), nameof(RuntimeHelpers.RunClassConstructor)));
            return UnmanagedTypeHelper<T>.IsUnmanaged;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsUnmanageTypeSlow(Type type)
        {
            if (!type.IsValueType)
                return false;
            Type helperType = typeof(UnmanagedTypeHelper<>).MakeGenericType(type);
            RuntimeHelpers.RunClassConstructor(helperType.TypeHandle);
            return (bool)helperType.GetField(nameof(UnmanagedTypeHelper<>.IsUnmanaged), BindingFlags.Public | BindingFlags.Static).GetValue(null);
        }

        // 此處只會有結構體類型進入
        private static class UnmanagedTypeHelper<T>
        {
            public static readonly bool IsUnmanaged = CheckIsUnmanaged();

            private static bool CheckIsUnmanaged()
            {
                foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!IsUnmanagedType(field.FieldType))
                        return false;
                }
                return true;
            }
        }
    }
}
#endif