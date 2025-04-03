using System;
using System.Numerics;

using InlineMethod;
using WitherTorch.Common.Helpers;



#if NET472_OR_GREATER
using LocalsInit;

using WitherTorch.Common.Helpers;
#else
using System.Runtime.CompilerServices;
#endif

namespace WitherTorch.CrossNative.Helpers
{
    partial class SequenceHelper
    {
        private enum IndexOfMethod
        {
            Include,
            Exclude,
            GreaterThan,
            GreaterOrEqualsThan,
            LessThan,
            LessOrEqualsThan,
            _Last
        }

#pragma warning disable CS0162
        private static unsafe class Core
        {
            [Inline(InlineBehavior.Remove)]
            public static bool Contains(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.Contains((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.Contains((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.Contains((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.Contains((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsExclude(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.ContainsExclude((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.ContainsExclude((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.ContainsExclude((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.ContainsExclude((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterThan(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.ContainsGreaterThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.ContainsGreaterThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.ContainsGreaterThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.ContainsGreaterThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterOrEqualsThan(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.ContainsGreaterOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.ContainsGreaterOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.ContainsGreaterOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.ContainsGreaterOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessThan(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.ContainsLessThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.ContainsLessThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.ContainsLessThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.ContainsLessThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessOrEqualsThan(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.ContainsLessOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return Core<long>.ContainsLessOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.ContainsLessOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return Core<long>.ContainsLessOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool Contains(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.Contains((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.Contains((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.Contains((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.Contains((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsExclude(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.ContainsExclude((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.ContainsExclude((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.ContainsExclude((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.ContainsExclude((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterThan(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.ContainsGreaterThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.ContainsGreaterThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.ContainsGreaterThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.ContainsGreaterThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsGreaterOrEqualsThan(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.ContainsGreaterOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.ContainsGreaterOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.ContainsGreaterOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.ContainsGreaterOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessThan(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.ContainsLessThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.ContainsLessThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.ContainsLessThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.ContainsLessThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool ContainsLessOrEqualsThan(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return Core<uint>.ContainsLessOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return Core<ulong>.ContainsLessOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.ContainsLessOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return Core<ulong>.ContainsLessOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static IntPtr* PointerIndexOf(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (IntPtr*)Core<int>.PointerIndexOf((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (IntPtr*)Core<long>.PointerIndexOf((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (IntPtr*)Core<int>.PointerIndexOf((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (IntPtr*)Core<long>.PointerIndexOf((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static IntPtr* PointerIndexOfExclude(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (IntPtr*)Core<int>.PointerIndexOfExclude((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (IntPtr*)Core<long>.PointerIndexOfExclude((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (IntPtr*)Core<int>.PointerIndexOfExclude((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (IntPtr*)Core<long>.PointerIndexOfExclude((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static IntPtr* PointerIndexOfGreaterThan(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (IntPtr*)Core<int>.PointerIndexOfGreaterThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (IntPtr*)Core<long>.PointerIndexOfGreaterThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (IntPtr*)Core<int>.PointerIndexOfGreaterThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (IntPtr*)Core<long>.PointerIndexOfGreaterThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static IntPtr* PointerIndexOfGreaterOrEqualsThan(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (IntPtr*)Core<int>.PointerIndexOfGreaterOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (IntPtr*)Core<long>.PointerIndexOfGreaterOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (IntPtr*)Core<int>.PointerIndexOfGreaterOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (IntPtr*)Core<long>.PointerIndexOfGreaterOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static IntPtr* PointerIndexOfLessThan(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (IntPtr*)Core<int>.PointerIndexOfLessThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (IntPtr*)Core<long>.PointerIndexOfLessThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (IntPtr*)Core<int>.PointerIndexOfLessThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (IntPtr*)Core<long>.PointerIndexOfLessThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static IntPtr* PointerIndexOfLessOrEqualsThan(IntPtr* ptr, IntPtr* ptrEnd, IntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return (IntPtr*)Core<int>.PointerIndexOfLessOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                    case sizeof(long):
                        return (IntPtr*)Core<long>.PointerIndexOfLessOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return (IntPtr*)Core<int>.PointerIndexOfLessOrEqualsThan((int*)ptr, (int*)ptrEnd, *(int*)&value);
                            case sizeof(long):
                                return (IntPtr*)Core<long>.PointerIndexOfLessOrEqualsThan((long*)ptr, (long*)ptrEnd, *(long*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static UIntPtr* PointerIndexOf(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (UIntPtr*)Core<uint>.PointerIndexOf((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (UIntPtr*)Core<ulong>.PointerIndexOf((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (UIntPtr*)Core<uint>.PointerIndexOf((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (UIntPtr*)Core<ulong>.PointerIndexOf((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static UIntPtr* PointerIndexOfExclude(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (UIntPtr*)Core<uint>.PointerIndexOfExclude((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (UIntPtr*)Core<ulong>.PointerIndexOfExclude((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (UIntPtr*)Core<uint>.PointerIndexOfExclude((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (UIntPtr*)Core<ulong>.PointerIndexOfExclude((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static UIntPtr* PointerIndexOfGreaterThan(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (UIntPtr*)Core<uint>.PointerIndexOfGreaterThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (UIntPtr*)Core<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (UIntPtr*)Core<uint>.PointerIndexOfGreaterThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (UIntPtr*)Core<ulong>.PointerIndexOfGreaterThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static UIntPtr* PointerIndexOfGreaterOrEqualsThan(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (UIntPtr*)Core<uint>.PointerIndexOfGreaterOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (UIntPtr*)Core<ulong>.PointerIndexOfGreaterOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (UIntPtr*)Core<uint>.PointerIndexOfGreaterOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (UIntPtr*)Core<ulong>.PointerIndexOfGreaterOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static UIntPtr* PointerIndexOfLessThan(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (UIntPtr*)Core<uint>.PointerIndexOfLessThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (UIntPtr*)Core<ulong>.PointerIndexOfLessThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (UIntPtr*)Core<uint>.PointerIndexOfLessThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (UIntPtr*)Core<ulong>.PointerIndexOfLessThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static UIntPtr* PointerIndexOfLessOrEqualsThan(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr value)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(uint):
                        return (UIntPtr*)Core<uint>.PointerIndexOfLessOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                    case sizeof(ulong):
                        return (UIntPtr*)Core<ulong>.PointerIndexOfLessOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return (UIntPtr*)Core<uint>.PointerIndexOfLessOrEqualsThan((uint*)ptr, (uint*)ptrEnd, *(uint*)&value);
                            case sizeof(ulong):
                                return (UIntPtr*)Core<ulong>.PointerIndexOfLessOrEqualsThan((ulong*)ptr, (ulong*)ptrEnd, *(ulong*)&value);
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

#if NET5_0_OR_GREATER
        [SkipLocalsInit]
#else
        [LocalsInit(false)]
#endif
        private static unsafe class Core<T> where T : unmanaged
        {
            [Inline(InlineBehavior.Remove)]
            private static bool CheckTypeCanBeVectorized()
                => (typeof(T) == typeof(byte)) ||
                       (typeof(T) == typeof(double)) ||
                       (typeof(T) == typeof(short)) ||
                       (typeof(T) == typeof(int)) ||
                       (typeof(T) == typeof(long)) ||
                       (typeof(T) == typeof(sbyte)) ||
                       (typeof(T) == typeof(float)) ||
                       (typeof(T) == typeof(ushort)) ||
                       (typeof(T) == typeof(uint)) ||
                       (typeof(T) == typeof(ulong));

            [Inline(InlineBehavior.Remove)]
            private static bool IsUnsigned()
                => (typeof(T) == typeof(byte)) ||
                       (typeof(T) == typeof(ushort)) ||
                       (typeof(T) == typeof(uint)) ||
                       (typeof(T) == typeof(ulong));

            public static bool Contains(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.Include, accurateResult: false) != null;

            public static bool ContainsExclude(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.Exclude, accurateResult: false) != null;

            public static bool ContainsGreaterThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.GreaterThan, accurateResult: false) != null;

            public static bool ContainsLessThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.LessThan, accurateResult: false) != null;

            public static bool ContainsGreaterOrEqualsThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.GreaterOrEqualsThan, accurateResult: false) != null;

            public static bool ContainsLessOrEqualsThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.LessOrEqualsThan, accurateResult: false) != null;

            public static T* PointerIndexOf(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.Include, accurateResult: true);

            public static T* PointerIndexOfExclude(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.Exclude, accurateResult: true);

            public static T* PointerIndexOfGreaterThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.GreaterThan, accurateResult: true);

            public static T* PointerIndexOfLessThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.LessThan, accurateResult: true);

            public static T* PointerIndexOfGreaterOrEqualsThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.GreaterOrEqualsThan, accurateResult: true);

            public static T* PointerIndexOfLessOrEqualsThan(T* ptr, T* ptrEnd, T value)
                => PointerIndexOfCore(ptr, ptrEnd, value, IndexOfMethod.LessOrEqualsThan, accurateResult: true);

            [Inline(InlineBehavior.Remove)]
            private static T* PointerIndexOfCore(T* ptr, T* ptrEnd, T value, [InlineParameter] IndexOfMethod method, [InlineParameter] bool accurateResult)
            {
                // Vector.IsHardwareAccelerated 與 Vector<T>.Count 會在執行時期被優化成常數，故不需要變數快取 (反而會妨礙 JIT 進行迴圈及條件展開)
                if (CheckTypeCanBeVectorized() && Vector.IsHardwareAccelerated)
                {
                    T* ptrLimit = ptrEnd - Vector<T>.Count;
                    if (ptr < ptrLimit)
                    {
                        Vector<T> maskVector = default;
                        {
                            T* pMask = (T*)UnsafeHelper.AsPointerRef(ref maskVector); // 將要比對的項目擴充成向量
                            for (int i = 0; i < Vector<T>.Count; i++)
                                pMask[i] = value;
                        }

                        do
                        {
                            Vector<T> valueVector = UnsafeHelper.Read<Vector<T>>(ptr);
                            Vector<T> resultVector = SimdIndexOfCore(valueVector, maskVector, method);
                            if (resultVector.Equals(default))
                                continue;
                            if (!accurateResult)
                                return ptr;
                            T* pResult = (T*)UnsafeHelper.AsPointerRef(ref resultVector);
                            for (int i = 0; i < Vector<T>.Count; i++)
                            {
                                if (*(byte*)(pResult + i) == 0xFF)
                                    return ptr + i;
                            }
                        } while ((ptr += Vector<T>.Count) < ptrLimit);
                        if (ptr == ptrEnd)
                            return null;
                    }
                }
                for (; ptr < ptrEnd; ptr++)
                {
                    if (LegacyIndexOfCore(ptr, value, method))
                        return ptr;
                }
                return null;
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector<T> SimdIndexOfCore(in Vector<T> valueVector, in Vector<T> maskVector, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => Vector.Equals(valueVector, maskVector),
                    IndexOfMethod.Exclude => ~Vector.Equals(valueVector, maskVector),
                    IndexOfMethod.GreaterThan => Vector.GreaterThan(valueVector, maskVector),
                    IndexOfMethod.GreaterOrEqualsThan => Vector.GreaterThanOrEqual(valueVector, maskVector),
                    IndexOfMethod.LessThan => Vector.LessThan(valueVector, maskVector),
                    IndexOfMethod.LessOrEqualsThan => Vector.LessThanOrEqual(valueVector, maskVector),
                    _ => throw new InvalidOperationException(),
                };

            [Inline(InlineBehavior.Remove)]
            private static bool LegacyIndexOfCore(T* ptr, T value, [InlineParameter] IndexOfMethod method)
                => method switch
                {
                    IndexOfMethod.Include => UnsafeHelper.Equals(*ptr, value),
                    IndexOfMethod.Exclude => UnsafeHelper.NotEquals(*ptr, value),
                    IndexOfMethod.GreaterThan => IsUnsigned() ? UnsafeHelper.IsGreaterThanUnsigned(*ptr, value) : UnsafeHelper.IsGreaterThan(*ptr, value),
                    IndexOfMethod.GreaterOrEqualsThan => IsUnsigned() ? UnsafeHelper.IsGreaterOrEqualsThanUnsigned(*ptr, value) : UnsafeHelper.IsGreaterOrEqualsThan(*ptr, value),
                    IndexOfMethod.LessThan => IsUnsigned() ? UnsafeHelper.IsLessThanUnsigned(*ptr, value) : UnsafeHelper.IsLessThan(*ptr, value),
                    IndexOfMethod.LessOrEqualsThan => IsUnsigned() ? UnsafeHelper.IsLessOrEqualsThanUnsigned(*ptr, value) : UnsafeHelper.IsLessOrEqualsThan(*ptr, value),
                    _ => throw new InvalidOperationException(),
                };
        }
    }
}
