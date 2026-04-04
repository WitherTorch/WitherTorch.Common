#if NET8_0_OR_GREATER
using System;
using System.Numerics;
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
                if (TryGetValueFromStandardInterface(type, out T result))
                    return result;
                FieldInfo? field = type.GetField(nameof(MaxValue), BindingFlags.Public | BindingFlags.Static);
                if (field is not null && field.IsInitOnly && field.FieldType == type)
                    return (T)field.GetValue(null)!;
                PropertyInfo? prop = type.GetProperty(nameof(MaxValue), BindingFlags.Public | BindingFlags.Static);
                if (prop is not null && prop.CanRead && !prop.CanWrite && prop.PropertyType == type)
                    return (T)prop.GetValue(null)!;
                throw new InvalidOperationException($"{typeof(T).FullName} doesn't have {nameof(MaxValue)}!");

                static bool TryGetValueFromStandardInterface(Type type, out T result)
                {
                    try
                    {
                        result = (T)typeof(ConstraintedMinMaxHelper<>)
                            .MakeGenericType(type)
                            .GetField(nameof(ConstraintedMinMaxHelper<>.MaxValue), BindingFlags.Public | BindingFlags.Static)!
                            .GetValue(null)!;
                        return true;
                    }
                    catch (Exception)
                    {
                        result = default;
                        return false;
                    }
                }
            }
        }

        private static class MinHelper<T> where T : unmanaged
        {
            public static readonly T MinValue = GetMinValue();

            private static T GetMinValue()
            {
                Type type = typeof(T);
                if (TryGetValueFromStandardInterface(type, out T result))
                    return result;
                FieldInfo? field = type.GetField(nameof(MinValue), BindingFlags.Public | BindingFlags.Static);
                if (field is not null && field.IsInitOnly && field.FieldType == type)
                    return (T)field.GetValue(null)!;
                PropertyInfo? prop = type.GetProperty(nameof(MinValue), BindingFlags.Public | BindingFlags.Static);
                if (prop is not null && prop.CanRead && !prop.CanWrite && prop.PropertyType == type)
                    return (T)prop.GetValue(null)!;
                throw new InvalidOperationException($"{typeof(T).FullName} doesn't have {nameof(MinValue)}!");

                static bool TryGetValueFromStandardInterface(Type type, out T result)
                {
                    try
                    {
                        result = (T)typeof(ConstraintedMinMaxHelper<>)
                            .MakeGenericType(type)
                            .GetField(nameof(ConstraintedMinMaxHelper<>.MinValue), BindingFlags.Public | BindingFlags.Static)!
                            .GetValue(null)!;
                        return true;
                    }
                    catch (Exception)
                    {
                        result = default;
                        return false;
                    }
                }
            }
        }

        private static class ConstraintedMinMaxHelper<T> where T : unmanaged, IMinMaxValue<T>
        {
            public static readonly T MinValue = T.MinValue;
            public static readonly T MaxValue = T.MaxValue;
        }
    }
}
#endif