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
                if (CheckTypeCanBeVectorized())
                {
                    VectorizedUnaryOperationCore(ref ptr, ptrEnd, method);
                    return;
                }
                if (UnsafeHelper.IsPrimitiveType<T>())
                {
                    FastUnaryOperationCore(ref ptr, ptrEnd, method);
                    return;
                }
                SlowUnaryOperationCore(ref ptr, ptrEnd, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedUnaryOperationCore(ref T* ptr, T* ptrEnd, [InlineParameter] UnaryOperationMethod method)
            {
#if NET6_0_OR_GREATER
                if (Vector512.IsHardwareAccelerated)
                    goto Vector512;
                if (Vector256.IsHardwareAccelerated)
                    goto Vector256;
                if (Vector128.IsHardwareAccelerated)
                    goto Vector128;
                if (Limits.UseVector64Acceleration && Vector64.IsHardwareAccelerated)
                    goto Vector64;

                FastUnaryOperationCore(ref ptr, ptrEnd, method);
                return;

                Vector512:
                if (ptr + Vector512<T>.Count < ptrEnd)
                {
                    do
                    {
                        Vector512<T> valueVector = Vector512.Load(ptr);
                        VectorizedUnaryOperationCore_512(valueVector, method).Store(ptr);
                        ptr += Vector512<T>.Count;
                    }
                    while (ptr + Vector512<T>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return;
                }
                if (Vector256.IsHardwareAccelerated)
                    goto Vector256;
                if (Vector128.IsHardwareAccelerated)
                    goto Vector128;
                if (Limits.UseVector64Acceleration && Vector64.IsHardwareAccelerated)
                    goto Vector64;
                if (ptr + Vector512<T>.Count / 2 < ptrEnd)
                {
                    Vector512<T> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    valueVector = VectorizedUnaryOperationCore_512(valueVector, method);
                    UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                    return;
                }
                for (int i = 0; i < Vector512<T>.Count / 2; i++)
                {
                    *ptr = LegacyUnaryOperationCoreFast(*ptr, method);
                    if (++ptr >= ptrEnd)
                        break;
                }
                return;

            Vector256:
                if (ptr + Vector256<T>.Count < ptrEnd)
                {
                    do
                    {
                        Vector256<T> valueVector = Vector256.Load(ptr);
                        VectorizedUnaryOperationCore_256(valueVector, method).Store(ptr);
                        ptr += Vector256<T>.Count;
                    }
                    while (ptr + Vector256<T>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return;
                }
                if (Vector128.IsHardwareAccelerated)
                    goto Vector128;
                if (Limits.UseVector64Acceleration && Vector64.IsHardwareAccelerated)
                    goto Vector64;
                if (ptr + Vector256<T>.Count / 2 < ptrEnd)
                {
                    Vector256<T> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    valueVector = VectorizedUnaryOperationCore_256(valueVector, method);
                    UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                    return;
                }
                for (int i = 0; i < Vector256<T>.Count / 2; i++)
                {
                    *ptr = LegacyUnaryOperationCoreFast(*ptr, method);
                    if (++ptr >= ptrEnd)
                        break;
                }
                return;

            Vector128:
                if (ptr + Vector128<T>.Count < ptrEnd)
                {
                    do
                    {
                        Vector128<T> valueVector = Vector128.Load(ptr);
                        VectorizedUnaryOperationCore_128(valueVector, method).Store(ptr);
                        ptr += Vector128<T>.Count;
                    }
                    while (ptr + Vector128<T>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return;
                }
                if (Limits.UseVector64Acceleration && Vector64.IsHardwareAccelerated)
                    goto Vector64;
                if (ptr + Vector128<T>.Count / 2 < ptrEnd)
                {
                    Vector128<T> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    valueVector = VectorizedUnaryOperationCore_128(valueVector, method);
                    UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                    return;
                }
                for (int i = 0; i < Vector128<T>.Count / 2; i++)
                {
                    *ptr = LegacyUnaryOperationCoreFast(*ptr, method);
                    if (++ptr >= ptrEnd)
                        break;
                }
                return;

            Vector64:
                if (ptr + Vector64<T>.Count < ptrEnd)
                {
                    do
                    {
                        Vector64<T> valueVector = Vector64.Load(ptr);
                        VectorizedUnaryOperationCore_64(valueVector, method).Store(ptr);
                        ptr += Vector64<T>.Count;
                    }
                    while (ptr + Vector64<T>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return;
                }
                if (ptr + Vector64<T>.Count / 2 < ptrEnd)
                {
                    Vector64<T> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    valueVector = VectorizedUnaryOperationCore_64(valueVector, method);
                    UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                    return;
                }
                for (int i = 0; i < Vector64<T>.Count / 2; i++)
                {
                    *ptr = LegacyUnaryOperationCoreFast(*ptr, method);
                    if (++ptr >= ptrEnd)
                        break;
                }
                return;
#else
                if (Vector.IsHardwareAccelerated)
                    goto Vector;

                FastUnaryOperationCore(ref ptr, ptrEnd, method);
                return;

            Vector:
                if (ptr + Vector<T>.Count < ptrEnd)
                {
                    do
                    {
                        Vector<T> valueVector = UnsafeHelper.Read<Vector<T>>(ptr);
                        UnsafeHelper.Write(ptr, VectorizedUnaryOperationCore(valueVector, method));
                        ptr += Vector<T>.Count;
                    }
                    while (ptr + Vector<T>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return;
                }
                if (ptr + Vector<T>.Count / 2 < ptrEnd)
                {
                    Vector<T> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    valueVector = VectorizedUnaryOperationCore(valueVector, method);
                    UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                    return;
                }
                for (int i = 0; i < Vector<T>.Count / 2; i++)
                {
                    *ptr = LegacyUnaryOperationCoreFast(*ptr, method);
                    if (++ptr >= ptrEnd)
                        return;
                }
#endif
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
            private static void VectorizedUnaryOperationCore_Internal(ref T* ptr, T* ptrEnd, [InlineParameter] UnaryOperationMethod method)
            {
            }

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
            private static void FastUnaryOperationCore(ref T* ptr, T* ptrEnd, [InlineParameter] UnaryOperationMethod method)
            {
                for (; ptr < ptrEnd; ptr++)
                    *ptr = LegacyUnaryOperationCoreFast(*ptr, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void SlowUnaryOperationCore(ref T* ptr, T* ptrEnd, [InlineParameter] UnaryOperationMethod method)
            {
                nint functionPointer = _unaryOperatorsLazy.Value[(int)method];
                if (functionPointer == default)
                    throw new InvalidOperationException($"Cannot find the {method} operator for {typeof(T).Name}!");
                for (; ptr < ptrEnd; ptr++)
                    *ptr = LegacyUnaryOperationCoreSlow(*ptr, functionPointer);
            }

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
