using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

using InlineIL;

using InlineMethod;

#pragma warning disable CS0162
#pragma warning disable CS8601
#pragma warning disable IDE0060

namespace WitherTorch.Common.Helpers
{
#if NET8_0_OR_GREATER
    [SkipLocalsInit]
#else
    [LocalsInit.LocalsInit(false)]
#endif
    public static unsafe partial class UnsafeHelper
    {
        public const int PointerSizeConstant_Indeterminate = 0;

        public const int PointerSizeConstant
#if ANYCPU
                = PointerSizeConstant_Indeterminate;
#elif B32_ARCH
                = sizeof(uint);
#elif B64_ARCH
                = sizeof(ulong);
#else
                = PointerSizeConstant_Indeterminate;
#endif

        private static readonly ConcurrentDictionary<Type, bool> _unmanagedTypeCheckCacheDict = new ConcurrentDictionary<Type, bool>();

        public static int PointerSize
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => PointerSizeConstant switch
            {
                PointerSizeConstant_Indeterminate => sizeof(void*),
                _ => PointerSizeConstant,
            };
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsPrimitiveType<T>()
                => (typeof(T) == typeof(bool)) ||
                       (typeof(T) == typeof(byte)) ||
                       (typeof(T) == typeof(short)) ||
                       (typeof(T) == typeof(int)) ||
                       (typeof(T) == typeof(long)) ||
                       (typeof(T) == typeof(sbyte)) ||
                       (typeof(T) == typeof(ushort)) ||
                       (typeof(T) == typeof(uint)) ||
                       (typeof(T) == typeof(ulong)) ||
                       (typeof(T) == typeof(float)) ||
                       (typeof(T) == typeof(double)) ||
                       (typeof(T) == typeof(decimal)) ||
                       (typeof(T) == typeof(char)) ||
                       (typeof(T) == typeof(nint)) ||
                       (typeof(T) == typeof(nuint));

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsPrimitiveType([InlineParameter] Type type)
                => (type == typeof(bool)) ||
                       (type == typeof(byte)) ||
                       (type == typeof(short)) ||
                       (type == typeof(int)) ||
                       (type == typeof(long)) ||
                       (type == typeof(sbyte)) ||
                       (type == typeof(ushort)) ||
                       (type == typeof(uint)) ||
                       (type == typeof(ulong)) ||
                       (type == typeof(float)) ||
                       (type == typeof(double)) ||
                       (type == typeof(decimal)) ||
                       (type == typeof(char)) ||
                       (type == typeof(nint)) ||
                       (type == typeof(nuint));

#pragma warning disable CS0618
        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsUnmanagedType<T>()
                => IsPrimitiveType<T>() || typeof(T).IsEnum || typeof(T).IsPointer || IsUnmanageTypeSlow(typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static bool IsUnmanagedType(Type type)
            => IsPrimitiveType(type) || type.IsEnum || type.IsPointer || IsUnmanageTypeSlow(type);
#pragma warning restore CS0618

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Obsolete("Don't call this method directly!")]
        public static bool IsUnmanageTypeSlow(Type type)
            => type.IsValueType && _unmanagedTypeCheckCacheDict.GetOrAdd(type, IsUnmanageTypeSlowCore);

        private static bool IsUnmanageTypeSlowCore(Type type)
        {
            foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!IsUnmanagedType(field.FieldType))
                    return false;
            }
            return true;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsGreaterThan<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Cgt();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsGreaterThanUnsigned<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Cgt_Un();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsGreaterOrEqualsThan<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsGreaterOrEqualsThanUnsigned<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt_Un();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsLessThan<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsLessThanUnsigned<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt_Un();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsLessOrEqualsThan<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Cgt();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsLessOrEqualsThanUnsigned<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt_Un();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool Equals<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool NotEquals<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Ceq();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Add<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Add();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Substract<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Sub();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Multiply<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Mul();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Divide<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Div();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T DivideUnsigned<T>(T a, T b) where T : unmanaged
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Div_Un();
            return IL.Return<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(void* source)
        {
            IL.Push(source);
            IL.Emit.Ldobj(typeof(T));
            return IL.Return<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(void* destination, T value)
        {
            IL.Push(destination);
            IL.Push(value);
            IL.Emit.Stobj(typeof(T));
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SwapBlock(void* destination, void* source, uint byteCount)
        {
            byte* iteratorSource = (byte*)source;
            byte* iteratorDest = (byte*)destination;
            uint blockSize = Math.Min(256, byteCount);
            byte* buffer = stackalloc byte[unchecked((int)blockSize)];
            do
            {
                CopyBlock(buffer, iteratorSource, blockSize);
                CopyBlock(iteratorSource, iteratorDest, blockSize);
                CopyBlock(iteratorDest, buffer, blockSize);
                iteratorSource += blockSize;
                iteratorDest += blockSize;
                byteCount -= blockSize;
                if (byteCount >= blockSize)
                    continue;
                blockSize = byteCount;
            }
            while (byteCount > 0u);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(void* destination, void* source, uint byteCount)
        {
            IL.Push(destination);
            IL.Push(source);
            IL.Push(byteCount);
            IL.Emit.Cpblk();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlockUnaligned(void* destination, void* source, uint byteCount)
        {
            IL.Push(destination);
            IL.Push(source);
            IL.Push(byteCount);
            IL.Emit.Unaligned(1);
            IL.Emit.Cpblk();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static unsafe ref TTo As<TFrom, TTo>(ref TFrom source)
        {
            IL.PushInRef(ref source);
            return ref IL.ReturnRef<TTo>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static unsafe TTo As<TFrom, TTo>(TFrom source)
        {
            IL.Push(source);
            return IL.Return<TTo>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SecuritySafeCritical]
        public static unsafe T As<T>(object source) where T : class
        {
            IL.Push(source);
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void Fill<T>(T* ptr, T value, int count) where T : unmanaged
        {
            for (int i = 0; i < count; i++)
                ptr[i] = value;
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void InitBlock<T>(ref T location, byte value, int count) where T : unmanaged
        {
            IL.Emit.Ldarg_0();
            IL.Push(value);
            IL.Push(count);
            IL.Push(sizeof(T));
            IL.Emit.Mul();
            IL.Emit.Initblk();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void InitBlock(void* ptr, byte value, uint size)
        {
            IL.Push(ptr);
            IL.Push(value);
            IL.Push(size);
            IL.Emit.Initblk();
        }

#pragma warning disable CS8500
        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe T* AsPointerRef<T>(ref T value)
        {
            IL.Emit.Ldarg_0();
            return (T*)IL.ReturnPointer();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe T* AsPointerIn<T>(in T value)
        {
            IL.Emit.Ldarg_0();
            return (T*)IL.ReturnPointer();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe T* AsPointerOut<T>(out T value)
        {
            IL.PushOutRef(out value);
            return (T*)IL.ReturnPointer();
        }
#pragma warning restore CS8500

        [Inline(InlineBehavior.Keep, export: true)]
        public static void SkipInit<T>(out T value)
        {
#if NET8_0_OR_GREATER
            Unsafe.SkipInit(out value);
#else
            _ = AsPointerOut(out value);
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe int SizeOf<T>()
        {
            IL.Emit.Sizeof<T>();
            return IL.Return<int>();
        }
    }
}
