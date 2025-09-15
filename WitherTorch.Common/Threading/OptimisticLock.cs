using System.Runtime.CompilerServices;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Threading
{
    public static class OptimisticLock
    {
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
}
