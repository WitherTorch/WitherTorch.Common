using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

using RiceTea.Core.Helpers;

namespace RiceTea.Core.Extensions;

partial class ArrayPoolExtensions
{
    private static unsafe class ReadOnlyCollectionUnwrapper<T>
    {
        private static readonly void* _unwrapFunc;
        private static readonly bool _isEnabled;

        static ReadOnlyCollectionUnwrapper()
        {
            void* func = (void*)ReflectionHelper.GetPropertyGetterPointer(typeof(ReadOnlyCollection<T>), "Items", typeof(IList<T>),
                flags: BindingFlags.Instance | BindingFlags.NonPublic);
            _isEnabled = func is not null;
            _unwrapFunc = func;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryUnwrap(ReadOnlyCollection<T> collection, [NotNullWhen(true)] out IList<T>? result)
        {
            if (!_isEnabled)
            {
                result = default;
                return false;
            }
            result = ((delegate* managed<ReadOnlyCollection<T>, IList<T>>)_unwrapFunc)(collection);
            return true;
        }
    }
}
