using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using InlineMethod;

namespace WitherTorch.Common.Helpers
{
    public static class ReflectionHelper
    {
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
    }
}
