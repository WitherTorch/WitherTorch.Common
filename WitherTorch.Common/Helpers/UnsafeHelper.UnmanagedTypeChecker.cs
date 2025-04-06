using System;
using System.Reflection;
using System.Threading;

using WitherTorch.Common.Threading;

namespace WitherTorch.Common.Helpers
{
    partial class UnsafeHelper
    {
        private interface IUnmanagedTypeChecker
        {
            bool IsUnmanagedType();
        }

        private sealed class UnmanagedTypeChecker<T> : IUnmanagedTypeChecker
        {
            private static readonly UnmanagedTypeChecker<T> _instance = new UnmanagedTypeChecker<T>();
            private readonly Lazy<bool> _resultLazy;

            public static UnmanagedTypeChecker<T> Instance => _instance;

            private UnmanagedTypeChecker()
            {
                _resultLazy = new Lazy<bool>(GetResult, LazyThreadSafetyMode.ExecutionAndPublication); 
            }

            public bool IsUnmanagedType() => _resultLazy.Value;

            private bool GetResult()
            {
                Type type = typeof(T);
                foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!UnsafeHelper.IsUnmanagedType(field.FieldType)) 
                        return false;
                }
                return true;
            }
        }
    }
}
