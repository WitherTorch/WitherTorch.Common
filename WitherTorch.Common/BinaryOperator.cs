using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using InlineIL;

using InlineMethod;

using WitherTorch.Common.Helpers;

namespace WitherTorch.Common
{
    public delegate T BinaryOperation<T>(T a, T b);

    public interface IBinaryOperator<T>
    {
        T Operate(T a, T b);

        BinaryOperation<T> ToOperation();
    }

    internal interface IDefaultBinaryOperator<T> : IBinaryOperator<T>
    {
        BinaryOperatorType Type { get; }
    }

    public enum BinaryOperatorType : uint
    {
        Left,
        Right,
        Or,
        And,
        Xor,
        Add,
        Subtract,
        Multiply,
        Divide,
        Min,
        Max
    }

    public abstract class BinaryOperator<T> : IBinaryOperator<T>
    {
        public static BinaryOperator<T> Left => LeftImpl.Instance;
        public static BinaryOperator<T> Right => RightImpl.Instance;
        public static BinaryOperator<T> Or => OrImpl.Instance;
        public static BinaryOperator<T> And => AndImpl.Instance;
        public static BinaryOperator<T> Xor => XorImpl.Instance;
        public static BinaryOperator<T> Add => AddImpl.Instance;
        public static BinaryOperator<T> Subtract => SubtractImpl.Instance;
        public static BinaryOperator<T> Multiply => MultiplyImpl.Instance;
        public static BinaryOperator<T> Divide => DivideImpl.Instance;
        public static BinaryOperator<T> Min => MinImpl.Instance;
        public static BinaryOperator<T> Max => MaxImpl.Instance;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BinaryOperator<T> Create(BinaryOperation<T> operation) => new DelegateImpl(operation);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BinaryOperator<T> GetDefault(BinaryOperatorType type)
            => type switch
            {
                BinaryOperatorType.Left => Left,
                BinaryOperatorType.Right => Right,
                BinaryOperatorType.Or => Or,
                BinaryOperatorType.And => And,
                BinaryOperatorType.Xor => Xor,
                BinaryOperatorType.Add => Add,
                BinaryOperatorType.Subtract => Subtract,
                BinaryOperatorType.Multiply => Multiply,
                BinaryOperatorType.Divide => Divide,
                BinaryOperatorType.Min => Min,
                BinaryOperatorType.Max => Max,
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };

        public abstract T Operate(T a, T b);

        public virtual BinaryOperation<T> ToOperation() => Operate;

        private sealed class DelegateImpl : BinaryOperator<T>
        {
            private readonly BinaryOperation<T> _operation;

            public DelegateImpl(BinaryOperation<T> operation) => _operation = operation;

            public override T Operate(T a, T b) => _operation.Invoke(a, b);

            public override BinaryOperation<T> ToOperation() => _operation;
        }

        private sealed class LeftImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance = new LeftImpl();

            private LeftImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.Left;

            public override T Operate(T a, T b) => a;

            public override BinaryOperation<T> ToOperation() => static (a, b) => a;
        }

        private sealed class RightImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance = new RightImpl();

            private RightImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.Right;

            public override T Operate(T a, T b) => b;

