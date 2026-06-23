using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WitherTorch.Common.Collections;
using WitherTorch.Common.Exceptions;
using WitherTorch.Common.Extensions;
using WitherTorch.Common.Helpers;

namespace WitherTorch.Common.Buffers;

partial class ArrayPool<T>
{
	public ref struct RentScope : IList<T>, IDisposable
	{
		private ArrayPool<T>? _pool;
		private T[]? _array;
		private int _count;

		public readonly T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (index < 0 || index >= _count)
					return IndexOutOfRangeException.Throw<T>();

				T[]? array = _array;
				DebugHelper.ThrowIf(array is null);
				return array.AsUnsafeRef()[index];
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (index < 0 || index >= _count)
					IndexOutOfRangeException.Throw();

				T[]? array = _array;
				DebugHelper.ThrowIf(array is null);
				array.AsUnsafeRef()[index] = value;
			}
		}

		public readonly int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				int count = _count;
				if (count <= 0)
					return 0;
				T[]? array = _array;
				DebugHelper.ThrowIf(array is null);
				return array.Length;
			}
		}

		public readonly int Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _count;
		}

		readonly bool ICollection<T>.IsReadOnly
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly void Clear()
		{
			int count = _count;
			if (count <= 0)
				return;
			T[]? array = _array;
			DebugHelper.ThrowIf(array is null);
			Array.Clear(array, 0, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool Contains(T item)
		{
			int count = _count;
			if (count <= 0)
				return false;
			int index = IndexOfCore(item, count);
			return index >= 0 && index < _count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly void CopyTo(T[] destination, int arrayIndex)
		{
			int count = _count;
			if (count <= 0)
				return;
			T[]? array = _array;
			DebugHelper.ThrowIf(array is null);
			Array.Copy(array, 0, destination, arrayIndex, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly Enumerator GetEnumerator()
		{
			T[]? array = _array;
			DebugHelper.ThrowIf(array is null);
			return new Enumerator(array, _count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int IndexOf(T item)
		{
			int count = _count;
			if (count <= 0)
				return -1;
			return IndexOfCore(item, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Resize(int newCount, bool moveArray = true)
		{
			ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newCount);

			int count = _count;
			if (newCount == count)
				return;

			ArrayPool<T>? pool = _pool;
			if (pool is null)
			{
				ObjectDisposedException.Throw(null);
				return;
			}

			T[]? array = _array;
			if (count > newCount)
			{
				DebugHelper.ThrowIf(array is null);
				if (!_isUnmanagedType)
					Array.Clear(array, newCount, count - newCount);
				return;
			}
			DebugHelper.ThrowIf(count == newCount, "Unexpectable!");

			int capacity;
			if (array is null || (capacity = array.Length) <= 0)
			{
				array = pool.Rent(newCount);
				DebugHelper.ThrowIf(array.Length < newCount);
				_array = array;
				_count = newCount;
			}
			else if (capacity >= newCount)
				_count = newCount;
			else
			{
				T[] newArray = pool.Rent(newCount);
				DebugHelper.ThrowIf(newArray.Length < newCount);
				try
				{
					if (moveArray)
						Array.Copy(array, 0, newArray, 0, count);
				}
				finally
				{
					pool.Return(array);
					_array = newArray;
					_count = newCount;
				}
			}
		}

		readonly void IList<T>.Insert(int index, T item)
			=> NotSupportedException.Throw();

		readonly void IList<T>.RemoveAt(int index)
			=> NotSupportedException.Throw();

		readonly void ICollection<T>.Add(T item)
			=> NotSupportedException.Throw();

		readonly bool ICollection<T>.Remove(T item)
			=> NotSupportedException.Throw<bool>();

		readonly IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			int count = _count;
			if (count <= 0)
				return EnumeratorHelper.CreateEmptyEnumerator<T>();
			T[]? array = _array;
			DebugHelper.ThrowIf(array is null);
			return new LimitedArrayEnumerator<T>(array, count);
		}

		readonly IEnumerator IEnumerable.GetEnumerator()
		{
			int count = _count;
			if (count <= 0)
				return EnumeratorHelper.CreateEmptyEnumerator<T>();
			T[]? array = _array;
			DebugHelper.ThrowIf(array is null);
			return new LimitedArrayEnumerator<T>(array, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly int IndexOfCore(T item, int count)
		{
			T[]? array = _array;
			DebugHelper.ThrowIf(array is null);
			if (_isUnmanagedType)
				return SequenceHelper.IndexOf(array, item, 0, count);
			else
				return Array.IndexOf(array, item, 0, count);
		}

		public void Dispose()
		{
			ArrayPool<T>? pool = _pool;
			if (pool is null)
				return;

			T[]? array = _array;
			DebugHelper.ThrowIf(array is null);
			pool.Return(array);

			_pool = null;
			_array = null;
			_count = 0;
		}

		public ref struct Enumerator : IEnumerator<T>
		{
			private readonly T[] _array;
			private readonly int _count;

			private int _index;

			public Enumerator(T[] array, int count)
			{
				_array = array;
				_count = count;
				_index = -1;
			}

			public readonly T Current
			{
				get
				{
					int index = _index;
					if (index < 0 || index >= _count)
						return InvalidOperationException.Throw<T>();
					return _array[index];
				}
			}

			readonly object? IEnumerator.Current => Current;

			public readonly void Dispose() { }

			public bool MoveNext()
			{
				int index = _index + 1;
				int count = _count;
				if (index < count)
				{
					_index = index;
					return index >= 0;
				}
				return false;
			}

			public void Reset()
			{
				_index = 0;
			}
		}
	}
}
