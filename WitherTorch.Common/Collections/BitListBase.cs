using System;
using System.Collections;
using System.Collections.Generic;

using InlineMethod;

using WitherTorch.Common.Helpers;

using System.Runtime.CompilerServices;



#if NET472_OR_GREATER
using WitherTorch.Common.Extensions;
#endif

namespace WitherTorch.Common.Collections
{
    public abstract class BitListBase : IList<bool>, IReadOnlyList<bool>
    {
        protected const nuint One = 1;
#if B64_ARCH
        protected const int SectionSize = sizeof(ulong) * 8;
#elif B32_ARCH
        protected const int SectionSize = sizeof(uint) * 8;
#else
        protected static readonly int SectionSize = sizeof(nuint) * 8;
#endif

        protected nuint[] _array;
        protected int _count;

        protected BitListBase(nuint[] array)
        {
            _array = array;
            _count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int GetRequiredSectionCount(int count) 
            => MathHelper.CeilDiv(MathHelper.Max(count, 0), SectionSize);

        public bool this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return GetBit(index);
            }
            set
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                SetBit(index, value);
            }
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(bool item)
        {
            int index = _count++;
            EnsureCapacity();
            SetBit(index, item);
        }

        public void Clear()
        {
            _count = 0;
            Array.Clear(_array);
        }

        public unsafe bool Contains(bool item)
        {
            int count = _count;
            if (count <= 0)
                return false;
            int fullSections = MathHelper.DivRem(count, SectionSize, out int lastSectionBits);
            fixed (nuint* ptr = _array)
            {
                if (item)
                    return ContainsTrue(ptr, fullSections, lastSectionBits);
                return ContainsFalse(ptr, fullSections, lastSectionBits);
            }
        }

