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
    public static unsafe partial class M512
    {
        public const int SizeInBytes = 512 / 8;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly M512<T> AsM512<T>(in this Vector<T> vector) where T : struct
        {
            if (sizeof(Vector<T>) != SizeInBytes)
                throw new PlatformNotSupportedException();
            return ref UnsafeHelper.As<Vector<T>, M512<T>>(ref UnsafeHelper.AsRefIn(in vector));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly Vector<T> AsVector<T>(in this M512<T> _this) where T : struct
        {
            if (sizeof(Vector<T>) != SizeInBytes)
                throw new PlatformNotSupportedException();
            return ref UnsafeHelper.As<M512<T>, Vector<T>>(ref UnsafeHelper.AsRefIn(in _this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly M512<TTo> As<TFrom, TTo>(in this M512<TFrom> _this)
            => ref UnsafeHelper.As<M512<TFrom>, M512<TTo>>(ref UnsafeHelper.AsRefIn(in _this));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M512<T> Load<T>(T* source) => UnsafeHelper.ReadUnaligned<M512<T>>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Store<T>(this M512<T> _this, T* destination) => UnsafeHelper.WriteUnaligned(destination, _this);
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct M512<T>
    {
        private static readonly int _count = M512.SizeInBytes / sizeof(T);

        internal readonly Register512 _register;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(out M256<T> low, out M256<T> high)
        {
            fixed (byte* ptr = _register)
            {
                low = UnsafeHelper.ReadUnaligned<M256<T>>(ptr);
                high = UnsafeHelper.ReadUnaligned<M256<T>>(ptr + M256.SizeInBytes);
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

    [StructLayout(LayoutKind.Sequential, Size = M512.SizeInBytes)]
    internal readonly unsafe struct Register512
    {
        private readonly byte _head;

        public readonly ref readonly byte GetPinnableReference() => ref _head;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T GetValue<T>(nuint offset)
            => UnsafeHelper.AddByteOffset(ref UnsafeHelper.As<byte, T>(ref UnsafeHelper.AsRefIn(in _head)), offset);
    }
}
