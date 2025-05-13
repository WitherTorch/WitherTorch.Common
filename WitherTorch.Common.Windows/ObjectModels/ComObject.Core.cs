using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Windows.ObjectModels
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
        private static int QueryInterfaceCore(ref void* nativePointer, in Guid iid)
        {
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.QueryInterface);
            return ((delegate* unmanaged[Stdcall]<void*, Guid*, void**, int>)functionPointer)(nativePointer, 
                UnsafeHelper.AsPointerIn(in iid), UnsafeHelper.AsPointerRef(ref nativePointer));
        }

        [Inline(InlineBehavior.Remove)]
        private static ulong AddRefCore(void* nativePointer)
        {
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.AddRef);
            return ((delegate* unmanaged[Stdcall]<void*, ulong>)functionPointer)(nativePointer);
        }

        [Inline(InlineBehavior.Remove)]
        private static ulong ReleaseCore(void* nativePointer)
        {
            void* functionPointer = GetFunctionPointerOrThrow(nativePointer, (int)MethodTable.Release);
            return ((delegate* unmanaged[Stdcall]<void*, ulong>)functionPointer)(nativePointer);
        }
    }
}
