using InlineMethod;

using WitherTorch.Common.Helpers;
using System.Runtime.CompilerServices;
using System;



#if !DEBUG
using InlineIL;
#endif

namespace WitherTorch.Common.Windows
{
    unsafe partial class ComObject
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void* GetFunctionPointerCore(void* nativePointer, int offset)
            => UnsafeHelper.PointerSizeConstant switch
            {
                sizeof(uint) => *(void**)(*(uint**)nativePointer + offset),
                sizeof(ulong) => *(void**)(*(ulong**)nativePointer + offset),
                UnsafeHelper.PointerSizeConstant_Indeterminate => UnsafeHelper.PointerSize switch
                {
                    sizeof(uint) => *(void**)(*(uint**)nativePointer + offset),
                    sizeof(ulong) => *(void**)(*(ulong**)nativePointer + offset),
                    _ => throw new NotSupportedException($"Pointer size {UnsafeHelper.PointerSize} is not supported.")
                },
                _ => throw new NotSupportedException($"Pointer size {UnsafeHelper.PointerSizeConstant} is not supported.")
            };

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
