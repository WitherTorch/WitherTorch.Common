using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
#pragma warning disable CS0162
#pragma warning disable CS8500
        unsafe partial class FastCore
        {
            [Inline(InlineBehavior.Remove)]
            public static nuint CountOf(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.CountOf((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.CountOf((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.CountOf((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.CountOf((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOfExclude(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.CountOfExclude((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.CountOfExclude((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.CountOfExclude((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.CountOfExclude((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOfGreaterThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.CountOfGreaterThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.CountOfGreaterThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.CountOfGreaterThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.CountOfGreaterThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOfGreaterThanOrEquals(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.CountOfGreaterThanOrEquals((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.CountOfGreaterThanOrEquals((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.CountOfGreaterThanOrEquals((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.CountOfGreaterThanOrEquals((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOfLessThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.CountOfLessThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.CountOfLessThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.CountOfLessThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.CountOfLessThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOfLessThanOrEquals(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.CountOfLessThanOrEquals((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.CountOfLessThanOrEquals((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.CountOfLessThanOrEquals((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.CountOfLessThanOrEquals((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOf(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.CountOf((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.CountOf((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.CountOf((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.CountOf((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOfExclude(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.CountOfExclude((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.CountOfExclude((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.CountOfExclude((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.CountOfExclude((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOfGreaterThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.CountOfGreaterThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.CountOfGreaterThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.CountOfGreaterThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.CountOfGreaterThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOfGreaterThanOrEquals(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.CountOfGreaterThanOrEquals((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.CountOfGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.CountOfGreaterThanOrEquals((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.CountOfGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOfLessThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.CountOfLessThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.CountOfLessThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.CountOfLessThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.CountOfLessThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint CountOfLessThanOrEquals(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.CountOfLessThanOrEquals((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.CountOfLessThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.CountOfLessThanOrEquals((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.CountOfLessThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
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
            public static nuint CountOf(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, ref length, value, CompareMethod.Include);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static nuint CountOfExclude(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, ref length, value, CompareMethod.Exclude);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static nuint CountOfGreaterThan(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, ref length, value, CompareMethod.GreaterThan);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static nuint CountOfLessThan(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, ref length, value, CompareMethod.LessThan);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static nuint CountOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, ref length, value, CompareMethod.GreaterThanOrEquals);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static nuint CountOfLessThanOrEquals(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, ref length, value, CompareMethod.LessThanOrEquals);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static nuint VectorizedCountOf(T* ptr, nuint length, T value)
                => VectorizedCountOfCore(ref ptr, ref length, value, CompareMethod.Include);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static nuint VectorizedCountOfExclude(T* ptr, nuint length, T value)
                => VectorizedCountOfCore(ref ptr, ref length, value, CompareMethod.Exclude);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static nuint VectorizedCountOfGreaterThan(T* ptr, nuint length, T value)
                => VectorizedCountOfCore(ref ptr, ref length, value, CompareMethod.GreaterThan);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static nuint VectorizedCountOfLessThan(T* ptr, nuint length, T value)
                => VectorizedCountOfCore(ref ptr, ref length, value, CompareMethod.LessThan);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static nuint VectorizedCountOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedCountOfCore(ref ptr, ref length, value, CompareMethod.GreaterThanOrEquals);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static nuint VectorizedCountOfLessThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedCountOfCore(ref ptr, ref length, value, CompareMethod.LessThanOrEquals);

            [Inline(InlineBehavior.Remove)]
            private static nuint CountOfCore(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method)
            {
                if (CheckTypeCanBeVectorized() && length > GetLimitForVectorizing())
                    return method switch
                    {
                        CompareMethod.Include => VectorizedCountOf(ptr, length, value),
                        CompareMethod.Exclude => VectorizedCountOfExclude(ptr, length, value),
                        CompareMethod.GreaterThan => VectorizedCountOfGreaterThan(ptr, length, value),
                        CompareMethod.GreaterThanOrEquals => VectorizedCountOfGreaterThanOrEquals(ptr, length, value),
                        CompareMethod.LessThan => VectorizedCountOfLessThan(ptr, length, value),
                        CompareMethod.LessThanOrEquals => VectorizedCountOfLessThanOrEquals(ptr, length, value),
                        _ => throw new ArgumentOutOfRangeException(nameof(method))
                    };

                return ScalarizedCountOfCore(ref ptr, ref length, value, method);
            }

            [LocalsInit(false)]
            [Inline(InlineBehavior.Remove)]
            private static partial nuint VectorizedCountOfCore(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method);

            [Inline(InlineBehavior.Remove)]
            private static nuint ScalarizedCountOfCore(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method)
            {
                nuint counter = 0;
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    counter +=
                        MathHelper.BooleanToNativeUnsigned(ScalarizedCompare(ptr[0], value, method)) +
                        MathHelper.BooleanToNativeUnsigned(ScalarizedCompare(ptr[1], value, method)) +
                        MathHelper.BooleanToNativeUnsigned(ScalarizedCompare(ptr[2], value, method)) +
                        MathHelper.BooleanToNativeUnsigned(ScalarizedCompare(ptr[3], value, method));
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    goto Result;
                counter += MathHelper.BooleanToNativeUnsigned(ScalarizedCompare(*ptr, value, method));
                ptr++;
                if (ptr >= ptrEnd)
                    goto Result;
                counter += MathHelper.BooleanToNativeUnsigned(ScalarizedCompare(*ptr, value, method));
                ptr++;
                if (ptr >= ptrEnd)
                    goto Result;
                counter += MathHelper.BooleanToNativeUnsigned(ScalarizedCompare(*ptr, value, method));

            Result:
                return counter;
            }
        }

        unsafe partial class SlowCore<T>
        {
            public static nuint CountOf(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, length, value, CompareMethod.Include);

            public static nuint CountOfExclude(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, length, value, CompareMethod.Exclude);

            public static nuint CountOfGreaterThan(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, length, value, CompareMethod.GreaterThan);

            public static nuint CountOfLessThan(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, length, value, CompareMethod.LessThan);

            public static nuint CountOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, length, value, CompareMethod.GreaterThanOrEquals);

            public static nuint CountOfLessThanOrEquals(T* ptr, nuint length, T value)
                => CountOfCore(ref ptr, length, value, CompareMethod.LessThanOrEquals);

            [Inline(InlineBehavior.Remove)]
            private static nuint CountOfCore(ref T* ptr, nuint length, T value, [InlineParameter] CompareMethod method)
            {
                nuint counter = 0;
                if (method == CompareMethod.Include || method == CompareMethod.Exclude)
                {
                    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (Compare(comparer, *ptr, value, method))
                            counter++;
                    }
                }
                else
                {
                    Comparer<T> comparer = Comparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (Compare(comparer, *ptr, value, method))
                            counter++;
                    }
                }
                return counter;
            }
        }
    }
}
