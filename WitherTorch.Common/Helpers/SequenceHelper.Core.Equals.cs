using System;
using System.Collections.Generic;

using InlineMethod;

#if NET6_0_OR_GREATER
using System.Runtime.Intrinsics;
#else
using System.Numerics;
#endif

namespace WitherTorch.Common.Helpers
{
    partial class SequenceHelper
    {
#pragma warning disable CS0162
        unsafe partial class Core
        {
            [Inline(InlineBehavior.Remove)]
            public static bool Equals(IntPtr* ptr, IntPtr* ptrEnd, IntPtr* ptr2)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<int>.Equals((int*)ptr, (int*)ptrEnd, (int*)ptr2);
                    case sizeof(long):
                        return Core<long>.Equals((long*)ptr, (long*)ptrEnd, (long*)ptr2);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(int):
                                return Core<int>.Equals((int*)ptr, (int*)ptrEnd, (int*)ptr2);
                            case sizeof(long):
                                return Core<long>.Equals((long*)ptr, (long*)ptrEnd, (long*)ptr2);
                            default:
                                break;
                        }
                        goto default;
                    default:
                        throw new NotImplementedException();
                }
            }

            [Inline(InlineBehavior.Remove)]
            public static bool Equals(UIntPtr* ptr, UIntPtr* ptrEnd, UIntPtr* ptr2)
            {
                switch (UnsafeHelper.PointerSizeConstant)
                {
                    case sizeof(int):
                        return Core<uint>.Equals((uint*)ptr, (uint*)ptrEnd, (uint*)ptr2);
                    case sizeof(long):
                        return Core<ulong>.Equals((ulong*)ptr, (ulong*)ptrEnd, (ulong*)ptr2);
                    case UnsafeHelper.PointerSizeConstant_Indeterminate:
                        switch (UnsafeHelper.PointerSize)
                        {
                            case sizeof(uint):
                                return Core<uint>.Equals((uint*)ptr, (uint*)ptrEnd, (uint*)ptr2);
                            case sizeof(ulong):
                                return Core<ulong>.Equals((ulong*)ptr, (ulong*)ptrEnd, (ulong*)ptr2);
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
            public static bool Equals(T* ptr, T* ptrEnd, T* ptr2)
            {
                if (CheckTypeCanBeVectorized())
                    return VectorizedEquals(ptr, ptrEnd, ptr2);
                if (UnsafeHelper.IsPrimitiveType<T>())
                {
                    for (; ptr < ptrEnd; ptr++, ptr2++)
                    {
                        if (UnsafeHelper.NotEquals(*ptr, *ptr2))
                            return false;
                    }
                    return true;
                }
                EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                for (; ptr < ptrEnd; ptr++, ptr2++)
                {
                    if (!comparer.Equals(*ptr, *ptr2))
                        return false;
                }
                return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedEquals(T* ptr, T* ptrEnd, T* ptr2)
            {
#if NET6_0_OR_GREATER
                if (Vector512.IsHardwareAccelerated)
                {
                    if (ptr + Vector512<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector512<T> valueVector = UnsafeHelper.Read<Vector512<T>>(ptr);
                            Vector512<T> valueVector2 = UnsafeHelper.Read<Vector512<T>>(ptr2);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector512<T>.Count;
                            ptr2 += Vector512<T>.Count;
                        } while (ptr + Vector512<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                }
                if (Vector256.IsHardwareAccelerated)
                {
                    if (ptr + Vector256<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector256<T> valueVector = UnsafeHelper.Read<Vector256<T>>(ptr);
                            Vector256<T> valueVector2 = UnsafeHelper.Read<Vector256<T>>(ptr2);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector256<T>.Count;
                            ptr2 += Vector256<T>.Count;
                        } while (ptr + Vector256<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                }
                if (Vector128.IsHardwareAccelerated)
                {
                    if (ptr + Vector128<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector128<T> valueVector = UnsafeHelper.Read<Vector128<T>>(ptr);
                            Vector128<T> valueVector2 = UnsafeHelper.Read<Vector128<T>>(ptr2);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector128<T>.Count;
                            ptr2 += Vector128<T>.Count;
                        } while (ptr + Vector128<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                }
                if (Vector64.IsHardwareAccelerated)
                {
                    if (ptr + Vector64<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector64<T> valueVector = UnsafeHelper.Read<Vector64<T>>(ptr);
                            Vector64<T> valueVector2 = UnsafeHelper.Read<Vector64<T>>(ptr2);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector64<T>.Count;
                            ptr2 += Vector64<T>.Count;
                        } while (ptr + Vector64<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                    if (ptr + 2 < ptrEnd)
                    {
                        Vector64<T> valueVector = default;
                        Vector64<T> valueVector2 = default;
                        uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                        UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                        UnsafeHelper.CopyBlockUnaligned(&valueVector2, ptr2, byteCount);
                        return valueVector.Equals(valueVector2);
                    }
                    for (int i = 0; i < 2; i++, ptr2++) // CLR 編譯時會展開
                    {
                        if (UnsafeHelper.NotEquals(*ptr, *ptr2))
                            return false;
                        if (++ptr >= ptrEnd)
                            break;
                    }
                    return true;
                }
#else
                if (Vector.IsHardwareAccelerated)
                {
                    if (ptr + Vector<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector<T> valueVector = UnsafeHelper.Read<Vector<T>>(ptr);
                            Vector<T> valueVector2 = UnsafeHelper.Read<Vector<T>>(ptr2);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector<T>.Count;
                            ptr2 += Vector<T>.Count;
                        } while (ptr + Vector<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                    if (ptr + 2 < ptrEnd)
                    {
                        Vector<T> valueVector = default;
                        Vector<T> valueVector2 = default;
                        uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                        UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                        UnsafeHelper.CopyBlockUnaligned(&valueVector2, ptr2, byteCount);
                        return valueVector.Equals(valueVector2);
                    }
                    for (int i = 0; i < 2; i++, ptr2++) // CLR 編譯時會展開
                    {
                        if (UnsafeHelper.NotEquals(*ptr, *ptr2))
                            return false;
                        if (++ptr >= ptrEnd)
                            break;
                    }
                    return true;
                }
#endif
                for (; ptr < ptrEnd; ptr++, ptr2++)
                {
                    if (UnsafeHelper.NotEquals(*ptr, *ptr2))
                        return false;
                }
                return true;
            }

            [Inline(InlineBehavior.Remove)]
            private static bool VectorizedEquals_IgnoreCase(char* ptr, char* ptrEnd, char* ptr2)
            {
#if NET6_0_OR_GREATER
                if (Vector512.IsHardwareAccelerated)
                {
                    if (ptr + Vector512<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector512<T> valueVector = UnsafeHelper.Read<Vector512<T>>(ptr);
                            Vector512<T> valueVector2 = UnsafeHelper.Read<Vector512<T>>(ptr2);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector512<T>.Count;
                            ptr2 += Vector512<T>.Count;
                        } while (ptr + Vector512<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                }
                if (Vector256.IsHardwareAccelerated)
                {
                    if (ptr + Vector256<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector256<T> valueVector = UnsafeHelper.Read<Vector256<T>>(ptr);
                            Vector256<T> valueVector2 = UnsafeHelper.Read<Vector256<T>>(ptr2);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector256<T>.Count;
                            ptr2 += Vector256<T>.Count;
                        } while (ptr + Vector256<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                }
                if (Vector128.IsHardwareAccelerated)
                {
                    if (ptr + Vector128<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector128<T> valueVector = UnsafeHelper.Read<Vector128<T>>(ptr);
                            Vector128<T> valueVector2 = UnsafeHelper.Read<Vector128<T>>(ptr2);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector128<T>.Count;
                            ptr2 += Vector128<T>.Count;
                        } while (ptr + Vector128<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                }
                if (Vector64.IsHardwareAccelerated)
                {
                    if (ptr + Vector64<T>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector64<T> valueVector = UnsafeHelper.Read<Vector64<T>>(ptr);
                            Vector64<T> valueVector2 = UnsafeHelper.Read<Vector64<T>>(ptr2);
                            if (!valueVector.Equals(valueVector2))
                                return false;
                            ptr += Vector64<T>.Count;
                            ptr2 += Vector64<T>.Count;
                        } while (ptr + Vector64<T>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                    if (ptr + 2 < ptrEnd)
                    {
                        Vector64<T> valueVector = default;
                        Vector64<T> valueVector2 = default;
                        uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                        UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                        UnsafeHelper.CopyBlockUnaligned(&valueVector2, ptr2, byteCount);
                        return valueVector.Equals(valueVector2);
                    }
                    for (int i = 0; i < 2; i++, ptr2++) // CLR 編譯時會展開
                    {
                        if (UnsafeHelper.NotEquals(*ptr, *ptr2))
                            return false;
                        if (++ptr >= ptrEnd)
                            break;
                    }
                    return true;
                }
#else
                if (Vector.IsHardwareAccelerated)
                {
                    if (ptr + Vector<ushort>.Count < ptrEnd)
                    {
                        do
                        {
                            Vector<ushort> valueVector = UnsafeHelper.Read<Vector<ushort>>(ptr);
                            Vector<ushort> valueVector2 = UnsafeHelper.Read<Vector<ushort>>(ptr2);
                            if (!ToLowerAscii(valueVector).Equals(valueVector2))
                                return false;
                            ptr += Vector<ushort>.Count;
                            ptr2 += Vector<ushort>.Count;
                        } while (ptr + Vector<ushort>.Count < ptrEnd);
                        if (ptr >= ptrEnd)
                            return true;
                    }
                    if (ptr + 2 < ptrEnd)
                    {
                        Vector<ushort> valueVector = default;
                        Vector<ushort> valueVector2 = default;
                        uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                        UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                        UnsafeHelper.CopyBlockUnaligned(&valueVector2, ptr2, byteCount);
                        return valueVector.Equals(valueVector2);
                    }
                    for (int i = 0; i < 2; i++, ptr2++) // CLR 編譯時會展開
                    {
                        if (UnsafeHelper.NotEquals(*ptr, *ptr2))
                            return false;
                        if (++ptr >= ptrEnd)
                            break;
                    }
                    return true;
                }
#endif
                for (; ptr < ptrEnd; ptr++, ptr2++)
                {
                    if (UnsafeHelper.NotEquals(*ptr, *ptr2))
                        return false;
                }
                return true;
            }

#if NET6_0_OR_GREATER
            [Inline(InlineBehavior.Remove)]
            private static Vector512<ushort> ToLowerAscii(in Vector512<ushort> vector)
            {
                Vector512<ushort> resultVector = Vector512.GreaterThanOrEqual(vector, Vector512.Create<ushort>('A')) &
                    Vector512.LessThanOrEqual(vector, Vector512.Create<ushort>('Z'));
                if (resultVector.Equals(default))
                    return vector;
                return vector + (resultVector & Vector512.Create<ushort>(unchecked((ushort)('A' - 'a'))));
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector256<ushort> ToLowerAscii(in Vector256<ushort> vector)
            {
                Vector256<ushort> resultVector = Vector256.GreaterThanOrEqual(vector, Vector256.Create<ushort>('A')) &
                    Vector256.LessThanOrEqual(vector, Vector256.Create<ushort>('Z'));
                if (resultVector.Equals(default))
                    return vector;
                return vector + (resultVector & Vector256.Create<ushort>(unchecked((ushort)('A' - 'a'))));
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector128<ushort> ToLowerAscii(in Vector128<ushort> vector)
            {
                Vector128<ushort> resultVector = Vector128.GreaterThanOrEqual(vector, Vector128.Create<ushort>('A')) &
                    Vector128.LessThanOrEqual(vector, Vector128.Create<ushort>('Z'));
                if (resultVector.Equals(default))
                    return vector;
                return vector + (resultVector & Vector128.Create<ushort>(unchecked((ushort)('A' - 'a'))));
            }

            [Inline(InlineBehavior.Remove)]
            private static Vector64<ushort> ToLowerAscii(in Vector64<ushort> vector)
            {
                Vector64<ushort> resultVector = Vector64.GreaterThanOrEqual(vector, Vector64.Create<ushort>('A')) &
                    Vector64.LessThanOrEqual(vector, Vector64.Create<ushort>('Z'));
                if (resultVector.Equals(default))
                    return vector;
                return vector + (resultVector & Vector64.Create<ushort>(unchecked((ushort)('A' - 'a'))));
            }
#else
            [Inline(InlineBehavior.Remove)]
            private static Vector<ushort> ToLowerAscii(in Vector<ushort> vector)
            {
                Vector<ushort> resultVector = Vector.GreaterThanOrEqual(vector, new Vector<ushort>('A')) &
                    Vector.LessThanOrEqual(vector, new Vector<ushort>('Z'));
                if (resultVector.Equals(default))
                    return vector;
                return vector + (resultVector & new Vector<ushort>(unchecked((ushort)('A' - 'a'))));
            }
#endif
        }
    }
}
