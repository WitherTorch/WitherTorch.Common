using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Threading
{
    public static class OptimisticLock
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OptimisticLock<T> Create<T>() where T : new()
            => new OptimisticLock<T>(new T());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OptimisticLock<T> Create<T>(T value)
            => new OptimisticLock<T>(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Enter<T>(ref T versionReference) where T : unmanaged => GenericVolatileRead(ref versionReference);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(location))]
        public static TResult? EnterWithObject<TVersion, TResult>(ref readonly TResult? location, ref readonly TVersion versionReference, out TVersion version)
            where TVersion : unmanaged where TResult : class
        {
            version = GenericVolatileRead(in versionReference);
            return Volatile.Read(ref UnsafeHelper.AsRefIn(in location));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult EnterWithPrimitive<TVersion, TResult>(ref readonly TResult location, ref readonly TVersion versionReference, out TVersion version)
            where TVersion : unmanaged where TResult : unmanaged
        {
            version = GenericVolatileRead(in versionReference);
            return GenericVolatileRead(in location);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Increase<T>(ref T versionReference) where T : unmanaged
        {
            if (typeof(T) == typeof(int))
                return UnsafeHelper.As<int, T>(InterlockedHelper.Increment(ref UnsafeHelper.As<T, int>(ref versionReference)));
            if (typeof(T) == typeof(uint))
                return UnsafeHelper.As<uint, T>(InterlockedHelper.Increment(ref UnsafeHelper.As<T, uint>(ref versionReference)));
            if (typeof(T) == typeof(long))
                return UnsafeHelper.As<long, T>(InterlockedHelper.Increment(ref UnsafeHelper.As<T, long>(ref versionReference)));
            if (typeof(T) == typeof(ulong))
                return UnsafeHelper.As<ulong, T>(InterlockedHelper.Increment(ref UnsafeHelper.As<T, ulong>(ref versionReference)));
            if (typeof(T) == typeof(nint))
                return UnsafeHelper.As<nint, T>(InterlockedHelper.Increment(ref UnsafeHelper.As<T, nint>(ref versionReference)));
            if (typeof(T) == typeof(nuint))
                return UnsafeHelper.As<nuint, T>(InterlockedHelper.Increment(ref UnsafeHelper.As<T, nuint>(ref versionReference)));
            throw new NotSupportedException($"Unsupported version type: {typeof(T).FullName}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLeave<T>(ref readonly T versionReference, ref T currentVersion) where T : unmanaged
        {
            T newVersion = GenericVolatileRead(in versionReference);
            if (UnsafeHelper.Equals(newVersion, currentVersion))
                return true;
            GenericVolatileWrite(ref currentVersion, newVersion);
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLeaveWithObject<TVersion, TObject>(ref readonly TObject? objectReference, ref readonly TVersion versionReference,
           [NotNullIfNotNull(nameof(objectReference))] ref TObject? currentObject, ref TVersion currentVersion) where TVersion : unmanaged where TObject : class
        {
            TVersion newVersion = GenericVolatileRead(in versionReference);
            if (UnsafeHelper.Equals(newVersion, currentVersion))
                return true;
            GenericVolatileWrite(ref currentVersion, newVersion);
            Volatile.Write(ref currentObject, Volatile.Read(ref UnsafeHelper.AsRefIn(in objectReference)));
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLeaveWithPrimitive<TVersion, TPrimitive>(ref readonly TPrimitive primitiveReference, ref readonly TVersion versionReference,
            ref TPrimitive currentPrimitive, ref TVersion currentVersion) where TVersion : unmanaged where TPrimitive : unmanaged
        {
            TVersion newVersion = GenericVolatileRead(in versionReference);
            if (UnsafeHelper.Equals(newVersion, currentVersion))
                return true;
            GenericVolatileWrite(ref currentVersion, newVersion);
            GenericVolatileWrite(ref currentPrimitive, GenericVolatileRead(in primitiveReference));
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T GenericVolatileRead<T>(ref readonly T location) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                return UnsafeHelper.As<bool, T>(Volatile.Read(ref UnsafeHelper.As<T, bool>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(byte))
                return UnsafeHelper.As<byte, T>(Volatile.Read(ref UnsafeHelper.As<T, byte>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(sbyte))
                return UnsafeHelper.As<sbyte, T>(Volatile.Read(ref UnsafeHelper.As<T, sbyte>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(short))
                return UnsafeHelper.As<short, T>(Volatile.Read(ref UnsafeHelper.As<T, short>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(ushort))
                return UnsafeHelper.As<ushort, T>(Volatile.Read(ref UnsafeHelper.As<T, ushort>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(int))
                return UnsafeHelper.As<int, T>(Volatile.Read(ref UnsafeHelper.As<T, int>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(uint))
                return UnsafeHelper.As<uint, T>(Volatile.Read(ref UnsafeHelper.As<T, uint>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(long))
                return UnsafeHelper.As<long, T>(Volatile.Read(ref UnsafeHelper.As<T, long>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(ulong))
                return UnsafeHelper.As<ulong, T>(Volatile.Read(ref UnsafeHelper.As<T, ulong>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(nint))
                return UnsafeHelper.As<nint, T>(Volatile.Read(ref UnsafeHelper.As<T, nint>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(nuint))
                return UnsafeHelper.As<nuint, T>(Volatile.Read(ref UnsafeHelper.As<T, nuint>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(float))
                return UnsafeHelper.As<float, T>(Volatile.Read(ref UnsafeHelper.As<T, float>(ref UnsafeHelper.AsRefIn(in location))));
            if (typeof(T) == typeof(double))
                return UnsafeHelper.As<double, T>(Volatile.Read(ref UnsafeHelper.As<T, double>(ref UnsafeHelper.AsRefIn(in location))));
            throw new NotSupportedException($"Unsupported version type: {typeof(T).FullName}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GenericVolatileWrite<T>(ref T location, T value) where T : unmanaged
        {
            if (typeof(T) == typeof(bool))
                Volatile.Write(ref UnsafeHelper.As<T, bool>(ref location), UnsafeHelper.As<T, bool>(value));
            else if (typeof(T) == typeof(byte))
                Volatile.Write(ref UnsafeHelper.As<T, byte>(ref location), UnsafeHelper.As<T, byte>(value));
            else if (typeof(T) == typeof(sbyte))
                Volatile.Write(ref UnsafeHelper.As<T, sbyte>(ref location), UnsafeHelper.As<T, sbyte>(value));
            else if (typeof(T) == typeof(short))
                Volatile.Write(ref UnsafeHelper.As<T, short>(ref location), UnsafeHelper.As<T, short>(value));
            else if (typeof(T) == typeof(ushort))
                Volatile.Write(ref UnsafeHelper.As<T, ushort>(ref location), UnsafeHelper.As<T, ushort>(value));
            else if (typeof(T) == typeof(int))
                Volatile.Write(ref UnsafeHelper.As<T, int>(ref location), UnsafeHelper.As<T, int>(value));
            else if (typeof(T) == typeof(uint))
                Volatile.Write(ref UnsafeHelper.As<T, uint>(ref location), UnsafeHelper.As<T, uint>(value));
            else if (typeof(T) == typeof(long))
                Volatile.Write(ref UnsafeHelper.As<T, long>(ref location), UnsafeHelper.As<T, long>(value));
            else if (typeof(T) == typeof(ulong))
                Volatile.Write(ref UnsafeHelper.As<T, ulong>(ref location), UnsafeHelper.As<T, ulong>(value));
            else if (typeof(T) == typeof(nint))
                Volatile.Write(ref UnsafeHelper.As<T, nint>(ref location), UnsafeHelper.As<T, nint>(value));
            else if (typeof(T) == typeof(nuint))
                Volatile.Write(ref UnsafeHelper.As<T, nuint>(ref location), UnsafeHelper.As<T, nuint>(value));
            else if (typeof(T) == typeof(float))
                Volatile.Write(ref UnsafeHelper.As<T, float>(ref location), UnsafeHelper.As<T, float>(value));
            else if (typeof(T) == typeof(double))
                Volatile.Write(ref UnsafeHelper.As<T, double>(ref location), UnsafeHelper.As<T, double>(value));
            else
                throw new NotSupportedException($"Unsupported version type: {typeof(T).FullName}");
        }
    }

    public sealed class OptimisticLock<T>
    {
        private T _value;
        private nuint _version;

        public T Value
        {
            get => GetValueCore();
            set
            {
                _value = value;
                OptimisticLock.Increase(ref _version);
            }
        }

        public OptimisticLock(T value)
        {
            _value = value;
            _version = 0;
        }

        private T GetValueCore()
        {
            T result;
            ref nuint versionRef = ref _version;

            nuint currentVersion = OptimisticLock.Enter(ref versionRef);
            do
            {
                result = _value;
            } while (!OptimisticLock.TryLeave(ref versionRef, ref currentVersion));
            return result;
        }

        public TResult Read<TResult>(Func<T, TResult> factory)
        {
            TResult result;
            ref nuint versionRef = ref _version;
            nuint currentVersion = OptimisticLock.Enter(ref versionRef);
            do
            {
                result = factory.Invoke(_value);
            } while (!OptimisticLock.TryLeave(ref versionRef, ref currentVersion));
            return result;
        }

        public TResult Read<TArg, TResult>(Func<T, TArg, TResult> factory, TArg argument)
        {
            TResult result;
            ref nuint versionRef = ref _version;
            nuint currentVersion = OptimisticLock.Enter(ref versionRef);
            do
            {
                result = factory.Invoke(_value, argument);
            } while (!OptimisticLock.TryLeave(ref versionRef, ref currentVersion));
            return result;
        }

        public void Write(Action<T> action)
        {
            T value = GetValueCore();
            action.Invoke(value);
            OptimisticLock.Increase(ref _version);
        }

        public void Write<TArg>(Action<T, TArg> action, TArg argument)
        {
            T value = GetValueCore();
            action.Invoke(value, argument);
            OptimisticLock.Increase(ref _version);
        }

        public TResult Write<TResult>(Func<T, TResult> factory)
        {
            T value = GetValueCore();
            TResult result = factory.Invoke(value);
            OptimisticLock.Increase(ref _version);
            return result;
        }

        public TResult Write<TArg, TResult>(Func<T, TArg, TResult> factory, TArg argument)
        {
            T value = GetValueCore();
            TResult result = factory.Invoke(value, argument);
            OptimisticLock.Increase(ref _version);
            return result;
        }

        public void MarkChanged() => OptimisticLock.Increase(ref _version);

        public override string? ToString() => GetValueCore()?.ToString();
    }
}
