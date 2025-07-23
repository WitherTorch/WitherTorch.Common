using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WitherTorch.Common.Helpers
{
    public static class ReflectionHelper
    {
        public static ConstructorInfo? GetConstuctor(Type type, Type[]? parameterTypes) 
            => type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, parameterTypes ?? [], []);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint GetConstuctorPointer(Type type, Type[]? parameterTypes)
        {
            ConstructorInfo? constuctor = GetConstuctor(type, parameterTypes);
            if (constuctor is null)
                return default;
            return constuctor.MethodHandle.GetFunctionPointer();
        }

        public static MethodInfo? GetMethod(Type type, string methodName, Type[]? parameterTypes, Type returnType,
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            int parameterTypeCount = parameterTypes is null ? 0 : parameterTypes.Length;
            MethodInfo[] methods = type.GetMethods(flags);
            for (int i = 0, length = methods.Length; i < length; i++)
            {
                MethodInfo method = methods[i];
                if (!returnType.Equals(method.ReturnType) || !SequenceHelper.Equals(methodName, method.Name))
                    continue;
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length != parameterTypeCount)
                    continue;
                int j;
                for (j = 0; j < parameterTypeCount; j++)
                {
                    if (!parameterTypes![j].Equals(parameters[j].ParameterType))
                        break;
                }
                if (j < parameterTypeCount)
                    continue;
                return method;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint GetMethodPointer(Type type, string methodName, Type[]? parameterTypes, Type returnType,
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            MethodInfo? method = GetMethod(type, methodName, parameterTypes, returnType, flags);
            if (method is null)
                return default;
            return method.MethodHandle.GetFunctionPointer();
        }

        public static MethodInfo? GetPropertyGetter(Type type, string propertyName, Type propertyType,
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            PropertyInfo? property = type.GetProperty(propertyName, flags);
            if (property is null || !propertyType.Equals(property.PropertyType))
                return null;
            return property.GetGetMethod((flags & BindingFlags.NonPublic) == BindingFlags.NonPublic);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint GetPropertyGetterPointer(Type type, string propertyName, Type propertyType,
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            MethodInfo? method = GetPropertyGetter(type, propertyName, propertyType, flags);
            if (method is null)
                return default;
            return method.MethodHandle.GetFunctionPointer();
        }
    }
}
