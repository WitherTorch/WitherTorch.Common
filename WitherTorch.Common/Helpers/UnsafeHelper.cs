using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

using InlineIL;

using InlineMethod;

using LocalsInit;

#pragma warning disable CS0162
#pragma warning disable CS8601
#pragma warning disable IDE0060

namespace WitherTorch.Common.Helpers
{
    [LocalsInit(false)]
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

        public static void* PointerMinValue
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => (void*)UIntPtrMinValue;
        }

        public static void* PointerMaxValue
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => (void*)UIntPtrMaxValue;
        }

        public static nint IntPtrMinValue
        {
            [Inline(InlineBehavior.Keep, export: true)]
#if NET6_0_OR_GREATER
            get => nint.MinValue;
#else
            get => PointerSizeConstant switch
            {
                sizeof(int) => int.MinValue,
                sizeof(long) => unchecked((nint)long.MinValue),
                _ => PointerSize switch
                {
                    sizeof(int) => int.MinValue,
                    sizeof(long) => unchecked((nint)long.MinValue),
                    _ => throw new NotSupportedException("Unsupported pointer size: " + PointerSize),
                },
            };
#endif
        }

        public static nint IntPtrMaxValue
        {
            [Inline(InlineBehavior.Keep, export: true)]
#if NET6_0_OR_GREATER
            get => nint.MaxValue;
#else
            get => PointerSizeConstant switch
            {
                sizeof(int) => int.MaxValue,
                sizeof(long) => unchecked((nint)long.MaxValue),
                _ => PointerSize switch
                {
                    sizeof(int) => int.MaxValue,
                    sizeof(long) => unchecked((nint)long.MaxValue),
                    _ => throw new NotSupportedException("Unsupported pointer size: " + PointerSize),
                },
            };
#endif
        }

        public static nuint UIntPtrMinValue
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => 0;
        }

        public static nuint UIntPtrMaxValue
        {
            [Inline(InlineBehavior.Keep, export: true)]
            get => unchecked((nuint)(-1));
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
        public static bool IsIntegerType<T>()
                => (typeof(T) == typeof(byte)) ||
                       (typeof(T) == typeof(short)) ||
                       (typeof(T) == typeof(int)) ||
                       (typeof(T) == typeof(long)) ||
                       (typeof(T) == typeof(sbyte)) ||
                       (typeof(T) == typeof(ushort)) ||
                       (typeof(T) == typeof(uint)) ||
                       (typeof(T) == typeof(ulong)) ||
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

        [Inline(InlineBehavior.Remove)]
        internal static bool IsUnsigned<T>()
            => (typeof(T) == typeof(byte)) ||
                   (typeof(T) == typeof(ushort)) ||
                   (typeof(T) == typeof(uint)) ||
                   (typeof(T) == typeof(ulong));

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsGreaterThan<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Cgt();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsGreaterThanUnsigned<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Cgt_Un();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsGreaterOrEqualsThan<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsGreaterOrEqualsThanUnsigned<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt_Un();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsLessThan<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsLessThanUnsigned<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt_Un();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsLessOrEqualsThan<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Cgt();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool IsLessOrEqualsThanUnsigned<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Clt_Un();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool Equals<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static bool NotEquals<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Ceq();
            IL.Emit.Ldc_I4_0();
            IL.Emit.Ceq();
            return IL.Return<bool>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Or<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Or();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T And<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.And();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Xor<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Xor();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Negate<T>(T value)
        {
            IL.Push(value);
            IL.Emit.Neg();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Not<T>(T value)
        {
            IL.Push(value);
            IL.Emit.Not();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Add<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Add();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Substract<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Sub();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Multiply<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Mul();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T Divide<T>(T a, T b)
        {
            IL.Push(a);
            IL.Push(b);
            IL.Emit.Div();
            return IL.Return<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static T DivideUnsigned<T>(T a, T b)
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
        public static T ReadUnaligned<T>(void* source)
        {
            IL.Push(source);
            IL.Emit.Unaligned(1);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUnaligned<T>(void* destination, T value)
        {
            IL.Push(destination);
            IL.Push(value);
            IL.Emit.Unaligned(1);
            IL.Emit.Stobj(typeof(T));
        }

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
        public static void CopyBlock(void* destination, void* source, uint byteCount)
        {
            IL.Push(destination);
            IL.Push(source);
            IL.Push(byteCount);
            IL.Emit.Cpblk();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static void CopyBlock(void* destination, void* source, nuint byteCount)
        {
            IL.Push(destination);
            IL.Push(source);
            IL.Push(byteCount);
            IL.Emit.Cpblk();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static void CopyBlockUnaligned(void* destination, void* source, uint byteCount)
        {
            IL.Push(destination);
            IL.Push(source);
            IL.Push(byteCount);
            IL.Emit.Unaligned(1);
            IL.Emit.Cpblk();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static void CopyBlockUnaligned(void* destination, void* source, nuint byteCount)
        {
            IL.Push(destination);
            IL.Push(source);
            IL.Push(byteCount);
            IL.Emit.Unaligned(1);
            IL.Emit.Cpblk();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe ref TTo As<TFrom, TTo>(ref TFrom source)
        {
#if NET8_0_OR_GREATER
            return ref Unsafe.As<TFrom, TTo>(ref source);
#else
            IL.PushInRef(ref source);
            return ref IL.ReturnRef<TTo>();
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe TTo As<TFrom, TTo>(TFrom source)
        {
#if NET8_0_OR_GREATER
            return Unsafe.As<TFrom, TTo>(ref source);
#else
            IL.Push(source);
            return IL.Return<TTo>();
#endif
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe T As<T>(object source) where T : class
        {
#if NET8_0_OR_GREATER
            return Unsafe.As<T>(source);
#else
            IL.Push(source);
            return IL.Return<T>();
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe TTo RewriteManagedObjectType<TFrom, TTo>(TFrom obj, TTo template) where TFrom : class where TTo : class
        {
#pragma warning disable CS8500
            **(nuint***)&obj = **(nuint***)&template;
#pragma warning restore CS8500
            return As<TFrom, TTo>(obj);
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void Fill<T>(T* ptr, T value, [InlineParameter] int count) where T : unmanaged
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

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void InitBlockUnaligned(void* ptr, byte value, uint size)
        {
            IL.Push(ptr);
            IL.Push(value);
            IL.Push(size);
            IL.Emit.Unaligned(sizeof(byte));
            IL.Emit.Initblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint ByteOffset<T>(ref T origin, ref T target)
        {
            IL.PushInRef(ref target);
            IL.PushInRef(ref origin);
            IL.Emit.Sub();
            return IL.Return<nint>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint ByteOffsetUnsigned<T>(ref T origin, ref T target)
        {
            IL.PushInRef(ref target);
            IL.PushInRef(ref origin);
            IL.Emit.Sub();
            return IL.Return<nuint>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AddByteOffset<T>(ref T source, nuint byteOffset)
        {
            IL.PushInRef(ref source);
            IL.Push(byteOffset);
            IL.Emit.Add();
            return ref IL.ReturnRef<T>();
        }

#pragma warning disable CS8500
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T NullRef<T>()
        {
            IL.Emit.Ldnull();
            return ref IL.ReturnRef<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ref T AsRef<T>(T* value)
        {
            IL.Emit.Ldarg_0();
            return ref IL.ReturnRef<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ref T AsRefIn<T>(in T value)
        {
            IL.Emit.Ldarg_0();
            return ref IL.ReturnRef<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static ref T AsRefOut<T>(out T value)
        {
            IL.PushOutRef(out value);
            return ref IL.ReturnRef<T>();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe T* AsPointerRef<T>(ref T value)
        {
            IL.Emit.Ldarg_0();
            return (T*)IL.ReturnPointer();
        }

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe void** AsPointerRef(ref void* value)
        {
            IL.Emit.Ldarg_0();
            return (void**)IL.ReturnPointer();
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

        [Inline(InlineBehavior.Keep, export: true)]
        public static unsafe uint SizeOf<T>()
        {
            IL.Emit.Sizeof<T>();
            return IL.Return<uint>();
        }
    }
}
