#if NET8_0_OR_GREATER
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

using RiceTea.Core.Buffers;
using RiceTea.Core.Helpers;

namespace RiceTea.Core.Extensions;

partial class ArrayPoolExtensions
{
    partial class CaptureCore<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPool<T>.RentScope FromImmutableArray(ArrayPool<T> _this, scoped ref ImmutableArray<T> array)
        {
            if (array.IsDefaultOrEmpty)
                return _this.EnterRentScope();
            return FromArray(_this, UnsafeHelper.As<ImmutableArray<T>, T[]>(ref array));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPool<T>.RentScope FromImmutableList<TList>(ArrayPool<T> _this, TList list) where TList : IImmutableList<T>, ICollection<T>
        {
            ArrayPool<T>.RentScope scope = _this.EnterRentScope();
            try
            {
                scope.CopyFrom(list, startIndex: 0, count: ((ICollection<T>)list).Count);
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
            return scope;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPool<T>.RentScope FromImmutableSet<TSet>(ArrayPool<T> _this, TSet set) where TSet : IImmutableSet<T>, ICollection<T>
        {
            ArrayPool<T>.RentScope scope = _this.EnterRentScope();
            try
            {
                scope.CopyFrom(set, startIndex: 0, count: ((ICollection<T>)set).Count);
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
            return scope;
        }
    }
}
#endif