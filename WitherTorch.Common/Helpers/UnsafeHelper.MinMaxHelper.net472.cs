#if NET472_OR_GREATER
using System;
using System.Reflection;

namespace WitherTorch.Common.Helpers
{
    partial class UnsafeHelper
    {
        private static class MaxHelper<T> where T : unmanaged
        {
            public static readonly T MaxValue = GetMaxValue();

            private static T GetMaxValue()
            {
                Type type = typeof(T);
                FieldInfo? field = type.GetField(nameof(MaxValue), BindingFlags.Public | BindingFlags.Static);
                if (field is not null && field.IsInitOnly && field.FieldType == type)
                    return (T)field.GetValue(null)!;
                PropertyInfo? prop = type.GetProperty(nameof(MaxValue), BindingFlags.Public | BindingFlags.Static);
                if (prop is not null && prop.CanRead && !prop.CanWrite && prop.PropertyType == type)
                    return (T)prop.GetValue(null)!;
                throw new InvalidOperationException($"{typeof(T).FullName} doesn't have {nameof(MaxValue)}!");
            }
        }

        private static class MinHelper<T> where T : unmanaged
        {
            public static readonly T MinValue = GetMinValue();

            private static T GetMinValue()
            {
                Type type = typeof(T);
                FieldInfo? field = type.GetField(nameof(MinValue), BindingFlags.Public | BindingFlags.Static);
                if (field is not null && field.IsInitOnly && field.FieldType == type)
                    return (T)field.GetValue(null)!;
                PropertyInfo? prop = type.GetProperty(nameof(MinValue), BindingFlags.Public | BindingFlags.Static);
                if (prop is not null && prop.CanRead && !prop.CanWrite && prop.PropertyType == type)
                    return (T)prop.GetValue(null)!;
                throw new InvalidOperationException($"{typeof(T).FullName} doesn't have {nameof(MinValue)}!");
            }
        }
    }
}
#endif