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
            public static bool Contains(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.Contains((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.Contains((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.Contains((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.Contains((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsExclude(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsExclude((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsExclude((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsExclude((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsExclude((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsGreaterThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsGreaterThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsGreaterThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsGreaterThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterThanOrEquals(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsGreaterThanOrEquals((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsGreaterThanOrEquals((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsGreaterThanOrEquals((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsGreaterThanOrEquals((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsLessThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsLessThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsLessThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsLessThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessThanOrEquals(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return FastCore<int>.ContainsLessThanOrEquals((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return FastCore<long>.ContainsLessThanOrEquals((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return FastCore<int>.ContainsLessThanOrEquals((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return FastCore<long>.ContainsLessThanOrEquals((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool Contains(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.Contains((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.Contains((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.Contains((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.Contains((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsExclude(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsExclude((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsExclude((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsExclude((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsExclude((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsGreaterThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsGreaterThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsGreaterThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsGreaterThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterThanOrEquals(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsGreaterThanOrEquals((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsGreaterThanOrEquals((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsLessThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsLessThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsLessThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsLessThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessThanOrEquals(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return FastCore<uint>.ContainsLessThanOrEquals((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return FastCore<ulong>.ContainsLessThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return FastCore<uint>.ContainsLessThanOrEquals((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return FastCore<ulong>.ContainsLessThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOf(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOf((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOf((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOf((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOf((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfExclude(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfExclude((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfExclude((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfExclude((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfExclude((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfGreaterThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfGreaterThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfGreaterThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfGreaterThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfGreaterThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfGreaterThanOrEquals(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfGreaterThanOrEquals((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfGreaterThanOrEquals((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfGreaterThanOrEquals((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfGreaterThanOrEquals((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfLessThan(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfLessThan((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfLessThan((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfLessThan((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfLessThan((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nint* PointerIndexOfLessThanOrEquals(nint* ptr, nuint length, nint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (nint*)FastCore<int>.PointerIndexOfLessThanOrEquals((int*)ptr, length, *(int*)&value);
                    case sizeof(long):
                        return (nint*)FastCore<long>.PointerIndexOfLessThanOrEquals((long*)ptr, length, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (nint*)FastCore<int>.PointerIndexOfLessThanOrEquals((int*)ptr, length, *(int*)&value);
                            case sizeof(long):
                                return (nint*)FastCore<long>.PointerIndexOfLessThanOrEquals((long*)ptr, length, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOf(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOf((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOf((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOf((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOf((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfExclude(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfExclude((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfExclude((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfExclude((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfExclude((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfGreaterThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfGreaterThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfGreaterThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfGreaterThanOrEquals(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfGreaterThanOrEquals((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfGreaterThanOrEquals((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfLessThan(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfLessThan((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfLessThan((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfLessThan((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfLessThan((ulong*)ptr, length, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static nuint* PointerIndexOfLessThanOrEquals(nuint* ptr, nuint length, nuint value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (nuint*)FastCore<uint>.PointerIndexOfLessThanOrEquals((uint*)ptr, length, *(uint*)&value);
                    case sizeof(ulong):
                        return (nuint*)FastCore<ulong>.PointerIndexOfLessThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (nuint*)FastCore<uint>.PointerIndexOfLessThanOrEquals((uint*)ptr, length, *(uint*)&value);
                            case sizeof(ulong):
                                return (nuint*)FastCore<ulong>.PointerIndexOfLessThanOrEquals((ulong*)ptr, length, *(ulong*)&value);
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
            public static bool Contains(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Include, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsExclude(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Exclude, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsGreaterThan(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThan, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsLessThan(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThan, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsGreaterThanOrEquals(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThanOrEquals, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsLessThanOrEquals(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThanOrEquals, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOf(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Include, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfExclude(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Exclude, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfGreaterThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThan, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfLessThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThan, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThanOrEquals, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfLessThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThanOrEquals, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContains(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Include, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsExclude(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Exclude, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsGreaterThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThan, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsLessThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThan, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsGreaterThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThanOrEquals, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsLessThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThanOrEquals, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOf(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Include, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfExclude(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.Exclude, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfGreaterThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThan, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfLessThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThan, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.GreaterThanOrEquals, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfLessThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, CompareMethod.LessThanOrEquals, accurateResult: true);

            [Inline(InlineBehavior.Remove)]
            private static T* PointerIndexOfCore(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method, [InlineParameter] bool accurateResult)
            {
                if (CheckTypeCanBeVectorized() && length > GetLimitForVectorizing())
                    return method switch
                    {
                        CompareMethod.Include => accurateResult ? VectorizedPointerIndexOf(ptr, length, value) : VectorizedContains(ptr, length, value),
                        CompareMethod.Exclude => accurateResult ? VectorizedPointerIndexOfExclude(ptr, length, value) : VectorizedContainsExclude(ptr, length, value),
                        CompareMethod.GreaterThan => accurateResult ? VectorizedPointerIndexOfGreaterThan(ptr, length, value) : VectorizedContainsGreaterThan(ptr, length, value),
                        CompareMethod.GreaterThanOrEquals => accurateResult ? VectorizedPointerIndexOfGreaterThanOrEquals(ptr, length, value) : VectorizedContainsGreaterThanOrEquals(ptr, length, value),
                        CompareMethod.LessThan => accurateResult ? VectorizedPointerIndexOfLessThan(ptr, length, value) : VectorizedContainsLessThan(ptr, length, value),
                        CompareMethod.LessThanOrEquals => accurateResult ? VectorizedPointerIndexOfLessThanOrEquals(ptr, length, value) : VectorizedContainsLessThanOrEquals(ptr, length, value),
                        _ => throw new ArgumentOutOfRangeException(nameof(method))
                    };

                return ScalarizedPointerIndexOfCore(ref ptr, ref length, value, method, accurateResult);
            }

            [LocalsInit(false)]
            [Inline(InlineBehavior.Remove)]
            private static partial T* VectorizedPointerIndexOfCore(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method, [InlineParameter] bool accurateResult);

            [Inline(InlineBehavior.Remove)]
            private static T* ScalarizedPointerIndexOfCore(ref T* ptr, ref nuint length, T value, [InlineParameter] CompareMethod method, [InlineParameter] bool accurateResult)
            {
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    if (ScalarizedCompare(ptr[0], value, method))
                        return accurateResult ? ptr : (T*)Booleans.TrueNative;
                    if (ScalarizedCompare(ptr[1], value, method))
                        return accurateResult ? ptr + 1 : (T*)Booleans.TrueNative;
                    if (ScalarizedCompare(ptr[2], value, method))
                        return accurateResult ? ptr + 2 : (T*)Booleans.TrueNative;
                    if (ScalarizedCompare(ptr[3], value, method))
                        return accurateResult ? ptr + 3 : (T*)Booleans.TrueNative;
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    goto NotFound;
                if (ScalarizedCompare(*ptr, value, method))
                    return accurateResult ? ptr : (T*)Booleans.TrueNative;
                ptr++;
                if (ptr >= ptrEnd)
                    goto NotFound;
                if (ScalarizedCompare(*ptr, value, method))
                    return accurateResult ? ptr : (T*)Booleans.TrueNative;
                ptr++;
                if (ptr >= ptrEnd)
                    goto NotFound;
                if (ScalarizedCompare(*ptr, value, method))
                    return accurateResult ? ptr : (T*)Booleans.TrueNative;

            NotFound:
                return null;
            }
        }

        unsafe partial class SlowCore<T>
        {
            public static bool Contains(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.Include) != null;

            public static bool ContainsExclude(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.Exclude) != null;

            public static bool ContainsGreaterThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.GreaterThan) != null;

            public static bool ContainsLessThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.LessThan) != null;

            public static bool ContainsGreaterThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.GreaterThanOrEquals) != null;

            public static bool ContainsLessThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.LessThanOrEquals) != null;

            public static T* PointerIndexOf(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.Include);

            public static T* PointerIndexOfExclude(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.Exclude);

            public static T* PointerIndexOfGreaterThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.GreaterThan);

            public static T* PointerIndexOfLessThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.LessThan);

            public static T* PointerIndexOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.GreaterThanOrEquals);

            public static T* PointerIndexOfLessThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, CompareMethod.LessThanOrEquals);

            [Inline(InlineBehavior.Remove)]
            private static T* PointerIndexOfCore(ref T* ptr, nuint length, T value, [InlineParameter] CompareMethod method)
            {
                if (method == CompareMethod.Include || method == CompareMethod.Exclude)
                {
                    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (Compare(comparer, *ptr, value, method))
                            return ptr;
                    }
                }
                else
                {
                    Comparer<T> comparer = Comparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (Compare(comparer, *ptr, value, method))
                            return ptr;
                    }
                }
                return null;
            }
        }
    }
}
