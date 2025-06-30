using InlineMethod;

using System;
using System.Runtime.CompilerServices;
using System.Reflection;

using WitherTorch.Common.Threading;

#if NET6_0_OR_GREATER
using System.Runtime.Intrinsics;
#else
using System.Numerics;
#endif

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        private enum UnaryOperationMethod
        {
            Not,
            _Last
        }

#pragma warning disable CS0162
        unsafe partial class Core
        {
            internal static readonly string[] UnaryOperatorNames = new string[(int)UnaryOperationMethod._Last]
            {
                "op_OnesComplement",
            };

            [Inline(InlineBehavior.Remove)]
            public static void Not(nint* ptr, nint* ptrEnd)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Not((int*)ptr, (int*)ptrEnd);
                        return;
                    case sizeof(long):
                        Core<long>.Not((long*)ptr, (long*)ptrEnd);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Not((int*)ptr, (int*)ptrEnd);
                                return;
                            case sizeof(long):
                                Core<long>.Not((long*)ptr, (long*)ptrEnd);
                                return;
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static void Not(nuint* ptr, nuint* ptrEnd)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        Core<uint>.Not((uint*)ptr, (uint*)ptrEnd);
                        return;
                    case sizeof(long):
                        Core<ulong>.Not((ulong*)ptr, (ulong*)ptrEnd);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                Core<uint>.Not((uint*)ptr, (uint*)ptrEnd);
                                return;
                            case sizeof(ulong):
                                Core<ulong>.Not((ulong*)ptr, (ulong*)ptrEnd);
                                return;
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
#pragma warning restore CS0162

        unsafe partial class Core<T>
        {
            private static readonly LazyTinyStruct<nint[]> _unaryOperatorsLazy = new LazyTinyStruct<nint[]>(InitializeUnaryOperators);

            private static nint[] InitializeUnaryOperators()
            {
                nint[] result = new nint[(int)UnaryOperationMethod._Last];
                for (int i = 0; i < (int)UnaryOperationMethod._Last; i++)
                    result[i] = GetFunctionPointerSafely(ReflectionHelper.GetMethod(typeof(T), Core.UnaryOperatorNames[i], [typeof(T)], typeof(T),
                        BindingFlags.Public | BindingFlags.Static));
                return result;
            }

            public static void Not(T* ptr, T* ptrEnd)
                => UnaryOperationCore(ref ptr, ptrEnd, UnaryOperationMethod.Not);

            [Inline(InlineBehavior.Remove)]
            private static void UnaryOperationCore(ref T* ptr, T* ptrEnd, [InlineParameter] UnaryOperationMethod method)
            {
                // Vector.IsHardwareAccelerated 與 Vector<T>.Count 會在執行時期被優化成常數，故不需要變數快取 (反而會妨礙 JIT 進行迴圈及條件展開)
                if (CheckTypeCanBeVectorized())
                {
                    VectorizedUnaryOperationCore(ref ptr, ptrEnd, method);
                    return;
                }
                if (UnsafeHelper.IsPrimitiveType<T>())
                {
                    for (; ptr < ptrEnd; ptr++)
                        *ptr = LegacyUnaryOperationCoreFast(*ptr, method);
                    return;
                }
                nint functionPointer = _unaryOperatorsLazy.Value[(int)method];
                if (functionPointer == default)
                    throw new InvalidOperationException($"Cannot find the {method} operator for {typeof(T).Name}!");
                for (; ptr < ptrEnd; ptr++)
                    *ptr = LegacyUnaryOperationCoreSlow(*ptr, functionPointer);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedUnaryOperationCore(ref T* ptr, T* ptrEnd, [InlineParameter] UnaryOperationMethod method)
            {
#if NET6_0_OR_GREATER
                if (Vector512.IsHardwareAccelerated)
                {
                    if (ptr + Vector512<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector512<T> valueVector = Vector512.Load(ptr);
                            VectorizedUnaryOperationCore_512(valueVector, method).Store(ptr);
                        }
                        while ((ptr += Vector512<T>.Count) < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                if (Vector256.IsHardwareAccelerated)
                {
                    if (ptr + Vector256<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector256<T> valueVector = Vector256.Load(ptr);
                            VectorizedUnaryOperationCore_256(valueVector, method).Store(ptr);
                        }
                        while ((ptr += Vector256<T>.Count) < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                if (Vector128.IsHardwareAccelerated)
                {
                    if (ptr + Vector128<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector128<T> valueVector = Vector128.Load(ptr);
                            VectorizedUnaryOperationCore_128(valueVector, method).Store(ptr);
                        }
                        while ((ptr += Vector128<T>.Count) < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                if (Vector64.IsHardwareAccelerated)
                {
                    if (ptr + Vector64<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector64<T> valueVector = Vector64.Load(ptr);
                            VectorizedUnaryOperationCore_64(valueVector, method).Store(ptr);
                        }
                        while ((ptr += Vector64<T>.Count) < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                    if (ptr + 2 < ptrEnd)
                    {
                        Vector64<T> valueVector = default;
                        uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                        UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                        valueVector = VectorizedUnaryOperationCore_64(valueVector, method);
                        UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                        return;
                    }
                    for (int i = 0; i < 2; i++) // CLR 編譯時會展開
                        *ptr++ = LegacyUnaryOperationCoreFast(*ptr, method);
                    return;
                }
#else
                if (Vector.IsHardwareAccelerated)
                {
                    if (ptr + Vector<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector<T> valueVector = UnsafeHelper.Read<Vector<T>>(ptr);
                            UnsafeHelper.Write(ptr, VectorizedUnaryOperationCore(valueVector, method));
                        }
                        while ((ptr += Vector<T>.Count) < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                    if (ptr + 2 < ptrEnd)
                    {
                        Vector<T> valueVector = default;
                        uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                        UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                        valueVector = VectorizedUnaryOperationCore(valueVector, method);
                        UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                        return;
                    }
                    for (int i = 0; i < 2; i++) // CLR 編譯時會展開
                        *ptr++ = LegacyUnaryOperationCoreFast(*ptr, method);
                    return;
                }
#endif
                for (; ptr < ptrEnd; ptr++)
                    *ptr = LegacyUnaryOperationCoreFast(*ptr, method);
                return;
            }

#if NET6_0_OR_GREATER
            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedUnaryOperationCore_512(in Vector512<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedUnaryOperationCore_256(in Vector256<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedUnaryOperationCore_128(in Vector128<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedUnaryOperationCore_64(in Vector64<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };
#else
            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedUnaryOperationCore(in Vector<T> valueVector,
                [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => ~valueVector,
                    _ => throw new InvalidOperationException(),
                };
#endif

            [Inline(InlineBehavior.Remove)]
            private static T LegacyUnaryOperationCoreFast(T item, [InlineParameter] UnaryOperationMethod method)
                => method switch
                {
                    UnaryOperationMethod.Not => UnsafeHelper.Not(item),
                    _ => throw new InvalidOperationException(),
                };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static T LegacyUnaryOperationCoreSlow(T item, nint functionPointer)
                => ((delegate* managed<T, T>)functionPointer)(item);
        }
    }
}
