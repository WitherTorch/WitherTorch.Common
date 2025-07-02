using InlineMethod;

using System;
using System.Runtime.CompilerServices;
using System.Reflection;

using WitherTorch.Common.Threading;

using LocalsInit;

#if NET6_0_OR_GREATER
using System.Runtime.Intrinsics;
#else
using System.Numerics;
#endif

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        private enum BinaryOperationMethod
        {
            Or,
            And,
            Xor,
            Add,
            Substract,
            Multiply,
            Divide,
            _Last
        }

#pragma warning disable CS8500
#pragma warning disable CS0162
        unsafe partial class FastCore
        {
            [Inline(InlineBehavior.Remove)]
            public static void Or(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Or((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.Or((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Or((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.Or((long*)ptr, length, *(long*)&value);
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
            public static void And(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.And((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.And((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.And((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.And((long*)ptr, length, *(long*)&value);
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
            public static void Xor(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Xor((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.Xor((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Xor((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.Xor((long*)ptr, length, *(long*)&value);
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
            public static void Add(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Add((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.Add((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Add((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.Add((long*)ptr, length, *(long*)&value);
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
            public static void Substract(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Substract((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.Substract((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Substract((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.Substract((long*)ptr, length, *(long*)&value);
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
            public static void Multiply(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Multiply((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.Multiply((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Multiply((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.Multiply((long*)ptr, length, *(long*)&value);
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
            public static void Divide(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Divide((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.Divide((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Divide((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.Divide((long*)ptr, length, *(long*)&value);
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
            public static void Or(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Or((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Or((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Or((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Or((ulong*)ptr, length, *(ulong*)&value);
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
            public static void And(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.And((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.And((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.And((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.And((ulong*)ptr, length, *(ulong*)&value);
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
            public static void Xor(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Xor((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Xor((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Xor((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Xor((ulong*)ptr, length, *(ulong*)&value);
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
            public static void Add(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Add((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Add((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Add((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Add((ulong*)ptr, length, *(ulong*)&value);
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
            public static void Substract(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Substract((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Substract((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Substract((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Substract((ulong*)ptr, length, *(ulong*)&value);
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
            public static void Multiply(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Multiply((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Multiply((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Multiply((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Multiply((ulong*)ptr, length, *(ulong*)&value);
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
            public static void Divide(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Divide((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Divide((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Divide((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Divide((ulong*)ptr, length, *(ulong*)&value);
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

        unsafe partial class FastCore<T>
        {
            public static void Or(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Or);

            public static void And(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.And);

            public static void Xor(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Xor);

            public static void Add(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Add);

            public static void Substract(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Substract);

            public static void Multiply(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Multiply);

            public static void Divide(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Divide);

            [Inline(InlineBehavior.Remove)]
            private static void BinaryOperationCore(ref T* ptr, nuint length, T value, [InlineParameter] BinaryOperationMethod method)
            {
                T* ptrEnd = ptr + length;
                if (CheckTypeCanBeVectorized())
                {
                    VectorizedBinaryOperationCore(ref ptr, ptrEnd, value, method);
                    return;
                }
                LegacyBinaryOperationCore(ref ptr, ptrEnd, value, method);
                return;
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore(ref T* ptr, T* ptrEnd, T value, [InlineParameter] BinaryOperationMethod method)
            {
#if NET6_0_OR_GREATER
                if (Limits.UseVector512())
                {
                    Vector512<T>* ptrLimit = ((Vector512<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector512<T> maskVector = Vector512.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector512<T> valueVector = Vector512.Load(ptr);
                            VectorizedBinaryOperationCore_512(valueVector, maskVector, method).Store(ptr);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                if (Limits.UseVector256())
                {
                    Vector256<T>* ptrLimit = ((Vector256<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector256<T> maskVector = Vector256.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector256<T> valueVector = Vector256.Load(ptr);
                            VectorizedBinaryOperationCore_256(valueVector, maskVector, method).Store(ptr);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                if (Limits.UseVector128())
                {
                    Vector128<T>* ptrLimit = ((Vector128<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector128<T> maskVector = Vector128.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector128<T> valueVector = Vector128.Load(ptr);
                            VectorizedBinaryOperationCore_128(valueVector, maskVector, method).Store(ptr);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
                if (Limits.UseVector64())
                {
                    Vector64<T>* ptrLimit = ((Vector64<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector64<T> maskVector = Vector64.Create(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector64<T> valueVector = Vector64.Load(ptr);
                            VectorizedBinaryOperationCore_64(valueVector, maskVector, method).Store(ptr);
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
#else
                if (Limits.UseVector())
                {
                    Vector<T>* ptrLimit = ((Vector<T>*)ptr) + 1;
                    if (ptrLimit < ptrEnd)
                    {
                        Vector<T> maskVector = new Vector<T>(value); // 將要比對的項目擴充成向量
                        do
                        {
                            Vector<T> valueVector = UnsafeHelper.ReadUnaligned<Vector<T>>(ptr);
                            UnsafeHelper.WriteUnaligned(ptr, VectorizedBinaryOperationCore(valueVector, maskVector, method));
                            ptr = (T*)ptrLimit;
                        } while (++ptrLimit < ptrEnd);
                        if (ptr >= ptrEnd)
                            return;
                    }
                }
#endif
                LegacyBinaryOperationCore(ref ptr, ptrEnd, value, method);
            }

#if NET6_0_OR_GREATER
            [Inline(InlineBehavior.Remove)]
            private static Vector512<T> VectorizedBinaryOperationCore_512(in Vector512<T> valueVector, in Vector512<T> maskVector,
                [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => valueVector | maskVector,
                    BinaryOperationMethod.And => valueVector & maskVector,
                    BinaryOperationMethod.Xor => valueVector ^ maskVector,
                    BinaryOperationMethod.Add => valueVector + maskVector,
                    BinaryOperationMethod.Substract => valueVector - maskVector,
                    BinaryOperationMethod.Multiply => valueVector * maskVector,
                    BinaryOperationMethod.Divide => valueVector / maskVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector256<T> VectorizedBinaryOperationCore_256(in Vector256<T> valueVector, in Vector256<T> maskVector,
                [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => valueVector | maskVector,
                    BinaryOperationMethod.And => valueVector & maskVector,
                    BinaryOperationMethod.Xor => valueVector ^ maskVector,
                    BinaryOperationMethod.Add => valueVector + maskVector,
                    BinaryOperationMethod.Substract => valueVector - maskVector,
                    BinaryOperationMethod.Multiply => valueVector * maskVector,
                    BinaryOperationMethod.Divide => valueVector / maskVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector128<T> VectorizedBinaryOperationCore_128(in Vector128<T> valueVector, in Vector128<T> maskVector,
                [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => valueVector | maskVector,
                    BinaryOperationMethod.And => valueVector & maskVector,
                    BinaryOperationMethod.Xor => valueVector ^ maskVector,
                    BinaryOperationMethod.Add => valueVector + maskVector,
                    BinaryOperationMethod.Substract => valueVector - maskVector,
                    BinaryOperationMethod.Multiply => valueVector * maskVector,
                    BinaryOperationMethod.Divide => valueVector / maskVector,
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static Vector64<T> VectorizedBinaryOperationCore_64(in Vector64<T> valueVector, in Vector64<T> maskVector,
                [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => valueVector | maskVector,
                    BinaryOperationMethod.And => valueVector & maskVector,
                    BinaryOperationMethod.Xor => valueVector ^ maskVector,
                    BinaryOperationMethod.Add => valueVector + maskVector,
                    BinaryOperationMethod.Substract => valueVector - maskVector,
                    BinaryOperationMethod.Multiply => valueVector * maskVector,
                    BinaryOperationMethod.Divide => valueVector / maskVector,
                    _ => throw new InvalidOperationException(),
                };
#else
            [Inline(InlineBehavior.Remove)]
            private static Vector<T> VectorizedBinaryOperationCore(in Vector<T> valueVector, in Vector<T> maskVector,
                [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => valueVector | maskVector,
                    BinaryOperationMethod.And => valueVector & maskVector,
                    BinaryOperationMethod.Xor => valueVector ^ maskVector,
                    BinaryOperationMethod.Add => valueVector + maskVector,
                    BinaryOperationMethod.Substract => valueVector - maskVector,
                    BinaryOperationMethod.Multiply => valueVector * maskVector,
                    BinaryOperationMethod.Divide => valueVector / maskVector,
                    _ => throw new InvalidOperationException(),
                };
#endif

            [Inline(InlineBehavior.Remove)]
            private static void LegacyBinaryOperationCore(ref T* ptr, T* ptrEnd, T value, [InlineParameter] BinaryOperationMethod method)
            {
                for (; ptr < ptrEnd; ptr++)
                    *ptr = LegacyBinaryOperationCore(*ptr, value, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static T LegacyBinaryOperationCore(T item, T value, [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => UnsafeHelper.Or(item, value),
                    BinaryOperationMethod.And => UnsafeHelper.And(item, value),
                    BinaryOperationMethod.Xor => UnsafeHelper.Xor(item, value),
                    BinaryOperationMethod.Add => UnsafeHelper.Add(item, value),
                    BinaryOperationMethod.Substract => UnsafeHelper.Substract(item, value),
                    BinaryOperationMethod.Multiply => UnsafeHelper.Multiply(item, value),
                    BinaryOperationMethod.Divide => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.DivideUnsigned(item, value) : UnsafeHelper.Divide(item, value),
                    _ => throw new InvalidOperationException(),
                };
        }
        unsafe partial class SlowCore
        {
            internal static readonly string[] BinaryOperatorNames = new string[(int)BinaryOperationMethod._Last]
            {
                "op_BitwiseOr",
                "op_BitwiseAnd",
                "op_ExclusiveOr",
                "op_Addition",
                "op_Subtraction",
                "op_Multiply",
                "op_Division"
            };
        }

        unsafe partial class SlowCore<T>
        {
            private static readonly LazyTinyStruct<nint[]> _binaryOperatorsLazy = new LazyTinyStruct<nint[]>(InitializeBinaryOperators);

            private static nint[] InitializeBinaryOperators()
            {
                nint[] result = new nint[(int)BinaryOperationMethod._Last];
                for (int i = 0; i < (int)BinaryOperationMethod._Last; i++)
                    result[i] = ReflectionHelper.GetMethodPointer(typeof(T), SlowCore.BinaryOperatorNames[i], [typeof(T), typeof(T)], typeof(T),
                        BindingFlags.Public | BindingFlags.Static);
                return result;
            }

            public static void Or(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Or);

            public static void And(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.And);

            public static void Xor(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Xor);

            public static void Add(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Add);

            public static void Substract(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Substract);

            public static void Multiply(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Multiply);

            public static void Divide(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, length, value, BinaryOperationMethod.Divide);

            [Inline(InlineBehavior.Remove)]
            private static void BinaryOperationCore(ref T* ptr, nuint length, T value, [InlineParameter] BinaryOperationMethod method)
            {
                nint functionPointer = _binaryOperatorsLazy.Value[(int)method];
                if (functionPointer == default)
                    throw new InvalidOperationException($"Cannot find the {method} operator for {typeof(T).Name}!");
                for (nuint i = 0; i < length; i++, ptr++)
                    *ptr = BinaryOperationCore(*ptr, value, functionPointer);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static T BinaryOperationCore(T item, T value, nint functionPointer)
                => ((delegate* managed<T, T, T>)functionPointer)(item, value);
        }
    }
}
