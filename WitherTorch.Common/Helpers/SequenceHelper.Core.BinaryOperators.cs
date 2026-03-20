using InlineMethod;

using System;
using System.Runtime.CompilerServices;
using System.Reflection;

using WitherTorch.Common.Threading;

using LocalsInit;

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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Or(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Or);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void And(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.And);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Xor(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Xor);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Add(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Add);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Substract(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Substract);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Multiply(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Multiply);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Divide(T* ptr, nuint length, T value)
                => BinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Divide);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedOr(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Or);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedAnd(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.And);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedXor(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Xor);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedAdd(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Add);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedSubstract(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Substract);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedMultiply(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Multiply);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedDivide(T* ptr, nuint length, T value)
                => VectorizedBinaryOperationCore(ref ptr, ref length, value, BinaryOperationMethod.Divide);

            [Inline(InlineBehavior.Remove)]
            private static void BinaryOperationCore(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperationMethod method)
            {
                if (CheckTypeCanBeVectorized() && length > GetLimitForVectorizing())
                {
                    switch (method)
                    {
                        case BinaryOperationMethod.Or:
                            VectorizedOr(ptr, length, value);
                            break;
                        case BinaryOperationMethod.And:
                            VectorizedAnd(ptr, length, value);
                            break;
                        case BinaryOperationMethod.Xor:
                            VectorizedXor(ptr, length, value);
                            break;
                        case BinaryOperationMethod.Add:
                            VectorizedAdd(ptr, length, value);
                            break;
                        case BinaryOperationMethod.Substract:
                            VectorizedSubstract(ptr, length, value);
                            break;
                        case BinaryOperationMethod.Multiply:
                            VectorizedMultiply(ptr, length, value);
                            break;
                        case BinaryOperationMethod.Divide:
                            VectorizedDivide(ptr, length, value);
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
            private static partial void VectorizedBinaryOperationCore(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperationMethod method);

            [Inline(InlineBehavior.Remove)]
            private static void ScalarizedBinaryOperationCore(ref T* ptr, ref nuint length, T value, [InlineParameter] BinaryOperationMethod method)
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
            private static T ScalarizedBinaryOperation(T item, T value, [InlineParameter] BinaryOperationMethod method)
                => method switch
                {
                    BinaryOperationMethod.Or => UnsafeHelper.Or(item, value),
                    BinaryOperationMethod.And => UnsafeHelper.And(item, value),
                    BinaryOperationMethod.Xor => UnsafeHelper.Xor(item, value),
                    BinaryOperationMethod.Add => UnsafeHelper.Add(item, value),
                    BinaryOperationMethod.Substract => UnsafeHelper.Substract(item, value),
                    BinaryOperationMethod.Multiply => UnsafeHelper.Multiply(item, value),
                    BinaryOperationMethod.Divide => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.DivideUnsigned(item, value) : UnsafeHelper.Divide(item, value),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };
        }

        partial class SlowCore
        {
            internal static readonly string[] BinaryOperatorNames = new string[(int)BinaryOperationMethod._Last]
            {
                "op_BitwiseOr",
                "op_BitwiseAnd",
                "op_ExclusiveOr",
                "op_Addition",
                "op_Substraction",
                "op_Multiply",
                "op_Division"
            };
        }

        unsafe partial class SlowCore<T>
        {
            private static readonly LazyTiny<nint[]> _binaryOperatorsLazy = new LazyTiny<nint[]>(InitializeBinaryOperators);

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
