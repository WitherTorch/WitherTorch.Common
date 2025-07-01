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

#pragma warning disable CS0162
        unsafe partial class Core
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

            [Inline(InlineBehavior.Remove)]
            public static void Or(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Or((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Or((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Or((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Or((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void And(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.And((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.And((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.And((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.And((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Xor(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Xor((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Xor((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Xor((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Xor((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Add(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Add((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Add((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Add((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Add((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Substract(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Substract((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Substract((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Substract((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Substract((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Multiply(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Multiply((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Multiply((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Multiply((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Multiply((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Divide(nint* ptr, nint* ptrEnd, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Divide((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Divide((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Divide((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Divide((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Or(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Or((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Or((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Or((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Or((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void And(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.And((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.And((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.And((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.And((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Xor(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Xor((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Xor((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Xor((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Xor((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Add(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Add((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Add((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Add((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Add((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Substract(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Substract((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Substract((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Substract((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Substract((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Multiply(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Multiply((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Multiply((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Multiply((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Multiply((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            public static void Divide(nuint* ptr, nuint* ptrEnd, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        Core<int>.Divide((int*)ptr, (int*)ptrEnd, *(int*)&value);
                        return;
                    case sizeof(long):
                        Core<long>.Divide((long*)ptr, (long*)ptrEnd, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                Core<int>.Divide((int*)ptr, (int*)ptrEnd, *(int*)&value);
                                return;
                            case sizeof(long):
                                Core<long>.Divide((long*)ptr, (long*)ptrEnd, *(long*)&value);
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
            private static readonly LazyTinyStruct<nint[]> _binaryOperatorsLazy = new LazyTinyStruct<nint[]>(InitializeBinaryOperators);

            private static nint[] InitializeBinaryOperators()
            {
                nint[] result = new nint[(int)BinaryOperationMethod._Last];
                for (int i = 0; i < (int)BinaryOperationMethod._Last; i++)
                    result[i] = GetFunctionPointerSafely(ReflectionHelper.GetMethod(typeof(T), Core.BinaryOperatorNames[i], [typeof(T), typeof(T)], typeof(T),
                        BindingFlags.Public | BindingFlags.Static));
                return result;
            }

            public static void Or(T* ptr, T* ptrEnd, T value)
                => BinaryOperationCore(ref ptr, ptrEnd, value, BinaryOperationMethod.Or);

            public static void And(T* ptr, T* ptrEnd, T value)
                => BinaryOperationCore(ref ptr, ptrEnd, value, BinaryOperationMethod.And);

            public static void Xor(T* ptr, T* ptrEnd, T value)
                => BinaryOperationCore(ref ptr, ptrEnd, value, BinaryOperationMethod.Xor);

            public static void Add(T* ptr, T* ptrEnd, T value)
                => BinaryOperationCore(ref ptr, ptrEnd, value, BinaryOperationMethod.Add);

            public static void Substract(T* ptr, T* ptrEnd, T value)
                => BinaryOperationCore(ref ptr, ptrEnd, value, BinaryOperationMethod.Substract);

            public static void Multiply(T* ptr, T* ptrEnd, T value)
                => BinaryOperationCore(ref ptr, ptrEnd, value, BinaryOperationMethod.Multiply);

            public static void Divide(T* ptr, T* ptrEnd, T value)
                => BinaryOperationCore(ref ptr, ptrEnd, value, BinaryOperationMethod.Divide);

            [Inline(InlineBehavior.Remove)]
            private static void BinaryOperationCore(ref T* ptr, T* ptrEnd, T value, [InlineParameter] BinaryOperationMethod method)
            {
                if (CheckTypeCanBeVectorized())
                {
                    VectorizedBinaryOperationCore(ref ptr, ptrEnd, value, method);
                    return;
                }
                if (UnsafeHelper.IsPrimitiveType<T>())
                {
                    FastBinaryOperationCore(ref ptr, ptrEnd, value, method);
                    return;
                }
                SlowBinaryOperationCore(ref ptr, ptrEnd, value, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void VectorizedBinaryOperationCore(ref T* ptr, T* ptrEnd, T value, [InlineParameter] BinaryOperationMethod method)
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

                FastBinaryOperationCore(ref ptr, ptrEnd, value, method);
                return;

            Vector512:
                if (ptr + Vector512<T>.Count < ptrEnd)
                {
                    Vector512<T> maskVector = Vector512.Create(value); // 將要比對的項目擴充成向量
                    do
                    {
                        Vector512<T> valueVector = Vector512.Load(ptr);
                        VectorizedBinaryOperationCore_512(valueVector, maskVector, method).Store(ptr);
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
                    Vector512<T> maskVector = Vector512.Create(value); // 將要比對的項目擴充成向量
                    Vector512<T> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    valueVector = VectorizedBinaryOperationCore_512(valueVector, maskVector, method);
                    UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                    return;
                }
                for (int i = 0; i < Vector512<T>.Count / 2; i++)
                {
                    *ptr++ = LegacyBinaryOperationCoreFast(*ptr, value, method);
                    if (++ptr >= ptrEnd)
                        return;
                }
                return;

            Vector256:
                if (ptr + Vector256<T>.Count < ptrEnd)
                {
                    Vector256<T> maskVector = Vector256.Create(value); // 將要比對的項目擴充成向量
                    do
                    {
                        Vector256<T> valueVector = Vector256.Load(ptr);
                        VectorizedBinaryOperationCore_256(valueVector, maskVector, method).Store(ptr);
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
                    Vector256<T> maskVector = Vector256.Create(value); // 將要比對的項目擴充成向量
                    Vector256<T> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    valueVector = VectorizedBinaryOperationCore_256(valueVector, maskVector, method);
                    UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                    return;
                }
                for (int i = 0; i < Vector256<T>.Count / 2; i++)
                {
                    *ptr++ = LegacyBinaryOperationCoreFast(*ptr, value, method);
                    if (++ptr >= ptrEnd)
                        return;
                }
                return;

            Vector128:
                if (ptr + Vector128<T>.Count < ptrEnd)
                {
                    Vector128<T> maskVector = Vector128.Create(value); // 將要比對的項目擴充成向量
                    do
                    {
                        Vector128<T> valueVector = Vector128.Load(ptr);
                        VectorizedBinaryOperationCore_128(valueVector, maskVector, method).Store(ptr);
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
                    Vector128<T> maskVector = Vector128.Create(value); // 將要比對的項目擴充成向量
                    Vector128<T> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    valueVector = VectorizedBinaryOperationCore_128(valueVector, maskVector, method);
                    UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                    return;
                }
                for (int i = 0; i < Vector128<T>.Count / 2; i++)
                {
                    *ptr++ = LegacyBinaryOperationCoreFast(*ptr, value, method);
                    if (++ptr >= ptrEnd)
                        return;
                }
                return;

            Vector64:
                if (ptr + Vector64<T>.Count < ptrEnd)
                {
                    Vector64<T> maskVector = Vector64.Create(value); // 將要比對的項目擴充成向量
                    do
                    {
                        Vector64<T> valueVector = Vector64.Load(ptr);
                        VectorizedBinaryOperationCore_64(valueVector, maskVector, method).Store(ptr);
                        ptr += Vector64<T>.Count;
                    }
                    while (ptr + Vector64<T>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return;
                }
                if (ptr + Vector64<T>.Count / 2 < ptrEnd)
                {
                    Vector64<T> maskVector = Vector64.Create(value); // 將要比對的項目擴充成向量
                    Vector64<T> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    valueVector = VectorizedBinaryOperationCore_64(valueVector, maskVector, method);
                    UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                    return;
                }
                for (int i = 0; i < Vector64<T>.Count / 2; i++)
                {
                    *ptr++ = LegacyBinaryOperationCoreFast(*ptr, value, method);
                    if (++ptr >= ptrEnd)
                        return;
                }
                return;

#else
                if (Vector.IsHardwareAccelerated)
                    goto Vector;

                FastBinaryOperationCore(ref ptr, ptrEnd, value, method);
                return;

            Vector:
                if (ptr + Vector<T>.Count < ptrEnd)
                {
                    Vector<T> maskVector = new Vector<T>(value); // 將要比對的項目擴充成向量
                    do
                    {
                        Vector<T> valueVector = UnsafeHelper.Read<Vector<T>>(ptr);
                        UnsafeHelper.Write(ptr, VectorizedBinaryOperationCore(valueVector, maskVector, method));
                        ptr += Vector<T>.Count;
                    }
                    while (ptr + Vector<T>.Count < ptrEnd);
                    if (ptr >= ptrEnd)
                        return;
                }
                if (ptr + Vector<T>.Count / 2 < ptrEnd)
                {
                    Vector<T> maskVector = new Vector<T>(value); // 將要比對的項目擴充成向量
                    Vector<T> valueVector = default;
                    uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                    UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                    valueVector = VectorizedBinaryOperationCore(valueVector, maskVector, method);
                    UnsafeHelper.CopyBlockUnaligned(ptr, &valueVector, byteCount);
                    return;
                }
                for (int i = 0; i < Vector<T>.Count / 2; i++)
                {
                    *ptr++ = LegacyBinaryOperationCoreFast(*ptr, value, method);
                    if (++ptr >= ptrEnd)
                        return;
                }
                return;
#endif
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
            private static void FastBinaryOperationCore(ref T* ptr, T* ptrEnd, T value, [InlineParameter] BinaryOperationMethod method)
            {
                for (; ptr < ptrEnd; ptr++)
                    *ptr = LegacyBinaryOperationCoreFast(*ptr, value, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static void SlowBinaryOperationCore(ref T* ptr, T* ptrEnd, T value, [InlineParameter] BinaryOperationMethod method)
            {
                nint functionPointer = _binaryOperatorsLazy.Value[(int)method];
                if (functionPointer == default)
                    throw new InvalidOperationException($"Cannot find the {method} operator for {typeof(T).Name}!");
                for (; ptr < ptrEnd; ptr++)
                    *ptr = LegacyBinaryOperationCoreSlow(*ptr, value, functionPointer);
            }

            [Inline(InlineBehavior.Remove)]
            private static T LegacyBinaryOperationCoreFast(T item, T value, [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => UnsafeHelper.Or(item, value),
                    BinaryOperationMethod.And => UnsafeHelper.And(item, value),
                    BinaryOperationMethod.Xor => UnsafeHelper.Xor(item, value),
                    BinaryOperationMethod.Add => UnsafeHelper.Add(item, value),
                    BinaryOperationMethod.Substract => UnsafeHelper.Substract(item, value),
                    BinaryOperationMethod.Multiply => UnsafeHelper.Multiply(item, value),
                    BinaryOperationMethod.Divide => IsUnsigned() ? UnsafeHelper.DivideUnsigned(item, value) : UnsafeHelper.Divide(item, value),
                    _ => throw new InvalidOperationException(),
                };

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static T LegacyBinaryOperationCoreSlow(T item, T value, nint functionPointer)
                => ((delegate* managed<T, T, T>)functionPointer)(item, value);
        }
    }
}
