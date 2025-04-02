using InlineMethod;

using WitherTorch.CrossNative.Helpers;

#if !DEBUG
using InlineIL;
#endif

namespace WitherTorch.CrossNative.Windows
{
    unsafe partial class ComObject
    {
        [Inline(InlineBehavior.Remove)]
        private static void* GetFunctionPointerCore(void* nativePointer, [InlineParameter] int offset)
        {
#if DEBUG
            return *(void**)((nint)(*(void**)nativePointer) + offset * UnsafeHelper.PointerSize);
#else
            IL.Push(nativePointer);
            IL.Emit.Ldind_I();
            IL.Push(offset);
            IL.Push(UnsafeHelper.PointerSize);
            IL.Emit.Mul();
            IL.Emit.Add();
            IL.Emit.Ldind_I();
            return IL.ReturnPointer();
#endif
        }

        [Inline(InlineBehavior.Remove)]
        private static ulong AddRefCore(void* nativePointer)
        {
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.AddRef);
            return ((delegate*<void*, ulong>)functionPointer)(nativePointer);
        }

        [Inline(InlineBehavior.Remove)]
        private static ulong ReleaseCore(void* nativePointer)
        {
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.Release);
            return ((delegate*<void*, ulong>)functionPointer)(nativePointer);
        }
    }
}
