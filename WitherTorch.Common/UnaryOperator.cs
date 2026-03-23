using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using WitherTorch.Common.Helpers;
using WitherTorch.Common.Threading;

namespace WitherTorch.Common
{
    public delegate T UnaryOperation<T>(T value);

    public interface IUnaryOperator<T>
    {
        T Operate(T value);

        UnaryOperation<T> ToOperation();
    }

    internal interface IDefaultUnaryOperator<T> : IUnaryOperator<T>
    {
        UnaryOperatorType Type { get; }
    }

    internal enum UnaryOperatorType
    {
        Identity,
        Not,
        _Last
    }

    public abstract class UnaryOperator<T> : IUnaryOperator<T>
    {
        public static UnaryOperator<T> Identity => IdentityImpl.Instance;
        public static UnaryOperator<T> Not => NotImpl.Instance;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnaryOperator<T> Create(UnaryOperation<T> operation) => new DelegateImpl(operation);

        public abstract T Operate(T value);

        public virtual UnaryOperation<T> ToOperation() => Operate;

        private sealed class DelegateImpl : UnaryOperator<T>
        {
            private readonly UnaryOperation<T> _operation;

            public DelegateImpl(UnaryOperation<T> operation) => _operation = operation;

            public override T Operate(T value) => _operation.Invoke(value);

            public override UnaryOperation<T> ToOperation() => _operation;
        }

        private sealed class IdentityImpl : UnaryOperator<T>, IDefaultUnaryOperator<T>
        {
            public static readonly UnaryOperator<T> Instance = new IdentityImpl();

            private IdentityImpl() { }

            public UnaryOperatorType Type => UnaryOperatorType.Identity;

            public override T Operate(T value) => value;

            public override UnaryOperation<T> ToOperation() => static (value) => value;
        }

        internal sealed unsafe class ReflectionImpl : UnaryOperator<T>, IDefaultUnaryOperator<T>
        {
            private readonly delegate* managed<T, T> _func;
            private readonly UnaryOperatorType _type;

            private UnaryOperation<T>? _cachedOperation;

            public ReflectionImpl(UnaryOperatorType type, string funcName)
            {
                _type = type;
                nint functionPointer = ReflectionHelper.GetMethodPointer(typeof(T), funcName, [typeof(T)], typeof(T),
                        BindingFlags.Public | BindingFlags.Static);
                if (functionPointer == default)
                    throw new InvalidOperationException($"{typeof(T)} doesn't have the operator of {type}!");
                _func = (delegate* managed<T, T>)functionPointer;
            }

            public UnaryOperatorType Type => _type;

            public override T Operate(T value) => _func(value);

            public override UnaryOperation<T> ToOperation()
                => _cachedOperation ??= Marshal.GetDelegateForFunctionPointer<UnaryOperation<T>>((IntPtr)_func);
        }

        private sealed class NotImpl : UnaryOperator<T>, IDefaultUnaryOperator<T>
        {
            public static readonly UnaryOperator<T> Instance;

            static NotImpl()
            {
                if (UnsafeHelper.IsPrimitiveType<T>())
                {
                    if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
                        throw new InvalidOperationException($"{typeof(T)} doesn't have the operator of {UnaryOperatorType.Not}!");
                    Instance = new NotImpl();
                }
                else
                    Instance = new ReflectionImpl(UnaryOperatorType.Not, "op_OnesComplement");
            }

            public UnaryOperatorType Type => UnaryOperatorType.Not;

            public override T Operate(T value) => UnsafeHelper.Not(value);

            public override UnaryOperation<T> ToOperation() => static (value) => UnsafeHelper.Not(value);
        }
    }
}