            public override BinaryOperation<T> ToOperation() => static (a, b) => b;
        }

        internal sealed unsafe class ReflectionImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            private readonly delegate* managed<T, T, T> _func;
            private readonly BinaryOperatorType _type;

            private BinaryOperation<T>? _cachedOperation;

            public ReflectionImpl(BinaryOperatorType type, string funcName)
            {
                _type = type;
                nint functionPointer = ReflectionHelper.GetMethodPointer(typeof(T), funcName, [typeof(T), typeof(T)], typeof(T),
                        BindingFlags.Public | BindingFlags.Static);
                if (functionPointer == default)
                    throw new InvalidOperationException($"{typeof(T)} doesn't have the operator of {type}!");
                _func = (delegate* managed<T, T, T>)functionPointer;
            }

            public BinaryOperatorType Type => _type;

            public override T Operate(T a, T b) => _func(a, b);

            public override BinaryOperation<T> ToOperation()
            {
                BinaryOperation<T>? operation = _cachedOperation;
                if (operation is null)
                {
                    IL.Emit.Ldnull();
                    IL.Push(_func);
                    IL.Emit.Newobj(MethodRef.Constructor(typeof(BinaryOperation<T>), typeof(object), typeof(nint)));
                    IL.Pop(out operation);
                    _cachedOperation = operation;
                }
                return operation!;
            }
        }

        private sealed class OrImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance;

            static OrImpl()
            {
                if (UnsafeHelper.IsPrimitiveType<T>())
                {
                    if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
                        throw new InvalidOperationException($"{typeof(T)} doesn't have the operator of {BinaryOperatorType.Or}!");
                    Instance = new OrImpl();
                }
                else
                    Instance = new ReflectionImpl(BinaryOperatorType.Or, "op_BitwiseOr");
            }

            private OrImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.Or;

            public override T Operate(T a, T b) => UnsafeHelper.Or(a, b);

            public override BinaryOperation<T> ToOperation() => UnsafeHelper.Or;
        }

        private sealed class AndImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance;

            static AndImpl()
            {
                if (UnsafeHelper.IsPrimitiveType<T>())
                {
                    if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
                        throw new InvalidOperationException($"{typeof(T)} doesn't have the operator of {BinaryOperatorType.And}!");
                    Instance = new AndImpl();
                }
                else
                    Instance = new ReflectionImpl(BinaryOperatorType.And, "op_BitwiseAnd");
            }

            private AndImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.And;

            public override T Operate(T a, T b) => UnsafeHelper.And(a, b);

            public override BinaryOperation<T> ToOperation() => UnsafeHelper.And;
        }

        private sealed class XorImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance;

            static XorImpl()
            {
                if (UnsafeHelper.IsPrimitiveType<T>())
                {
                    if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
                        throw new InvalidOperationException($"{typeof(T)} doesn't have the operator of {BinaryOperatorType.Xor}!");
                    Instance = new XorImpl();
                }
                else
                    Instance = new ReflectionImpl(BinaryOperatorType.Xor, "op_ExclusiveOr");
            }

            private XorImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.Xor;

            public override T Operate(T a, T b) => UnsafeHelper.Xor(a, b);

            public override BinaryOperation<T> ToOperation() => UnsafeHelper.Xor;
        }

        private sealed class AddImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance;

            static AddImpl()
            {
                if (UnsafeHelper.IsPrimitiveType<T>())
                    Instance = new AddImpl();
                else
                    Instance = new ReflectionImpl(BinaryOperatorType.Add, "op_Addition");
            }

            private AddImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.Add;

            public override T Operate(T a, T b) => UnsafeHelper.Add(a, b);

            public override BinaryOperation<T> ToOperation() => UnsafeHelper.Add;
        }

        private sealed class SubtractImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance;

            static SubtractImpl()
            {
                if (UnsafeHelper.IsPrimitiveType<T>())
                    Instance = new SubtractImpl();
                else
                    Instance = new ReflectionImpl(BinaryOperatorType.Subtract, "op_Subtraction");
            }

            private SubtractImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.Subtract;

            public override T Operate(T a, T b) => UnsafeHelper.Subtract(a, b);

            public override BinaryOperation<T> ToOperation() => UnsafeHelper.Subtract;
        }

        private sealed class MultiplyImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance;

            static MultiplyImpl()
            {
                if (UnsafeHelper.IsPrimitiveType<T>())
                    Instance = new MultiplyImpl();
                else
                    Instance = new ReflectionImpl(BinaryOperatorType.Multiply, "op_Multiply");
            }

            private MultiplyImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.Multiply;

            public override T Operate(T a, T b) => UnsafeHelper.Multiply(a, b);

            public override BinaryOperation<T> ToOperation() => UnsafeHelper.Multiply;
        }

        private sealed class DivideImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance;

            static DivideImpl()
            {
                if (UnsafeHelper.IsPrimitiveType<T>())
                    Instance = new DivideImpl();
                else
                    Instance = new ReflectionImpl(BinaryOperatorType.Divide, "op_Division");
            }

            private DivideImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.Divide;

            public override T Operate(T a, T b)
                => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.DivideUnsigned(a, b) : UnsafeHelper.Divide(a, b);

            public override BinaryOperation<T> ToOperation() =>
                static (a, b) => UnsafeHelper.IsUnsigned<T>() ? UnsafeHelper.DivideUnsigned(a, b) : UnsafeHelper.Divide(a, b);
        }

        private sealed class MinImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance = new MinImpl();

            private MinImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.Min;

            public override T Operate(T a, T b) => MinCore(a, b);

            public override BinaryOperation<T> ToOperation() => MinCore;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static T MinCore(T a, T b)
            {
                if (UnsafeHelper.IsPrimitiveType<T>())
                    return UnsafeHelper.Min(a, b);
                else
                    return Comparer<T>.Default.Compare(a, b) < 0 ? a : b;
            }
        }

        private sealed class MaxImpl : BinaryOperator<T>, IDefaultBinaryOperator<T>
        {
            public static readonly BinaryOperator<T> Instance = new MaxImpl();

            private MaxImpl() { }

            public BinaryOperatorType Type => BinaryOperatorType.Max;

            public override T Operate(T a, T b) => MaxCore(a, b);

            public override BinaryOperation<T> ToOperation() => MaxCore;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static T MaxCore(T a, T b)
            {
                if (UnsafeHelper.IsPrimitiveType<T>())
                    return UnsafeHelper.Max(a, b);
                else
                    return Comparer<T>.Default.Compare(a, b) > 0 ? a : b;
            }
        }
    }
}
