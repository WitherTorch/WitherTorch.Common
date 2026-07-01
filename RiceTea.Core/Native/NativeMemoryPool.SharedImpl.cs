using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

using RiceTea.Core.Extensions;
using RiceTea.Core.Helpers;
using RiceTea.Core.Threading;

namespace RiceTea.Core.Native;

unsafe partial class NativeMemoryPool
{
    private static partial NativeMemoryPool CreateSharedPool() => new SharedImpl();

    private sealed partial class SharedImpl : NativeMemoryPool
    {
        private const int GenerationNull = 0;
        private const int GenerationEden = 1;
        private const int MaxLocalGeneration = 3;
        private const int GlobalArrayStackPreserveCount = 1;

        private const int LocalBucketCount = 6;
        private const int LocalMemoryBlockSizeLimit = 1 << (LocalBucketCount + 3);
        private const int GlobalBucketCount = 17;
        private const int GlobalMemoryBlockSizeLimit = 1 << (GlobalBucketCount + 3);

        [ThreadStatic]
        private static LocalMemoryBlock[]? _localArrayBuckets;
        [ThreadStatic]
        private static GlobalMemoryBlockStack[]? _globalArrayStackBuckets;

        private readonly HashSet<GCHandle> _localArrayCollectSet = new();
        private readonly ProcessorLocal<GlobalMemoryBlockStack[]> _globalArrayStacksGroup = new(CreateGlobalArrayStacks);
        private readonly Lock _syncLock = new();

        private ulong _lastTrimTimestamp = 0;

        public SharedImpl() => LocalMemoryBlockTrimCaller.Register(this);

