using System;
using System.Runtime.CompilerServices;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
#pragma warning disable CS8500
#pragma warning disable CS0162
        unsafe partial class FastCore
        {
            [Inline(InlineBehavior.Remove)]
            public static void Right(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Right((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.Right((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Right((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.Right((long*)ptr, length, *(long*)&value);
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
            public static void Subtract(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Subtract((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.Subtract((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Subtract((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.Subtract((long*)ptr, length, *(long*)&value);
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
            public static void Min(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Min((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.Min((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Min((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.Min((long*)ptr, length, *(long*)&value);
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
            public static void Max(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Max((int*)ptr, length, *(int*)&value);
                        return;
                    case sizeof(long):
                        FastCore<long>.Max((long*)ptr, length, *(long*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Max((int*)ptr, length, *(int*)&value);
                                return;
                            case sizeof(long):
                                FastCore<long>.Max((long*)ptr, length, *(long*)&value);
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
            public static void Right(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Right((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Right((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Right((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Right((ulong*)ptr, length, *(ulong*)&value);
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
            public static void Subtract(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Subtract((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Subtract((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Subtract((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Subtract((ulong*)ptr, length, *(ulong*)&value);
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

            [Inline(InlineBehavior.Remove)]
            public static void Min(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Min((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Min((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Min((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Min((ulong*)ptr, length, *(ulong*)&value);
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
            public static void Max(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Max((uint*)ptr, length, *(uint*)&value);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Max((ulong*)ptr, length, *(ulong*)&value);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Max((uint*)ptr, length, *(uint*)&value);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Max((ulong*)ptr, length, *(ulong*)&value);
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Right(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Right);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Or(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Or);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void And(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.And);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Xor(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Xor);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Add(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Add);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Subtract(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Subtract);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Multiply(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Multiply);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Divide(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Divide);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Min(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Min);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Max(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Max);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedRight(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Right);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedOr(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Or);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedAnd(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.And);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedXor(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Xor);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedAdd(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Add);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedSubtract(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Subtract);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedMultiply(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Multiply);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedDivide(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Divide);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedMin(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Min);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedMax(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Max);

            [Inline(InlineBehavior.Remove)]
            private static void BinaryOperationCore(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperatorType method)
            {
                if (Limits.CheckTypeCanBeVectorized<T>() && length > Limits.GetLimitForVectorizing<T>())
                {
                    switch (method)
                    {
                        case BinaryOperatorType.Right:
                            VectorizedRight(ptr, length, value);
                            break;
                        case BinaryOperatorType.Or:
                            VectorizedOr(ptr, length, value);
                            break;
                        case BinaryOperatorType.And:
                            VectorizedAnd(ptr, length, value);
                            break;
                        case BinaryOperatorType.Xor:
                            VectorizedXor(ptr, length, value);
                            break;
                        case BinaryOperatorType.Add:
                            VectorizedAdd(ptr, length, value);
                            break;
                        case BinaryOperatorType.Subtract:
                            VectorizedSubtract(ptr, length, value);
                            break;
                        case BinaryOperatorType.Multiply:
                            VectorizedMultiply(ptr, length, value);
                            break;
                        case BinaryOperatorType.Divide:
                            VectorizedDivide(ptr, length, value);
                            break;
                        case BinaryOperatorType.Min:
                            VectorizedMin(ptr, length, value);
                            break;
                        case BinaryOperatorType.Max:
                            VectorizedMax(ptr, length, value);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(method));
                    }
                    return;
                }
                ScalarizedBinaryOperationCore(ref ptr, ref length, value, method);
                return;
            }

            [Inline(InlineBehavior.Remove)]
            private static partial void VectorizedBinaryOperationCore(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperatorType type);

            [Inline(InlineBehavior.Remove)]
            private static void ScalarizedBinaryOperationCore(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperatorType method)
            {
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    ptr[0] = ScalarizedBinaryOperation(ptr[0], value, method);
                    ptr[1] = ScalarizedBinaryOperation(ptr[1], value, method);
                    ptr[2] = ScalarizedBinaryOperation(ptr[2], value, method);
                    ptr[3] = ScalarizedBinaryOperation(ptr[3], value, method);
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    return;
                *ptr = ScalarizedBinaryOperation(*ptr, value, method);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = ScalarizedBinaryOperation(*ptr, value, method);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = ScalarizedBinaryOperation(*ptr, value, method);
            }

            [Inline(InlineBehavior.Remove)]
            private static T ScalarizedBinaryOperation(T item, T value, [InlineParameter] BinaryOperatorType method)
                => method switch
                {
                    BinaryOperatorType.Left => item,
                    BinaryOperatorType.Right => value,
                    BinaryOperatorType.Or => UnsafeHelper.Or(item, value),
                    BinaryOperatorType.And => UnsafeHelper.And(item, value),
                    BinaryOperatorType.Xor => UnsafeHelper.Xor(item, value),
                    BinaryOperatorType.Add => UnsafeHelper.Add(item, value),
                    BinaryOperatorType.Subtract => UnsafeHelper.Subtract(item, value),
                    BinaryOperatorType.Multiply => UnsafeHelper.Multiply(item, value),
                    BinaryOperatorType.Divide => UnsafeHelper.IsUnsignedIntegerType<T>() ? UnsafeHelper.DivideUnsigned(item, value) : UnsafeHelper.Divide(item, value),
                    BinaryOperatorType.Min => UnsafeHelper.Min(item, value),
                    BinaryOperatorType.Max => UnsafeHelper.Max(item, value),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }

        unsafe partial class SlowCore<T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Right(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Right);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Or(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Or);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void And(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.And);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Xor(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Xor);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Add(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Add);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Subtract(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Subtract);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Multiply(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Multiply);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Divide(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Divide);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Min(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Min);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Max(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperatorType.Max);

            [Inline(InlineBehavior.Remove)]
            private static void BinaryOperationCore(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperatorType operatorType)
            {
                if (operatorType == BinaryOperatorType.Left)
                    return;
                BinaryOperationCore(ptr, length, value, operatorType switch
                {
                    BinaryOperatorType.Left => BinaryOperator<T>.Left,
                    BinaryOperatorType.Right => BinaryOperator<T>.Right,
                    BinaryOperatorType.Or => BinaryOperator<T>.Or,
                    BinaryOperatorType.And => BinaryOperator<T>.And,
                    BinaryOperatorType.Xor => BinaryOperator<T>.Xor,
                    BinaryOperatorType.Add => BinaryOperator<T>.Add,
                    BinaryOperatorType.Subtract => BinaryOperator<T>.Subtract,
                    BinaryOperatorType.Multiply => BinaryOperator<T>.Multiply,
                    BinaryOperatorType.Divide => BinaryOperator<T>.Divide,
                    BinaryOperatorType.Min => BinaryOperator<T>.Min,
                    BinaryOperatorType.Max => BinaryOperator<T>.Max,
                    _ => throw new ArgumentOutOfRangeException(nameof(operatorType))
                });
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void BinaryOperationCore(T* ptr, nuint length, T value, IBinaryOperator<T> @operator)
            {
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    ptr[0] = @operator.Operate(ptr[0], value);
                    ptr[1] = @operator.Operate(ptr[1], value);
                    ptr[2] = @operator.Operate(ptr[2], value);
                    ptr[3] = @operator.Operate(ptr[3], value);
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    return;
                *ptr = @operator.Operate(*ptr, value);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = @operator.Operate(*ptr, value);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = @operator.Operate(*ptr, value);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void BinaryOperationCore(T* ptr, nuint length, T value, BinaryOperation<T> operation)
            {
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    ptr[0] = operation.Invoke(ptr[0], value);
                    ptr[1] = operation.Invoke(ptr[1], value);
                    ptr[2] = operation.Invoke(ptr[2], value);
                    ptr[3] = operation.Invoke(ptr[3], value);
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    return;
                *ptr = operation.Invoke(*ptr, value);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = operation.Invoke(*ptr, value);
                ptr++;
                if (ptr >= ptrEnd)
                    return;
                *ptr = operation.Invoke(*ptr, value);
            }
        }
    }
}
