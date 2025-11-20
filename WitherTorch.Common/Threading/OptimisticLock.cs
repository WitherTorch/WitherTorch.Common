using System;
using System.Runtime.CompilerServices;

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
        public static void Enter(ref int versionReference, out int currentVersion) => currentVersion = versionReference;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enter(ref uint versionReference, out uint currentVersion) => currentVersion = versionReference;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enter(ref long versionReference, out long currentVersion) => currentVersion = versionReference;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enter(ref ulong versionReference, out ulong currentVersion) => currentVersion = versionReference;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enter(ref nint versionReference, out nint currentVersion) => currentVersion = versionReference;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enter(ref nuint versionReference, out nuint currentVersion) => currentVersion = versionReference;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLeave(ref int versionReference, ref int currentVersion)
        {
            int version = currentVersion;
            int newVersion = InterlockedHelper.CompareExchange(ref versionReference, version + 1, version);
            return newVersion == version;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLeave(ref uint versionReference, ref uint currentVersion)
        {
            uint version = currentVersion;
            uint newVersion = InterlockedHelper.CompareExchange(ref versionReference, version + 1, version);
            return newVersion == version;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLeave(ref long versionReference, ref long currentVersion)
        {
            long version = currentVersion;
            long newVersion = InterlockedHelper.CompareExchange(ref versionReference, version + 1, version);
            return newVersion == version;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLeave(ref ulong versionReference, ref ulong currentVersion)
        {
            ulong version = currentVersion;
            ulong newVersion = InterlockedHelper.CompareExchange(ref versionReference, version + 1, version);
            return newVersion == version;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLeave(ref nint versionReference, ref nint currentVersion)
        {
            nint version = currentVersion;
            nint newVersion = InterlockedHelper.CompareExchange(ref versionReference, version + 1, version);
            return newVersion == version;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLeave(ref nuint versionReference, ref nuint currentVersion)
        {
            nuint version = currentVersion;
            nuint newVersion = InterlockedHelper.CompareExchange(ref versionReference, version + 1, version);
            return newVersion == version;
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
                _version++;
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

            OptimisticLock.Enter(ref versionRef, out nuint currentVersion);
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
            OptimisticLock.Enter(ref versionRef, out nuint currentVersion);
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
            OptimisticLock.Enter(ref versionRef, out nuint currentVersion);
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
            _version++;
        }

        public TResult Write<TResult>(Func<T, TResult> factory)
        {
            T value = GetValueCore();
            TResult result = factory.Invoke(value);
            _version++;
            return result;
        }

        public TResult Write<TArg, TResult>(Func<T, TArg, TResult> factory, TArg argument)
        {
            T value = GetValueCore();
            TResult result = factory.Invoke(value, argument);
            _version++;
            return result;
        }

        public void MarkChanged() => _version++;

        public override string? ToString() => GetValueCore()?.ToString();
    }
}