        private static GlobalMemoryBlockStack[] CreateGlobalArrayStacks()
        {
            GlobalMemoryBlockStack[] stacks = new GlobalMemoryBlockStack[GlobalBucketCount];
            ref GlobalMemoryBlockStack stackRef = ref UnsafeHelper.GetArrayDataReference(stacks);
            for (nuint i = 16, j = 0; i <= GlobalMemoryBlockSizeLimit; i <<= 1, j++)
                UnsafeHelper.AddTypedOffset(ref stackRef, j) = new GlobalMemoryBlockStack(i);
            return stacks;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static LocalMemoryBlock[]? GetLocalArrayBucketsOrNull() => _localArrayBuckets;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private LocalMemoryBlock[] GetOrCreateLocalArrayBuckets()
        {
            LocalMemoryBlock[]? buckets = _localArrayBuckets;
            if (buckets is null)
            {
                buckets = new LocalMemoryBlock[LocalBucketCount];
                ref LocalMemoryBlock bucketsRef = ref UnsafeHelper.GetArrayDataReference(buckets);
                for (nuint i = 16, j = 0; i <= LocalMemoryBlockSizeLimit; i <<= 1, j++)
                    UnsafeHelper.AddTypedOffset(ref bucketsRef, j) = new LocalMemoryBlock(i);
                _localArrayBuckets = buckets;
                lock (_syncLock)
                    _localArrayCollectSet.Add(GCHandle.Alloc(buckets, GCHandleType.Weak));
            }
            return buckets;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GlobalMemoryBlockStack GetGlobalArrayStack(int index) => (_globalArrayStackBuckets ??= _globalArrayStacksGroup.Value).AsUnsafeRef()[index];

        protected override void* RentCore(ref nuint capacity)
        {
            if (capacity > GlobalMemoryBlockSizeLimit)
                return NativeMethods.AllocMemory(capacity);

            capacity >>= 4;
            int index = MathHelper.Log2(capacity);
            index += MathHelper.BooleanToInt32(capacity >= (1U << index));
            capacity = (nuint)(1 << (index + 4));

            void* result;
            if (index < LocalBucketCount)
                goto Local;
            else
                goto Global;

        Local:
            LocalMemoryBlock[]? localBuckets = GetLocalArrayBucketsOrNull();
            if (localBuckets is null)
                goto GlobalTemporarily;

            LocalMemoryBlock localBucket = localBuckets.AsUnsafeRef()[index];
            while (InterlockedHelper.CompareExchange(ref localBucket.Barrier, 1, 0) != 0)
            {
                SpinWait spin = new SpinWait();
                while (InterlockedHelper.Read(ref localBucket.Barrier) != 0)
                    spin.SpinOnce();
            }
            try
            {
                result = (void*)ReferenceHelper.Exchange(ref localBucket.Pointer, default);
                if (result is not null)
                {
                    localBucket.Generation = GenerationEden;
                    Thread.MemoryBarrier();
                    return result;
                }
                if (localBucket.Generation == GenerationEden)
                    goto Global;
                return NativeMethods.AllocMemoryBlock(capacity).NativePointer;
            }
            finally
            {
                InterlockedHelper.Write(ref localBucket.Barrier, 0);
            }

        Global:
            return GetGlobalArrayStack(index).Pop();

        GlobalTemporarily:
            return GetGlobalArrayStack(index).Pop();
        }

        protected override void ReturnCore(void* ptr, nuint length)
        {
            if (length < 16 || length > GlobalMemoryBlockSizeLimit || !MathHelper.IsPow2(length))
                return;
            int index = MathHelper.Log2((uint)length) - 4;
            if (index < LocalBucketCount)
                goto Local;
            else
                goto Global;

        Local:
            LocalMemoryBlock localBucket = GetOrCreateLocalArrayBuckets().AsUnsafeRef()[index];
            if (InterlockedHelper.Read(ref localBucket.Generation) != GenerationNull || InterlockedHelper.Read(ref localBucket.Pointer) != default)
                goto Global;
            while (InterlockedHelper.CompareExchange(ref localBucket.Barrier, 1, 0) != 0)
            {
                SpinWait spin = new SpinWait();
                while (InterlockedHelper.Read(ref localBucket.Barrier) != 0)
                    spin.SpinOnce();
            }
            try
            {
                localBucket.Pointer = (nuint)ptr;
                localBucket.Generation = GenerationEden;
            }
            finally
            {
                InterlockedHelper.Write(ref localBucket.Barrier, 0);
            }
            return;

        Global:
            GetGlobalArrayStack(index).Push(ptr);
        }

        private void TrimLocals()
        {
            const ulong MinimumTrimPeriod = 10 * TimeSpan.TicksPerSecond;

            Lock syncLock = _syncLock;
            try
            {
                if (!syncLock.TryEnter())
                    return;
                try
                {
                    ulong now = NativeMethods.GetTicksForSystem();
                    if (now - _lastTrimTimestamp < MinimumTrimPeriod)
                        return;
                    _lastTrimTimestamp = now;
                    _localArrayCollectSet.RemoveWhere(static (GCHandle handle) =>
                    {
                        if (handle.Target is not LocalMemoryBlock[] buckets)
                            return true;

                        ref LocalMemoryBlock bucketRef = ref UnsafeHelper.GetArrayDataReference(buckets);
                        for (int i = 0; i < LocalBucketCount; i++)
                        {
                            LocalMemoryBlock bucket = UnsafeHelper.AddTypedOffset(ref bucketRef, i);
                            while (InterlockedHelper.CompareExchange(ref bucket.Barrier, 1, 0) != 0)
                            {
                                SpinWait spin = new SpinWait();
                                while (InterlockedHelper.Read(ref bucket.Barrier) != 0)
                                    spin.SpinOnce();
                            }
                            try
                            {
                                if (bucket.Pointer == default || ++bucket.Generation <= MaxLocalGeneration)
                                    continue;
                                bucket.Generation = GenerationEden;
                                nuint pointer = InterlockedHelper.Exchange(ref bucket.Pointer, default);
                                if (pointer != default)
                                    NativeMethods.FreeMemoryBlock(new NativeMemoryBlock((void*)pointer, 1U << (i + 4)));
                            }
                            finally
                            {
                                InterlockedHelper.Write(ref bucket.Barrier, 0);
                            }
                        }

                        return false;
                    });
                }
                finally
                {
                    syncLock.Exit();
                }
            }
            finally
            {
                LocalMemoryBlockTrimCaller.Register(this);
            }
        }

        private sealed class LocalMemoryBlock
        {
            private readonly nuint _blockSize;

            public nuint Pointer;
            public nuint Generation;
            public nuint Barrier;

            public LocalMemoryBlock(nuint blockSize) => _blockSize = blockSize;

            ~LocalMemoryBlock() => NativeMethods.FreeMemoryBlock(new NativeMemoryBlock((void*)Pointer, _blockSize));
        }

        private sealed class LocalMemoryBlockTrimCaller
        {
            private readonly SharedImpl _owner;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private LocalMemoryBlockTrimCaller(SharedImpl owner) => _owner = owner;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Register(SharedImpl owner) => new LocalMemoryBlockTrimCaller(owner);

            ~LocalMemoryBlockTrimCaller() => _owner.TrimLocals();
        }
    }
}