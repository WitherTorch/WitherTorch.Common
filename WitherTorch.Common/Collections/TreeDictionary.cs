using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using WitherTorch.Common.Extensions;

namespace WitherTorch.Common.Collections
{
    public sealed class TreeDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey?, TValue?>> where TKey : class where TValue : class
    {
        private readonly KeyValuePair<TKey?, object?>[] _bucket;
        private readonly IEqualityComparer<TKey> _comparer;

        private const int BucketCountExponent = 8;
        private const int BucketCount = 1 << BucketCountExponent;
        private const int BucketMask = BucketCount - 1;
        private const int HashCodeSizeInBits = sizeof(int) * 8;
        private const int BucketDepth = HashCodeSizeInBits / BucketCountExponent;

        public TreeDictionary() : this(EqualityComparer<TKey>.Default) { }
        
        public TreeDictionary(IEqualityComparer<TKey> comparer)
        {
            _bucket = new KeyValuePair<TKey?, object?>[BucketCount];
            _comparer = comparer;
        }

        public TValue? this[TKey key]
        {
            get
            {
                if (key is null)
                    return null;
                return GetItem(key);
            }
            set
            {
                if (key is null)
                    return;
                if (value is null)
                {
                    Remove(key);
                    return;
                }
                SetItem(key, value);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private TValue? GetItem(TKey key)
        {
            IEqualityComparer<TKey> comparer = _comparer;
            int hash = comparer.GetHashCode(key);
            KeyValuePair<TKey?, object?>[] bucket = _bucket;
            int i;
            for (i = 0; i < HashCodeSizeInBits - BucketCountExponent; i += BucketCountExponent)
            {
                int index = hash >> i & BucketMask;
                object? value = TryGetValueOrSubBucketInBucket(bucket, index, comparer, key);
                if (value is KeyValuePair<TKey?, object?>[] subBucket)
                {
                    bucket = subBucket;
                    continue;
                }
                return value as TValue;
            }
            return GetValueInBottomBucket(bucket, hash >> i & BucketMask, comparer, key);
        }

        [Inline(InlineBehavior.Remove)]
        private static object? TryGetValueOrSubBucketInBucket(KeyValuePair<TKey?, object?>[] bucket, int index, IEqualityComparer<TKey> comparer, TKey key)
        {
            bucket[index].Destruct(out TKey? pairKey, out object? pairValue);
            if (pairKey is null)
                return pairValue as KeyValuePair<TKey?, object?>[];
            if (comparer.Equals(pairKey, key))
                return pairValue;
            return null;
        }

        [Inline(InlineBehavior.Remove)]
        private static TValue? GetValueInBottomBucket(KeyValuePair<TKey?, object?>[] bucket, int index, IEqualityComparer<TKey> comparer, TKey key)
        {
            bucket[index].Destruct(out TKey? pairKey, out object? pairValue);
            if (pairKey is null)
            {
                if (pairValue is null)
                    return null;
                if (pairValue is List<KeyValuePair<TKey?, TValue?>> objectList)
                {
                    for (int i = 0, count = objectList.Count; i < count; i++)
                    {
                        objectList[i].Destruct(out pairKey, out TValue? value);
                        if (pairKey is not null && comparer.Equals(pairKey, key))
                            return value;
                    }
                }
                return null;
            }
            if (comparer.Equals(pairKey, key))
                return pairValue as TValue;
            return null;
        }

        [Inline(InlineBehavior.Remove)]
        private void SetItem(TKey key, TValue value)
        {
            IEqualityComparer<TKey> comparer = _comparer;
            int hash = comparer.GetHashCode(key);
            KeyValuePair<TKey?, object?>[]? bucket = _bucket;
            int i;
            for (i = 0; i < HashCodeSizeInBits - BucketCountExponent; i += BucketCountExponent)
            {
                int index = hash >> i & BucketMask;
                bucket = TrySetItemInBucket(bucket, index, i, hash, comparer, key, value);
                if (bucket is null)
                    return;
            }
            SetItemInBottomBucket(bucket, hash >> i & BucketMask, comparer, key, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static KeyValuePair<TKey?, object?>[]? TrySetItemInBucket(KeyValuePair<TKey?, object?>[] bucket, int index, int i, int hash,
            IEqualityComparer<TKey> comparer ,TKey key, TValue? value)
        {
            bucket[index].Destruct(out TKey? pairKey, out object? pairValue);
            if (pairKey is null)
            {
                if (pairValue is null) //There is no entry
                {
                    bucket[index] = new KeyValuePair<TKey?, object?>(key, value);
                    return null;
                }
                return pairValue as KeyValuePair<TKey?, object?>[];
            }
            if (comparer.Equals(pairKey, key))
            {
                bucket[index] = new KeyValuePair<TKey?, object?>(key, value);
                return null;
            }
            KeyValuePair<TKey?, object?>[] newBucket = new KeyValuePair<TKey?, object?>[BucketCount];
            bucket[index] = new KeyValuePair<TKey?, object?>(null, newBucket);
            i += BucketCountExponent;
            if (i == HashCodeSizeInBits - BucketCountExponent)
            {
                int pairHash = comparer.GetHashCode(pairKey);
                SetItemInBottomBucket(newBucket, pairHash >> i & BucketMask, comparer, pairKey, pairValue as TValue);
                SetItemInBottomBucket(newBucket, hash >> i & BucketMask, comparer, key, value);
            }
            else
            {
                int pairHash = comparer.GetHashCode(pairKey);
                TrySetItemInBucket(newBucket, pairHash >> i & BucketMask, i, pairHash, comparer, pairKey, pairValue as TValue);
                TrySetItemInBucket(newBucket, hash >> i & BucketMask, i, hash, comparer, key, value);
            }
            return null;
        }

        [Inline(InlineBehavior.Remove)]
        private static void SetItemInBottomBucket(KeyValuePair<TKey?, object?>[] bucket, int index, IEqualityComparer<TKey> comparer, TKey key, TValue? value)
        {
            bucket[index].Destruct(out TKey? pairKey, out object? pairValue);
            if (pairKey is null)
            {
                if (pairValue is null) //There is no entry
                {
                    bucket[index] = new KeyValuePair<TKey?, object?>(key, value);
                    return;
                }
                if (pairValue is List<KeyValuePair<TKey?, TValue?>> list)
                {
                    for (int j = 0, count = list.Count; j < count; j++)
                    {
                        TKey? comparingKey = list[j].Key;
                        if (comparingKey is not null && comparer.Equals(comparingKey, key))
                        {
                            list[j] = new KeyValuePair<TKey?, TValue?>(key, value);
                            return;
                        }
                    }
                    list.Add(new KeyValuePair<TKey?, TValue?>(key, value));
                }
                return;
            }
            if (comparer.Equals(pairKey, key))
            {
                bucket[index] = new KeyValuePair<TKey?, object?>(key, value);
                return;
            }
            bucket[index] = new KeyValuePair<TKey?, object?>(null, new List<KeyValuePair<TKey?, TValue?>>()
                        {
                            new KeyValuePair<TKey?, TValue?>(pairKey, pairValue as TValue),
                            new KeyValuePair<TKey?, TValue?>(key, value),
                        });
        }

        public TValue? Remove(TKey key)
        {
            IEqualityComparer<TKey> comparer = _comparer;
            int hash = comparer.GetHashCode(key);
            KeyValuePair<TKey?, object?>[] bucket = _bucket;
            int i;
            for (i = 0; i < HashCodeSizeInBits - BucketCountExponent; i += BucketCountExponent)
            {
                int index = hash >> i & BucketMask;
                object? value = TryRemoveValueOrReturnSubBucketInBucket(bucket, index, comparer, key);
                if (value is KeyValuePair<TKey?, object?>[] subBucket)
                {
                    bucket = subBucket;
                    continue;
                }
                return value as TValue;
            }
            return RemoveValueInBottomBucket(bucket, hash >> i & BucketMask, comparer, key);
        }

        [Inline(InlineBehavior.Remove)]
        private static object? TryRemoveValueOrReturnSubBucketInBucket(KeyValuePair<TKey?, object?>[] bucket, int index, IEqualityComparer<TKey> comparer, TKey key)
        {
            bucket[index].Destruct(out TKey? pairKey, out object? pairValue);
            if (pairKey is null)
                return pairValue as KeyValuePair<TKey?, object?>[];
            if (comparer.Equals(pairKey, key))
            {
                bucket[index] = default;
                return pairValue;
            }
            return null;
        }

        [Inline(InlineBehavior.Remove)]
        private static TValue? RemoveValueInBottomBucket(KeyValuePair<TKey?, object?>[] bucket, int index, IEqualityComparer<TKey> comparer, TKey key)
        {
            bucket[index].Destruct(out TKey? pairKey, out object? pairValue);
            if (pairKey is null)
            {
                if (pairValue is null)
                    return null;
                if (pairValue is List<KeyValuePair<TKey?, TValue?>> objectList)
                {
                    int i, count;
                    TValue? value = null;
                    for (i = 0, count = objectList.Count; i < count; i++)
                    {
                        objectList[i].Destruct(out pairKey, out value);
                        if (pairKey is not null && comparer.Equals(pairKey, key))
                            break;
                    }
                    if (i < count)
                    {
                        objectList.RemoveAt(i);
                        return value;
                    }
                }
                return null;
            }
            if (comparer.Equals(pairKey, key))
            {
                bucket[index] = default;
                return pairValue as TValue;
            }
            return null;
        }

        public void Clear()
        {
            ClearBucket(_bucket);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ClearBucket(KeyValuePair<TKey?, object?>[] bucket)
        {
            for (int i = 0; i < BucketCount; i++)
            {
                bucket[i].Destruct(out TKey? key, out object? value);
                if (key is null)
                {
                    if (value is KeyValuePair<TKey?, object?>[] subBucket)
                    {
                        ClearBucket(subBucket);
                        continue;
                    }
                    bucket[i] = default;
                    continue;
                }
                if (value is List<KeyValuePair<TKey?, object?>> list)
                    list.Clear();
                bucket[i] = default;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_bucket);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(_bucket);
        }

        IEnumerator<KeyValuePair<TKey?, TValue?>> IEnumerable<KeyValuePair<TKey?, TValue?>>.GetEnumerator()
        {
            return new Enumerator(_bucket);
        }

        public unsafe struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly KeyValuePair<TKey?, object?>[] _bucket;

            private KeyValuePair<TKey, TValue> _current;

            private fixed int _path[BucketDepth + 1];

            public KeyValuePair<TKey, TValue> Current => _current;

            object IEnumerator.Current => _current;

            public Enumerator(KeyValuePair<TKey?, object?>[] bucket)
            {
                _bucket = bucket;
                _current = default;
                fixed (int* path = _path)
                {
                    for (int i = 0; i < BucketDepth + 1; i++)
                    {
                        path[i] = -1;
                    }
                }
            }

            public bool MoveNext()
            {
                fixed (int* path = _path)
                {
                    KeyValuePair<TKey?, TValue?> result = GetNextItem(_bucket, path);
                    if (result.Key is null || result.Value is null)
                        return false;
                    _current = result!;
                }
                return true;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static KeyValuePair<TKey?, TValue?> GetNextItem(KeyValuePair<TKey?, object?>[] bucket, int* path)
            {
                for (int i = *path + 1; i < BucketCount; i++)
                {
                    bucket[i].Destruct(out TKey? key, out object? value);
                    if (key is null)
                    {
                        if (value is null)
                            continue;
                        if (value is KeyValuePair<TKey?, object?>[] subBucket)
                        {
                            KeyValuePair<TKey?, TValue?> result = GetNextItem(subBucket, path + 1);
                            if (result.Key is null && result.Value is null)
                                continue;
                            *path = i;
                            return result;
                        }
                        if (value is List<KeyValuePair<TKey?, TValue?>> list)
                        {
                            KeyValuePair<TKey?, TValue?> result = GetNextItem(list, path + 1);
                            if (result.Key is null && result.Value is null)
                                continue;
                            *path = i;
                            return result;
                        }
                    }
                    *path = i;
                    return new KeyValuePair<TKey?, TValue?>(key, value as TValue);
                }
                *path = -1;
                return default;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static KeyValuePair<TKey?, TValue?> GetNextItem(List<KeyValuePair<TKey?, TValue?>> list, int* path)
            {
                int i = *path + 1;
                if (i < list.Count)
                {
                    *path = i;
                    return list[i];
                }
                *path = -1;
                return default;
            }

            public void Reset()
            {
                _current = default;
                fixed (int* path = _path)
                {
                    for (int i = 0; i < BucketDepth + 1; i++)
                    {
                        path[i] = -1;
                    }
                }
            }

            public void Remove()
            {
                _current = default;

                fixed (int* path = _path)
                {
                    KeyValuePair<TKey?, object?>[] bucket = _bucket;
                    int i;
                    for (i = 0; i < BucketDepth; i++)
                    {
                        int subPath = path[i];
                        if (subPath == -1 && i > 0)
                        {
                            bucket[path[i - 1]] = default;
                            return;
                        }
                        bucket[subPath].Destruct(out TKey? key, out object? value);
                        if (key is null)
                        {
                            if (value is KeyValuePair<TKey?, object?>[] subBucket)
                            {
                                bucket = subBucket;
                                continue;
                            }
                        }
                        bucket[subPath] = default;
                        return;
                    }
                    i = path[BucketDepth];
                    if (bucket[i] is IList<KeyValuePair<TKey?, object?>> list)
                    {
                        list.RemoveAt(i);
                        path[BucketDepth] = i - 1;
                        return;
                    }
                    bucket[i] = default;
                }
            }

            public void Dispose()
            {

            }
        }
    }
}
