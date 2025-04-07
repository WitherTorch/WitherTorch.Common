using System;
using System.Reflection;

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
                int j = 0;
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
    }
}
