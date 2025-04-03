using System;
using System.Collections.Generic;
using System.Reflection;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        partial class Core<T>
        {
            private sealed unsafe class ComparerImpl : IComparer<T>, IEqualityComparer<T>
            {
                private static readonly ComparerImpl _instance = new ComparerImpl();

                private readonly delegate*<T, T, int> _compareFunction;
                private readonly delegate*<T, T, bool> _equalsFunction;

                public static ComparerImpl Instance => _instance;

                private ComparerImpl()
                {
                    _compareFunction = GetCompareFunction();
                    _equalsFunction = GetEqualsFunction();
                }

                public int Compare(T x, T y)
                {
                    delegate*<T, T, int> compareFunction = _compareFunction;
                    if (compareFunction is null)
                        throw new InvalidOperationException($"{typeof(T).Name} doesn't implemented CompareTo");
                    throw new NotImplementedException();
                }

                public bool Equals(T x, T y) => _equalsFunction(x, y);

                public int GetHashCode(T obj) => obj.GetHashCode();

                private static delegate*<T, T, int> GetCompareFunction()
                {
                    if (!typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
                        return null;

                    MethodInfo? method = ReflectionHelper.GetMethod(typeof(T), nameof(IComparable<T>.CompareTo), 
                        [typeof(T)], typeof(int), BindingFlags.Instance | BindingFlags.Public);
                    if (method is null)
                        return &SlowCompare;
                    return (delegate*<T, T, int>)method.MethodHandle.GetFunctionPointer();
                }

                private static delegate*<T, T, bool> GetEqualsFunction()
                {
                    if (!typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                        return &VerySlowEquals;
                    MethodInfo? method = ReflectionHelper.GetMethod(typeof(T), nameof(IEquatable<T>.Equals),
                        [typeof(T)], typeof(int), BindingFlags.Instance | BindingFlags.Public);
                    if (method is null)
                        return &SlowEquals;
                    return (delegate*<T, T, bool>)method.MethodHandle.GetFunctionPointer();
                }

                private static int SlowCompare(T item1, T item2) => ((IComparable<T>)item1).CompareTo(item2);

                private static bool SlowEquals(T item1, T item2) => ((IEquatable<T>)item1).Equals(item2);

                private static bool VerySlowEquals(T item1, T item2) => item1.Equals(item2);
            }
        }
    }
}
