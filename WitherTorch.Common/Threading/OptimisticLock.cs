using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Threading
{
    public static class OptimisticLock
    {
        public static void Enter(ref int versionReference)
        {
            int version = versionReference;
            do
            {
                int newVersion = InterlockedHelper.CompareExchange(ref versionReference, version + 1, version);
                if (newVersion == version)
                    break;
                version = newVersion;
            } while (true);
        }

        public static void Enter(ref uint versionReference)
        {
            uint version = versionReference;
            do
            {
                uint newVersion = InterlockedHelper.CompareExchange(ref versionReference, version + 1, version);
                if (newVersion == version)
                    break;
                version = newVersion;
            } while (true);
        }

        public static void Enter(ref long versionReference)
        {
            long version = versionReference;
            do
            {
                long newVersion = InterlockedHelper.CompareExchange(ref versionReference, version + 1, version);
                if (newVersion == version)
                    break;
                version = newVersion;
            } while (true);
        }

        public static void Enter(ref ulong versionReference)
        {
            ulong version = versionReference;
            do
            {
                ulong newVersion = InterlockedHelper.CompareExchange(ref versionReference, version + 1, version);
                if (newVersion == version)
                    break;
                version = newVersion;
            } while (true);
        }
    }
}
