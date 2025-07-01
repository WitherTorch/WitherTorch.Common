using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security;

using InlineMethod;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Threading;

#if NET6_0_OR_GREATER
using System.Runtime.Intrinsics;
#endif

namespace WitherTorch.Common.Extensions
{
    public static class StringExtensions
    {
        private const int UpperLowerDiff = 'a' - 'A';

        [Inline(InlineBehavior.Keep, export: true)]
        public static char FirstOrDefault(this string obj, char defaultValue = '\0')
            => StringHelper.IsNullOrEmpty(obj) ? defaultValue : obj[0];

        [Inline(InlineBehavior.Keep, export: true)]
        public static char LastOrDefault(this string obj, char defaultValue = '\0')
            => StringHelper.IsNullOrEmpty(obj) ? defaultValue : obj[obj.Length - 1];

#if NET472_OR_GREATER
        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe bool Contains(this string obj, char value) => StringHelper.Contains(obj, value);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Contains(this string obj, params char[] values) => StringHelper.Contains(obj, values);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this string[] array, string value, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, 0, array.Length, comparison) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this string[] array, string value, int startIndex, int length, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, startIndex, length, comparison) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this string[] array, string value, StringComparison comparison = StringComparison.Ordinal) => array.IndexOf(value, 0, array.Length, comparison);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf(this string[] array, string value, int startIndex, int length, StringComparison comparison = StringComparison.Ordinal)
        {
            for (int i = startIndex; i < length; i++)
            {
                if (SequenceHelper.Equals(array[i], value, comparison))
                    return i;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWith(this string str, char c)
        {
            int count = str.Length;
            if (count > 0)
                return str[count - 1] == c;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWith(this string str, params char[] chars)
        {
            int count = str.Length;
            if (count > 0)
            {
                char c = str[count - 1];
                for (int i = 0, length = chars.Length; i < length; i++)
                {
                    if (c == chars[i])
                        return true;
                }
            }
            return false;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool StartsWith(this string str, char c)
            => !StringHelper.IsNullOrEmpty(str) && str[0] == c;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool StartsWith(this string str, params char[] chars)
        {
            if (StringHelper.IsNullOrEmpty(str))
                return false;
            return chars.Contains(str[0]);
        }

        public static string[] ToUpperAscii(this string[] array)
        {
            for (int i = 0, length = array.Length; i < length; i++)
                array[i] = ToUpperAscii(array[i]);
            return array;
        }

        public static string[] ToLowerAscii(this string[] array)
        {
            for (int i = 0, length = array.Length; i < length; i++)
                array[i] = ToLowerAscii(array[i]);
            return array;
        }

        public static unsafe string ToUpperAscii(this string value)
        {
            int length = value.Length;
            if (length <= 0)
                return string.Empty;
            fixed (char* ptr = value)
                return ToUpperAsciiCore(ptr, ptr + length) ?? value;
        }

        public static unsafe string ToLowerAscii(this string value)
        {
            int length = value.Length;
            if (length <= 0)
                return string.Empty;
            fixed (char* ptr = value)
                return ToLowerAsciiCore(ptr, ptr + length) ?? value;
        }

        private static unsafe string? ToUpperAsciiCore(char* ptr, char* ptrEnd)
            => ToLowerOrUpperAsciiCore(ptr, ptrEnd, isUpper: true);

        private static unsafe string? ToLowerAsciiCore(char* ptr, char* ptrEnd)
            => ToLowerOrUpperAsciiCore(ptr, ptrEnd, isUpper: false);

        public static string[] WithPrefix(this string[] array, string prefix)
        {
            for (int i = 0, length = array.Length; i < length; i++)
                array[i] = array[i].WithPrefix(prefix);
            return array;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static string WithPrefix(this string value, string prefix)
            => prefix + value;

        [Inline(InlineBehavior.Remove)]
        private static unsafe string? ToLowerOrUpperAsciiCore(char* ptr, char* ptrEnd, [InlineParameter] bool isUpper)
        {
            char* ptrStart = ptr;
            LazyTinyRefStruct<string> resultLazy = new LazyTinyRefStruct<string>(() =>
            {
                int length = unchecked((int)(ptrEnd - ptrStart));
                string result = StringHelper.AllocateRawString(length);
                fixed (char* ptrResult = result)
                    UnsafeHelper.CopyBlock(ptrResult, ptrStart, unchecked((uint)(length * sizeof(char))));
                return result;
            });
#if NET6_0_OR_GREATER
            if (Vector512.IsHardwareAccelerated)
                goto Vector512;
            if (Vector256.IsHardwareAccelerated)
                goto Vector256;
            if (Vector128.IsHardwareAccelerated)
                goto Vector128;
            if (Limits.UseVector64Acceleration && Vector64.IsHardwareAccelerated)
                goto Vector64;
            goto Fallback;

        Vector512:
            if (ptr + Vector512<ushort>.Count < ptrEnd)
            {
                Vector512<ushort> maskVectorLow = Vector512.Create<ushort>(isUpper ? 'a' : 'A');
                Vector512<ushort> maskVectorHigh = Vector512.Create<ushort>(isUpper ? 'z' : 'Z');
                Vector512<ushort> operationVector = Vector512.Create<ushort>(UpperLowerDiff);
                do
                {
                    Vector512<ushort> valueVector = Vector512.Load((ushort*)ptr);
                    Vector512<ushort> resultVector = Vector512.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector512.LessThanOrEqual(valueVector, maskVectorHigh);
                    if (!resultVector.Equals(default))
                    {
                        valueVector = VectorizedToLowerOrUpperAsciiCore_512(valueVector, operationVector, resultVector, isUpper);
                        fixed (char* ptrResult = resultLazy.Value)
                            valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                    }
                    ptr += Vector512<ushort>.Count;
                } while (ptr + Vector512<ushort>.Count < ptrEnd);
                if (ptr >= ptrEnd)
                    return resultLazy.GetValueDirectly();
            }
            if (Vector256.IsHardwareAccelerated)
                goto Vector256;
            if (Vector128.IsHardwareAccelerated)
                goto Vector128;
            if (Limits.UseVector64Acceleration && Vector64.IsHardwareAccelerated)
                goto Vector64;
            if (ptr + Vector512<ushort>.Count / 2 < ptrEnd)
            {
                Vector512<ushort> maskVectorLow = Vector512.Create<ushort>(isUpper ? 'a' : 'A');
                Vector512<ushort> maskVectorHigh = Vector512.Create<ushort>(isUpper ? 'z' : 'Z');
                Vector512<ushort> operationVector = Vector512.Create<ushort>(UpperLowerDiff);
                Vector512<ushort> valueVector = default;
                uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                Vector512<ushort> resultVector = Vector512.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector512.LessThanOrEqual(valueVector, maskVectorHigh);
                if (!resultVector.Equals(default))
                {
                    valueVector = VectorizedToLowerOrUpperAsciiCore_512(valueVector, operationVector, resultVector, isUpper);
                    fixed (char* ptrResult = resultLazy.Value)
                        UnsafeHelper.CopyBlockUnaligned(ptrResult + (ptr - ptrStart), &valueVector, byteCount);
                }
                return resultLazy.GetValueDirectly();
            }
            for (int i = 0; i < Vector512<ushort>.Count / 2; i++) // CLR 會自動展開這個迴圈
            {
                LegacyToLowerOrUpperAsciiCore(ptr, ptrStart, ref resultLazy, isUpper);
                if (++ptr >= ptrEnd)
                    break;
            }
            goto Return;

        Vector256:
            if (ptr + Vector256<ushort>.Count < ptrEnd)
            {
                Vector256<ushort> maskVectorLow = Vector256.Create<ushort>(isUpper ? 'a' : 'A');
                Vector256<ushort> maskVectorHigh = Vector256.Create<ushort>(isUpper ? 'z' : 'Z');
                Vector256<ushort> operationVector = Vector256.Create<ushort>(UpperLowerDiff);
                do
                {
                    Vector256<ushort> valueVector = Vector256.Load((ushort*)ptr);
                    Vector256<ushort> resultVector = Vector256.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector256.LessThanOrEqual(valueVector, maskVectorHigh);
                    if (!resultVector.Equals(default))
                    {
                        valueVector = VectorizedToLowerOrUpperAsciiCore_256(valueVector, operationVector, resultVector, isUpper);
                        fixed (char* ptrResult = resultLazy.Value)
                            valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                    }
                    ptr += Vector256<ushort>.Count;
                } while (ptr + Vector256<ushort>.Count < ptrEnd);
                if (ptr >= ptrEnd)
                    return resultLazy.GetValueDirectly();
            }
            if (Vector128.IsHardwareAccelerated)
                goto Vector128;
            if (Limits.UseVector64Acceleration && Vector64.IsHardwareAccelerated)
                goto Vector64;
            if (ptr + Vector256<ushort>.Count / 2 < ptrEnd)
            {
                Vector256<ushort> maskVectorLow = Vector256.Create<ushort>(isUpper ? 'a' : 'A');
                Vector256<ushort> maskVectorHigh = Vector256.Create<ushort>(isUpper ? 'z' : 'Z');
                Vector256<ushort> operationVector = Vector256.Create<ushort>(UpperLowerDiff);
                Vector256<ushort> valueVector = default;
                uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                Vector256<ushort> resultVector = Vector256.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector256.LessThanOrEqual(valueVector, maskVectorHigh);
                if (!resultVector.Equals(default))
                {
                    valueVector = VectorizedToLowerOrUpperAsciiCore_256(valueVector, operationVector, resultVector, isUpper);
                    fixed (char* ptrResult = resultLazy.Value)
                        UnsafeHelper.CopyBlockUnaligned(ptrResult + (ptr - ptrStart), &valueVector, byteCount);
                }
                return resultLazy.GetValueDirectly();
            }
            for (int i = 0; i < Vector256<ushort>.Count / 2; i++) // CLR 會自動展開這個迴圈
            {
                LegacyToLowerOrUpperAsciiCore(ptr, ptrStart, ref resultLazy, isUpper);
                if (++ptr >= ptrEnd)
                    break;
            }
            goto Return;

        Vector128:
            if (ptr + Vector128<ushort>.Count < ptrEnd)
            {
                Vector128<ushort> maskVectorLow = Vector128.Create<ushort>(isUpper ? 'a' : 'A');
                Vector128<ushort> maskVectorHigh = Vector128.Create<ushort>(isUpper ? 'z' : 'Z');
                Vector128<ushort> operationVector = Vector128.Create<ushort>(UpperLowerDiff);
                do
                {
                    Vector128<ushort> valueVector = Vector128.Load((ushort*)ptr);
                    Vector128<ushort> resultVector = Vector128.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector128.LessThanOrEqual(valueVector, maskVectorHigh);
                    if (!resultVector.Equals(default))
                    {
                        valueVector = VectorizedToLowerOrUpperAsciiCore_128(valueVector, operationVector, resultVector, isUpper);
                        fixed (char* ptrResult = resultLazy.Value)
                            valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                    }
                    ptr += Vector128<ushort>.Count;
                } while (ptr + Vector128<ushort>.Count < ptrEnd);
                if (ptr >= ptrEnd)
                    return resultLazy.GetValueDirectly();
            }
            if (Limits.UseVector64Acceleration && Vector64.IsHardwareAccelerated)
                goto Vector64;
            if (ptr + Vector128<ushort>.Count / 2 < ptrEnd)
            {
                Vector128<ushort> maskVectorLow = Vector128.Create<ushort>(isUpper ? 'a' : 'A');
                Vector128<ushort> maskVectorHigh = Vector128.Create<ushort>(isUpper ? 'z' : 'Z');
                Vector128<ushort> operationVector = Vector128.Create<ushort>(UpperLowerDiff);
                Vector128<ushort> valueVector = default;
                uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                Vector128<ushort> resultVector = Vector128.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector128.LessThanOrEqual(valueVector, maskVectorHigh);
                if (!resultVector.Equals(default))
                {
                    valueVector = VectorizedToLowerOrUpperAsciiCore_128(valueVector, operationVector, resultVector, isUpper);
                    fixed (char* ptrResult = resultLazy.Value)
                        UnsafeHelper.CopyBlockUnaligned(ptrResult + (ptr - ptrStart), &valueVector, byteCount);
                }
                return resultLazy.GetValueDirectly();
            }
            for (int i = 0; i < Vector128<ushort>.Count / 2; i++) // CLR 會自動展開這個迴圈
            {
                LegacyToLowerOrUpperAsciiCore(ptr, ptrStart, ref resultLazy, isUpper);
                if (++ptr >= ptrEnd)
                    break;
            }
            goto Return;

        Vector64:
            if (ptr + Vector64<ushort>.Count < ptrEnd)
            {
                Vector64<ushort> maskVectorLow = Vector64.Create<ushort>(isUpper ? 'a' : 'A');
                Vector64<ushort> maskVectorHigh = Vector64.Create<ushort>(isUpper ? 'z' : 'Z');
                Vector64<ushort> operationVector = Vector64.Create<ushort>(UpperLowerDiff);
                do
                {
                    Vector64<ushort> valueVector = Vector64.Load((ushort*)ptr);
                    Vector64<ushort> resultVector = Vector64.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector64.LessThanOrEqual(valueVector, maskVectorHigh);
                    if (!resultVector.Equals(default))
                    {
                        valueVector = VectorizedToLowerOrUpperAsciiCore_64(valueVector, operationVector, resultVector, isUpper);
                        fixed (char* ptrResult = resultLazy.Value)
                            valueVector.Store((ushort*)ptrResult + (ptr - ptrStart));
                    }
                    ptr += Vector64<ushort>.Count;
                } while (ptr + Vector64<ushort>.Count < ptrEnd);
                if (ptr >= ptrEnd)
                    return resultLazy.GetValueDirectly();
            }
            if (ptr + Vector64<ushort>.Count / 2 < ptrEnd)
            {
                Vector64<ushort> maskVectorLow = Vector64.Create<ushort>(isUpper ? 'a' : 'A');
                Vector64<ushort> maskVectorHigh = Vector64.Create<ushort>(isUpper ? 'z' : 'Z');
                Vector64<ushort> operationVector = Vector64.Create<ushort>(UpperLowerDiff);
                Vector64<ushort> valueVector = default;
                uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                Vector64<ushort> resultVector = Vector64.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector64.LessThanOrEqual(valueVector, maskVectorHigh);
                if (!resultVector.Equals(default))
                {
                    valueVector = VectorizedToLowerOrUpperAsciiCore_64(valueVector, operationVector, resultVector, isUpper);
                    fixed (char* ptrResult = resultLazy.Value)
                        UnsafeHelper.CopyBlockUnaligned(ptrResult + (ptr - ptrStart), &valueVector, byteCount);
                }
                return resultLazy.GetValueDirectly();
            }
            for (int i = 0; i < Vector64<ushort>.Count / 2; i++) // CLR 會自動展開這個迴圈
            {
                LegacyToLowerOrUpperAsciiCore(ptr, ptrStart, ref resultLazy, isUpper);
                if (++ptr >= ptrEnd)
                    break;
            }
            goto Return;

#else
            if (Vector.IsHardwareAccelerated)
                goto Vector;
            goto Fallback;

        Vector:
            if (ptr + Vector<ushort>.Count < ptrEnd)
            {
                Vector<ushort> maskVectorLow = new Vector<ushort>(isUpper ? 'a' : 'A');
                Vector<ushort> maskVectorHigh = new Vector<ushort>(isUpper ? 'z' : 'Z');
                Vector<ushort> operationVector = new Vector<ushort>(UpperLowerDiff);
                do
                {
                    Vector<ushort> valueVector = UnsafeHelper.Read<Vector<ushort>>(ptr);
                    Vector<ushort> resultVector = Vector.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector.LessThanOrEqual(valueVector, maskVectorHigh);
                    if (!resultVector.Equals(default))
                    {
                        valueVector = VectorizedToLowerOrUpperAsciiCore(valueVector, operationVector, resultVector, isUpper);
                        fixed (char* ptrResult = resultLazy.Value)
                            UnsafeHelper.Write(ptrResult + (ptr - ptrStart), valueVector);
                    }
                    ptr += Vector<ushort>.Count;
                } while (ptr + Vector<ushort>.Count < ptrEnd);
                if (ptr >= ptrEnd)
                    return resultLazy.GetValueDirectly();
            }
            if (ptr + Vector<ushort>.Count / 2 < ptrEnd)
            {
                Vector<ushort> maskVectorLow = new Vector<ushort>(isUpper ? 'a' : 'A');
                Vector<ushort> maskVectorHigh = new Vector<ushort>(isUpper ? 'z' : 'Z');
                Vector<ushort> operationVector = new Vector<ushort>(UpperLowerDiff);
                Vector<ushort> valueVector = default;
                uint byteCount = unchecked((uint)((byte*)ptrEnd - (byte*)ptr));
                UnsafeHelper.CopyBlockUnaligned(&valueVector, ptr, byteCount);
                Vector<ushort> resultVector = Vector.GreaterThanOrEqual(valueVector, maskVectorLow) & Vector.LessThanOrEqual(valueVector, maskVectorHigh);
                if (!resultVector.Equals(default))
                {
                    valueVector = VectorizedToLowerOrUpperAsciiCore(valueVector, operationVector, resultVector, isUpper);
                    fixed (char* ptrResult = resultLazy.Value)
                        UnsafeHelper.CopyBlockUnaligned(ptrResult + (ptr - ptrStart), &valueVector, byteCount);
                }
                return resultLazy.GetValueDirectly();
            }
            for (int i = 0; i < Vector<ushort>.Count / 2; i++) // CLR 會自動展開這個迴圈
            {
                LegacyToLowerOrUpperAsciiCore(ptr, ptrStart, ref resultLazy, isUpper);
                if (++ptr >= ptrEnd)
                    break;
            }
            goto Return;
#endif
        Fallback:
            for (; ptr < ptrEnd; ptr++)
                LegacyToLowerOrUpperAsciiCore(ptr, ptrStart, ref resultLazy, isUpper);
            goto Return;
        Return:
            return resultLazy.GetValueDirectly();
        }

#if NET6_0_OR_GREATER
        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector512<ushort> VectorizedToLowerOrUpperAsciiCore_512(in Vector512<ushort> valueVector, in Vector512<ushort> operationVector,
            in Vector512<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector256<ushort> VectorizedToLowerOrUpperAsciiCore_256(in Vector256<ushort> valueVector, in Vector256<ushort> operationVector,
            in Vector256<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector128<ushort> VectorizedToLowerOrUpperAsciiCore_128(in Vector128<ushort> valueVector, in Vector128<ushort> operationVector,
            in Vector128<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector64<ushort> VectorizedToLowerOrUpperAsciiCore_64(in Vector64<ushort> valueVector, in Vector64<ushort> operationVector,
            in Vector64<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }
#else
        [Inline(InlineBehavior.Remove)]
        private static unsafe Vector<ushort> VectorizedToLowerOrUpperAsciiCore(in Vector<ushort> valueVector, in Vector<ushort> operationVector,
            in Vector<ushort> maskVector, [InlineParameter] bool isUpper)
        {
            if (isUpper)
                return valueVector - (operationVector & maskVector);
            return valueVector + (operationVector & maskVector);
        }
#endif

        [Inline(InlineBehavior.Remove)]
        private static unsafe void LegacyToLowerOrUpperAsciiCore(char* ptr, char* ptrStart, ref LazyTinyRefStruct<string> resultLazy, [InlineParameter] bool isUpper)
        {
            char c = *ptr;
            if (isUpper)
            {
                if (c < 'a' || c > 'z')
                    return;
            }
            else
            {
                if (c < 'A' || c > 'Z')
                    return;
            }
            fixed (char* ptrResult = resultLazy.Value)
                *(ptrResult + (ptr - ptrStart)) =
                    isUpper ? unchecked((char)(c - UpperLowerDiff)) : unchecked((char)(c + UpperLowerDiff));
        }
    }
}
