using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using LocalsInit;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Text;

#pragma warning disable CS8500
#pragma warning disable CS9084

namespace WitherTorch.Common.Intrinsics
{
    public static unsafe partial class M128
    {
        public const int SizeInBytes = 128 / 8;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly M128<T> AsM128<T>(in this Vector<T> vector) where T : struct
        {
            if (sizeof(Vector<T>) != SizeInBytes)
                throw new PlatformNotSupportedException();
            return ref UnsafeHelper.As<Vector<T>, M128<T>>(ref UnsafeHelper.AsRefIn(in vector));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly Vector<T> AsVector<T>(in this M128<T> _this) where T : struct
        {
            if (sizeof(Vector<T>) != SizeInBytes)
                throw new PlatformNotSupportedException();
            return ref UnsafeHelper.As<M128<T>, Vector<T>>(ref UnsafeHelper.AsRefIn(in _this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly M128<TTo> As<TFrom, TTo>(in this M128<TFrom> _this)
            => ref UnsafeHelper.As<M128<TFrom>, M128<TTo>>(ref UnsafeHelper.AsRefIn(in _this));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M128<T> Load<T>(T* source) => UnsafeHelper.ReadUnaligned<M128<T>>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Store<T>(this M128<T> _this, T* destination) => UnsafeHelper.WriteUnaligned(destination, _this);
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct M128<T>
    {
        private static readonly int _count = M128.SizeInBytes / sizeof(T);

        internal readonly Register128 _register;

        public static int Count => _count;

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index < 0 || index > _count)
                    throw new IndexOutOfRangeException();
                return _register.GetValue<T>(unchecked((nuint)index * UnsafeHelper.SizeOf<T>()));
            }
        }

        [LocalsInit(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override readonly string ToString()
        {
            using StringBuilderTiny builder = new StringBuilderTiny();
            if (Limits.UseStackallocStringBuilder)
            {
                char* buffer = stackalloc char[Limits.MaxStackallocChars];
                builder.SetStartPointer(buffer, Limits.MaxStackallocChars);
            }
            builder.Append('[');
            for (int i = 0; i < _count; i++)
            {
                builder.Append(this[i]?.ToString() ?? "null");
                builder.Append(", ");
            }
            builder.RemoveLast();
            builder.RemoveLast();
            builder.Append(']');
            return builder.ToString();
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = M128.SizeInBytes)]
    internal readonly unsafe struct Register128
    {
        private readonly byte _head;

        public readonly ref readonly byte GetPinnableReference() => ref _head;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T GetValue<T>(nuint offset)
            => UnsafeHelper.AddByteOffset(ref UnsafeHelper.As<byte, T>(ref UnsafeHelper.AsRefIn(in _head)), offset);
    }
}
