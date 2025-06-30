using System.Reflection;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        private static unsafe partial class Core { }

        [LocalsInit(false)]
        private static unsafe partial class Core<T> where T : unmanaged
        {
            [Inline(InlineBehavior.Remove)]
            private static bool CheckTypeCanBeVectorized()
#if NET8_0_OR_GREATER
                => System.Numerics.Vector<T>.IsSupported;
#else
                => (typeof(T) == typeof(byte)) ||
                       (typeof(T) == typeof(short)) ||
                       (typeof(T) == typeof(int)) ||
                       (typeof(T) == typeof(long)) ||
                       (typeof(T) == typeof(sbyte)) ||
                       (typeof(T) == typeof(ushort)) ||
                       (typeof(T) == typeof(uint)) ||
                       (typeof(T) == typeof(ulong)) ||
                       (typeof(T) == typeof(float)) ||
                       (typeof(T) == typeof(double));
#endif

            [Inline(InlineBehavior.Remove)]
            private static bool IsUnsigned()
                => (typeof(T) == typeof(byte)) ||
                       (typeof(T) == typeof(ushort)) ||
                       (typeof(T) == typeof(uint)) ||
                       (typeof(T) == typeof(ulong));

            [Inline(InlineBehavior.Remove)]
            private static nint GetFunctionPointerSafely(MethodInfo? methodInfo)
            {
                if (methodInfo is null)
                    return default;
                return methodInfo.MethodHandle.GetFunctionPointer();
            }
        }
    }
}