        public unsafe void CopyTo(bool[] array, int arrayIndex)
        {
            int count = _count;
            if (arrayIndex < 0 || arrayIndex + count > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            int fullSections = MathHelper.DivRem(count, SectionSize, out int lastSectionBits);
            fixed (nuint* ptr = _array)
            {
                for (int i = 0; i < fullSections; i++)
                {
                    nuint section = ptr[i];
                    for (int bitIndex = 0; bitIndex < SectionSize; bitIndex++)
                        array[arrayIndex++] = (section & (One << bitIndex)) != 0;
                }
                if (lastSectionBits > 0)
                {
                    nuint section = ptr[fullSections];
                    for (int bitIndex = 0; bitIndex < lastSectionBits; bitIndex++)
                        array[arrayIndex++] = (section & (One << bitIndex)) != 0;
                }
            }
        }

        public IEnumerator<bool> GetEnumerator() => new Enumerator(this);

        public unsafe int IndexOf(bool item)
        {
            int count = _count;
            if (count <= 0)
                return -1;
            int fullSections = MathHelper.DivRem(count, SectionSize, out int lastSectionBits);
            fixed (nuint* ptr = _array)
            {
                if (item)
                    return IndexOfTrue(ptr, fullSections, lastSectionBits);
                return IndexOfFalse(ptr, fullSections, lastSectionBits);
            }
        }

        public void Insert(int index, bool item)
        {
            int oldCount = _count;
            if (index < 0 || index > oldCount)
                throw new ArgumentOutOfRangeException(nameof(index));
            _count = oldCount + 1;
            EnsureCapacity();
            if (index == oldCount)
            {
                SetBit(index, item); // Same as Add operation
                return;
            }
            InsertBit(index, item);
        }

        public bool Remove(bool item)
        {
            int indexOf = IndexOf(item);
            if (indexOf < 0)
                return false;
            RemoveAtCore(indexOf);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException(nameof(index));
            RemoveAtCore(index);
        }

        public void RemoveLast() => _count = MathHelper.Max(_count - 1, 0);

        private void RemoveAtCore(int index)
        {
            int oldCount = _count;
            if (index < oldCount)
                RemoveBit(index, oldCount);
            _count = oldCount - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool GetBit(int index)
        {
            int sectionIndex = MathHelper.DivRem(index, SectionSize, out int bitIndex);
            fixed (nuint* ptr = _array)
                return (ptr[sectionIndex] & (One << bitIndex)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void SetBit(int index, bool value)
        {
            int sectionIndex = MathHelper.DivRem(index, SectionSize, out int bitIndex);
            fixed (nuint* ptr = _array)
                SetBit(ref ptr[sectionIndex], bitIndex, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void SetBit(ref nuint section, int bitIndex, bool value)
        {
            fixed (nuint* ptr = _array)
            {
                nuint mask = One << bitIndex;
                if (value)
                    section |= mask;
                else
                    section &= ~mask;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void InsertBit(int index, bool value)
        {
            int sectionIndex = MathHelper.DivRem(index, SectionSize, out int bitIndex);
            int sectionCount = MathHelper.CeilDiv(_count, SectionSize);
            fixed (nuint* ptr = _array)
            {
                nuint mask = One << bitIndex;
                nuint section = ptr[sectionIndex];
                nuint movingPart = (section & ((One << bitIndex) - 1)) >>> 1;
                nuint travelingBit = section & 0b01;
                ptr[sectionIndex] = (section & (UnsafeHelper.GetMaxValue<nuint>() << (bitIndex + 1))) |
                    (MathHelper.BooleanToNativeUnsigned(value) << bitIndex) | movingPart;
                for (int i = sectionIndex; i < sectionCount; i++) // 後續區塊內的位元全部向右移一位
                {
                    section = ptr[i];
                    ptr[i] = section >>> 1 | travelingBit << (SectionSize - 1);
                    travelingBit = section & 0b01;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void RemoveBit(int index, int count)
        {
            int sectionIndex = MathHelper.DivRem(index, SectionSize, out int bitIndex);
            int sectionCount = MathHelper.CeilDiv(count, SectionSize);
            fixed (nuint* ptr = _array)
            {
                nuint mask = One << bitIndex;
                nuint section = ptr[sectionIndex];
                nuint movingPart = (section & ((One << bitIndex) - 1)) << 1;
                ptr[sectionIndex] = (section & (UnsafeHelper.GetMaxValue<nuint>() << (bitIndex + 1))) | movingPart;
                if (sectionCount <= sectionIndex)
                    return;
                UnsafeHelper.SkipInit(out nuint travelingBit);
                int i = sectionCount - 1;
                do
                {
                    section = ptr[i];
                    ptr[i] = section >>> 1 | travelingBit << (SectionSize - 1);
                    travelingBit = section & 0b01;
                } while (--i > sectionIndex);
                ptr[sectionIndex] |= travelingBit << (SectionSize - 1); // 補上位元
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe bool ContainsTrue(nuint* sections, int fullSectionCount, int lastSectionBits)
        {
            for (int i = 0; i < fullSectionCount; i++)
            {
                if (sections[i] != 0)
                    return true;
            }
            if (lastSectionBits <= 0)
                return false;
            nuint mask = (One << lastSectionBits) - 1;
            return (sections[fullSectionCount] & mask) != 0;
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe bool ContainsFalse(nuint* sections, int fullSectionCount, int lastSectionBits)
        {
            for (int i = 0; i < fullSectionCount; i++)
            {
                if (sections[i] != UnsafeHelper.GetMaxValue<nuint>())
                    return true;
            }
            if (lastSectionBits <= 0)
                return false;
            nuint mask = (One << lastSectionBits) - 1;
            return (~sections[fullSectionCount] & mask) != 0;
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe int IndexOfTrue(nuint* sections, int fullSectionCount, int lastSectionBits)
        {
            for (int i = 0; i < fullSectionCount; i++)
            {
                nuint section = sections[i];
                if (section == 0)
                    continue;
                return i * SectionSize + MathHelper.TrailingZeroCount(section);
            }
            if (lastSectionBits > 0)
            {
                nuint section = sections[fullSectionCount] & (One << lastSectionBits) - 1;
                if (section > 0)
                    return fullSectionCount * SectionSize + MathHelper.TrailingZeroCount(section);
            }
            return -1;
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe int IndexOfFalse(nuint* sections, int fullSectionCount, int lastSectionBits)
        {
            for (int i = 0; i < fullSectionCount; i++)
            {
                nuint section = sections[i];
                if (section == UnsafeHelper.GetMaxValue<nuint>())
                    continue;
                return i * SectionSize + MathHelper.TrailingZeroCount(~section);
            }
            if (lastSectionBits > 0)
            {
                nuint section = ~sections[fullSectionCount] & (One << lastSectionBits) - 1;
                if (section > 0)
                    return fullSectionCount * SectionSize + MathHelper.TrailingZeroCount(section);
            }
            return -1;
        }

        protected abstract void EnsureCapacity();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private sealed class Enumerator : IEnumerator<bool>
        {
            private readonly BitListBase _list;

            private int _index;

            public Enumerator(BitListBase list)
            {
                _list = list;
                _index = -1;
            }

            public bool Current
            {
                get
                {
                    BitListBase list = _list;
                    int index = _index;
                    if (index < 0 || index >= list.Count)
                        throw new InvalidOperationException();
                    return list.GetBit(index);
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                BitListBase list = _list;
                int nextIndex = _index + 1;
                int limit = list.Count;
                if (nextIndex > limit)
                    return false;
                _index = nextIndex;
                return nextIndex < limit;
            }

            public void Reset() => _index = -1;
        }
    }
}
