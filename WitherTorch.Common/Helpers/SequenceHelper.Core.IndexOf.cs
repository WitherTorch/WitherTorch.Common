using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using LocalsInit;

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
        private enum IndexOfMethod
        {
            Include,
            Exclude,
            GreaterThan,
            GreaterThanOrEquals,
            LessThan,
            LessThanOrEquals,
            _Last
        }

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

            [Inline(InlineBehavior.Remove)]
            public static void Replace(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.Replace((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.Replace((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.Replace((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.Replace((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void ReplaceExclude(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.ReplaceExclude((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.ReplaceExclude((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.ReplaceExclude((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.ReplaceExclude((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void ReplaceGreaterThan(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.ReplaceGreaterThan((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.ReplaceGreaterThan((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.ReplaceGreaterThan((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.ReplaceGreaterThan((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void ReplaceGreaterThanOrEquals(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.ReplaceGreaterThanOrEquals((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.ReplaceGreaterThanOrEquals((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.ReplaceGreaterThanOrEquals((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.ReplaceGreaterThanOrEquals((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void ReplaceLessThan(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.ReplaceLessThan((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.ReplaceLessThan((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.ReplaceLessThan((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.ReplaceLessThan((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void ReplaceLessThanOrEquals(nint* ptr, nuint length, nint filter, nint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        FastCore<int>.ReplaceLessThanOrEquals((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                        return;
                    case sizeof(long):
                        FastCore<long>.ReplaceLessThanOrEquals((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                FastCore<int>.ReplaceLessThanOrEquals((int*)ptr, length, *(int*)&filter, *(int*)&replacement);
                                return;
                            case sizeof(long):
                                FastCore<long>.ReplaceLessThanOrEquals((long*)ptr, length, *(long*)&filter, *(long*)&replacement);
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
            public static void Replace(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.Replace((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.Replace((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.Replace((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.Replace((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static void ReplaceExclude(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.ReplaceExclude((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.ReplaceExclude((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.ReplaceExclude((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.ReplaceExclude((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static void ReplaceGreaterThan(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.ReplaceGreaterThan((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.ReplaceGreaterThan((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.ReplaceGreaterThan((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.ReplaceGreaterThan((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static void ReplaceGreaterThanOrEquals(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.ReplaceGreaterThanOrEquals((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.ReplaceGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.ReplaceGreaterThanOrEquals((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.ReplaceGreaterThanOrEquals((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static void ReplaceLessThan(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.ReplaceLessThan((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.ReplaceLessThan((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.ReplaceLessThan((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.ReplaceLessThan((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static void ReplaceLessThanOrEquals(nuint* ptr, nuint length, nuint filter, nuint replacement)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        FastCore<uint>.ReplaceLessThanOrEquals((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                        return;
                    case sizeof(ulong):
                        FastCore<ulong>.ReplaceLessThanOrEquals((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
                        return;
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                FastCore<uint>.ReplaceLessThanOrEquals((uint*)ptr, length, *(uint*)&filter, *(uint*)&replacement);
                                return;
                            case sizeof(ulong):
                                FastCore<ulong>.ReplaceLessThanOrEquals((ulong*)ptr, length, *(ulong*)&filter, *(ulong*)&replacement);
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
            public static bool Contains(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Include, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsExclude(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Exclude, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsGreaterThan(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterThan, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsLessThan(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessThan, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsGreaterThanOrEquals(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterThanOrEquals, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool ContainsLessThanOrEquals(T* ptr, nuint length, T value)
                => MathHelper.ToBooleanUnsafe(PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessThanOrEquals, accurateResult: false));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOf(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Include, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfExclude(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Exclude, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfGreaterThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterThan, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfLessThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessThan, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterThanOrEquals, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T* PointerIndexOfLessThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessThanOrEquals, accurateResult: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Replace(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.Include);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ReplaceExclude(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.Exclude);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ReplaceGreaterThan(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.GreaterThan);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ReplaceLessThan(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.LessThan);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ReplaceGreaterThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.GreaterThanOrEquals);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ReplaceLessThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.LessThanOrEquals);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContains(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.Include, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsExclude(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.Exclude, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsGreaterThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.GreaterThan, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsLessThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.LessThan, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsGreaterThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.GreaterThanOrEquals, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedContainsLessThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.LessThanOrEquals, accurateResult: false);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOf(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.Include, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfExclude(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.Exclude, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfGreaterThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.GreaterThan, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfLessThan(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.LessThan, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.GreaterThanOrEquals, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static T* VectorizedPointerIndexOfLessThanOrEquals(T* ptr, nuint length, T value)
                => VectorizedPointerIndexOfCore(ref ptr, ref length, value, IndexOfMethod.LessThanOrEquals, accurateResult: true);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplace(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.Include);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplaceExclude(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.Exclude);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplaceGreaterThan(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.GreaterThan);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplaceLessThan(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.LessThan);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplaceGreaterThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.GreaterThanOrEquals);

            [LocalsInit(false)]
            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void VectorizedReplaceLessThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.LessThanOrEquals);

            [Inline(InlineBehavior.Remove)]
            private static T* PointerIndexOfCore(ref T* ptr, nuint length, T value, [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
                if (CheckTypeCanBeVectorized() && length >= GetMinimumVectorCount())
                    return method switch
                    {
                        IndexOfMethod.Include => accurateResult ? VectorizedPointerIndexOf(ptr, length, value) : VectorizedContains(ptr, length, value),
                        IndexOfMethod.Exclude => accurateResult ? VectorizedPointerIndexOfExclude(ptr, length, value) : VectorizedContainsExclude(ptr, length, value),
                        IndexOfMethod.GreaterThan => accurateResult ? VectorizedPointerIndexOfGreaterThan(ptr, length, value) : VectorizedContainsGreaterThan(ptr, length, value),
                        IndexOfMethod.GreaterThanOrEquals => accurateResult ? VectorizedPointerIndexOfGreaterThanOrEquals(ptr, length, value) : VectorizedContainsGreaterThanOrEquals(ptr, length, value),
                        IndexOfMethod.LessThan => accurateResult ? VectorizedPointerIndexOfLessThan(ptr, length, value) : VectorizedContainsLessThan(ptr, length, value),
                        IndexOfMethod.LessThanOrEquals => accurateResult ? VectorizedPointerIndexOfLessThanOrEquals(ptr, length, value) : VectorizedContainsLessThanOrEquals(ptr, length, value),
                        _ => throw new ArgumentOutOfRangeException(nameof(method))
                    };

                return LegacyPointerIndexOfCore(ref ptr, length, value, method, accurateResult);
            }

            [LocalsInit(false)]
            [Inline(InlineBehavior.Remove)]
            private static partial T* VectorizedPointerIndexOfCore(ref T* ptr, ref nuint length, T value, [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult);

            [Inline(InlineBehavior.Remove)]
            private static T* LegacyPointerIndexOfCore(ref T* ptr, nuint length, T value, [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
                for (; length >= 4; length -= 4, ptr += 4) // 4x 展開
                {
                    if (LegacyIndexOfCore(ptr[0], value, method))
                        return accurateResult ? ptr : (T*)Booleans.TrueNative;
                    if (LegacyIndexOfCore(ptr[1], value, method))
                        return accurateResult ? ptr + 1 : (T*)Booleans.TrueNative;
                    if (LegacyIndexOfCore(ptr[2], value, method))
                        return accurateResult ? ptr + 2 : (T*)Booleans.TrueNative;
                    if (LegacyIndexOfCore(ptr[3], value, method))
                        return accurateResult ? ptr + 3 : (T*)Booleans.TrueNative;
                }
                T* ptrEnd = ptr + length;
                if (ptr >= ptrEnd)
                    goto NotFound;
                if (LegacyIndexOfCore(*ptr, value, method))
                    return accurateResult ? ptr : (T*)Booleans.TrueNative;
                ptr++;
                if (ptr >= ptrEnd)
                    goto NotFound;
                if (LegacyIndexOfCore(*ptr, value, method))
                    return accurateResult ? ptr : (T*)Booleans.TrueNative;
                ptr++;
                if (ptr >= ptrEnd)
                    goto NotFound;
                if (LegacyIndexOfCore(*ptr, value, method))
                    return accurateResult ? ptr : (T*)Booleans.TrueNative;

            NotFound:
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool LegacyIndexOfCore(T item, T value, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => UnsafeHelper.Equals(item, value),
                    IndexOfMethod.Exclude => UnsafeHelper.NotEquals(item, value),
                    IndexOfMethod.GreaterThan => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsGreaterThanUnsigned(item, value) : UnsafeHelper.IsGreaterThan(item, value),
                    IndexOfMethod.GreaterThanOrEquals => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsGreaterThanOrEqualsUnsigned(item, value) : UnsafeHelper.IsGreaterThanOrEquals(item, value),
                    IndexOfMethod.LessThan => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsLessThanUnsigned(item, value) : UnsafeHelper.IsLessThan(item, value),
                    IndexOfMethod.LessThanOrEquals => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.IsLessThanOrEqualsUnsigned(item, value) : UnsafeHelper.IsLessThanOrEquals(item, value),
                    _ => throw new ArgumentOutOfRangeException(nameof(method)),
                };

            [Inline(InlineBehavior.Remove)]
            private static void ReplaceCore(ref T* ptr, nuint length, T filter, T replacement, [InlineParameter] IndexOfMethod method)
            {
                T* ptrEnd = ptr + length;
                if (CheckTypeCanBeVectorized())
                {
                    VectorizedReplaceCore(ref ptr, ptrEnd, filter, replacement, method);
                    return;
                }

                LegacyReplaceCore(ref ptr, ptrEnd, filter, replacement, method);
            }


            [Inline(InlineBehavior.Remove)]
            private static partial void VectorizedReplaceCore(ref T* ptr, T* ptrEnd, T filter, T replacement, [InlineParameter] IndexOfMethod method);

            [Inline(InlineBehavior.Remove)]
            private static void LegacyReplaceCore(ref T* ptr, T* ptrEnd, T filter, T replacement, [InlineParameter] IndexOfMethod method)
            {
                for (; ptr < ptrEnd; ptr++)
                {
                    if (LegacyIndexOfCore(*ptr, filter, method))
                        *ptr = replacement;
                }
            }
        }

        unsafe partial class SlowCore<T>
        {
            public static bool Contains(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Include) != null;

            public static bool ContainsExclude(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Exclude) != null;

            public static bool ContainsGreaterThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterThan) != null;

            public static bool ContainsLessThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessThan) != null;

            public static bool ContainsGreaterThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterThanOrEquals) != null;

            public static bool ContainsLessThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessThanOrEquals) != null;

            public static T* PointerIndexOf(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Include);

            public static T* PointerIndexOfExclude(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.Exclude);

            public static T* PointerIndexOfGreaterThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterThan);

            public static T* PointerIndexOfLessThan(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessThan);

            public static T* PointerIndexOfGreaterThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.GreaterThanOrEquals);

            public static T* PointerIndexOfLessThanOrEquals(T* ptr, nuint length, T value)
                => PointerIndexOfCore(ref ptr, length, value, IndexOfMethod.LessThanOrEquals);

            public static void Replace(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.Include);

            public static void ReplaceExclude(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.Exclude);

            public static void ReplaceGreaterThan(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.GreaterThan);

            public static void ReplaceLessThan(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.LessThan);

            public static void ReplaceGreaterThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.GreaterThanOrEquals);

            public static void ReplaceLessThanOrEquals(T* ptr, nuint length, T filter, T replacement)
                => ReplaceCore(ref ptr, length, filter, replacement, IndexOfMethod.LessThanOrEquals);

            [Inline(InlineBehavior.Remove)]
            private static T* PointerIndexOfCore(ref T* ptr, nuint length, T value, [InlineParameter] IndexOfMethod method)
            {
                if (method == IndexOfMethod.Include || method == IndexOfMethod.Exclude)
                {
                    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (IndexOfCore(comparer, *ptr, value, method))
                            return ptr;
                    }
                }
                else
                {
                    Comparer<T> comparer = Comparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (IndexOfCore(comparer, *ptr, value, method))
                            return ptr;
                    }
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static void ReplaceCore(ref T* ptr, nuint length, T filter, T replacement, [InlineParameter] IndexOfMethod method)
            {
                if (method == IndexOfMethod.Include || method == IndexOfMethod.Exclude)
                {
                    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (IndexOfCore(comparer, *ptr, filter, method))
                            *ptr = replacement;
                    }
                }
                else
                {
                    Comparer<T> comparer = Comparer<T>.Default;
                    for (nuint i = 0; i < length; i++, ptr++)
                    {
                        if (IndexOfCore(comparer, *ptr, filter, method))
                            *ptr = replacement;
                    }
                }
            }

            [Inline(InlineBehavior.Remove)]
            private static bool IndexOfCore(EqualityComparer<T> comparer, T item, T value, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => comparer.Equals(item, value),
                    IndexOfMethod.Exclude => !comparer.Equals(item, value),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static bool IndexOfCore(Comparer<T> comparer, T item, T value, [InlineParameter] IndexOfMethod method)
            {
                int result = comparer.Compare(item, value);
                return method switch
                {
                    IndexOfMethod.GreaterThan => result > 0,
                    IndexOfMethod.GreaterThanOrEquals => result >= 0,
                    IndexOfMethod.LessThan => result < 0,
                    IndexOfMethod.LessThanOrEquals => result <= 0,
                    _ => throw new InvalidOperationException(),
                };
            }
        }
    }
}
